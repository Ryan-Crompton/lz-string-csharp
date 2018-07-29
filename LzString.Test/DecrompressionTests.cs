using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using static LZString.LzString;

namespace LzString.Test
{
    [TestClass]
    public class DecompressionTests
    {
        public IEnumerable<KeyValuePair<string, string>> EncodedUriComponentCompressionDict;

        public DecompressionTests()
        {
            using (var fs = File.Open($"c:\\test\\compressEncodedUriComponent.json", FileMode.Open))
            using (var reader = new StreamReader(fs, Encoding.Unicode))
                EncodedUriComponentCompressionDict = JsonConvert.DeserializeObject<List<KeyValuePair<string, string>>>(reader.ReadToEnd());
        }

        [TestMethod]
        public void ShouldDecompress(string value)
        {
            var compressed = Compress(value);
            var output = Decompress(compressed);
            Assert.AreEqual(value, output);
        }

        [TestMethod]
        public void ShouldDecompressFromBase64(string value)
        {
            var compressed = CompressToBase64(value);
            var output = DecompressFromBase64(compressed);
            Assert.AreEqual(value, output);
        }


        [TestMethod]
        public void ShouldDecompressFromUtf16(string value)
        {
            var compressed = CompressToUtf16(value);
            var output = DecompressFromUtf16(compressed);
            Assert.AreEqual(value, output);
        }


        [TestMethod]
        public void ShouldDecompressFromUint8Array(string value)
        {
            var compressed = CompressToUint8Array(value);
            var output = DecompressFromUint8Array(compressed);
            Assert.AreEqual(value, output);
        }


        [TestMethod]
        public void ShouldDecompressFromEncodedUriComponent()
        {
            foreach (var entry in EncodedUriComponentCompressionDict)
            {
                var output = DecompressFromEncodedUriComponent(entry.Value);
                Assert.AreEqual(entry.Key, output);
            }
        }
    }
}
