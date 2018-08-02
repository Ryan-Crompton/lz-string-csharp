using System;
using System.Collections.Generic;
using System.Text;

namespace LZString
{
    public partial class LzString
    {
        public static string CompressToBase64(string input)
        {
            if (input == null) return "";
            var res = _compress(input, 6, (a) => KeyStrBase64[a]);
            switch (res.Length % 4)
            {
                case 0: return res;
                case 1: return res + "===";
                case 2: return res + "==";
                case 3: return res + "=";
                default:
                    return null;
            }
        }

        public static string CompressToUtf16(string input)
        {
            if (input == null) return "";
            return _compress(input, 15, a => F(a + 32)) + " ";
        }


        public static byte[] CompressToUint8Array(string uncompressed)
        {
            var compressed = Compress(uncompressed);
            var buf = new byte[compressed.Length * 2];

            for (int i = 0, totalLen = compressed.Length; i < totalLen; i++)
            {
                var currentValue = Convert.ToInt32(compressed[i]);
                buf[i * 2] = (byte) ((uint) currentValue >> 8);
                buf[i * 2 + 1] = (byte) (currentValue % 256);
            }

            return buf;
        }

        public static string CompressToEncodedUriComponent(string input)
        {
            return input == null ? "" : _compress(input, 6, (a) => KeyStrUriSafe[a]);
        }

        public static string Compress(string uncompressed)
        {
            return _compress(uncompressed, 16, F);
        }

        private static string _compress(string uncompressed, int bitsPerChar, GetCharFromInt getCharFromInt)
        {
            if (uncompressed == null) return "";
            int i,
                value,
                ii,
                contextEnlargeIn = 2,
                contextDictSize = 3,
                contextNumBits = 2,
                contextDataVal = 0,
                contextDataPosition = 0;
            var contextDictionaryToCreate = new Dictionary<string, bool>();
            var contextDictionary = new Dictionary<string, int>();
            var contextData = new StringBuilder();
            var contextW = "";

            for (ii = 0; ii < uncompressed.Length; ii++)
            {
                var contextC = uncompressed[ii].ToString();
                if (!contextDictionary.ContainsKey(contextC))
                {
                    contextDictionary[contextC] = contextDictSize++;
                    contextDictionaryToCreate[contextC] = true;
                }

                var contextWc = contextW + contextC;
                if (contextDictionary.ContainsKey(contextWc))
                {
                    contextW = contextWc;
                }
                else
                {
                    if (contextDictionaryToCreate.ContainsKey(contextW))
                    {
                        if (Convert.ToInt32(contextW[0]) < 256)
                        {
                            for (i = 0; i < contextNumBits; i++)
                            {
                                contextDataVal = contextDataVal << 1;
                                if (contextDataPosition == bitsPerChar - 1)
                                {
                                    contextDataPosition = 0;
                                    contextData.Append(getCharFromInt(contextDataVal));
                                    contextDataVal = 0;
                                }
                                else
                                {
                                    contextDataPosition++;
                                }
                            }

                            value = Convert.ToInt32(contextW[0]);
                            for (i = 0; i < 8; i++)
                            {
                                contextDataVal = (contextDataVal << 1) | (value & 1);
                                if (contextDataPosition == bitsPerChar - 1)
                                {
                                    contextDataPosition = 0;
                                    contextData.Append(getCharFromInt(contextDataVal));
                                    contextDataVal = 0;
                                }
                                else
                                {
                                    contextDataPosition++;
                                }

                                value = value >> 1;
                            }
                        }
                        else
                        {
                            value = 1;
                            for (i = 0; i < contextNumBits; i++)
                            {
                                contextDataVal = (contextDataVal << 1) | value;
                                if (contextDataPosition == bitsPerChar - 1)
                                {
                                    contextDataPosition = 0;
                                    contextData.Append(getCharFromInt(contextDataVal));
                                    contextDataVal = 0;
                                }
                                else
                                {
                                    contextDataPosition++;
                                }

                                value = 0;
                            }

                            value = Convert.ToInt32(contextW[0]);
                            for (i = 0; i < 16; i++)
                            {
                                contextDataVal = (contextDataVal << 1) | (value & 1);
                                if (contextDataPosition == bitsPerChar - 1)
                                {
                                    contextDataPosition = 0;
                                    contextData.Append(getCharFromInt(contextDataVal));
                                    contextDataVal = 0;
                                }
                                else
                                {
                                    contextDataPosition++;
                                }

                                value = value >> 1;
                            }
                        }

                        contextEnlargeIn--;
                        if (contextEnlargeIn == 0)
                        {
                            contextEnlargeIn = (int) Math.Pow(2, contextNumBits);
                            contextNumBits++;
                        }

                        contextDictionaryToCreate.Remove(contextW);
                    }
                    else
                    {
                        value = contextDictionary[contextW];
                        for (i = 0; i < contextNumBits; i++)
                        {
                            contextDataVal = (contextDataVal << 1) | (value & 1);
                            if (contextDataPosition == bitsPerChar - 1)
                            {
                                contextDataPosition = 0;
                                contextData.Append(getCharFromInt(contextDataVal));
                                contextDataVal = 0;
                            }
                            else
                            {
                                contextDataPosition++;
                            }

                            value = value >> 1;
                        }
                    }

                    contextEnlargeIn--;
                    if (contextEnlargeIn == 0)
                    {
                        contextEnlargeIn = (int) Math.Pow(2, contextNumBits);
                        contextNumBits++;
                    }

                    //Add wc to the dictionary
                    contextDictionary[contextWc] = contextDictSize++;
                    contextW = contextC;
                }
            }

            //Output the code for w
            if (contextW != "")
            {
                if (contextDictionaryToCreate.ContainsKey(contextW))
                {
                    if (Convert.ToInt32(contextW[0]) < 256)
                    {
                        for (i = 0; i < contextNumBits; i++)
                        {
                            contextDataVal = contextDataVal << 1;
                            if (contextDataPosition == bitsPerChar - 1)
                            {
                                contextDataPosition = 0;
                                contextData.Append(getCharFromInt(contextDataVal));
                                contextDataVal = 0;
                            }
                            else
                            {
                                contextDataPosition++;
                            }
                        }

                        value = Convert.ToInt32(contextW[0]);
                        for (i = 0; i < 8; i++)
                        {
                            contextDataVal = (contextDataVal << 1) | (value & 1);
                            if (contextDataPosition == bitsPerChar - 1)
                            {
                                contextDataPosition = 0;
                                contextData.Append(getCharFromInt(contextDataVal));
                                contextDataVal = 0;
                            }
                            else
                            {
                                contextDataPosition++;
                            }

                            value = value >> 1;
                        }
                    }
                    else
                    {
                        value = 1;
                        for (i = 0; i < contextNumBits; i++)
                        {
                            contextDataVal = (contextDataVal << 1) | value;
                            if (contextDataPosition == bitsPerChar - 1)
                            {
                                contextDataPosition = 0;
                                contextData.Append(getCharFromInt(contextDataVal));
                                contextDataVal = 0;
                            }
                            else
                            {
                                contextDataPosition++;
                            }

                            value = 0;
                        }

                        value = Convert.ToInt32(contextW[0]);
                        for (i = 0; i < 16; i++)
                        {
                            contextDataVal = (contextDataVal << 1) | (value & 1);
                            if (contextDataPosition == bitsPerChar - 1)
                            {
                                contextDataPosition = 0;
                                contextData.Append(getCharFromInt(contextDataVal));
                                contextDataVal = 0;
                            }
                            else
                            {
                                contextDataPosition++;
                            }

                            value = value >> 1;
                        }
                    }

                    contextEnlargeIn--;
                    if (contextEnlargeIn == 0)
                    {
                        contextEnlargeIn = (int) Math.Pow(2, contextNumBits);
                        contextNumBits++;
                    }

                    contextDictionaryToCreate.Remove(contextW);
                }
                else
                {
                    value = contextDictionary[contextW];
                    for (i = 0; i < contextNumBits; i++)
                    {
                        contextDataVal = (contextDataVal << 1) | (value & 1);
                        if (contextDataPosition == bitsPerChar - 1)
                        {
                            contextDataPosition = 0;
                            contextData.Append(getCharFromInt(contextDataVal));
                            contextDataVal = 0;
                        }
                        else
                        {
                            contextDataPosition++;
                        }

                        value = value >> 1;
                    }
                }

                contextEnlargeIn--;
                if (contextEnlargeIn == 0)
                {
                    contextEnlargeIn = (int) Math.Pow(2, contextNumBits);
                    contextNumBits++;
                }
            }

            //Mark the end of the stream
            value = 2;
            for (i = 0; i < contextNumBits; i++)
            {
                contextDataVal = (contextDataVal << 1) | (value & 1);
                if (contextDataPosition == bitsPerChar - 1)
                {
                    contextDataPosition = 0;
                    contextData.Append(getCharFromInt(contextDataVal));
                    contextDataVal = 0;
                }
                else
                {
                    contextDataPosition++;
                }

                value = value >> 1;
            }

            //Flush the last char
            while (true)
            {
                contextDataVal = contextDataVal << 1;
                if (contextDataPosition == bitsPerChar - 1)
                {
                    contextData.Append(getCharFromInt(contextDataVal));
                    break;
                }

                contextDataPosition++;
            }

            return contextData.ToString();
        }
    }
}
