using System.Collections.Generic;
using System.Linq;
using LzString.Extensions.Compression;
using LzString.Extensions.Decompression;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Random = LzString.Common.Random;

namespace LzString.Test
{
    [TestClass]
    public class HelperTests
    {
        public IEnumerable<string> Strings = Random.Strings(10000, 1000);
        public IEnumerable<string> ComplexStrings = Random.StringsComplex(10000, 1000);

        [TestMethod]
        public void ShouldDecompress()
        {
            var compressedStrings = Strings.CompressToKvps();
            var decompressedStrings = compressedStrings.DecompressFromKvps();
            Assert.IsTrue(decompressedStrings.All(kvp => kvp.Value == kvp.Key));
        }

        [TestMethod]
        public void ShouldDecompressComplex()
        {
            var compressedStrings = ComplexStrings.CompressToKvps();
            var decompressedStrings = compressedStrings.DecompressFromKvps();
            Assert.IsTrue(decompressedStrings.All(kvp => kvp.Value == kvp.Key));
        }

        [TestMethod]
        public void ShouldDecompressFromBase64()
        {
            var compressedStrings = Strings.CompressToBase64Kvps();
            var decompressedStrings = compressedStrings.DecompressFromBase64Kvps();
            Assert.IsTrue(decompressedStrings.All(kvp => kvp.Value == kvp.Key));
        }

        [TestMethod]
        public void ShouldDecompressFromBase64Complex()
        {
            var compressedStrings = ComplexStrings.CompressToBase64Kvps();
            var decompressedStrings = compressedStrings.DecompressFromBase64Kvps();
            Assert.IsTrue(decompressedStrings.All(kvp => kvp.Value == kvp.Key));
        }


        [TestMethod]
        public void ShouldDecompressFromUtf16()
        {
            var compressedStrings = Strings.CompressToUtf16Kvps();
            var decompressedStrings = compressedStrings.DecompressFromUtf16Kvps();
            Assert.IsTrue(decompressedStrings.All(kvp => kvp.Value == kvp.Key));
        }

        [TestMethod]
        public void ShouldDecompressFromUtf16Complex()
        {
            var compressedStrings = ComplexStrings.CompressToUtf16Kvps();
            var decompressedStrings = compressedStrings.DecompressFromUtf16Kvps();
            Assert.IsTrue(decompressedStrings.All(kvp => kvp.Value == kvp.Key));
        }


        [TestMethod]
        public void ShouldDecompressFromUint8Array()
        {
            var compressedStrings = Strings.CompressToUint8ArrayKvps();
            var decompressedStrings = compressedStrings.DecompressFromUint8ArrayKvps();
            Assert.IsTrue(decompressedStrings.All(kvp => kvp.Value == kvp.Key));
        }

        [TestMethod]
        public void ShouldDecompressFromUint8ArrayComplex()
        {
            var compressedStrings = ComplexStrings.CompressToUint8ArrayKvps();
            var decompressedStrings = compressedStrings.DecompressFromUint8ArrayKvps();
            Assert.IsTrue(decompressedStrings.All(kvp => kvp.Value == kvp.Key));
        }

        [TestMethod]
        public void ShouldDecompressFromEncodedUriComponent()
        {
            var compressedStrings = Strings.CompressToEncodedUriComponentKvps();
            var decompressedStrings = compressedStrings.DecompressFromEncodedUriComponentKvps();
            Assert.IsTrue(decompressedStrings.All(kvp => kvp.Value == kvp.Key));
        }

        [TestMethod]
        public void ShouldDecompressFromEncodedUriComponentComplex()
        {
            var compressedStrings = ComplexStrings.CompressToEncodedUriComponentKvps();
            var decompressedStrings = compressedStrings.DecompressFromEncodedUriComponentKvps();
            Assert.IsTrue(decompressedStrings.All(kvp => kvp.Value == kvp.Key));
        }
    }
}
