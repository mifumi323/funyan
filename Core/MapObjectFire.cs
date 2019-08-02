using System;
using System.Collections.Generic;

namespace MifuminSoft.funyan.Core
{
    public class Cf3MapObjectFire : Cf3MapObjectIceBase, IDisposable
    {
        private const int PHASEMAX = 32;
        private static HashSet<Cf3MapObjectFire> m_FireList = new HashSet<Cf3MapObjectFire>();

        private int m_Phase;
        private int m_Size;
        private int m_Delay;

        public void Synergy()
        {
            if (m_Delay == 0)
            {
                // ふにゃ
                Cf3MapObjectBase** it;
                for (it = m_pParent->GetMapObjects(m_nCX - 1, m_nCY - 1, m_nCX + 1, m_nCY + 1,f3MapObjectType.MOT_FUNYA); (*it) != null; it++)
                {
                    if ((*it)->IsValid() && ((Cf3MapObjectMain*)(*it))->IsFrozen())
                    {
                        (*it)->GetPos(out var objX, out var objY);
                        // あたった！
                        if ((objX - m_X) * (objX - m_X) + (objY - m_Y) * (objY - m_Y) < 256) m_Delay = 200;
                    }
                }
                // 氷
                for (it = m_pParent->GetMapObjects(m_nCX - 1, m_nCY - 1, m_nCX + 1, m_nCY + 1,f3MapObjectType.MOT_ICE); (*it) != null; it++)
                {
                    if ((*it)->IsValid())
                    {
                        (*it)->GetPos(out var objX, out var objY);
                        // あたった！
                        if ((objX - m_X) * (objX - m_X) + (objY - m_Y) * (objY - m_Y) < 256) m_Delay = 200;
                    }
                }
            }
        }
        public bool IsActive() { return m_Delay == 0; }
        public static IEnumerable<Cf3MapObjectFire> All() { return m_FireList; }
        public void OnPreDraw()
        {
            if (CApp.theApp.random(40)) { m_Phase++; m_Phase %= PHASEMAX; }
            if (m_Size < GetSize()) { m_Size++; }
            else if (m_Size > GetSize()) { m_Size--; }
        }
        public void OnMove()
        {
            if (m_Delay > 0) m_Delay--;
            else if (m_Delay < 0) m_Delay++;
        }
        public void OnDraw(CDIB32* lp)
        {
            if (!IsValid()) return;
            RECT rc = { (15 - m_Size) * 64, 64, (16 - m_Size) * 64, 128, };
            SetViewPos(-32, -32);
            lp->BltNatural(m_Graphic, m_nVX, m_nVY, &rc);
        }
        public int GetSize()
        {
            int s = abs((PHASEMAX / 2) - m_Phase) * 6 / PHASEMAX;
            if (m_Delay == 0) s += 10;
            return s;
        }
        public static void SynergyAll()
        {
            foreach (var it in m_FireList)
            {
                if (it.IsValid()) it.Synergy();
            }
        }
        public static void OnPreDrawAll()
        {
            foreach (var it in m_FireList)
            {
                if (it.IsValid()) it.OnPreDraw();
            }
        }
        public static void OnMoveAll()
        {
            foreach (var it in m_FireList)
            {
                if (it.IsValid()) it.OnMove();
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
            for (Cf3MapObjectBase** it = m_pParent->GetMapObjects(sx - 1, sy - 1, ex + 1, ey + 1,f3MapObjectType.MOT_FIRE); (*it) != null; it++)
            {
                if ((*it)->IsValid()) (*it)->OnDraw(lp);
            }
        }
        public Cf3MapObjectFire(int x, int y) : base(f3MapObjectType.MOT_FIRE)
        {
            m_Delay = 0;
            m_FireList.Add(this);
            SetPos(x * 32 + 16, y * 32 + 16);
            m_Phase = CApp.theApp.random(PHASEMAX);
            m_Size = GetSize();
        }
        public override void Dispose()
        {
            m_FireList.Remove(this);
            base.Dispose();
        }

    }
}
