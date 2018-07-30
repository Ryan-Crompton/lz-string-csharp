using System;
using LzString.Common;
using Random = LzString.Common.Random;

namespace LzString.TestHarness
{
    public class Program
    {
        static void Main(string[] args)
        {
            OutputHelper.WriteInfo("Generation input parameters required.");

            uint maxStringLength = 0, maxRows = 0;
            while (maxStringLength == 0)
            {
                OutputHelper.WriteRequest("Max string length?");
                maxStringLength = InputHelper.ReadRequestReply<uint>();
            }

            while (maxRows == 0)
            {
                OutputHelper.WriteRequest("Maximum rows?");
                maxRows = InputHelper.ReadRequestReply<uint>();
            }

            var filePath = "";
            while (string.IsNullOrEmpty(filePath))
            {
                OutputHelper.WriteRequest("File path?");
                filePath = InputHelper.ReadRequestReply<string>();
            }

            var strings = Random.Strings(maxStringLength, maxRows);
            var encodedUriComponentDict = strings.CompressToEncodedUriComponentKvps();

            try
            {
                FileHelper.WriteJsonFile(filePath, "compressEncodedUriComponent", encodedUriComponentDict);
            }
            catch (Exception)
            {
                OutputHelper.WriteError($"Unable to write to file: {filePath}");
                Console.ReadLine();
                return;
            }

            OutputHelper.WriteInfo("Compression Files created");
            Console.ReadLine();
        }
    }
}
