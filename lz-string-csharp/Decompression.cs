using System;
using System.Collections.Generic;
using System.Text;

namespace LZString
{
    public partial class LzString
    {

        private static int GetBaseValue(string alphabet, char character)
        {
            if (BaseReverseDic.ContainsKey(alphabet))
            {
                return BaseReverseDic[alphabet][character];
            }

            BaseReverseDic[alphabet] = new Dictionary<char, int>();
            for (var i = 0; i < alphabet.Length; i++)
            {
                BaseReverseDic[alphabet][alphabet[i]] = i;
            }
            return BaseReverseDic[alphabet][character];
        }

        public static string DecompressFromBase64(string input)
        {
            if (input == null) return "";
            return input == "" ? null : _decompress(input.Length, 32, (index) => GetBaseValue(KeyStrBase64, input[index]));
        }

        public static string DecompressFromUtf16(string compressed)
        {
            if (compressed == null) return "";
            return compressed == "" ? null : _decompress(compressed.Length, 16384, index => Convert.ToInt32(compressed[index]) - 32);
        }

        public static string DecompressFromUint8Array(byte[] compressed)
        {
            if (compressed == null) return "";

            var buf = new int[compressed.Length / 2];
            for (int i = 0, totalLen = buf.Length; i < totalLen; i++)
            {
                buf[i] = (int)compressed[i * 2] * 256 + (int)compressed[i * 2 + 1];
            }
            var result = new char[buf.Length];
            for (var i = 0; i < buf.Length; i++)
            {
                result[i] = F(buf[i]);
            }
            return Decompress(new string(result));
        }

        public static string DecompressFromEncodedUriComponent(string input)
        {
            if (input == null) return "";
            if (input == "") return null;
            input = input.Replace(' ', '+');
            return _decompress(input.Length, 32, (index) => GetBaseValue(KeyStrUriSafe, input[index]));
        }

        public static string Decompress(string compressed)
        {
            if (compressed == null) return "";
            return compressed == "" ? null : _decompress(compressed.Length, 32768, (index) => Convert.ToInt32(compressed[index]));
        }

        private static string _decompress(int length, int resetValue, GetNextValue getNextValue)
        {
            var dictionary = new Dictionary<int, string>();
            int next, enlargeIn = 4, dictSize = 4, numBits = 3, i, resb;
            var c = 0;
            var result = new StringBuilder();
            var data = new DataStruct() { Val = getNextValue(0), Position = resetValue, Index = 1 };

            for (i = 0; i < 3; i++)
            {
                dictionary[i] = Convert.ToChar(i).ToString();
            }

            var bits = 0;
            var maxpower = (int)Math.Pow(2, 2);
            var power = 1;
            while (power != maxpower)
            {
                resb = data.Val & data.Position;
                data.Position >>= 1;
                if (data.Position == 0)
                {
                    data.Position = resetValue;
                    data.Val = getNextValue(data.Index++);
                }
                bits |= (resb > 0 ? 1 : 0) * power;
                power <<= 1;
            }

            switch (next = bits)
            {
                case 0:
                    bits = 0;
                    maxpower = (int)Math.Pow(2, 8);
                    power = 1;
                    while (power != maxpower)
                    {
                        resb = data.Val & data.Position;
                        data.Position >>= 1;
                        if (data.Position == 0)
                        {
                            data.Position = resetValue;
                            data.Val = getNextValue(data.Index++);
                        }
                        bits |= (resb > 0 ? 1 : 0) * power;
                        power <<= 1;
                    }
                    c = Convert.ToInt32(F(bits));
                    break;
                case 1:
                    bits = 0;
                    maxpower = (int)Math.Pow(2, 16);
                    power = 1;
                    while (power != maxpower)
                    {
                        resb = data.Val & data.Position;
                        data.Position >>= 1;
                        if (data.Position == 0)
                        {
                            data.Position = resetValue;
                            data.Val = getNextValue(data.Index++);
                        }
                        bits |= (resb > 0 ? 1 : 0) * power;
                        power <<= 1;
                    }
                    c = Convert.ToInt32(F(bits));
                    break;
                case 2:
                    return "";
            }
            dictionary[3] = Convert.ToChar(c).ToString();
            var w = Convert.ToChar(c).ToString();
            result.Append(Convert.ToChar(c));
            while (true)
            {
                if (data.Index > length)
                {
                    return "";
                }

                bits = 0;
                maxpower = (int)Math.Pow(2, numBits);
                power = 1;
                while (power != maxpower)
                {
                    resb = data.Val & data.Position;
                    data.Position >>= 1;
                    if (data.Position == 0)
                    {
                        data.Position = resetValue;
                        data.Val = getNextValue(data.Index++);
                    }
                    bits |= (resb > 0 ? 1 : 0) * power;
                    power <<= 1;
                }

                switch (c = bits)
                {
                    case 0:
                        bits = 0;
                        maxpower = (int)Math.Pow(2, 8);
                        power = 1;
                        while (power != maxpower)
                        {
                            resb = data.Val & data.Position;
                            data.Position >>= 1;
                            if (data.Position == 0)
                            {
                                data.Position = resetValue;
                                data.Val = getNextValue(data.Index++);
                            }
                            bits |= (resb > 0 ? 1 : 0) * power;
                            power <<= 1;
                        }

                        dictionary[dictSize++] = F(bits).ToString();
                        c = dictSize - 1;
                        enlargeIn--;
                        break;
                    case 1:
                        bits = 0;
                        maxpower = (int)Math.Pow(2, 16);
                        power = 1;
                        while (power != maxpower)
                        {
                            resb = data.Val & data.Position;
                            data.Position >>= 1;
                            if (data.Position == 0)
                            {
                                data.Position = resetValue;
                                data.Val = getNextValue(data.Index++);
                            }
                            bits |= (resb > 0 ? 1 : 0) * power;
                            power <<= 1;
                        }
                        dictionary[dictSize++] = F(bits).ToString();
                        c = dictSize - 1;
                        enlargeIn--;
                        break;
                    case 2:
                        return result.ToString();
                }

                if (enlargeIn == 0)
                {
                    enlargeIn = (int)Math.Pow(2, numBits);
                    numBits++;
                }

                var entry = "";

                if (dictionary.ContainsKey(c))
                {
                    entry = dictionary[c];
                }
                else
                {
                    if (c == dictSize)
                    {
                        entry = w + w[0];
                    }
                    else
                    {
                        return null;
                    }
                }
                result.Append(entry);

                //Add w+entry[0] to the dictionary.
                dictionary[dictSize++] = w + entry[0];
                enlargeIn--;
                w = entry;

                if (enlargeIn != 0) continue;

                enlargeIn = (int)Math.Pow(2, numBits);
                numBits++;
            }
        }

        private struct DataStruct
        {
            public int Val, Position, Index;
        }
    }
}
