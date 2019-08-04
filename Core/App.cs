namespace MifuminSoft.funyan.Core
{
    public abstract class CApp
    {
        public static CApp theApp { get; set; }

        public abstract CBGMBase GetBGM();
        public abstract int random(int v);
        public abstract bool MakeFileName(out string filename,string v1,int m_RecordNumber,bool v2);
    }
}
