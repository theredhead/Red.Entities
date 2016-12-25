using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Red.Utility
{
	static public class StringAdditions
	{
		const string digits = "0123456789";

		static private SHA1CryptoServiceProvider _sha1Provider = new SHA1CryptoServiceProvider();

		static public string NumericSuffix(this string aString)
		{
			if (aString.Equals(string.Empty)) return aString;

			int index = aString.Length - 1;
			while (index > -1 && digits.Contains(aString.Substring(index, 1)))
			{
				index--;
			}
			return aString.Substring(index + 1);
		}

		static public string NextInSequence(this string aString)
		{
			string suffix = aString.NumericSuffix();
			string prefix = aString.Substring(0, aString.Length - suffix.Length);
			int number = suffix.Length > 0 ? int.Parse(suffix) : 0;
			number++;
			return $"{prefix}{number}";
		}

		static public string ToBase64EncodedString(this string plainText)
		{
			return Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));
		}

		static public string ToBase64DecodedString(this string base64EncodedData)
		{
			return Encoding.UTF8.GetString(Convert.FromBase64String(base64EncodedData));
		}

		static public string ToSHA1HashedString(this string input)
		{
			return Encoding.UTF8.GetString(_sha1Provider.ComputeHash(Encoding.UTF8.GetBytes(input)));
		}
	}
}
