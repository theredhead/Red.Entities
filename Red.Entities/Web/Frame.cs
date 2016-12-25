using System.Linq;
using System.Text;
using Red.Utility;

namespace Red.Entities.Web
{
	public class Frame
	{
		private byte[] _bytes;
		private byte[] _dataBytes;
		private byte[] _maskBytes;

		public Frame(byte[] bytes)
		{
			_bytes = bytes;

			int lengthIndicator = _bytes[1] & 127;
			switch (lengthIndicator)
			{
				case 126:
					_maskBytes = bytes.Slice(4, 8);
					_dataBytes = bytes.Slice(8);
					break;
				case 127:
					_maskBytes = bytes.Slice(10, 14);
					_dataBytes = bytes.Slice(14);
					break;
				default:
					_maskBytes = bytes.Slice(2, 6);
					_dataBytes = bytes.Slice(6);
					break;
			}
		}

		public bool IsFinalFrame
		{
			get
			{
				return (_bytes[0] & 128) == 0;
			}
		}

		public long Length { get { return _dataBytes.Length; } }

		private byte[] _decodedData;
		public byte[] DecodedData
		{
			get
			{
				if (_decodedData == null)
				{
					_decodedData = _dataBytes.Select((b, ix) =>
									{
										return (byte)(b ^ _maskBytes[ix % 4]);
									}).ToArray();
				}
				return _decodedData;
			}
		}

		public string MessageText { get { return Encoding.UTF8.GetString(DecodedData); } }
	}
}
