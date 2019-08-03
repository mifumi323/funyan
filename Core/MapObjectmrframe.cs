using System;
using System.Collections.Generic;
using System.Drawing;

namespace MifuminSoft.funyan.Core
{
    public class Cf3MapObjectmrframe : Cf3MapObjectBase, IDisposable
    {
        protected CDIB32 m_Graphic;

        protected Cf3MapObjectfunya m_funya;
        protected int m_nLife;

        public override void UpdateCPos()
        {
            m_funya.UpdateCPos();
        }
        public bool IsFrozen()
        {
            return m_funya.IsFrozen();
        }
        public static HashSet<Cf3MapObjectmrframe> m_EnemyList = new HashSet<Cf3MapObjectmrframe>();
        public static void OnDrawAll(CDIB32 lp)
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
        public override void Synergy()
        {
            m_funya.Synergy();
        }
        public override void OnPreDraw()
        {
            m_funya.OnPreDraw();
        }
        public override void OnMove()
        {
            m_funya.OnMove();
            SetPos(m_funya.X, m_funya.Y);
            if (m_funya.IsDied())
            {
                if (--m_nLife <= 0)
                {
                    Kill();
                    m_funya.Kill();
                    new Cf3MapObjectEffect(m_X, m_Y, 0);
                }
            }
        }
        public override void OnDraw(CDIB32 lp)
        {
            if (!IsValid()) return;
            if (m_pParent.ItemCompleted()) m_funya.Smile();
            int CX = 0, CY = (int)m_funya.Direction;
            SetViewPos(-16, -15);
            if (m_funya.State == f3MainCharaState.STANDING)
            {   // 立ってるとき
            }
            else if (m_funya.State == f3MainCharaState.RUNNING)
            {
                CX = m_funya.PoseCounter < 6 ? m_funya.PoseCounter + 2 : 14 - m_funya.PoseCounter;
            }
            else if (m_funya.State == f3MainCharaState.WALKING)
            {
                CX = 11;
            }
            else if (m_funya.State == f3MainCharaState.CHARGING)
            {
                CX =
                (m_funya.ChargePower >= m_funya.JumpFunc[0].Power ? 24 :
                (m_funya.ChargePower >= m_funya.JumpFunc[1].Power ? 11 :
                (m_funya.ChargePower >= m_funya.JumpFunc[2].Power ? 25 :
                (m_funya.ChargePower >= m_funya.JumpFunc[3].Power ? 12 :
                12))));
            }
            else if (m_funya.State == f3MainCharaState.JUMPING)
            {
                CX = ((m_funya.DY >= 0) ? 10 : 9);
            }
            else if (m_funya.State == f3MainCharaState.BREATHEIN)
            {
                if (m_funya.ChargePower < 40.0f) { CX = 15; }
                else if (m_funya.ChargePower < 120.0f) { CX = 16; }
                else { CX = 17; }
                if (!m_funya.HitBottom) CX += 12;
            }
            else if (m_funya.State == f3MainCharaState.BREATHEOUT)
            {
                CX = 14;
                if (!m_funya.HitBottom) CX += 12;
            }
            else if (m_funya.State == f3MainCharaState.TIRED)
            {
                CX = ((m_funya.PoseCounter + 1) % 40 < 20) ? 21 : 22;
            }
            else if (m_funya.State == f3MainCharaState.DAMAGED)
            {
                CX = 13;
            }
            else if (m_funya.State == f3MainCharaState.FROZEN)
            {
                CX = 23;
            }
            else if (m_funya.State == f3MainCharaState.DEAD)
            {
                CX = 13; CY = 0;
            }
            else if (m_funya.State == f3MainCharaState.SMILING)
            {
                CX = 18; CY = 0;
            }
            else if (m_funya.State == f3MainCharaState.SLEEPING)
            {
                CX = 19 + (m_funya.PoseCounter >= 20 ? 1 : 0);
                if (m_funya.Power < -1.0f / 4096.0f) CX += 2;
                CY = 0;
            }
            else if (m_funya.State == f3MainCharaState.BLINKING)
            {
                CX = 1;
            }
            var rc = new Rectangle(CX * 32, CY * 32, 32, 32);
            lp.BltNatural(m_Graphic, m_nVX, m_nVY, rc);
        }
        public Cf3MapObjectmrframe(int nCX, int nCY) : base(f3MapObjectType.MOT_FUNYA)
        {
            m_EnemyList.Add(this);
            m_funya = new Cf3MapObjectfunya(nCX, nCY, false);
            RemoveCharaFromList(m_funya);
            m_Graphic = CResourceManager.ResourceManager.Get(RID.RID_MRFRAME);
            SetPos(m_funya.X, m_funya.Y);
            m_nLife = 100;
        }
        public override void Dispose()
        {
            m_EnemyList.Remove(this);
            m_funya.Dispose();
            base.Dispose();
        }
    }
}
