using System;
using System.Linq;

namespace LzString.Common
{
    public static class Random
    {
        public static char[] Chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
        public static char[] AnyChars = Enumerable.Range(char.MinValue, char.MaxValue).Select(val => (char)val).ToArray();

        public static string String(uint length, bool complex = false) => complex
            ? new string(AnyChars.OrderBy(_ => Guid.NewGuid()).Take((int) length).ToArray())
            : new string(Chars.OrderBy(_ => Guid.NewGuid()).Take((int) length).ToArray());

        public static string[] StringsComplex(uint maxStringLength, uint maxRows) =>
            Enumerable.Range(0, (int)maxRows).Select(_ => String(maxStringLength, true)).ToArray();

        public static string[] Strings(uint maxStringLength, uint maxRows) =>
            Enumerable.Range(0, (int)maxRows).Select(_ => String(maxStringLength)).ToArray();
        
    }
}