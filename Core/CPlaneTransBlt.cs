namespace MifuminSoft.funyan.Core
{
    public abstract class CPlaneTransBlt
    {
        public abstract void MirrorBlt1(CDIB32 src, CDIB32 dst, int x, int y, int v);
        public abstract void MirrorBlt2(CDIB32 src, CDIB32 dst, int x, int y, int v);
        public abstract void FlushBlt1(CDIB32 src, CDIB32 dst, int x, int y, int v);
    }
}
