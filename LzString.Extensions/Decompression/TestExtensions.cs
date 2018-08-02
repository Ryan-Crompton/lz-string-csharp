using System.Collections.Generic;
using System.Linq;

namespace LzString.Extensions.Decompression
{
    public static class TestExtensions
    {
        public static IEnumerable<KeyValuePair<string, string>> DecompressFromKvps(this IEnumerable<KeyValuePair<string, string>> kvps) =>
            kvps.Select(kvp => new KeyValuePair<string, string>(kvp.Key, LZString.LzString.Decompress(kvp.Value)));

        public static IEnumerable<KeyValuePair<string, string>> DecompressFromBase64Kvps(this IEnumerable<KeyValuePair<string, string>> kvps) =>
            kvps.Select(kvp => new KeyValuePair<string, string>(kvp.Key, LZString.LzString.DecompressFromBase64(kvp.Value)));


        public static IEnumerable<KeyValuePair<string, string>> DecompressFromUtf16Kvps(this IEnumerable<KeyValuePair<string, string>> kvps) =>
            kvps.Select(kvp => new KeyValuePair<string, string>(kvp.Key, LZString.LzString.DecompressFromUtf16(kvp.Value)));

        public static IEnumerable<KeyValuePair<string, string>> DecompressFromUint8ArrayKvps(this IEnumerable<KeyValuePair<string, byte[]>> kvps) =>
            kvps.Select(kvp => new KeyValuePair<string, string>(kvp.Key, LZString.LzString.DecompressFromUint8Array(kvp.Value)));

        public static IEnumerable<KeyValuePair<string, string>> DecompressFromEncodedUriComponentKvps(this IEnumerable<KeyValuePair<string, string>> kvps) =>
            kvps.Select(kvp => new KeyValuePair<string, string>(kvp.Key, LZString.LzString.DecompressFromEncodedUriComponent(kvp.Value)));
    }
}