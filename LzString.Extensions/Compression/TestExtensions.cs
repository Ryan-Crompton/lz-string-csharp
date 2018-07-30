using System.Collections.Generic;
using System.Linq;

namespace LzString.Extensions.Compression
{
    public static class TestExtensions
    {
        public static IEnumerable<KeyValuePair<string, string>> CompressToKvps(this IEnumerable<string> strings) =>
            strings.Select(str => new KeyValuePair<string, string>(str, LZString.LzString.Compress(str)));

        public static IEnumerable<KeyValuePair<string, string>> CompressToBase64Kvps(this IEnumerable<string> strings) =>
            strings.Select(str => new KeyValuePair<string, string>(str, LZString.LzString.CompressToBase64(str)));

        public static IEnumerable<KeyValuePair<string, string>> CompressToUtf16Kvps(this IEnumerable<string> strings) =>
            strings.Select(str => new KeyValuePair<string, string>(str, LZString.LzString.CompressToUtf16(str)));

        public static IEnumerable<KeyValuePair<string, byte[]>> CompressToUint8ArrayKvps(this IEnumerable<string> strings) =>
            strings.Select(str => new KeyValuePair<string, byte[]>(str, LZString.LzString.CompressToUint8Array(str)));

        public static IEnumerable<KeyValuePair<string, string>> CompressToEncodedUriComponentKvps(this IEnumerable<string> strings) =>
            strings.Select(str => new KeyValuePair<string, string>(str, LZString.LzString.CompressToEncodedUriComponent(str)));
    }
}