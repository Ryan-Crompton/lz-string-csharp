using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using LzString.Extensions.Compression;
using Newtonsoft.Json;

namespace LzString.Common
{
    public static class FileHelper
    {
        private static void WriteJsonFile(string filePath, string fileName, object json)
        {
            var str = JsonConvert.SerializeObject(json);
            using (var fs = File.Open($"{filePath}\\{fileName}.json", FileMode.OpenOrCreate))
            using (var writer = new StreamWriter(fs, Encoding.Unicode))
                writer.Write(str);
        }

        private static string ReadJsonFile(string filePath, string fileName, bool useUnicodeEncoding)
        {
            using (var fs = File.Open($"{filePath}\\{fileName}.json", FileMode.Open))
            {
                if (useUnicodeEncoding)
                {
                    using (var reader = new StreamReader(fs, Encoding.Unicode))
                        return reader.ReadToEnd();
                }
                using (var reader = new StreamReader(fs,Encoding.UTF8))
                    return reader.ReadToEnd();
            }
        }

        public static void GenerateJsonFile(string filePath, string fileName, string[] strings) =>
            WriteJsonFile(filePath, fileName, strings.CompressToEncodedUriComponentKvps());

        public static IEnumerable<KeyValuePair<string, string>>
            GetJsonKeyValuePairs(string filePath, string fileName, bool useUnicodeEncoding) => JsonConvert.DeserializeObject<IEnumerable<KeyValuePair<string,string>>>(ReadJsonFile(filePath, fileName, useUnicodeEncoding));

        public static void GenerateJsonFileFromJavaScript(string getTempPath, string tempFileName, string[] toArray)
        {
            const string cmdText = "npm install lz-string && node exportCompressedStrings.js && exit";
            var cmd = new Process
            {
                StartInfo =
                {
                    FileName = "cmd.exe",
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = false
                }
            };
            cmd.Start();
            cmd.StandardInput.WriteLine(cmdText);
            cmd.StandardInput.Flush();            
            cmd.WaitForExit();
        }
    }
}