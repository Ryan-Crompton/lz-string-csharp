using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LzString.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static LZString.LzString;
using Random = LzString.Common.Random;

namespace LzString.Test
{
    [TestClass]
    public class DecompressionTests
    {
        public IEnumerable<KeyValuePair<string, string>> EncodedUriComponentCompressionDict;
        public IEnumerable<string> Strings = Random.Strings(10000, 1000);
        public IEnumerable<string> ComplexStrings = Random.StringsComplex(10000, 1000);

        public DecompressionTests()
        {
        }

        [TestMethod]
        public void ShouldDecompress()
        {
            foreach (var value in Strings)
            {
                var compressed = Compress(value);
                var output = Decompress(compressed);
                Assert.AreEqual(value, output);
            }
        }

        [TestMethod]
        public void ShouldDecompressComplex()
        {
            foreach (var value in ComplexStrings)
            {
                var compressed = Compress(value);
                var output = Decompress(compressed);
                Assert.AreEqual(value, output);
            }
        }

        [TestMethod]
        public void ShouldDecompressFromBase64()
        {
            foreach (var value in Strings)
            {
                var compressed = CompressToBase64(value);
                var output = DecompressFromBase64(compressed);
                Assert.AreEqual(value, output);
            }
        }
        
        [TestMethod]
        public void ShouldDecompressFromBase64Complex()
        {
            foreach (var value in ComplexStrings)
            {
                var compressed = CompressToBase64(value);
                var output = DecompressFromBase64(compressed);
                Assert.AreEqual(value, output);
            }
        }

        [TestMethod]
        public void ShouldDecompressFromUtf16()
        {
            foreach (var value in Strings)
            {
                var compressed = CompressToUtf16(value);
                var output = DecompressFromUtf16(compressed);
                Assert.AreEqual(value, output);
            }
        }

        [TestMethod]
        public void ShouldDecompressFromUtf16Complex()
        {
            foreach (var value in ComplexStrings)
            {
                var compressed = CompressToUtf16(value);
                var output = DecompressFromUtf16(compressed);
                Assert.AreEqual(value, output);
            }
        }

        [TestMethod]
        public void ShouldDecompressFromUint8Array()
        {
            foreach (var value in Strings)
            {
                var compressed = CompressToUint8Array(value);
                var output = DecompressFromUint8Array(compressed);
                Assert.AreEqual(value, output);
            }
        }
        
        [TestMethod]
        public void ShouldDecompressFromUint8ArrayComplex()
        {
            foreach (var value in Strings)
            {
                var compressed = CompressToUint8Array(value);
                var output = DecompressFromUint8Array(compressed);
                Assert.AreEqual(value, output);
            }
        }

        [TestMethod]
        public void ShouldDecompressFromEncodedUriComponent()
        {
            foreach (var value in Strings)
            {
                var compressed = CompressToEncodedUriComponent(value);
                var output = DecompressFromEncodedUriComponent(compressed);
                Assert.AreEqual(value, output);
            }
        }


        [TestMethod]
        public void ShouldDecompressFromEncodedUriComponentComplex()
        {
            foreach (var value in ComplexStrings)
            {
                var compressed = CompressToEncodedUriComponent(value);
                var output = DecompressFromEncodedUriComponent(compressed);
                Assert.AreEqual(value, output);
            }
        }

        [TestMethod]
        public void ShouldDecompressFromEncodedUriComponentFromFile()
        {
            var tempFileName = Guid.NewGuid().ToString();
            FileHelper.GenerateJsonFile(Path.GetTempPath(), tempFileName, Strings.ToArray());
            EncodedUriComponentCompressionDict =
                FileHelper.GetJsonKeyValuePairs(Path.GetTempPath(), tempFileName, true);
            foreach (var entry in EncodedUriComponentCompressionDict)
            {
                var output = DecompressFromEncodedUriComponent(entry.Value);
                Assert.AreEqual(entry.Key, output);
            }
        }

        [TestMethod]
        public void ShouldDecompressFromEncodedUriComponentFromJavaScriptGeneratedFile()
        {
            var tempFileName = Guid.NewGuid().ToString();
            FileHelper.GenerateJsonFileFromJavaScript(Path.GetTempPath(), tempFileName, Strings.ToArray());
            EncodedUriComponentCompressionDict =
                FileHelper.GetJsonKeyValuePairs(Directory.GetCurrentDirectory(), "compress", false);
            foreach (var entry in EncodedUriComponentCompressionDict)
            {
                var output = DecompressFromEncodedUriComponent(entry.Value);
                Assert.AreEqual(entry.Key, output);
            }
        }
    }
}
