using System.Collections.Generic;
using System.IO;
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

        private static string ReadJsonFile(string filePath, string fileName)
        {
            using (var fs = File.Open($"{filePath}\\{fileName}.json", FileMode.Open))
            using (var reader = new StreamReader(fs, Encoding.Unicode))
                return reader.ReadToEnd();
        }

        public static void GenerateJsonFile(string filePath, string fileName, string[] strings) =>
            WriteJsonFile(filePath, fileName, strings.CompressToEncodedUriComponentKvps());

        public static IEnumerable<KeyValuePair<string, string>>
            GetJsonKeyValuePairs(string filePath, string fileName) => JsonConvert.DeserializeObject<IEnumerable<KeyValuePair<string,string>>>(ReadJsonFile(filePath, fileName));
    }
}