using System;

namespace MifuminSoft.funyan.Core
{
    public class CPlaneTransBlt
    {
        public static Action<CDIB32, CDIB32, int, int, int> MirrorBlt1 { get; set; }
        public static Action<CDIB32, CDIB32, int, int, int> MirrorBlt2 { get; set; }
        public static Action<CDIB32, CDIB32, int, int, int> FlushBlt1 { get; set; }
    }
}
