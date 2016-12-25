using System;

namespace Red.Utility
{
	static public class ArrayAdditions
	{
		public static T[] Slice<T>(this T[] data, int index, int length = -1)
		{
			if (length == -1)
				length = data.Length - index;

			T[] result = new T[length];
			Array.Copy(data, index, result, 0, length);
			return result;
		}
	}
}
