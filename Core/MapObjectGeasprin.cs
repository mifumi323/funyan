using System;
using System.Collections.Generic;

namespace MifuminSoft.funyan.Core
{
    public class Cf3MapObjectGeasprin : Cf3MapObjectBase
    {
        protected const int WalkDelay = 10;
        protected void Freeze()
        {
            m_Delay = 200;
            m_State = FROZEN;
        }
        protected void Back(f3MapObjectDirection dir)
        {
            if (dir != DIR_FRONT) m_Direction = dir;
            m_Delay = 1;
            m_State = BACK;
        }
        protected void Jump()
        {
            m_DY = -60;
            m_State = FALLING;
        }
        protected void Blow(f3MapObjectDirection dir = DIR_FRONT)
        {
            if (dir != DIR_FRONT) m_Direction = dir;
            m_Spring[m_Direction] = 32;
            m_Delay = 20;
            m_State = BLOWN;
        }
        protected void Laugh()
        {
            m_Delay = 80;
            m_State = LAUGHING;
        }
        protected void Stop()
        {
            if (m_pParent->GetHit((int)Math.Floor(m_X / 32), (int)Math.Floor((m_Y + 17) / 32), HIT.HIT_TOP))
            {
                m_Delay = 40;
                m_State = STANDING;
            }
            else
            {
                m_Delay = 40;
                m_DY = 0;
                m_State = FALLING;
            }
        }
        protected void Walk()
        {
            m_Delay = WalkDelay;
            m_State = WALKING;
            m_Direction = (m_Direction == DIR_LEFT ? DIR_RIGHT : DIR_LEFT);
        }
        protected CDIB32* m_Graphic;
        protected static Dictionary<int, Cf3MapObjectGeasprin> m_EnemyList = new Dictionary<int, Cf3MapObjectGeasprin>();

        protected enum f3GeasprinState
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
        protected f3GeasprinState m_State;
        int m_GX, m_GY;
        protected int m_DY;
        protected f3MapObjectDirection m_Direction;
        protected int m_Delay;
        protected int[] m_Spring = new int[3];
        protected int[] m_Spring2 = new int[3];

        public bool IsFrozen() { return m_State == FROZEN; }
        public void OnPreDraw()
        {
            if (m_Spring[DIR_FRONT])
            {
                if (m_Spring[DIR_FRONT] > 32) m_Spring[DIR_FRONT] = 32;
                m_Spring[DIR_FRONT]--;
            }
            if (m_Spring[DIR_LEFT])
            {
                if (m_Spring[DIR_LEFT] > 32) m_Spring[DIR_LEFT] = 32;
                m_Spring[DIR_LEFT]--;
            }
            if (m_Spring[DIR_RIGHT])
            {
                if (m_Spring[DIR_RIGHT] > 32) m_Spring[DIR_RIGHT] = 32;
                m_Spring[DIR_RIGHT]--;
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
            for (Cf3MapObjectBase** it = m_pParent->GetMapObjects(sx - 1, sy - 1, ex + 1, ey + 1, MOT_GEASPRIN); (*it) != null; it++)
            {
                if ((*it)->IsValid()) (*it)->OnDraw(lp);
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
        public static Dictionary<int, Cf3MapObjectGeasprin> All() { return m_EnemyList; }
        public void Reaction(Cf3MapObjectBase* obj)
        {
            if (obj == null) return;
            obj->GetPos(out var objX, out var objY);
            switch (obj->GetMapObjectType())
            {
                case MOT_FUNYA:
                    {
                        if (TL.IsIn(m_X - 16, objX, m_X + 16))
                        {
                            if (TL.IsIn(m_Y - 32, objY, m_Y))
                            {
                                // 踏まれた！
                                Laugh();
                                m_Spring[DIR_FRONT] = 32;
                                if (!m_pParent->GetHit((int)Math.Floor(m_X / 32), (int)Math.Floor((m_Y + 16) / 32), HIT.HIT_TOP)) m_GY++;
                            }
                        }
                        else if (TL.IsIn(m_X + 16, objX, m_X + 29))
                        {
                            if (TL.IsIn(m_Y - 16, objY, m_Y + 16))
                            {
                                // 右から来た！
                                if (m_pParent->GetHit((int)Math.Floor(m_X / 32), (int)Math.Floor((m_Y + 16) / 32), HIT.HIT_TOP)) Back(DIR_RIGHT);
                                else Laugh();
                                m_Spring[DIR_RIGHT] = 32;
                            }
                        }
                        else if (TL.IsIn(m_X - 29, objX, m_X - 16))
                        {
                            if (TL.IsIn(m_Y - 16, objY, m_Y + 16))
                            {
                                // 左から来た！
                                if (m_pParent->GetHit((int)Math.Floor(m_X / 32), (int)Math.Floor((m_Y + 16) / 32), HIT.HIT_TOP)) Back(DIR_LEFT);
                                else Laugh();
                                m_Spring[DIR_LEFT] = 32;
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
        public void Synergy()
        {
            if (!IsValid()) return;
            Cf3MapObjectBase** it;
            // ウナギカズラ
            for (it = m_pParent->GetMapObjects(m_nCX - 2, m_nCY - 2, m_nCX + 2, m_nCY + 2, MOT_EELPITCHER); (*it) != null; it++)
            {
                if ((*it)->IsValid())
                {
                    (*it)->GetPos(out var objX, out var objY);
                    if (TL.IsIn(objX - 16, m_X, objX + 16))
                    {
                        if (TL.IsIn(objY, m_Y, objY + 40))
                        {
                            // 食べられちゃった！！
                            m_State = DEAD;
                            new Cf3MapObjectEffect(m_X, m_Y, 0);
                            return;
                        }
                    }
                }
            }
            if (!IsFrozen())
            {
                // ふにゃ
                for (it = m_pParent->GetMapObjects(m_nCX - 2, m_nCY - 2, m_nCX + 2, m_nCY + 2, MOT_FUNYA); (*it) != null; it++)
                {
                    if ((*it)->IsValid()) Reaction((*it));
                }
                // ギヤバネ
                for (it = m_pParent->GetMapObjects(m_nCX - 2, m_nCY - 2, m_nCX + 2, m_nCY + 2, MOT_GEASPRIN); (*it) != null; it++)
                {
                    if ((*it) != this && (*it)->IsValid())
                    {
                        (*it)->GetPos(out var objX, out var objY);
                        if (TL.IsIn(m_X - 8, objX, m_X + 8))
                        {
                            if (TL.IsIn(m_Y - 32, objY, m_Y))
                            {
                                // 踏まれた！
                                Laugh();
                                m_Spring[DIR_FRONT] = 32;
                            }
                            else if (TL.IsIn(m_Y, objY, m_Y + 32))
                            {
                                // 踏んだ！
                                if (((Cf3MapObjectGeasprin*)(*it))->m_State != FROZEN)
                                {
                                    Jump();
                                }
                                else
                                {
                                    m_Y = objY - 32.125f;
                                    m_GY = m_Y * 8;
                                    Freeze();
                                }
                            }
                        }
                        else if (TL.IsIn(m_X + 16, objX, m_X + 29))
                        {
                            if (TL.IsIn(m_Y - 16, objY, m_Y + 16))
                            {
                                // 右から来た！
                                if (((Cf3MapObjectGeasprin*)(*it))->m_State != FROZEN)
                                {
                                    Blow(DIR_RIGHT);
                                    m_Spring[DIR_RIGHT] = 32;
                                }
                                else
                                {
                                    m_X = objX - 40;
                                    m_GX = m_X / 8;
                                    Freeze();
                                }
                            }
                        }
                        else if (TL.IsIn(m_X - 29, objX, m_X - 16))
                        {
                            if (TL.IsIn(m_Y - 16, objY, m_Y + 16))
                            {
                                // 左から来た！
                                if (((Cf3MapObjectGeasprin*)(*it))->m_State != FROZEN)
                                {
                                    Blow(DIR_LEFT);
                                    m_Spring[DIR_LEFT] = 32;
                                }
                                else
                                {
                                    m_X = objX + 40;
                                    m_GX = m_X / 8;
                                    Freeze();
                                }
                            }
                        }
                    }
                }
                // 氷
                for (it = m_pParent->GetMapObjects(m_nCX - 2, m_nCY - 2, m_nCX + 2, m_nCY + 2, MOT_ICE); (*it) != null; it++)
                {
                    if ((*it)->IsValid())
                    {
                        (*it)->GetPos(out var objX, out var objY);
                        if ((objX - m_X) * (objX - m_X) + (objY - m_Y) * (objY - m_Y) < 256)
                        {
                            // あたった！
                            Freeze();
                        }
                    }
                }
            }
        }
        public void SetPos(float x, float y)
        {
            m_X = x; m_Y = y;
            m_GX = (int)Math.Floor(x / 8); m_GY = (int)Math.Floor(y * 8);
        }
        public void OnMove()
        {
            if (!IsValid()) return;
            if (m_Delay) m_Delay--;
            if (m_State == WALKING)
            {
                if (m_Direction == DIR_LEFT && m_pParent->GetHit((int)Math.Floor((m_X - 17) / 32), (int)Math.Floor(m_Y / 32), HIT.HIT_RIGHT))
                {
                    Blow();
                }
                else if (m_Direction == DIR_RIGHT && m_pParent->GetHit((int)Math.Floor((m_X + 17) / 32), (int)Math.Floor(m_Y / 32), HIT.HIT_LEFT))
                {
                    Blow();
                }
                else if (!m_pParent->GetHit((int)Math.Floor((m_X + 17 * (m_Direction == DIR_LEFT ? -1 : 1)) / 32), (int)Math.Floor((m_Y + 17) / 32), HIT.HIT_TOP))
                {
                    Stop();
                }
                else
                {
                    if (m_Delay == 0)
                    {
                        m_Delay = WalkDelay;
                        m_GX += (m_Direction == DIR_LEFT ? -1 : 1);
                    }
                }
            }
            else if (m_State == STANDING)
            {
                if (!m_pParent->GetHit((int)Math.Floor(m_X / 32), (int)Math.Floor((m_Y + 17) / 32), HIT.HIT_TOP))
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
            else if (m_State == FALLING)
            {
                if (m_Delay == 0)
                {
                    m_DY += 2;
                    TL.Saturate(-60, ref m_DY, 60);
                    m_GY += m_DY;
                }
                m_X = m_GX * 8; m_Y = m_GY / 8;
                if (m_DY > 0 && m_pParent->GetHit((int)Math.Floor(m_X / 32), (int)Math.Floor((m_Y + 17) / 32), HIT.HIT_TOP))
                {
                    m_GY = (((int)Math.Floor((m_Y + 17) / 32) - 1) * 32 + 16) * 8;
                    Laugh();
                }
                else if (m_DY < 0 && m_pParent->GetHit((int)Math.Floor(m_X / 32), (int)Math.Floor((m_Y - 15) / 32), HIT.HIT_BOTTOM))
                {
                    m_GY = (m_GY + 127) & ~127;
                    m_DY = 0;
                }
            }
            else if (m_State == LAUGHING)
            {
                if (m_Delay == 0)
                {
                    Stop();
                }
            }
            else if (m_State == BLOWN)
            {
                if (m_Direction == DIR_LEFT && !m_pParent->GetHit((int)Math.Floor((m_X + 17) / 32), (int)Math.Floor(m_Y / 32), HIT.HIT_LEFT))
                {
                    m_GX++;
                }
                else if (m_Direction == DIR_RIGHT && !m_pParent->GetHit((int)Math.Floor((m_X - 17) / 32), (int)Math.Floor(m_Y / 32), HIT.HIT_RIGHT))
                {
                    m_GX--;
                }
                if (m_Delay == 0)
                {
                    Stop();
                }
            }
            else if (m_State == BACK)
            {
                if (!m_pParent->GetHit((int)Math.Floor((m_X + 17 * (m_Direction != DIR_LEFT ? -1 : 1)) / 32), (int)Math.Floor((m_Y + 17) / 32), HIT.HIT_TOP))
                {
                }
                else if (m_Direction == DIR_LEFT && !m_pParent->GetHit((int)Math.Floor((m_X + 17) / 32), (int)Math.Floor(m_Y / 32), HIT.HIT_LEFT))
                {
                    m_GX++;
                }
                else if (m_Direction == DIR_RIGHT && !m_pParent->GetHit((int)Math.Floor((m_X - 17) / 32), (int)Math.Floor(m_Y / 32), HIT.HIT_RIGHT))
                {
                    m_GX--;
                }
                if (m_Delay == 0)
                {
                    Laugh();
                }
            }
            else if (m_State == FROZEN)
            {
                if (m_Delay == 0)
                {
                    Stop();
                }
            }
            else if (m_State == DEAD)
            {
                Kill();
            }
            if (m_Spring[DIR_FRONT]) m_Spring2[DIR_FRONT] = CApp::random(m_Spring[DIR_FRONT]);
            if (m_Spring[DIR_LEFT]) m_Spring2[DIR_LEFT] = CApp::random(m_Spring[DIR_LEFT]);
            if (m_Spring[DIR_RIGHT]) m_Spring2[DIR_RIGHT] = CApp::random(m_Spring[DIR_RIGHT]);
            m_X = m_GX * 8; m_Y = m_GY / 8;
            if (m_Y > m_pParent->GetHeight() * 32 + 16)
            {
                Kill();
                new Cf3MapObjectEffect(m_X, m_Y, 1);
            }
        }
        public void OnDraw(CDIB32* lp)
        {
            if (!IsValid()) return;
            SetViewPos(-16, -16);
            // バネ
            if (m_Spring2[DIR_FRONT])
            {
                int h = m_Spring2[DIR_FRONT];
                RECT rc = { 32, 64, 64, 64 + h, };
                lp->BltNatural(m_Graphic, m_nVX, m_nVY + 8 - h, &rc);
            }
            if (m_Spring2[DIR_LEFT])
            {
                int w = m_Spring2[DIR_LEFT];
                RECT rc = { 0, 64, w, 96, };
                lp->BltNatural(m_Graphic, m_nVX + 8 - w, m_nVY, &rc);
            }
            if (m_Spring2[DIR_RIGHT])
            {
                int w = m_Spring2[DIR_RIGHT];
                RECT rc = { 32 - w, 64, 32, 96, };
                lp->BltNatural(m_Graphic, m_nVX + 24, m_nVY, &rc);
            }
            // 本体
            int CX = 0, CY = (m_Direction == DIR_LEFT ? 0 : 1);
            if (m_State == STANDING)
            {
            }
            else if (m_State == WALKING)
            {
                CX = 1 + (m_GX & 1);
            }
            else if (m_State == FALLING)
            {
                CX = 5;
            }
            else if (m_State == LAUGHING)
            {
                if (TL.IsIn(20, m_Delay, 40))
                {
                    CX = 3 + (m_Delay / 4 & 1);
                }
                else if (TL.IsIn(0, m_Delay, 20))
                {
                    CX = 4;
                }
            }
            else if (m_State == BLOWN)
            {
                CX = 6;
            }
            else if (m_State == FROZEN)
            {
                CX = ((m_Delay < 40 && (m_Delay >> 1) & 1) ? 6 : 7);
            }
            RECT rc = { CX * 32, CY * 32, CX * 32 + 32, CY * 32 + 32, };
            lp->BltNatural(m_Graphic, m_nVX, m_nVY, &rc);
        }
        public Cf3MapObjectGeasprin(int nCX, int nCY, f3MapObjectDirection direction = f3MapObjectDirection.DIR_LEFT) : base(f3MapObjectType.MOT_GEASPRIN)
        {
            m_EnemyList.Add(GetID(), this);
            m_Graphic = CResourceManager.ResourceManager.Get(RID.RID_GEASPRIN);
            SetPos(nCX * 32 + 16, nCY * 32 + 16);
            m_Direction = direction;
            m_Spring[DIR_FRONT] = m_Spring[DIR_LEFT] = m_Spring[DIR_RIGHT] = m_Spring2[DIR_FRONT] = m_Spring2[DIR_LEFT] = m_Spring2[DIR_RIGHT] = 0;
            Stop();
        }
        public override void Dispose()
        {
            m_EnemyList.Remove(GetID());
            base.Dispose();
        }

    }
}
