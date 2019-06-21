using System;
using System.Collections.Generic;
using System.Text;

namespace Biblifun.Common.Util
{
    public static class Strings
    {
        public static string ReplaceNonBreakingSpaces(this string s)
        {
            return s.Replace(Convert.ToChar(160), ' ');
        }
    }
}
