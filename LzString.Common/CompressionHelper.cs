using System.Collections.Generic;
using System.Linq;

namespace LzString.Common
{
    public static class CompressionHelper
    {
        public static IEnumerable<KeyValuePair<string, string>> CompressToDict(this IEnumerable<string> strings) =>
            strings.Select(str => new KeyValuePair<string, string>(str, LZString.LzString.Compress(str)));

        public static IEnumerable<KeyValuePair<string, string>> CompressToBase64Dict(this IEnumerable<string> strings) =>
            strings.Select(str => new KeyValuePair<string, string>(str, LZString.LzString.CompressToBase64(str)));

        public static IEnumerable<KeyValuePair<string, string>> CompressToUtf16Dict(this IEnumerable<string> strings) =>
            strings.Select(str => new KeyValuePair<string, string>(str, LZString.LzString.CompressToUtf16(str)));

        public static IEnumerable<KeyValuePair<string, byte[]>> CompressToUint8ArrayDict(this IEnumerable<string> strings) =>
            strings.Select(str => new KeyValuePair<string, byte[]>(str, LZString.LzString.CompressToUint8Array(str)));

        public static IEnumerable<KeyValuePair<string, string>> CompressToEncodedUriComponentDict(this IEnumerable<string> strings) =>
            strings.Select(str => new KeyValuePair<string, string>(str, LZString.LzString.CompressToEncodedUriComponent(str)));
    }
}