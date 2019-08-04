using System;

namespace MifuminSoft.funyan.Core
{
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
