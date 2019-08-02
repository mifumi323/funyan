using System;
using System.Collections.Generic;

namespace MifuminSoft.funyan.Core
{
    public class Cf3MapObjectmrframe : Cf3MapObjectBase, IDisposable
    {
        protected CDIB32 m_Graphic;

        protected Cf3MapObjectfunya m_funya;
        protected int m_nLife;

        public void UpdateCPos()
        {
            m_funya->UpdateCPos();
        }
        public bool IsFrozen()
        {
            return m_funya->IsFrozen();
        }
        public static HashSet<Cf3MapObjectmrframe> m_EnemyList = new HashSet<Cf3MapObjectmrframe>();
        public static void OnDrawAll(CDIB32* lp)
        {
            foreach (var it in m_EnemyList)
            {
                if (it.IsValid()) it.OnDraw(lp);
            }
        }
        public static void OnPreDrawAll()
        {
            foreach (var it in m_EnemyList)
            {
                if (it.IsValid()) it.OnPreDraw();
            }
        }
        public static void SynergyAll()
        {
            foreach (var it in m_EnemyList)
            {
                if (it.IsValid()) it.Synergy();
            }
        }
        public static void OnMoveAll()
        {
            foreach (var it in m_EnemyList)
            {
                if (it.IsValid()) it.OnMove();
            }
        }
        public static IEnumerable<Cf3MapObjectmrframe> All() { return m_EnemyList; }
        public void Synergy()
        {
            m_funya->Synergy();
        }
        public void OnPreDraw()
        {
            m_funya->OnPreDraw();
        }
        public void OnMove()
        {
            m_funya->OnMove();
            SetPos(m_funya->m_X, m_funya->m_Y);
            if (m_funya->IsDied())
            {
                if (--m_nLife <= 0)
                {
                    Kill();
                    m_funya->Kill();
                    new Cf3MapObjectEffect(m_X, m_Y, 0);
                }
            }
        }
        public void OnDraw(CDIB32* lp)
        {
            if (!IsValid()) return;
            if (m_pParent->ItemCompleted()) m_funya->Smile();
            int CX = 0, CY = m_funya->m_Direction;
            SetViewPos(-16, -15);
            if (m_funya->m_State == Cf3MapObjectfunya::STANDING)
            {   // 立ってるとき
            }
            else if (m_funya->m_State == Cf3MapObjectfunya::RUNNING)
            {
                CX = m_funya->m_PoseCounter < 6 ? m_funya->m_PoseCounter + 2 : 14 - m_funya->m_PoseCounter;
            }
            else if (m_funya->m_State == Cf3MapObjectfunya::WALKING)
            {
                CX = 11;
            }
            else if (m_funya->m_State == Cf3MapObjectfunya::CHARGING)
            {
                CX =
                (m_funya->m_ChargePower >= m_funya->m_JumpFunc[0].Power ? 24 :
                (m_funya->m_ChargePower >= m_funya->m_JumpFunc[1].Power ? 11 :
                (m_funya->m_ChargePower >= m_funya->m_JumpFunc[2].Power ? 25 :
                (m_funya->m_ChargePower >= m_funya->m_JumpFunc[3].Power ? 12 :
                12))));
            }
            else if (m_funya->m_State == Cf3MapObjectfunya::JUMPING)
            {
                CX = ((m_funya->m_DY >= 0) ? 10 : 9);
            }
            else if (m_funya->m_State == Cf3MapObjectfunya::BREATHEIN)
            {
                if (m_funya->m_ChargePower < 40.0f) { CX = 15; }
                else if (m_funya->m_ChargePower < 120.0f) { CX = 16; }
                else { CX = 17; }
                if (!m_funya->m_HitBottom) CX += 12;
            }
            else if (m_funya->m_State == Cf3MapObjectfunya::BREATHEOUT)
            {
                CX = 14;
                if (!m_funya->m_HitBottom) CX += 12;
            }
            else if (m_funya->m_State == Cf3MapObjectfunya::TIRED)
            {
                CX = ((m_funya->m_PoseCounter + 1) % 40 < 20) ? 21 : 22;
            }
            else if (m_funya->m_State == Cf3MapObjectfunya::DAMAGED)
            {
                CX = 13;
            }
            else if (m_funya->m_State == Cf3MapObjectfunya::FROZEN)
            {
                CX = 23;
            }
            else if (m_funya->m_State == Cf3MapObjectfunya::DEAD)
            {
                CX = 13; CY = 0;
            }
            else if (m_funya->m_State == Cf3MapObjectfunya::SMILING)
            {
                CX = 18; CY = 0;
            }
            else if (m_funya->m_State == Cf3MapObjectfunya::SLEEPING)
            {
                CX = 19 + (int)(m_funya->m_PoseCounter >= 20);
                if (m_funya->m_Power < -1.0f / 4096.0f) CX += 2;
                CY = 0;
            }
            else if (m_funya->m_State == Cf3MapObjectfunya::BLINKING)
            {
                CX = 1;
            }
            RECT rc = { CX * 32, CY * 32, CX * 32 + 32, CY * 32 + 32, };
            lp->BltNatural(m_Graphic, m_nVX, m_nVY, &rc);
        }
        public Cf3MapObjectmrframe(int nCX, int nCY) : base(f3MapObjectType.MOT_FUNYA)
        {
            m_EnemyList.Add(this);
            m_funya = new Cf3MapObjectfunya(nCX, nCY);
            RemoveCharaFromList(m_funya);
            m_funya->m_bOriginal = false;
            m_Graphic = CResourceManager.ResourceManager.Get(RID.RID_MRFRAME);
            SetPos(m_funya->m_X, m_funya->m_Y);
            m_nLife = 100;
        }
        public override void Dispose()
        {
            m_EnemyList.Remove(this);
            delete m_funya;
            base.Dispose();
        }

    }
}
