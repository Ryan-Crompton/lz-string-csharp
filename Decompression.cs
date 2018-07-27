using System;
using System.Collections.Generic;
using System.Text;

namespace LZString {
    public partial class LZString {

        private static int getBaseValue (string alphabet, char character) {
            if (!baseReverseDic.ContainsKey (alphabet)) {
                baseReverseDic[alphabet] = new Dictionary<char, int> ();
                for (int i = 0; i < alphabet.Length; i++) {
                    baseReverseDic[alphabet][alphabet[i]] = i;
                }
            }
            return baseReverseDic[alphabet][character];
        }

        public static string decompressFromBase64 (string input) {
            if (input == null) return "";
            if (input == "") return null;
            return _decompress (input.Length, 32, (index) => getBaseValue (keyStrBase64, input[index]));
        }

        public static string decompressFromUTF16 (string compressed) {
            if (compressed == null) return "";
            if (compressed == "") return null;
            return _decompress (compressed.Length, 16384, index => Convert.ToInt32 (compressed[index]) - 32);
        }

        public static string decompressFromUint8Array (byte[] compressed) {
            if (compressed == null) return "";
            else {
                int[] buf = new int[compressed.Length / 2];
                for (int i = 0, TotalLen = buf.Length; i < TotalLen; i++) {
                    buf[i] = ((int) compressed[i * 2]) * 256 + ((int) compressed[i * 2 + 1]);
                }
                char[] result = new char[buf.Length];
                for (int i = 0; i < buf.Length; i++) {
                    result[i] = f (buf[i]);
                }
                return decompress (new string (result));
            }
        }

        public static string decompressFromEncodedURIComponent (string input) {
            if (input == null) return "";
            if (input == "") return null;
            input = input.Replace (' ', '+');
            return _decompress (input.Length, 32, (index) => getBaseValue (keyStrUriSafe, input[index]));
        }

        public static string decompress (string compressed) {
            if (compressed == null) return "";
            if (compressed == "") return null;
            return _decompress (compressed.Length, 32768, (index) => Convert.ToInt32 (compressed[index]));
        }

        private static string _decompress (int length, int resetValue, GetNextValue getNextValue) {
            Dictionary<int, string> dictionary = new Dictionary<int, string> ();
            int next, enlargeIn = 4, dictSize = 4, numBits = 3, i, bits, resb, maxpower, power;
            int c = 0;
            string entry = "", w;
            StringBuilder result = new StringBuilder ();
            var data = new dataStruct () { val = getNextValue (0), position = resetValue, index = 1 };

            for (i = 0; i < 3; i++) {
                dictionary[i] = Convert.ToChar (i).ToString ();
            }

            bits = 0;
            maxpower = (int) Math.Pow (2, 2);
            power = 1;
            while (power != maxpower) {
                resb = data.val & data.position;
                data.position >>= 1;
                if (data.position == 0) {
                    data.position = resetValue;
                    data.val = getNextValue (data.index++);
                }
                bits |= (resb > 0 ? 1 : 0) * power;
                power <<= 1;
            }

            switch (next = bits) {
                case 0:
                    bits = 0;
                    maxpower = (int) Math.Pow (2, 8);
                    power = 1;
                    while (power != maxpower) {
                        resb = data.val & data.position;
                        data.position >>= 1;
                        if (data.position == 0) {
                            data.position = resetValue;
                            data.val = getNextValue (data.index++);
                        }
                        bits |= (resb > 0 ? 1 : 0) * power;
                        power <<= 1;
                    }
                    c = Convert.ToInt32 (f (bits));
                    break;
                case 1:
                    bits = 0;
                    maxpower = (int) Math.Pow (2, 16);
                    power = 1;
                    while (power != maxpower) {
                        resb = data.val & data.position;
                        data.position >>= 1;
                        if (data.position == 0) {
                            data.position = resetValue;
                            data.val = getNextValue (data.index++);
                        }
                        bits |= (resb > 0 ? 1 : 0) * power;
                        power <<= 1;
                    }
                    c = Convert.ToInt32 (f (bits));
                    break;
                case 2:
                    return "";
            }
            dictionary[3] = Convert.ToChar (c).ToString ();
            w = Convert.ToChar (c).ToString ();
            result.Append (Convert.ToChar (c));
            while (true) {
                if (data.index > length) {
                    return "";
                }

                bits = 0;
                maxpower = (int) Math.Pow (2, numBits);
                power = 1;
                while (power != maxpower) {
                    resb = data.val & data.position;
                    data.position >>= 1;
                    if (data.position == 0) {
                        data.position = resetValue;
                        data.val = getNextValue (data.index++);
                    }
                    bits |= (resb > 0 ? 1 : 0) * power;
                    power <<= 1;
                }

                switch (c = bits) {
                    case 0:
                        bits = 0;
                        maxpower = (int) Math.Pow (2, 8);
                        power = 1;
                        while (power != maxpower) {
                            resb = data.val & data.position;
                            data.position >>= 1;
                            if (data.position == 0) {
                                data.position = resetValue;
                                data.val = getNextValue (data.index++);
                            }
                            bits |= (resb > 0 ? 1 : 0) * power;
                            power <<= 1;
                        }

                        dictionary[dictSize++] = f (bits).ToString ();
                        c = dictSize - 1;
                        enlargeIn--;
                        break;
                    case 1:
                        bits = 0;
                        maxpower = (int) Math.Pow (2, 16);
                        power = 1;
                        while (power != maxpower) {
                            resb = data.val & data.position;
                            data.position >>= 1;
                            if (data.position == 0) {
                                data.position = resetValue;
                                data.val = getNextValue (data.index++);
                            }
                            bits |= (resb > 0 ? 1 : 0) * power;
                            power <<= 1;
                        }
                        dictionary[dictSize++] = f (bits).ToString ();
                        c = dictSize - 1;
                        enlargeIn--;
                        break;
                    case 2:
                        return result.ToString ();
                }

                if (enlargeIn == 0) {
                    enlargeIn = (int) Math.Pow (2, numBits);
                    numBits++;
                }

                if (dictionary.ContainsKey (c)) {
                    entry = dictionary[c];
                } else {
                    if (c == dictSize) {
                        entry = w + w[0].ToString ();
                    } else {
                        return null;
                    }
                }
                result.Append (entry);

                //Add w+entry[0] to the dictionary.
                dictionary[dictSize++] = w + entry[0].ToString ();
                enlargeIn--;
                w = entry;
                if (enlargeIn == 0) {
                    enlargeIn = (int) Math.Pow (2, numBits);
                    numBits++;
                }
            }
        }
    }
}
