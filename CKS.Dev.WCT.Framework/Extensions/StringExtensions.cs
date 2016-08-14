using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CKS.Dev.WCT.Framework.Extensions
{
    public static class StringExtensions
    {
        public static bool EqualsIgnoreCase(this string text, string value)
        {
            return text.Equals(value, StringComparison.OrdinalIgnoreCase);
        }

        public static bool StartsWithIgnoreCase(this string text, string value)
        {
            return text.StartsWith(value, StringComparison.OrdinalIgnoreCase);
        }

        public static bool EndsWithIgnoreCase(this string text, string value)
        {
            return text.EndsWith(value, StringComparison.OrdinalIgnoreCase);
        }


        public static string EnsureString(this string text)
        {
            if (text == null)
            {
                return string.Empty;
            }
            return text;
        }


        /// <summary>
        /// Returns a substring before the value. Not including the value.
        /// </summary>
        /// <param name="str">The source string.</param>
        /// <param name="value">The string value used to split.</param>
        /// <returns></returns>
        public static string SubStringBefore(this string str, string value)
        {
            string result = str;
            if(String.IsNullOrEmpty(str))
            {
                return string.Empty;
            }
            if (!String.IsNullOrEmpty(value))
            {
                int index = str.IndexOf(value);
                if (index >= 0)
                {
                    result = result.Substring(0, index);
                }
            }
            return result;
        }

        public static string SubStringAfter(this string str, string value)
        {
            string result = str;
            if (String.IsNullOrEmpty(str))
            {
                return string.Empty;
            }
            if (!String.IsNullOrEmpty(value))
            {
                int index = str.LastIndexOf(value);
                if (index >= 0)
                {
                    result = result.Substring(index + value.Length);
                }
            }
            return result;
        }


        public static string ReplaceIgnoreCase(this string original, string pattern, string replacement)
        {
            int count, position0, position1;
            count = position0 = position1 = 0;

            string upperString = original.ToUpper();
            string upperPattern = pattern.ToUpper();

            int inc = (original.Length / pattern.Length) * (replacement.Length - pattern.Length);

            char[] chars = new char[original.Length + Math.Max(0, inc)];
            while ((position1 = upperString.IndexOf(upperPattern, position0)) != -1)
            {
                for (int i = position0; i < position1; ++i)
                {
                    chars[count++] = original[i];
                }
                for (int i = 0; i < replacement.Length; ++i)
                {
                    chars[count++] = replacement[i];
                }
                position0 = position1 + pattern.Length;
            }
            if (position0 == 0)
            {
                return original;
            }
            for (int i = position0; i < original.Length; ++i)
            {
                chars[count++] = original[i];
            }
            return new string(chars, 0, count);
        }
    }
}
