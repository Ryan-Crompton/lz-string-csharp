using System;
using System.Collections.Generic;
using System.Text;

namespace LZString
{
    public partial class LZString
    {
        public static string compressToBase64(string input)
        {
            if (input == null) return "";
            string res = _compress(input, 6, (a) => keyStrBase64[a]);
            switch (res.Length%4)
            {
                case 0: return res;
                case 1: return res + "===";
                case 2: return res + "==";
                case 3: return res + "=";
            }
            return null;
        }

        public static string compressToUTF16(string input)
        {
            if (input == null) return "";
            return _compress(input, 15, (a) => f(a + 32)) + " ";
        }


        public static byte[] compressToUint8Array(string uncompressed)
        {
            string compressed = compress(uncompressed);
            byte[] buf = new byte[compressed.Length * 2];

            for (int i = 0, TotalLen = compressed.Length; i < TotalLen; i++)
            {
                int current_value = Convert.ToInt32(compressed[i]);
                buf[i * 2] = (byte)(((uint)current_value) >> 8);
                buf[i * 2 + 1] = (byte)(current_value % 256);
            }
            return buf;
        }

        public static string compressToEncodedURIComponent(string input)
        {
            if (input == null) return "";
            return _compress(input, 6, (a) => keyStrUriSafe[a]);
        }

        public static string compress(string uncompressed)
        {
            return _compress(uncompressed, 16, f);
        }

        private static string _compress(string uncompressed, int bitsPerChar, GetCharFromInt getCharFromInt)
        {
            if (uncompressed == null) return "";
            int i, value, ii, context_enlargeIn = 2, context_dictSize = 3, context_numBits = 2, context_data_val = 0, context_data_position = 0;
            Dictionary<string, bool> context_dictionaryToCreate = new Dictionary<string, bool>();
            Dictionary<string, int> context_dictionary = new Dictionary<string, int>();
            StringBuilder context_data = new StringBuilder();
            string context_c ="";
            string context_wc = "", context_w = "";

            for(ii=0;ii<uncompressed.Length;ii++)
            {
                context_c = uncompressed[ii].ToString();
                if(!context_dictionary.ContainsKey(context_c))
                {
                    context_dictionary[context_c] = context_dictSize++;
                    context_dictionaryToCreate[context_c] = true;
                }
                context_wc = context_w + context_c;
                if(context_dictionary.ContainsKey(context_wc))
                {
                    context_w = context_wc;
                }
                else
                {
                    if(context_dictionaryToCreate.ContainsKey(context_w))
                    {
                        if(Convert.ToInt32(context_w[0])<256)
                        {
                            for(i=0;i<context_numBits;i++)
                            {
                                context_data_val = (context_data_val << 1);
                                if(context_data_position==bitsPerChar-1)
                                {
                                    context_data_position = 0;
                                    context_data.Append(getCharFromInt(context_data_val));
                                    context_data_val = 0;
                                }
                                else
                                {
                                    context_data_position++;
                                }
                            }
                            value = Convert.ToInt32(context_w[0]);
                            for(i=0;i<8;i++)
                            {
                                context_data_val = (context_data_val << 1) | (value & 1);
                                if(context_data_position==bitsPerChar-1)
                                {
                                    context_data_position = 0;
                                    context_data.Append(getCharFromInt(context_data_val));
                                    context_data_val = 0;
                                }
                                else
                                {
                                    context_data_position++;
                                }
                                value = value >> 1;
                            }
                        }
                        else
                        {
                            value = 1;
                            for(i=0;i<context_numBits;i++)
                            {
                                context_data_val = (context_data_val << 1) | value;
                                if(context_data_position==bitsPerChar-1)
                                {
                                    context_data_position = 0;
                                    context_data.Append(getCharFromInt(context_data_val));
                                    context_data_val = 0;
                                }
                                else
                                {
                                    context_data_position++;
                                }
                                value = 0;
                            }
                            value = Convert.ToInt32(context_w[0]);
                            for(i=0;i<16;i++)
                            {
                                context_data_val = (context_data_val << 1) | (value & 1);
                                if(context_data_position==bitsPerChar-1)
                                {
                                    context_data_position = 0;
                                    context_data.Append(getCharFromInt(context_data_val));
                                    context_data_val = 0;
                                }
                                else
                                {
                                    context_data_position++;
                                }
                                value = value >> 1;
                            }
                        }
                        context_enlargeIn--;
                        if(context_enlargeIn==0)
                        {
                            context_enlargeIn = (int)Math.Pow(2, context_numBits);
                            context_numBits++;
                        }
                        context_dictionaryToCreate.Remove(context_w);
                    }
                    else
                    {
                        value = context_dictionary[context_w];
                        for(i=0;i<context_numBits;i++)
                        {
                            context_data_val = (context_data_val << 1) | (value & 1);
                            if(context_data_position==bitsPerChar-1)
                            {
                                context_data_position = 0;
                                context_data.Append(getCharFromInt(context_data_val));
                                context_data_val = 0;
                            }
                            else
                            {
                                context_data_position++;
                            }
                            value = value >> 1;
                        }
                    }
                    context_enlargeIn--;
                    if(context_enlargeIn==0)
                    {
                        context_enlargeIn = (int)Math.Pow(2, context_numBits);
                        context_numBits++;
                    }
                    //Add wc to the dictionary
                    context_dictionary[context_wc] = context_dictSize++;
                    context_w = context_c;
                }
            }
            //Output the code for w
            if(context_w!="")
            {
                if(context_dictionaryToCreate.ContainsKey(context_w))
                {
                    if(Convert.ToInt32(context_w[0])<256)
                    {
                        for(i=0;i<context_numBits;i++)
                        {
                            context_data_val = (context_data_val << 1);
                            if(context_data_position==bitsPerChar-1)
                            {
                                context_data_position = 0;
                                context_data.Append(getCharFromInt(context_data_val));
                                context_data_val = 0;
                            }
                            else
                            {
                                context_data_position++;
                            }
                        }
                        value = Convert.ToInt32(context_w[0]);
                        for(i=0;i<8;i++)
                        {
                            context_data_val = (context_data_val << 1) | (value & 1);
                            if(context_data_position==bitsPerChar-1)
                            {
                                context_data_position = 0;
                                context_data.Append(getCharFromInt(context_data_val));
                                context_data_val = 0;
                            }
                            else
                            {
                                context_data_position++;
                            }
                            value = value >> 1;
                        }
                    }
                    else
                    {
                        value = 1;
                        for(i=0;i<context_numBits;i++)
                        {
                            context_data_val = (context_data_val << 1) | value;
                            if(context_data_position==bitsPerChar-1)
                            {
                                context_data_position = 0;
                                context_data.Append(getCharFromInt(context_data_val));
                                context_data_val = 0;
                            }
                            else
                            {
                                context_data_position++;
                            }
                            value = 0;
                        }
                        value = Convert.ToInt32(context_w[0]);
                        for(i=0;i<16;i++)
                        {
                            context_data_val = (context_data_val << 1) | (value & 1);
                            if(context_data_position==bitsPerChar-1)
                            {
                                context_data_position = 0;
                                context_data.Append(getCharFromInt(context_data_val));
                                context_data_val = 0;
                            }
                            else
                            {
                                context_data_position++;
                            }
                            value = value >> 1;
                        }
                    }
                    context_enlargeIn--;
                    if(context_enlargeIn==0)
                    {
                        context_enlargeIn = (int)Math.Pow(2, context_numBits);
                        context_numBits++;
                    }
                    context_dictionaryToCreate.Remove(context_w);
                }
                else
                {
                    value = context_dictionary[context_w];
                    for(i=0;i<context_numBits;i++)
                    {
                        context_data_val = (context_data_val << 1) | (value & 1);
                        if(context_data_position==bitsPerChar-1)
                        {
                            context_data_position = 0;
                            context_data.Append(getCharFromInt(context_data_val));
                            context_data_val = 0;
                        }
                        else
                        {
                            context_data_position++;
                        }
                        value = value >> 1;
                    }
                }
                context_enlargeIn--;
                if(context_enlargeIn==0)
                {
                    context_enlargeIn = (int)Math.Pow(2, context_numBits);
                    context_numBits++;
                }
            }
            //Mark the end of the stream
            value = 2;
            for(i=0;i<context_numBits;i++)
            {
                context_data_val = (context_data_val << 1) | (value & 1);
                if(context_data_position==bitsPerChar-1)
                {
                    context_data_position = 0;
                    context_data.Append(getCharFromInt(context_data_val));
                    context_data_val = 0;
                }
                else
                {
                    context_data_position++;
                }
                value = value >> 1;
            }

            //Flush the last char
            while(true)
            {
                context_data_val = (context_data_val << 1);
                if (context_data_position == bitsPerChar - 1)
                {
                    context_data.Append(getCharFromInt(context_data_val));
                    break;
                }
                else context_data_position++;
            }
            return context_data.ToString();
        }

        private struct dataStruct
        {
            public int val, position, index;
        }        
}
