using System;

namespace MifuminSoft.funyan.Core
{
    // TODO: 代入忘れをビルド時に気付けるようにしたい(#4)
    public class CPlaneTransBlt
    {
        public static Action<CDIB32, CDIB32, int, int, int> MirrorBlt1 { get; set; } = (src, dst, x, y, v) => throw new NotImplementedException();
        public static Action<CDIB32, CDIB32, int, int, int> MirrorBlt2 { get; set; } = (src, dst, x, y, v) => throw new NotImplementedException();
        public static Action<CDIB32, CDIB32, int, int, int> FlushBlt1 { get; set; } = (src, dst, x, y, v) => throw new NotImplementedException();
    }
}
