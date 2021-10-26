using System;

namespace MifuminSoft.funyan.Core
{
    public abstract class CApp
    {
        private static CApp? m_theApp;

        public static CApp theApp { get => m_theApp ?? throw new Exception("CApp.theAppは使用前に非nullで初期化する必要があります。"); set => m_theApp = value; }

        public abstract CBGMBase GetBGM();
        public abstract int random(int v);
        public abstract bool MakeFileName(out string filename, string v1, int m_RecordNumber, bool v2);
    }
}
