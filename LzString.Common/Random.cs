using System;
using System.Linq;

namespace LzString.Common
{
    public static class Random
    {
        public static char[] Chars = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
        /*Enumerable.Range(char.MinValue, char.MaxValue).Select(val => (char)val).ToArray()*/

        public static string String(uint length)
        {
            return new string(Chars.OrderBy(_ => Guid.NewGuid()).Take((int)length).ToArray());
        }

        public static string[] Strings(uint maxStringLength, uint maxRows)
        {
            return Enumerable.Range(0, (int)maxRows).Select(_ => String(maxStringLength)).ToArray();
        }
    }
}