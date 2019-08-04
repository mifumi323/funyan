using System;

namespace MifuminSoft.funyan.Core
{
    /// <summary>
    /// f3TL - funya3 Typical Library
    /// 
    /// 定型的な処理を行う便利メソッド集です。
    /// もともとは Template Library でしたが、C# では四則演算のジェネリック化に無理があったので、個別実装になりました。
    /// </summary>
    public class TL
    {
        /// <summary>
        /// min以上max以下に制限する
        /// </summary>
        /// <param name="min"></param>
        /// <param name="num"></param>
        /// <param name="max"></param>
        public static void Saturate(int min, ref int num, int max)
        {
            if (num > max) { num = max; return; }
            if (num < min) num = min;
        }

        /// <summary>
        /// min以上max以下に制限する
        /// </summary>
        /// <param name="min"></param>
        /// <param name="num"></param>
        /// <param name="max"></param>
        public static void Saturate(float min, ref float num, float max)
        {
            if (num > max) { num = max; return; }
            if (num < min) num = min;
        }

        /// <summary>
        /// min以上max以下かどうか
        /// </summary>
        /// <param name="min"></param>
        /// <param name="num"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static bool IsIn(int min, int num, int max)
        {
            return min <= num && num <= max;
        }

        /// <summary>
        /// min以上max以下かどうか
        /// </summary>
        /// <param name="min"></param>
        /// <param name="num"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static bool IsIn(float min, float num, float max)
        {
            return min <= num && num <= max;
        }

        /// <summary>
        /// fromをtoにstepだけ近づける
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="step"></param>
        public static void BringClose(ref int from, int to, int step = 1)
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

        /// <summary>
        /// fromをtoにstepだけ近づける
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="step"></param>
        public static void BringClose(ref float from, float to, float step = 1.0f)
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

        public static void DELETE_SAFE<T>(ref T obj) where T : class, IDisposable
        {
            if (obj != null)
            {
                obj.Dispose();
                obj = null;
            }
        }

        public static void swap<T>(ref T a, ref T b)
        {
            var c = a;
            a = b;
            b = c;
        }
    }
}
