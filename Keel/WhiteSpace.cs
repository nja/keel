using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Keel
{
    public static class CharExtensions
    {
        public static bool IsWhiteSpace(this char ch)
        {
            const string whitespace = " \t\r\n";

            return whitespace.IndexOf(ch) > -1;
        }
    }
}
