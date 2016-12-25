using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using Red.Utility;

namespace Red.Entities.Web
{
	class RequestHeaders : Dictionary<string, string>
	{
		public string FirstLine { get; set; }

		public RequestHeaders(byte[] bytes) : base()
		{
			string body = Encoding.UTF8.GetString(bytes);
			string[] lines = body.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
			FirstLine = lines[0];
			for (int ix = 1; ix < lines.Length; ix++)
			{
				string label = lines[ix].Substring(0, lines[ix].IndexOf(':'));
				string value = lines[ix].Substring(lines[ix].IndexOf(':') + 1).Trim();
				base[label] = value;
			}
		}
	}


	/* request headers: 

	Cache-Control: no-cache
	Connection: Upgrade
	Pragma: no-cache
	Upgrade: websocket
	Cookie: ASP.NET_SessionId=503DA73E6EE709F69BC6D35A
	Host: 127.0.0.1:8080
	User-Agent: Mozilla/5.0 (Macintosh; Intel Mac OS X 10_12_1) AppleWebKit/602.2.14 (KHTML, like Gecko) Version/10.0.1 Safari/602.2.14
	Sec-WebSocket-Key: X+GGAmbls8/0QqLJ9xTgXQ==
	Origin: http://127.0.0.1:8080
	Sec-WebSocket-Version: 13
	Sec-WebSocket-Extensions: x-webkit-deflate-frame

	*/

	public class WebSocketClient
	{
		const string CRLF = "\r\n";
		const string WebSocketProtocolGuid = "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";

		private TcpClient _client;
		private NetworkStream _stream;

		private string _secWebSocketKey;
		private string _secWebSocketVersion;
		private string _secWebSocketExtensions;

		private bool ShouldBeRunning { get; set; } = true;
		public string ClientKey { get { return _secWebSocketKey ?? "[Identification Pending]"; } }

		public WebSocketClient(TcpClient client)
		{
			_client = client;
			_stream = client.GetStream();
			PerformHandshake();
			Run();
		}

		public void PerformHandshake()
		{
			Log($"Performing handshake.");

			Thread.Sleep(1);
			Byte[] bytes = new Byte[_client.Available];
			_stream.Read(bytes, 0, _client.Available);

			RequestHeaders headers = new RequestHeaders(bytes);
			_secWebSocketKey = headers["Sec-WebSocket-Key"];
			_secWebSocketVersion = headers["Sec-WebSocket-Version"];
			_secWebSocketExtensions = headers["Sec-WebSocket-Extensions"];

			Log($"Connect attempt from '{ClientKey}'");
			Log(_secWebSocketKey.ToBase64DecodedString());

			string handshakeResponse = Convert.ToBase64String(
				SHA1.Create().ComputeHash(
					Encoding.UTF8.GetBytes(
						_secWebSocketKey + WebSocketProtocolGuid
					)));
			try
			{
				Byte[] response = Encoding.UTF8.GetBytes("HTTP/1.1 101 Switching Protocols" + CRLF
					+ "Connection: Upgrade" + CRLF
					+ "Upgrade: websocket" + CRLF
					+ "Sec-WebSocket-Accept: " + handshakeResponse + CRLF
					+ CRLF);

				_stream.Write(response, 0, response.Length);

				Log($"Handshake sent to '{ClientKey}'");
			}
			catch (Exception ex)
			{
				SendErrorResponseAndDisconnect();
				throw ex;
			}
		}

		private void SendErrorResponseAndDisconnect()
		{
			Log("Connection Error'ed, disconnecting");

			Byte[] response = Encoding.UTF8.GetBytes("HTTP/1.1 500 Internal Server Error" + CRLF
				+ "Connection: Close" + CRLF
				+ CRLF);
			_stream.Write(response, 0, response.Length);
			_client.Close();
		}

		private void Run()
		{
			while (ShouldBeRunning)
			{
				if (_client.Available > 0)
				{
					Byte[] bytes = new Byte[_client.Available];
					_stream.Read(bytes, 0, bytes.Length);
					Frame frame = new Frame(bytes);
					Debug.Write(frame.MessageText);
					this.messageRecieved(frame.MessageText);
				}
				Thread.Sleep(10);
			}
		}

		protected virtual void messageRecieved(string message)
		{
			Log(message);
		}

		protected virtual void Log(string message)
		{
			Console.WriteLine($"{ClientKey} {message}");
		}
	}
}
