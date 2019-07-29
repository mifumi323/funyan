using System;
using System.Collections.Generic;

namespace MifuminSoft.funyan.Core
{
    public class MapObjectEelPitcher:MapObjectBase
    {

        private void Freeze() { m_State = EelPitcherState.EELFROZEN; m_Delay = 80; }
        private void Seed()
        {
            int d = 0;
            m_State = m_Delay != 0 ? EelPitcherState.EELBUD : EelPitcherState.EELLEAF;
            m_RootX = m_X;
            m_RootY = (float)Math.Floor(m_Y / 32) * 32;
            if (m_pParent.GetHit((int)Math.Floor((m_X - 14) / 32), (int)Math.Floor(m_Y / 32), Hit.Top)) d |= 1;
            if (m_pParent.GetHit((int)Math.Floor((m_X + 14) / 32), (int)Math.Floor(m_Y / 32), Hit.Top)) d |= 2;
            m_Direction = d == 1 ? MapObjectDirection.Right : (d == 2 ? MapObjectDirection.Left : ((App.Random(2) != 0) ? MapObjectDirection.Left : MapObjectDirection.Right));
        }

        private static Dictionary<int, MapObjectEelPitcher> m_EnemyList;

        private MapObjectDirection m_Direction;
        /// <summary>最大高さ</summary>
        private int m_Level;
        /// <summary>待ち時間</summary>
        private int m_Delay;
        private enum EelPitcherState
        {
            EELSEED,
            EELBUD,
            EELLEAF,
            EELFROZEN,
            EELDEAD,
        }
        private EelPitcherState m_State;
        private float m_DX, m_DY;
        private float m_RootX, m_RootY;         // 根元
        private bool m_bBlinking;

        public bool IsLeaf() { return m_State == EelPitcherState.EELLEAF || m_State == EelPitcherState.EELFROZEN; }
        public static void SynergyAll()
        {
            foreach (var enemy in m_EnemyList)
            {
                enemy.Value.Synergy();
            }
        }

        public static void OnMoveAll()
        {
            foreach (var enemy in m_EnemyList)
            {
                enemy.Value.OnMove();
            }
        }

        public static Dictionary<int, MapObjectEelPitcher> All() { return m_EnemyList; }

        public void Reaction(MapObjectBase obj)
        {
            if (obj == null || obj == this) return;
            obj.GetPos(out var objX, out var objY);
            switch (obj.GetMapObjectType())
            {
                case MapObjectType.funya:
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
                case MapObjectType.Needle:
                case MapObjectType.Geasprin:
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
            if (m_State == EelPitcherState.EELLEAF)
            {
                foreach (var it in m_pParent.GetMapObjects(cx - 2, cy - 2, cx + 2, cy + 2, MapObjectType.funya))
                {
                    if (it.IsValid()) Reaction(it);
                }
                if (m_RootY - m_Y > 16)
                {
                    foreach (var it in m_pParent.GetMapObjects(cx - 1, cy - 1, cx + 1, cy + 1, MapObjectType.Ice))
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
            foreach (var it in m_pParent.GetMapObjects(cx - 1, cy - 1, cx + 1, cy + 1, MapObjectType.EelPitcher))
            {
                if (it.IsValid() && it != this)
                {
                    it.GetPos(out var objX, out var objY);
                    if (m_State == EelPitcherState.EELLEAF || m_State == EelPitcherState.EELFROZEN)
                    {
                        if (TL.IsIn(m_X - 16, objX, m_X + 16))
                        {
                            if (TL.IsIn(m_Y, objY, m_Y + 16))
                            {
                                if (((MapObjectEelPitcher)it).m_State != EelPitcherState.EELLEAF)
                                {
                                    // 食べちゃった！！
                                    m_Level += ((MapObjectEelPitcher)it).m_Level;
                                }
                                else
                                {
                                    // 食べられちゃった！！
                                    m_State = EelPitcherState.EELDEAD;
                                }
                            }
                        }
                    }
                    else if(m_State == EelPitcherState.EELSEED) {
                        if (TL.IsIn(objX - 16, m_X, objX + 16))
                        {
                            if (TL.IsIn(objY, m_Y, objY + 16))
                            {
                                if (((MapObjectEelPitcher)it).m_State != EelPitcherState.EELDEAD)
                                {
                                    // 食べられちゃった！！
                                    m_State = EelPitcherState.EELDEAD;
                                }
                                else
                                {
                                    // 食べちゃった！！
                                    m_Level += ((MapObjectEelPitcher)it).m_Level;
                                }
                            }
                        }
                    }
                }
            }
            foreach (var it in m_pParent.GetMapObjects(cx - 2, cy - 2, cx + 2, cy + 2, MapObjectType.Geasprin))
            {
                if (it.IsValid()) Reaction(it);
            }
            foreach (var it in m_pParent.GetMapObjects(cx - 1, cy - 1, cx + 1, cy + 1, MapObjectType.Needle))
            {
                if (it.IsValid()) Reaction(it);
            }
        }

        public override void OnMove()
        {
            if (m_State == EelPitcherState.EELLEAF)
            {
                TL.BringClose(ref m_Y, m_RootY - m_Level * 32, 4.0f);
                m_DX = m_DX + (m_pParent.GetWind((int)Math.Floor(m_X / 32), (int)Math.Floor(m_Y / 32)) - m_DX) * m_Level * 0.1f + (m_RootX - m_X) * 0.025f;
                TL.Saturate(-14.0f, ref m_DX, 14.0f);
                m_X += m_DX;
                if (m_pParent.GetHit((int)Math.Floor((m_X - 16) / 32), (int)Math.Floor(m_Y / 32), Hit.Right))
                {
                    m_DX = 0;
                    m_X = (float)Math.Floor(m_X / 32) * 32 + 16;
                }
                else if(m_pParent.GetHit((int)Math.Floor((m_X + 16) / 32), (int)Math.Floor(m_Y / 32), Hit.Left)) {
                    m_DX = 0;
                    m_X = (float)Math.Floor(m_X / 32) * 32 + 16;
                }
            }
            else if(m_State == EelPitcherState.EELFROZEN) {
                if (--m_Delay == 0)
                {
                    m_Y += 16;
                    m_State = EelPitcherState.EELSEED;
                    m_Delay = 200;
                    new MapObjectEffect(m_X, m_Y, 0);
                }
            }
            else if (m_State == EelPitcherState.EELSEED) {
                TL.BringClose(ref m_DY, 8.0f, 1.0f);
                m_DX = m_DX + (m_pParent.GetWind((int)Math.Floor(m_X / 32), (int)Math.Floor(m_Y / 32)) - m_DX) * 0.2f;
                TL.Saturate(-14.0f, ref m_DX, 14.0f);
                m_X += m_DX;
                if (m_pParent.GetHit((int)Math.Floor((m_X - 16) / 32), (int)Math.Floor(m_Y / 32), Hit.Right))
                {
                    m_DX = 0;
                    m_X = (float)Math.Floor(m_X / 32) * 32 + 16;
                }
                else if(m_pParent.GetHit((int)Math.Floor((m_X + 16) / 32), (int)Math.Floor(m_Y / 32), Hit.Left)) {
                    m_DX = 0;
                    m_X = (float)Math.Floor(m_X / 32) * 32 + 16;
                }
                m_Y += m_DY;
                if (Math.Floor(m_Y / 32) != Math.Floor((m_Y - m_DY) / 32))
                {
                    // 32ドット境界をまたいだ！！
                    if (m_pParent.GetHit((int)Math.Floor(m_X / 32), (int)Math.Floor(m_Y / 32), Hit.Top)
                        || (int)Math.Floor(m_Y / 32) >= m_pParent.GetHeight())
                    {
                        Seed();
                    }
                    else
                    {
                        if (m_pParent.GetHit((int)Math.Floor((m_X - 16) / 32), (int)Math.Floor(m_Y / 32), Hit.Right))
                        {
                            m_DX = 0;
                            m_X = (float)Math.Floor(m_X / 32) * 32 + 16;
                        }
                        else if(m_pParent.GetHit((int)Math.Floor((m_X + 16) / 32), (int)Math.Floor(m_Y / 32), Hit.Left)) {
                            m_DX = 0;
                            m_X = (float)Math.Floor(m_X / 32) * 32 + 16;
                        }
                    }
                }
            }
            else if (m_State == EelPitcherState.EELBUD) {
                if (--m_Delay == 0) m_State = EelPitcherState.EELLEAF;
            }
            else if (m_State == EelPitcherState.EELDEAD) {
                Kill();
            }
        }

        public MapObjectEelPitcher(int nCX, int nCY) : base(MapObjectType.EelPitcher)
        {
            m_Delay = 0;
            m_Level = 1;
            m_DX = 0;
            m_DY = 0;
            m_State = EelPitcherState.EELSEED;
            m_bBlinking = false;
            m_EnemyList[GetID()] = this;
            SetPos(nCX * 32 + 16, nCY * 32 + 16);
        }

        public override void Dispose()
        {
            m_EnemyList.Remove(GetID());
            base.Dispose();
        }
    }
}
