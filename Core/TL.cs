using System;

namespace MifuminSoft.funyan.Core
{
    public class TL
    {
        public static void Saturate(int min,ref int num,int max)
        {
            if (num > max) { num = max; return; }
            if (num < min) num = min;
        }

        public static void Saturate(float min, ref float num, float max)
        {
            if (num > max) { num = max; return; }
            if (num < min) num = min;
        }

        public static bool IsIn(float min,float num,float max)
        {
            return min <= num && num <= max;
        }

        public static void BringClose(ref float from, float to, float step = 1)
        {
            if (step < 0) step = -step;
            if (from > to)
            {
                from -= step;
                if (from < to) from = to;
            }
            else if (from < to)
            {
                from += step;
                if (from > to) from = to;
            }
        }
    }
}
