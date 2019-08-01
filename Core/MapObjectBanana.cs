using System.Collections.Generic;

namespace MifuminSoft.funyan.Core
{
    public class Cf3MapObjectBanana : Cf3MapObjectBase
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
        public static void OnDrawAll(CDIB32* lp)
        {
            int sx, sy, ex, ey;
            sx = sy = 0;
            m_pParent->GetViewPos(sx, sy);
            sx = (-sx) >> 5; sy = (-sy) >> 5;
            ex = sx + 320 / 32; ey = sy + 224 / 32;
            TL.Saturate(sx, ref ex, m_pParent->GetWidth() - 1);
            TL.Saturate(sy, ref ey, m_pParent->GetHeight() - 1);
            for (Cf3MapObjectBase** it = m_pParent->GetMapObjects(sx, sy, ex, ey, MOT_BANANA); (*it) != null; it++) {
                if ((*it)->IsValid()) (*it)->OnDraw(lp);
            }
        }
        public static IEnumerable<Cf3MapObjectBanana> All() { return m_BananaList; }
        public void Reaction(Cf3MapObjectBase* obj)
        {
            if (!IsValid()) return;
            switch (obj->GetMapObjectType()) {
                case MOT_FUNYA: {
                        int cx1, cy1, cx2, cy2;
                        GetCPos(cx1, cy1);
                        obj->GetCPos(cx2, cy2);
                        if (cx1 == cx2 && cy1 == cy2) {
                            m_pParent->m_nGotBanana++;
                            Kill();
                        }
                        break;
                    }
                default: {
                        return;
                    }
            }
        }
        public void Synergy()
        {
            if (!IsValid()) return;
            Cf3MapObjectBase** it;
            for (it = m_pParent->GetMapObjects(m_nCX - 1, m_nCY - 1, m_nCX + 1, m_nCY + 1, MOT_FUNYA); (*it) != null; it++) {
                if ((*it)->IsValid()) Reaction((*it));
            }
        }
        public void OnDraw(CDIB32* lp)
        {
            if (!IsValid()) return;
            static CDIB32* pGraphic = ResourceManager.Get(RID_MAIN);
            RECT rc = { 320, 96, 352, 128, };
            SetViewPos(-16, -16);
            lp->BltNatural(pGraphic, m_nVX, m_nVY, &rc);
        }
        public Cf3MapObjectBanana(int nCX, int nCY) : base(f3MapObjectType.MOT_BANANA)
        {
            m_BananaList.Add(this);
            SetPos(nCX * 32 + 16, nCY * 32 + 16);
            m_pParent->AddMapObject(m_nCX = nCX, m_nCY = nCY, this);
        }
        public ~Cf3MapObjectBanana()
        {
            m_BananaList.Remove(this);
        }

    }
}
