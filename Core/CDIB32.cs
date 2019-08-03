using System;
using System.Drawing;

namespace MifuminSoft.funyan.Core
{
    public abstract class CDIB32 : IDisposable
    {
        #region 生成・破棄
        public static Func<CDIB32> Create { get; set; }
        public abstract void Dispose();

        public abstract void CreateSurface(int v1,int v2);
        public abstract int Load(char fn,bool v);
        #endregion

        #region 描画
        public abstract void Blt(CDIB32 dib,int x,int y,Rectangle rc);
        public abstract void BltFast(CDIB32 dib,int x,int y);
        public abstract void BltFast(CDIB32 dib,int x,int y,Rectangle rc);
        public abstract void BltNatural(CDIB32 dib,int m_xnVX,int y,Rectangle rc);
        public abstract void BlendBlt(CDIB32 dib,int x,int y,int v1,int v2,Rectangle r);
        public abstract void RotateBlt(CDIB32 dib,Rectangle rc,int x,int y,float angle,int v1,int v2);
        #endregion

        #region 編集
        public abstract void Clear(int v);
        public abstract uint[] GetPtr();
        public abstract void SetPixel(int v1,int v2,int v3);
        public abstract void SubColorFast(int v);
        #endregion
    }
}
