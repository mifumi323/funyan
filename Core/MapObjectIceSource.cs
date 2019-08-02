using System;
using System.Collections.Generic;

namespace MifuminSoft.funyan.Core
{
    public class Cf3MapObjectIceSource : Cf3MapObjectIceBase, IDisposable
    {
        protected const int PHASEMAX = 32;
        protected static HashSet<Cf3MapObjectIceSource> m_IceList = new HashSet<Cf3MapObjectIceSource>();
        protected int m_Phase;
        protected int m_Size;

        public static IEnumerable<Cf3MapObjectIceSource> All() { return m_IceList; }
        public int GetSize()
        {
            int s = abs((PHASEMAX / 2) - m_Phase) * 6 / PHASEMAX + 4;
            return s;
        }
        public static void OnPreDrawAll()
        {
            foreach (var it in m_IceList)
            {
                if (it.IsValid()) it.OnPreDraw();
            }
        }
        public static void OnDrawAll(CDIB32* lp)
        {
            int sx, sy, ex, ey;
            sx = sy = 0;
            m_pParent->GetViewPos(ref sx, ref sy);
            sx = (-sx) >> 5; sy = (-sy) >> 5;
            ex = sx + 320 / 32; ey = sy + 224 / 32;
            TL.Saturate(sx, ref ex, m_pParent->GetWidth() - 1);
            TL.Saturate(sy, ref ey, m_pParent->GetHeight() - 1);
            for (Cf3MapObjectBase** it = m_pParent->GetMapObjects(sx - 1, sy - 1, ex + 1, ey + 1, MOT_ICESOURCE); (*it) != null; it++)
            {
                if ((*it)->IsValid()) (*it)->OnDraw(lp);
            }
        }
        public void OnPreDraw()
        {
            if (CApp.theApp.random(40) != 0) { m_Phase++; m_Phase %= PHASEMAX; }
            m_Size = GetSize();
        }
        public void OnDraw(CDIB32* lp)
        {
            if (!IsValid()) return;
            RECT rc = { (7 - m_Size) * 64, 0, (8 - m_Size) * 64, 64, };
            SetViewPos(-32, -32);
            lp->BltNatural(m_Graphic, m_nVX, m_nVY, &rc);
        }
        public Cf3MapObjectIceSource(int x, int y) : base(f3MapObjectType.MOT_ICESOURCE)
        {
            m_IceList.Add(this);
            SetPos(x * 32 + 16, y * 32 + 16);
            m_Phase = CApp.theApp.random(PHASEMAX);
            m_Size = GetSize();
        }
        public override void Dispose()
        {
            m_IceList.Remove(this);
            base.Dispose();
        }

    }
}
