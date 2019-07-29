using System;
using System.Collections.Generic;

namespace MifuminSoft.funyan.Core
{
    public class MapObjectGeasprin:MapObjectBase
    {
        private const int WalkDelay = 10;

        protected void Freeze()
        {
            m_Delay = 200;
            m_State = GeasprinState.FROZEN;
        }

        protected void Back(MapObjectDirection dir)
        {
            if (dir != MapObjectDirection.Front) m_Direction = dir;
            m_Delay = 1;
            m_State = GeasprinState.BACK;
        }

        protected void Jump()
        {
            m_DY = -60;
            m_State = GeasprinState.FALLING;
        }

        protected void Blow(MapObjectDirection dir = MapObjectDirection.Front)
        {
            if (dir != MapObjectDirection.Front) m_Direction = dir;
            m_Spring[(int)m_Direction] = 32;
            m_Delay = 20;
            m_State = GeasprinState.BLOWN;
        }

        protected void Laugh()
        {
            m_Delay = 80;
            m_State = GeasprinState.LAUGHING;
        }

        protected void Stop()
        {
            if (m_pParent.GetHit((int)Math.Floor(m_X / 32), (int)Math.Floor((m_Y + 17) / 32), Hit.Top))
            {
                m_Delay = 40;
                m_State = GeasprinState.STANDING;
            }
            else
            {
                m_Delay = 40;
                m_DY = 0;
                m_State = GeasprinState.FALLING;
            }
        }

        protected void Walk()
        {
            m_Delay = WalkDelay;
            m_State = GeasprinState.WALKING;
            m_Direction = m_Direction == MapObjectDirection.Left ? MapObjectDirection.Right : MapObjectDirection.Left;
        }

        protected static Dictionary<int, MapObjectGeasprin> m_EnemyList;

        protected enum GeasprinState
        {
            STANDING,   // 立ち
            WALKING,    // 歩き
            FALLING,    // 落ち
            LAUGHING,   // 笑い
            BLOWN,      // 飛び
            BACK,       // 後ずさり
            FROZEN,     // 凍った
            DEAD,
        }
        protected GeasprinState m_State;
        protected int m_GX, m_GY;
        protected int m_DY;
        protected MapObjectDirection m_Direction;
        protected int m_Delay;
        protected uint[] m_Spring = new uint[3];
        protected uint[] m_Spring2 = new uint[3];

        public bool IsFrozen() { return m_State == GeasprinState.FROZEN; }
        public static void SynergyAll()
        {
            foreach (var it in m_EnemyList)
            {
                it.Value.Synergy();
            }
        }

        public static void OnMoveAll()
        {
            foreach (var it in m_EnemyList)
            {
                it.Value.OnMove();
            }
        }

        public static Dictionary<int, MapObjectGeasprin> All() { return m_EnemyList; }
        public void Reaction(MapObjectBase obj)
        {
            obj.GetPos(out var objX, out var objY);
            switch (obj.GetMapObjectType())
            {
                case MapObjectType.funya:
                    {
                        if (TL.IsIn(m_X - 16, objX, m_X + 16))
                        {
                            if (TL.IsIn(m_Y - 32, objY, m_Y))
                            {
                                // 踏まれた！
                                Laugh();
                                m_Spring[(int)MapObjectDirection.Front] = 32;
                                if (!m_pParent.GetHit((int)Math.Floor(m_X / 32), (int)Math.Floor((m_Y + 16) / 32), Hit.Top)) m_GY++;
                            }
                        }
                        else if(TL.IsIn(m_X + 16, objX, m_X + 29)) {
                            if (TL.IsIn(m_Y - 16, objY, m_Y + 16))
                            {
                                // 右から来た！
                                if (m_pParent.GetHit((int)Math.Floor(m_X / 32), (int)Math.Floor((m_Y + 16) / 32), Hit.Top)) Back(MapObjectDirection.Right);
                                else Laugh();
                                m_Spring[(int)MapObjectDirection.Right] = 32;
                            }
                        }
                        else if (TL.IsIn(m_X - 29, objX, m_X - 16)) {
                            if (TL.IsIn(m_Y - 16, objY, m_Y + 16))
                            {
                                // 左から来た！
                                if (m_pParent.GetHit((int)Math.Floor(m_X / 32), (int)Math.Floor((m_Y + 16) / 32), Hit.Top)) Back(MapObjectDirection.Left);
                                else Laugh();
                                m_Spring[(int)MapObjectDirection.Left] = 32;
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
            // ウナギカズラ
            foreach (var it in m_pParent.GetMapObjects(m_nCX - 2, m_nCY - 2, m_nCX + 2, m_nCY + 2, MapObjectType.EelPitcher))
            {
                if (it.IsValid())
                {
                    it.GetPos(out var objX, out var objY);
                    if (TL.IsIn(objX - 16, m_X, objX + 16))
                    {
                        if (TL.IsIn(objY, m_Y, objY + 40))
                        {
                            // 食べられちゃった！！
                            m_State = GeasprinState.DEAD;
                            new MapObjectEffect(m_X, m_Y, 0);
                            return;
                        }
                    }
                }
            }
            if (!IsFrozen())
            {
                // ふにゃ
                foreach (var it in m_pParent.GetMapObjects(m_nCX - 2, m_nCY - 2, m_nCX + 2, m_nCY + 2, MapObjectType.funya))
                {
                    if (it.IsValid()) Reaction(it);
                }
                // ギヤバネ
                foreach (var it in m_pParent.GetMapObjects(m_nCX - 2, m_nCY - 2, m_nCX + 2, m_nCY + 2, MapObjectType.Geasprin))
                {
                    if (it != this && it.IsValid())
                    {
                        it.GetPos(out var objX, out var objY);
                        if (TL.IsIn(m_X - 8, objX, m_X + 8))
                        {
                            if (TL.IsIn(m_Y - 32, objY, m_Y))
                            {
                                // 踏まれた！
                                Laugh();
                                m_Spring[(int)MapObjectDirection.Front] = 32;
                            }
                            else if(TL.IsIn(m_Y, objY, m_Y + 32)) {
                                // 踏んだ！
                                if (((MapObjectGeasprin)it).m_State != GeasprinState.FROZEN)
                                {
                                    Jump();
                                }
                                else
                                {
                                    m_Y = objY - 32.125f;
                                    m_GY = (int)(m_Y * 8);
                                    Freeze();
                                }
                            }
                        }
                        else if(TL.IsIn(m_X + 16, objX, m_X + 29)) {
                            if (TL.IsIn(m_Y - 16, objY, m_Y + 16))
                            {
                                // 右から来た！
                                if (((MapObjectGeasprin)it).m_State != GeasprinState.FROZEN)
                                {
                                    Blow(MapObjectDirection.Right);
                                    m_Spring[(int)MapObjectDirection.Right] = 32;
                                }
                                else
                                {
                                    m_X = objX - 40;
                                    m_GX = (int)(m_X / 8);
                                    Freeze();
                                }
                            }
                        }
                        else if (TL.IsIn(m_X - 29, objX, m_X - 16)) {
                            if (TL.IsIn(m_Y - 16, objY, m_Y + 16))
                            {
                                // 左から来た！
                                if (((MapObjectGeasprin)it).m_State != GeasprinState.FROZEN)
                                {
                                    Blow(MapObjectDirection.Left);
                                    m_Spring[(int)MapObjectDirection.Left] = 32;
                                }
                                else
                                {
                                    m_X = objX + 40;
                                    m_GX = (int)(m_X / 8);
                                    Freeze();
                                }
                            }
                        }
                    }
                }
                // 氷
                foreach ( var it in m_pParent.GetMapObjects(m_nCX - 2, m_nCY - 2, m_nCX + 2, m_nCY + 2, MapObjectType.Ice))
                {
                    if (it.IsValid())
                    {
                        it.GetPos(out var objX, out var objY);
                        if ((objX - m_X) * (objX - m_X) + (objY - m_Y) * (objY - m_Y) < 256)
                        {
                            // あたった！
                            Freeze();
                        }
                    }
                }
            }
        }

        public override void SetPos(float x, float y)
        {
            m_X = x;
            m_Y = y;
            m_GX = (int)Math.Floor(x / 8);
            m_GY = (int)Math.Floor(y * 8);
        }

        public override void OnMove()
        {
            if (!IsValid()) return;
            if (m_Delay != 0) m_Delay--;
            if (m_State == GeasprinState.WALKING)
            {
                if (m_Direction == MapObjectDirection.Left && m_pParent.GetHit((int)Math.Floor((m_X - 17) / 32), (int)Math.Floor(m_Y / 32), Hit.Right))
                {
                    Blow();
                }
                else if (m_Direction == MapObjectDirection.Right && m_pParent.GetHit((int)Math.Floor((m_X + 17) / 32), (int)Math.Floor(m_Y / 32), Hit.Left)){
                    Blow();
                }
                else if (!m_pParent.GetHit((int)Math.Floor((m_X + 17 * (m_Direction == MapObjectDirection.Left ? -1 : 1)) / 32), (int)Math.Floor((m_Y + 17) / 32), Hit.Top)){
                    Stop();
                }else
                {
                    if (m_Delay == 0)
                    {
                        m_Delay = WalkDelay;
                        m_GX += (m_Direction == MapObjectDirection.Left ? -1 : 1);
                    }
                }
            }
            else if (m_State == GeasprinState.STANDING) {
                if (!m_pParent.GetHit((int)Math.Floor(m_X / 32), (int)Math.Floor((m_Y + 17) / 32), Hit.Top))
                {
                    Stop();
                }
                else
                {
                    if (m_Delay == 0)
                    {
                        Walk();
                    }
                }
            }
            else if (m_State == GeasprinState.FALLING) {
                if (m_Delay == 0)
                {
                    m_DY += 2;
                    TL.Saturate(-60,ref m_DY, 60);
                    m_GY += m_DY;
                }
                m_X = m_GX * 8; m_Y = m_GY / 8;
                if (m_DY > 0 && m_pParent.GetHit((int)Math.Floor(m_X / 32),(int) Math.Floor((m_Y + 17) / 32), Hit.Top))
                {
                    m_GY = (int)(((Math.Floor((m_Y + 17) / 32) - 1) * 32 + 16) * 8);
                    Laugh();
                }
                else if (m_DY < 0 && m_pParent.GetHit((int)Math.Floor(m_X / 32), (int)Math.Floor((m_Y - 15) / 32), Hit.Bottom)){
                    m_GY = (m_GY + 127) & ~127;
                    m_DY = 0;
                }
            }
            else if (m_State == GeasprinState.LAUGHING) {
                if (m_Delay == 0)
                {
                    Stop();
                }
            }
            else if (m_State == GeasprinState.BLOWN) {
                if (m_Direction == MapObjectDirection.Left && !m_pParent.GetHit((int)Math.Floor((m_X + 17) / 32), (int)Math.Floor(m_Y / 32), Hit.Left))
                {
                    m_GX++;
                }
                else if (m_Direction == MapObjectDirection.Right && !m_pParent.GetHit((int)Math.Floor((m_X - 17) / 32), (int)Math.Floor(m_Y / 32), Hit.Right)){
                    m_GX--;
                }
                if (m_Delay == 0)
                {
                    Stop();
                }
            }
            else if (m_State == GeasprinState.BACK) {
                if (!m_pParent.GetHit((int)Math.Floor((m_X + 17 * (m_Direction != MapObjectDirection.Left ? -1 : 1)) / 32), (int)Math.Floor((m_Y + 17) / 32), Hit.Top))
                {
                }
                else if (m_Direction == MapObjectDirection.Left && !m_pParent.GetHit((int)Math.Floor((m_X + 17) / 32), (int)Math.Floor(m_Y / 32), Hit.Left)){
                    m_GX++;
                }
                else if (m_Direction == MapObjectDirection.Right && !m_pParent.GetHit((int)Math.Floor((m_X - 17) / 32), (int)Math.Floor(m_Y / 32), Hit.Right)){
                    m_GX--;
                }
                if (m_Delay == 0)
                {
                    Laugh();
                }
            }
            else if (m_State == GeasprinState.FROZEN) {
                if (m_Delay == 0)
                {
                    Stop();
                }
            }
            else if (m_State == GeasprinState.DEAD) {
                Kill();
            }
            if (m_Spring[(int)MapObjectDirection.Front] != 0) m_Spring2[(int)MapObjectDirection.Front] = (uint)App.Random((int)m_Spring[(int)MapObjectDirection.Front]);
            if (m_Spring[(int)MapObjectDirection.Left] != 0) m_Spring2[(int)MapObjectDirection.Left] = (uint)App.Random((int)m_Spring[(int)MapObjectDirection.Left]);
            if (m_Spring[(int)MapObjectDirection.Right] != 0) m_Spring2[(int)MapObjectDirection.Right] = (uint)App.Random((int)m_Spring[(int)MapObjectDirection.Right]);
            m_X = m_GX * 8; m_Y = m_GY / 8;
            if (m_Y > m_pParent.GetHeight() * 32 + 16)
            {
                Kill();
                new MapObjectEffect(m_X, m_Y, 1);
            }
        }

        public MapObjectGeasprin(int nCX, int nCY, MapObjectDirection direction = MapObjectDirection.Left):base(MapObjectType.Geasprin)
        {
            m_EnemyList[GetID()]= this;
            SetPos(nCX * 32 + 16, nCY * 32 + 16);
            m_Direction = direction;
            m_Spring[(int)MapObjectDirection.Front] =
            m_Spring[(int)MapObjectDirection.Left] =
            m_Spring[(int)MapObjectDirection.Right] =
            m_Spring2[(int)MapObjectDirection.Front] =
            m_Spring2[(int)MapObjectDirection.Left] =
            m_Spring2[(int)MapObjectDirection.Right] = 0;
            Stop();
        }

        public override void Dispose()
        {
            m_EnemyList.Remove(GetID());
            base.Dispose();
        }
    }
}
