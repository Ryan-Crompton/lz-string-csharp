using System;
using System.Collections.Generic;
using System.Text;

namespace LZString
{
    public partial class LZString
    {
        static string keyStrBase64 = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";
        static string keyStrUriSafe = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+-$";
        static Dictionary<string, Dictionary<char, int>> baseReverseDic = new Dictionary<string, Dictionary<char, int>>();
        private delegate char GetCharFromInt(int a);
        private static GetCharFromInt f = (a) => Convert.ToChar(a);
        private delegate int GetNextValue(int index);
    }
}