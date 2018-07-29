using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace LzString.Common
{
    public static class FileHelper
    {
        public static void WriteJsonFile(string filePath, string fileName, object json)
        {
            var str = JsonConvert.SerializeObject(json);
            
                using (var fs = File.Open($"{filePath}\\{fileName}.json", FileMode.OpenOrCreate))
                using (var writer = new StreamWriter(fs, Encoding.Unicode))
                {
                    writer.Write(str);
                }
        }
    }
}