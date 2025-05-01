using System;

namespace MifuminSoft.funyan.Core
{
    public abstract class CResourceManager : IDisposable
    {
        public abstract void Quit();
        public abstract void Init();
        public virtual void Dispose() { Quit(); }

        public abstract CDIB32 Get(RID i);
    }
}
