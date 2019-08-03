using System;
using System.Collections.Generic;
using System.Drawing;

namespace MifuminSoft.funyan.Core
{
    public class Cf3MapObjectBanana : Cf3MapObjectBase, IDisposable
    {
        //	CDIB32* m_Graphic;
        private static HashSet<Cf3MapObjectBanana> m_BananaList = new HashSet<Cf3MapObjectBanana>();

        public void UpdateCPos() { }
        public static void OnPreDrawAll() { }
        public static void SynergyAll()
        {
            foreach (var it in m_BananaList) {
                if (it.IsValid()) it.Synergy();
            }
        }
        public static void OnDrawAll(CDIB32 lp)
        {
            int sx, sy, ex, ey;
            sx = sy = 0;
            m_pParent.GetViewPos(ref sx, ref sy);
            sx = (-sx) >> 5; sy = (-sy) >> 5;
            ex = sx + 320 / 32; ey = sy + 224 / 32;
            TL.Saturate(sx, ref ex, m_pParent.GetWidth() - 1);
            TL.Saturate(sy, ref ey, m_pParent.GetHeight() - 1);
            foreach (var it in m_pParent.GetMapObjects(sx, sy, ex, ey, f3MapObjectType.MOT_BANANA)) {
                if (it.IsValid()) it.OnDraw(lp);
            }
        }
        public static IEnumerable<Cf3MapObjectBanana> All() { return m_BananaList; }
        public void Reaction(Cf3MapObjectBase obj)
        {
            if (!IsValid()) return;
            switch (obj.GetMapObjectType()) {
                case f3MapObjectType.MOT_FUNYA: {
                        GetCPos(out var cx1, out var cy1);
                        obj.GetCPos(out var cx2, out var cy2);
                        if (cx1 == cx2 && cy1 == cy2) {
                            m_pParent.m_nGotBanana++;
                            Kill();
                        }
                        break;
                    }
                default: {
                        return;
                    }
            }
        }
        public override void Synergy()
        {
            if (!IsValid()) return;
            foreach (var it in m_pParent.GetMapObjects(m_nCX - 1, m_nCY - 1, m_nCX + 1, m_nCY + 1, f3MapObjectType.MOT_FUNYA)) {
                if (it.IsValid()) Reaction(it);
            }
        }
        public override void OnDraw(CDIB32 lp)
        {
            if (!IsValid()) return;
            var pGraphic = CResourceManager.ResourceManager.Get(RID.RID_MAIN);
            var rc = new Rectangle(320, 96, 32, 32);
            SetViewPos(-16, -16);
            lp.BltNatural(pGraphic, m_nVX, m_nVY, rc);
        }
        public Cf3MapObjectBanana(int nCX, int nCY) : base(f3MapObjectType.MOT_BANANA)
        {
            m_BananaList.Add(this);
            SetPos(nCX * 32 + 16, nCY * 32 + 16);
            m_pParent.AddMapObject(m_nCX = nCX, m_nCY = nCY, this);
        }
        public override void Dispose()
        {
            m_BananaList.Remove(this);
            base.Dispose();
        }

    }
}
