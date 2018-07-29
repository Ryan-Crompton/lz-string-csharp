using Microsoft.VisualStudio.TestTools.UnitTesting;
using static LZString.LzString;

namespace LzString.Test
{
    [TestClass]
    public class CompressionTests
    {
        public CompressionTests()
        {
        }

        [TestMethod]
        public void ShouldCompress(string value, string expectedOutput)
        {
            var output = Compress(value);
            Assert.AreEqual(expectedOutput, output);
        }
        
        [TestMethod]
        public void ShouldCompressToBase64(string value, string expectedOutput)
        {
            var output = CompressToBase64(value);
            Assert.AreEqual(expectedOutput, output);
        }


        [TestMethod]
        public void ShouldCompressToUtf16(string value, string expectedOutput)
        {
            var output = CompressToUtf16(value);
            Assert.AreEqual(expectedOutput, output);
        }


        [TestMethod]
        public void ShouldCompressToUint8Array(string value, byte[] expectedOutput)
        {
            var output = CompressToUint8Array(value);
            Assert.AreEqual(expectedOutput, output);
        }


        [TestMethod]
        public void ShouldCompressToEncodedUriComponent(string value, string expectedOutput)
        {
            var output = CompressToEncodedUriComponent(value);
            Assert.AreEqual(expectedOutput, output);
        }
    }
}
