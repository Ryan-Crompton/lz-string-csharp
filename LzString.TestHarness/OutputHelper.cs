using System;

namespace LzString.TestHarness
{
    public static class OutputHelper
    {
        public static void WriteRequest(string request)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(request);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void WriteInfo(string info)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(info);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void WriteError(string error)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(error);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}