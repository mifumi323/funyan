using System;
using System.Collections.Generic;

namespace MifuminSoft.funyan.Core
{
    public class Cf3MapObjectNeedle : Cf3MapObjectBase, IDisposable
    {
        protected void Reaction(Cf3MapObjectBase* obj)
        {
            if (obj == null) return;
            obj->GetPos(out var objX, out var objY);
            switch (obj->GetMapObjectType())
            {
                case MOT_FUNYA:
                case MOT_GEASPRIN:
                    {
                        if (m_Type != NDT_VERTICAL || m_State != NDS_STOP || m_Speed != 0) return;
                        if (TL.IsIn(m_X - 16, objX, m_X + 16))
                        {
                            if (TL.IsIn(m_Y + 15, objY, m_Y + 271))
                            {
                                m_State = NDS_DOWN;
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
        protected CDIB32 m_Graphic;
        protected static HashSet<Cf3MapObjectNeedle> m_EnemyList = new HashSet<Cf3MapObjectNeedle>();

        protected float m_StartY;
        protected float m_Speed;
        protected enum NDT
        {
            NDT_UNDEFINED,
            NDT_HORIZONTAL,
            NDT_VERTICAL,
            NDT_DEAD,
        }
        protected NDT m_Type;
        protected enum NDS
        {
            NDS_WAIT,
            NDS_STOP,
            NDS_LEFT,
            NDS_RIGHT,
            NDS_DOWN,
            NDS_UP,
        }
        protected NDS m_State;

        public static void OnDrawAll(CDIB32* lp)
        {
            int sx, sy, ex, ey;
            sx = sy = 0;
            m_pParent->GetViewPos(ref sx, ref sy);
            sx = (-sx) >> 5; sy = (-sy) >> 5;
            ex = sx + 320 / 32; ey = sy + 224 / 32;
            TL.Saturate(sx, ref ex, m_pParent->GetWidth() - 1);
            TL.Saturate(sy, ref ey, m_pParent->GetHeight() - 1);
            for (Cf3MapObjectBase** it = m_pParent->GetMapObjects(sx, sy, ex, ey, MOT_NEEDLE); (*it) != null; it++)
            {
                if ((*it)->IsValid()) (*it)->OnDraw(lp);
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
        public static IEnumerable<Cf3MapObjectNeedle> All() { return m_EnemyList; }
        public void Synergy()
        {
            if (!IsValid()) return;
            Cf3MapObjectBase** it;
            for (it = m_pParent->GetMapObjects(m_nCX - 1, m_nCY, m_nCX + 1, m_nCY + 10, MOT_FUNYA); (*it) != null; it++)
            {
                if ((*it)->IsValid()) Reaction((*it));
            }
            for (it = m_pParent->GetMapObjects(m_nCX - 1, m_nCY, m_nCX + 1, m_nCY + 10, MOT_GEASPRIN); (*it) != null; it++)
            {
                if ((*it)->IsValid()) Reaction((*it));
            }
            for (it = m_pParent->GetMapObjects(m_nCX - 1, m_nCY - 1, m_nCX + 1, m_nCY + 1, MOT_EELPITCHER); (*it) != null; it++)
            {
                if ((*it)->IsValid())
                {
                    (*it)->GetPos(out var objX, out var objY);
                    if (TL.IsIn(objX - 16, m_X, objX + 16))
                    {
                        if (TL.IsIn(objY, m_Y, objY + 40))
                        {
                            // 食べられちゃった！！
                            m_Type = NDT_DEAD;
                            new Cf3MapObjectEffect(m_X, m_Y, 0);
                        }
                    }
                }
            }
        }
        public virtual void OnMove()
        {
            if (!IsValid()) return;
            if (m_Type == NDT_UNDEFINED)
            {
                // このタイミングで初期化
                if (m_pParent->GetHit((int)Math.Floor(m_X / 32), (int)Math.Floor((m_Y + 16) / 32), HIT.HIT_TOP))
                {
                    m_Type = NDT_HORIZONTAL;
                    m_State = NDS_STOP;
                }
                else
                {
                    m_Type = NDT_VERTICAL;
                    m_StartY = m_Y;
                    m_State = NDS_STOP;
                }
            }
            if (m_Type == NDT_HORIZONTAL)
            {
                if (m_State == NDS_STOP)
                {
                    TL.BringClose(ref m_Speed, 0.0f, 1.0f);
                    if (m_Speed == 0)
                    {
                        if (!m_pParent->GetHit((int)Math.Floor((m_X + 15) / 32), (int)Math.Floor((m_Y + 16) / 32), HIT.HIT_TOP) ||
                            m_pParent->GetHit((int)Math.Floor((m_X + 15) / 32), (int)Math.Floor((m_Y) / 32), HIT.HIT_LEFT))
                        {
                            m_State = NDS_LEFT;
                        }
                        else
                        {
                            m_State = NDS_RIGHT;
                        }
                    }
                }
                else if (m_State == NDS_LEFT)
                {
                    m_X -= 1;
                    if (!m_pParent->GetHit((int)Math.Floor((m_X - 16) / 32), (int)Math.Floor((m_Y + 16) / 32), HIT.HIT_TOP) ||
                        m_pParent->GetHit((int)Math.Floor((m_X - 16) / 32), (int)Math.Floor((m_Y) / 32), HIT.HIT_RIGHT))
                    {
                        m_State = NDS_STOP;
                        m_Speed = 20;
                    }
                }
                else if (m_State == NDS_RIGHT)
                {
                    m_X += 1;
                    if (!m_pParent->GetHit((int)Math.Floor((m_X + 15) / 32), (int)Math.Floor((m_Y + 16) / 32), HIT.HIT_TOP) ||
                        m_pParent->GetHit((int)Math.Floor((m_X + 15) / 32), (int)Math.Floor((m_Y) / 32), HIT.HIT_LEFT))
                    {
                        m_State = NDS_STOP;
                        m_Speed = 20;
                    }
                }
            }
            else if (m_Type == NDT_VERTICAL)
            {
                if (m_State == NDS_STOP)
                {
                    if (m_Speed != 0)
                    {
                        TL.BringClose(ref m_Speed, 0.0f, 1.0f);
                        if (m_Speed == 0)
                        {
                            m_State = NDS_UP;
                        }
                    }
                }
                else if (m_State == NDS_UP)
                {
                    TL.BringClose(ref m_Y, m_StartY, 1.0f);
                    if (m_Y == m_StartY)
                    {
                        m_State = NDS_STOP;
                    }
                }
                else if (m_State == NDS_DOWN)
                {
                    m_Speed += 0.2f;
                    TL.Saturate(0.0f, ref m_Speed, 10.0f);
                    m_Y += m_Speed;
                    if (m_pParent->GetHit((int)Math.Floor(m_X / 32), (int)Math.Floor((m_Y + 16) / 32), HIT.HIT_TOP))
                    {
                        m_Y = (float)Math.Floor((m_Y + 16) / 32) * 32 - 15;
                        m_Speed = 20;
                        m_State = NDS_STOP;
                    }
                    else if (m_Y > m_pParent->GetHeight() * 32 + 16)
                    {
                        m_Type = NDT_DEAD;
                        new Cf3MapObjectEffect(m_X, m_Y, 1);
                    }
                }
            }
            else if (m_Type == NDT_DEAD)
            {
                Kill();
            }
        }
        public virtual void OnDraw(CDIB32* lp)
        {
            if (!IsValid()) return;
            SetViewPos(-15, -15);
            lp->Blt(m_Graphic, m_nVX, m_nVY);
        }
        public Cf3MapObjectNeedle(int nCX, int nCY, int nType = 0) : base(f3MapObjectType.MOT_NEEDLE)
        {
            m_EnemyList.Add(this);
            m_Graphic = CResourceManager.ResourceManager.Get(RID.RID_NEEDLE);
            SetPos(nCX * 32 + 16, nCY * 32 + 17);
            switch (nType)
            {
                case 1:
                    m_Type = NDT_HORIZONTAL;
                    m_State = NDS_LEFT;
                    break;
                case 2:
                    m_Type = NDT_VERTICAL;
                    m_StartY = m_Y;
                    m_State = NDS_STOP;
                    break;
                case 3:
                    m_Type = NDT_HORIZONTAL;
                    m_State = NDS_RIGHT;
                    break;
                default:
                    m_Type = NDT_UNDEFINED;
                    break;
            }
            m_Speed = 0;
        }
        public override void Dispose()
        {
            m_EnemyList.Remove(this);
            base.Dispose();
        }

    }
}
