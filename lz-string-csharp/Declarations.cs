using System;
using System.Collections.Generic;

namespace LZString
{
    public partial class LzString
    {
        private static readonly string KeyStrBase64 = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";
        private static readonly string KeyStrUriSafe = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+-$";
        private static readonly Dictionary<string, Dictionary<char, int>> BaseReverseDic = new Dictionary<string, Dictionary<char, int>>();
        private delegate char GetCharFromInt(int a);
        private static readonly GetCharFromInt F = Convert.ToChar;
        private delegate int GetNextValue(int index);
    }
}