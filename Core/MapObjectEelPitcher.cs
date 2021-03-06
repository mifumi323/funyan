﻿using System;
using System.Collections.Generic;
using System.Drawing;

namespace MifuminSoft.funyan.Core
{
    public class Cf3MapObjectEelPitcher : Cf3MapObjectBase, IDisposable
    {
        private void Freeze() { m_State = f3EelPitcherState.EELFROZEN; m_Delay = 80; }
        private void Seed()
        {
            int d = 0;
            m_State = m_Delay != 0 ? f3EelPitcherState.EELBUD : f3EelPitcherState.EELLEAF;
            m_RootX = m_X;
            m_RootY = (float)Math.Floor(m_Y / 32) * 32;
            if (m_pParent.GetHit((int)Math.Floor((m_X - 14) / 32), (int)Math.Floor(m_Y / 32), HIT.HIT_TOP)) d |= 1;
            if (m_pParent.GetHit((int)Math.Floor((m_X + 14) / 32), (int)Math.Floor(m_Y / 32), HIT.HIT_TOP)) d |= 2;
            m_Direction = d == 1 ? f3MapObjectDirection.DIR_RIGHT : (d == 2 ? f3MapObjectDirection.DIR_LEFT : (CApp.theApp.random(2) != 0 ? f3MapObjectDirection.DIR_LEFT : f3MapObjectDirection.DIR_RIGHT));
        }
        private static Dictionary<int, Cf3MapObjectEelPitcher> m_EnemyList = new Dictionary<int, Cf3MapObjectEelPitcher>();

        private f3MapObjectDirection m_Direction;
        private int m_Level;                        // 最大高さ
        private int m_Delay;                        // 待ち時間
        private enum f3EelPitcherState
        {
            EELSEED,
            EELBUD,
            EELLEAF,
            EELFROZEN,
            EELDEAD,
        }
        private f3EelPitcherState m_State;
        private float m_DX, m_DY;
        private float m_RootX, m_RootY;         // 根元
        private bool m_bBlinking;

        public bool IsLeaf() { return m_State == f3EelPitcherState.EELLEAF || m_State == f3EelPitcherState.EELFROZEN; }
        public static void OnDrawAll(CDIB32 lp)
        {
            foreach (var it in m_EnemyList)
            {
                if (it.Value.IsValid()) it.Value.OnDraw(lp);
            }
        }
        public static void OnPreDrawAll()
        {
            foreach (var it in m_EnemyList)
            {
                if (it.Value.IsValid()) it.Value.OnPreDraw();
            }
        }
        public static void SynergyAll()
        {
            foreach (var it in m_EnemyList)
            {
                if (it.Value.IsValid()) it.Value.Synergy();
            }
        }
        public static void OnMoveAll()
        {
            foreach (var it in m_EnemyList)
            {
                if (it.Value.IsValid()) it.Value.OnMove();
            }
        }
        public static Dictionary<int, Cf3MapObjectEelPitcher> All() { return m_EnemyList; }
        public void Reaction(Cf3MapObjectBase obj)
        {
            if (obj == null || obj == this) return;
            obj.GetPos(out var objX, out var objY);
            switch (obj.GetMapObjectType())
            {
                case f3MapObjectType.MOT_FUNYA:
                    {
                        if (TL.IsIn(m_X - 16, objX, m_X + 16))
                        {
                            if (TL.IsIn(m_Y - 16, objY, m_Y))
                            {
                                // 踏まれた！！
                                m_bBlinking = true;
                            }
                        }
                        break;
                    }
                case f3MapObjectType.MOT_NEEDLE:
                case f3MapObjectType.MOT_GEASPRIN:
                    {
                        if (TL.IsIn(m_X - 16, objX, m_X + 16))
                        {
                            if (TL.IsIn(m_Y, objY, m_Y + 40))
                            {
                                // 食べちゃった！！
                                m_Level++;
                            }
                        }
                        break;
                    }
                default:
                    {
                        return;
                    }
            }
        }
        public override void Synergy()
        {
            if (!IsValid()) return;
            m_bBlinking = false;
            GetCPos(out var cx, out var cy);
            if (m_State == f3EelPitcherState.EELLEAF)
            {
                foreach (var it in m_pParent.GetMapObjects(cx - 2, cy - 2, cx + 2, cy + 2, f3MapObjectType.MOT_FUNYA))
                {
                    if (it.IsValid()) Reaction(it);
                }
                if (m_RootY - m_Y > 16)
                {
                    foreach (var it in m_pParent.GetMapObjects(cx - 1, cy - 1, cx + 1, cy + 1, f3MapObjectType.MOT_ICE))
                    {
                        if (it.IsValid())
                        {
                            it.GetPos(out var objX, out var objY);
                            if (TL.IsIn(m_X - 16, objX, m_X + 16))
                            {
                                if (TL.IsIn(m_Y, objY, m_Y + 40))
                                {
                                    Freeze();
                                }
                            }
                        }
                    }
                }
            }
            foreach (var it in m_pParent.GetMapObjects(cx - 1, cy - 1, cx + 1, cy + 1, f3MapObjectType.MOT_EELPITCHER))
            {
                if (it.IsValid() && it != this)
                {
                    it.GetPos(out var objX, out var objY);
                    if (m_State == f3EelPitcherState.EELLEAF || m_State == f3EelPitcherState.EELFROZEN)
                    {
                        if (TL.IsIn(m_X - 16, objX, m_X + 16))
                        {
                            if (TL.IsIn(m_Y, objY, m_Y + 16))
                            {
                                if (((Cf3MapObjectEelPitcher)it).m_State != f3EelPitcherState.EELLEAF)
                                {
                                    // 食べちゃった！！
                                    m_Level += ((Cf3MapObjectEelPitcher)it).m_Level;
                                }
                                else
                                {
                                    // 食べられちゃった！！
                                    m_State = f3EelPitcherState.EELDEAD;
                                }
                            }
                        }
                    }
                    else if (m_State == f3EelPitcherState.EELSEED)
                    {
                        if (TL.IsIn(objX - 16, m_X, objX + 16))
                        {
                            if (TL.IsIn(objY, m_Y, objY + 16))
                            {
                                if (((Cf3MapObjectEelPitcher)it).m_State != f3EelPitcherState.EELDEAD)
                                {
                                    // 食べられちゃった！！
                                    m_State = f3EelPitcherState.EELDEAD;
                                }
                                else
                                {
                                    // 食べちゃった！！
                                    m_Level += ((Cf3MapObjectEelPitcher)it).m_Level;
                                }
                            }
                        }
                    }
                }
            }
            foreach (var it in m_pParent.GetMapObjects(cx - 2, cy - 2, cx + 2, cy + 2, f3MapObjectType.MOT_GEASPRIN))
            {
                if (it.IsValid()) Reaction(it);
            }
            foreach (var it in m_pParent.GetMapObjects(cx - 1, cy - 1, cx + 1, cy + 1, f3MapObjectType.MOT_NEEDLE))
            {
                if (it.IsValid()) Reaction(it);
            }
        }
        public override void OnMove()
        {
            if (m_State == f3EelPitcherState.EELLEAF)
            {
                TL.BringClose(ref m_Y, m_RootY - m_Level * 32, 4.0f);
                m_DX = m_DX + (m_pParent.GetWind((int)Math.Floor(m_X / 32), (int)Math.Floor(m_Y / 32)) - m_DX) * m_Level * 0.1f + (m_RootX - m_X) * 0.025f;
                TL.Saturate(-14.0f, ref m_DX, 14.0f);
                m_X += m_DX;
                if (m_pParent.GetHit((int)Math.Floor((m_X - 16) / 32), (int)Math.Floor(m_Y / 32), HIT.HIT_RIGHT))
                {
                    m_DX = 0;
                    m_X = (int)Math.Floor(m_X / 32) * 32 + 16;
                }
                else if (m_pParent.GetHit((int)Math.Floor((m_X + 16) / 32), (int)Math.Floor(m_Y / 32), HIT.HIT_LEFT))
                {
                    m_DX = 0;
                    m_X = (int)Math.Floor(m_X / 32) * 32 + 16;
                }
            }
            else if (m_State == f3EelPitcherState.EELFROZEN)
            {
                if (--m_Delay == 0)
                {
                    m_Y += 16;
                    m_State = f3EelPitcherState.EELSEED;
                    m_Delay = 200;
                    new Cf3MapObjectEffect(m_X, m_Y, 0);
                }
            }
            else if (m_State == f3EelPitcherState.EELSEED)
            {
                TL.BringClose(ref m_DY, 8.0f, 1.0f);
                m_DX += (m_pParent.GetWind((int)Math.Floor(m_X / 32), (int)Math.Floor(m_Y / 32)) - m_DX) * 0.2f;
                TL.Saturate(-14.0f, ref m_DX, 14.0f);
                m_X += m_DX;
                if (m_pParent.GetHit((int)Math.Floor((m_X - 16) / 32), (int)Math.Floor(m_Y / 32), HIT.HIT_RIGHT))
                {
                    m_DX = 0;
                    m_X = (int)Math.Floor(m_X / 32) * 32 + 16;
                }
                else if (m_pParent.GetHit((int)Math.Floor((m_X + 16) / 32), (int)Math.Floor(m_Y / 32), HIT.HIT_LEFT))
                {
                    m_DX = 0;
                    m_X = (int)Math.Floor(m_X / 32) * 32 + 16;
                }
                m_Y += m_DY;
                if ((int)Math.Floor(m_Y / 32) != (int)Math.Floor((m_Y - m_DY) / 32))
                {
                    // 32ドット境界をまたいだ！！
                    if (m_pParent.GetHit((int)Math.Floor(m_X / 32), (int)Math.Floor(m_Y / 32), HIT.HIT_TOP)
                        || (int)Math.Floor(m_Y / 32) >= m_pParent.GetHeight())
                    {
                        Seed();
                    }
                    else
                    {
                        if (m_pParent.GetHit((int)Math.Floor((m_X - 16) / 32), (int)Math.Floor(m_Y / 32), HIT.HIT_RIGHT))
                        {
                            m_DX = 0;
                            m_X = (int)Math.Floor(m_X / 32) * 32 + 16;
                        }
                        else if (m_pParent.GetHit((int)Math.Floor((m_X + 16) / 32), (int)Math.Floor(m_Y / 32), HIT.HIT_LEFT))
                        {
                            m_DX = 0;
                            m_X = (int)Math.Floor(m_X / 32) * 32 + 16;
                        }
                    }
                }
            }
            else if (m_State == f3EelPitcherState.EELBUD)
            {
                if (--m_Delay == 0) m_State = f3EelPitcherState.EELLEAF;
            }
            else if (m_State == f3EelPitcherState.EELDEAD)
            {
                Kill();
            }
        }
        public override void OnDraw(CDIB32 lp)
        {
            SetViewPos(-16, -16);
            int height = (int)(m_RootY - m_Y);
            Rectangle rc;
            var graphic = CResourceManager.ResourceManager.Get(RID.RID_EELPITCHER);
            if (m_State == f3EelPitcherState.EELLEAF || m_State == f3EelPitcherState.EELFROZEN)
            {
                int offset1 = (m_State == f3EelPitcherState.EELLEAF ? 0 : 96);
                int offset2 = (m_Direction == f3MapObjectDirection.DIR_LEFT ? 32 : 64);
                int offset3 = (m_bBlinking ? 32 : 0);
                // あたま
                rc = new Rectangle(offset1 + offset2, offset3, 32, height >= 16 ? 32 : height + 16);
                lp.Blt(graphic, m_nVX, m_nVY, rc);
                // 茎
                if (height > 16)
                {
                    int i, h, h2;
                    h = (int)Math.Floor((m_RootY - m_Y) / (Math.Abs(m_RootY - m_Y) + 1) + 1);
                    if (h > 32) h = 32;
                    for (i = 16; i < height; i += h)
                    {
                        h2 = h <= height - i ? h : height - i;
                        rc = new Rectangle(offset1, offset2, 32, h2);
                        // +0.5fは小数点以下四捨五入のため
                        SetViewPos(-16 + (m_RootX - m_X) * (i - 16) / (height - 16) + 0.5f, -16);
                        lp.Blt(graphic, m_nVX, m_nVY + 16 + i, rc);
                    }
                }
                SetViewPos(-16, -16);
                if (TL.IsIn(17, height, 32))
                {
                    // あご
                    rc = new Rectangle(offset1 + offset2, 80, 32, height - 16);
                    lp.Blt(graphic, m_nVX, m_nVY + 32, rc);
                }
                else if (TL.IsIn(33, height, 48))
                {
                    int height2 = height - 32;
                    // ふくろ
                    rc = new Rectangle(offset1 + offset2, 64, 32, height2);
                    lp.Blt(graphic, m_nVX, m_nVY + 32, rc);
                    // あご
                    rc = new Rectangle(offset1 + offset2, 80, 32, 16);
                    lp.Blt(graphic, m_nVX, m_nVY + height, rc);
                }
                else if (height > 48)
                {
                    // ふくろ
                    rc = new Rectangle(offset1 + offset2, 64, 32, 16);
                    lp.Blt(graphic, m_nVX, m_nVY + 32, rc);
                    // あご
                    rc = new Rectangle(offset1 + offset2, 80, 32, 16);
                    lp.Blt(graphic, m_nVX, m_nVY + 48, rc);
                }
            }
            else if (m_State == f3EelPitcherState.EELSEED)
            {
                rc = new Rectangle(0, 0, 32, 32);
                lp.Blt(graphic, m_nVX, m_nVY, rc);
            }
        }
        public Cf3MapObjectEelPitcher(int nCX, int nCY) : base(f3MapObjectType.MOT_EELPITCHER)
        {
            m_Delay = 0;
            m_Level = 1;
            m_DX = 0;
            m_DY = 0;
            m_State = f3EelPitcherState.EELSEED;
            m_bBlinking = (false);
            m_EnemyList.Add(GetID(), this);
            SetPos(nCX * 32 + 16, nCY * 32 + 16);
        }
        public override void Dispose()
        {
            m_EnemyList.Remove(GetID());
            base.Dispose();
        }
    }
}
