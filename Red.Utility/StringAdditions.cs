using System;
namespace Red.Utility
{
    static public class StringAdditions
    {
        const string digits = "0123456789";

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
    }
}
