using System;

namespace MifuminSoft.funyan.Core
{
    public class App
    {
        private static Random random = new Random();

        public static int Random(int maxValue)
        {
            return random.Next(maxValue);
        }
    }
}
