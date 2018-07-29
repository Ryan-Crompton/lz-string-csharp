using System;

namespace LzString.TestHarness
{
    public static class InputHelper
    {
        public static T ReadRequestReply<T>()
        {
            var input = Console.ReadLine();

            try
            {
                return (T)Convert.ChangeType(input, typeof(T));
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        public static T ForceRead<T>()
        {
            var casted = default(T);
            while (casted == null || casted.Equals(default(T)))
            {
                var input = Console.ReadLine();
                try
                {
                    casted = (T)Convert.ChangeType(input, typeof(T));
                }
                catch (InvalidCastException)
                {
                    casted = default(T);
                }
            }
            return casted;
        }
    }
}