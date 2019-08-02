using System;

namespace MifuminSoft.funyan.Core
{
    public enum RID
    {
        RID_MAIN,
        RID_MAINICY,
        RID_NEEDLE,
        RID_GEASPRIN,
        RID_EELPITCHER,
        RID_ICEFIRE,
        RID_MRFRAME,
        RID_EFFECT,
        RID_NAVI,
        RID_HIT,

        RID_END
    }

    public abstract class CResourceManager : IDisposable
    {
        public abstract void Quit();
        public abstract void Init();
        public virtual void Dispose() { Quit(); }

        public abstract CDIB32 Get(RID i);

        /// <summary>CResourceManagerの実装を設定・取得します。</summary>
        public static CResourceManager ResourceManager { get; set; }
    }
}
