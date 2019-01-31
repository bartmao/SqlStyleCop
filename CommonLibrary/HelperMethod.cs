using System;
using System.Text;

namespace TST
{
    /// <summary>
    /// Common helper methods
    /// </summary>
    public static class HelperMethod
    {
        /// <summary>
        /// Splits a string to a readable title
        /// <remarks>
        /// More TBD
        /// </remarks>
        /// </summary>
        public static string GetSuggestedName(string str)
        {
            if (str.Length == 0 || str.Length == 1) return str;

            var sb = new StringBuilder(str.Length);
            if (str[0] >= 97 && str[0] <= 122) sb.Append((char)(str[0] - 32));
            else sb.Append(str[0]);
            for (var i = 1; i < str.Length; ++i)
            {
                if (str[i] >= 65 && str[i] <= 96)
                    sb.Append(" ");
                sb.Append(str[i]);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Removes all the blank spaces in string
        /// </summary>
        public static string RemoveBlankSpaces(string str)
        {
            var sb = new StringBuilder(str.Length);
            foreach (var c in str)
                if (c != ' ') sb.Append(c);

            return sb.ToString();
        }

        /// <summary>
        /// Gets file name by it's full path
        /// </summary>
        public static string GetFileNameByPath(string path)
        {
            var start = path.LastIndexOf("\\");
            var end = path.LastIndexOf(".");
            if (start + 1 > end) return "";
            return path.Substring(start + 1, end - start - 1);
        }

        /// <summary>
        /// Gets fold which contains the specified file path.
        /// </summary>
        public static string GetFoldByPath(string path)
        {
            var end = path.LastIndexOf("\\");
            if (end == 0) return "";
            return path.Substring(0, end);
        }
    }
}
