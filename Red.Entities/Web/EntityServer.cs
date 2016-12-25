using System;
using System.Web;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.WebSockets;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;

namespace Red.Entities.Web
{
	public class EntityServer<DB> where DB : EntityDatabase
	{
		private List<WebSocketClient> clients = new List<WebSocketClient>();

		protected DB Database { get; private set; }

		public EntityServer(DB database)
		{
			Database = database;
		}

		[RemoteMethod]
		public IEnumerable<Entity> Search(EntityType type, string searchText)
		{
			return type.Search(searchText);
		}

		private bool serverRunning = false;

		private Thread serverThread;
		private AutoResetEvent connectionWaitHandle = new AutoResetEvent(false);

		private void HandleAsyncConnection(IAsyncResult result)
		{
			TcpListener listener = (TcpListener)result.AsyncState;
			TcpClient client = listener.EndAcceptTcpClient(result);
			connectionWaitHandle.Set(); //Inform the main thread this connection is now handled

			try
			{
				WebSocketClient wsc = new WebSocketClient(listener.AcceptTcpClient());
				Debug.WriteLine("Accepted WebSocket client.");
				clients.Add(wsc);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
		}

		private void StartServer(IPAddress ip, int port)
		{
			TcpListener listener = new TcpListener(ip, port);

			listener.Start();
			serverRunning = true;

			while (serverRunning)
			{
				listener.BeginAcceptTcpClient(HandleAsyncConnection, listener);
				connectionWaitHandle.WaitOne(); // Wait until a client has begun handling an event
				connectionWaitHandle.Reset(); // Reset wait handle or the loop goes as fast as it can (after first request)
			}

			Debug.WriteLine("Stopped listening for WebSocket connections.");
		}

		public void Run(IPAddress ip, int port)
		{
			serverThread = new Thread(new ThreadStart(() => { StartServer(ip, port); }));
			serverThread.Start();
		}
	}
}
