using System.Collections.Generic;
using System.Linq;
using static LZString.LzString;

namespace LzString.Common
{
    public static class DecompressionHelper
    {
        public static IEnumerable<KeyValuePair<string, string>> DecompressFromDict(IEnumerable<KeyValuePair<string, string>> kvps) =>
            kvps.Select(kvp => new KeyValuePair<string, string>(kvp.Key, Decompress(kvp.Value)));


        public static IEnumerable<KeyValuePair<string, string>> DecompressFromBase64Dict(IEnumerable<KeyValuePair<string, string>> kvps) =>
            kvps.Select(kvp => new KeyValuePair<string, string>(kvp.Key, DecompressFromBase64(kvp.Value)));


        public static IEnumerable<KeyValuePair<string, string>> DecompressFromUtf16Dict(IEnumerable<KeyValuePair<string, string>> kvps) =>
            kvps.Select(kvp => new KeyValuePair<string, string>(kvp.Key, DecompressFromUtf16(kvp.Value)));

        public static IEnumerable<KeyValuePair<string, string>> DecompressFromUint8ArrayDict(IEnumerable<KeyValuePair<string, byte[]>> kvps) =>
            kvps.Select(kvp => new KeyValuePair<string, string>(kvp.Key, DecompressFromUint8Array(kvp.Value)));

        public static IEnumerable<KeyValuePair<string, string>> DecompressFromEncodedUriComponentDict(IEnumerable<KeyValuePair<string, string>> kvps) =>
            kvps.Select(kvp => new KeyValuePair<string, string>(kvp.Key, DecompressFromEncodedUriComponent(kvp.Value)));
    }
}