using System;
using System.Linq;

namespace MifuminSoft.funyan.Core
{
    /// <summary>
    /// yaneSDK 座標系での三角関数を提供します。
    /// 1周が512で、出力は-65536～65536の範囲です。
    /// </summary>
    public class CSinTable
    {
        static readonly int[] m_lTable = Enumerable.Range(0, 512)
            .Select(i => (int)(Math.Cos(i * Math.PI / 256) * 65536))
            .ToArray();

        public static int Cos(int n) => m_lTable[n & 511];

        public static int Sin(int n) => m_lTable[(n + 384) & 511];
    }
}
