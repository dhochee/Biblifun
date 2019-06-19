using System.Text.RegularExpressions;

namespace Biblifun.Common.Util
{
    public static class Numbers
    {
        /// <summary>
        /// Given a string, extracts the first number found. Includes whole or fractional numbers, 
        /// but converts negative numbers to positive as the "-" sign is ignored.
        /// </summary>
        public static string GetNumberFromStr(string str)
        {
            str = str.Trim();

            return Regex.Match(str, @"[\d\.]+").Value;
        }
    }
}
