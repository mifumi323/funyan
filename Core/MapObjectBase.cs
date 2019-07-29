using System;
using System.Collections.Generic;

namespace MifuminSoft.funyan.Core
{
    public class MapObjectBase:IDisposable
    {
        private bool m_bValid;
        private MapObjectType m_eType;
        private int m_nID;

        private static int m_nNextID;
        private static HashSet<MapObjectBase> m_CharaList = new HashSet<MapObjectBase>();

        protected static void RemoveCharaFromList(MapObjectBase lp) { m_CharaList.Remove(lp); }

        protected void SetViewPos(float offsetx = 0,float offsety = 0)
        {
            m_nVX = (int)(m_X + offsetx);
            m_nVY = (int)(m_Y + offsety);
            if (m_pParent != null) m_pParent.GetViewPos(ref m_nVX,ref m_nVY);
        }

        protected float m_X, m_Y;
        protected int m_nVX, m_nVY;
        protected int m_nCX, m_nCY;

        protected static Map m_pParent;

        public static int Count() { return m_CharaList.Count; }

        public static void UpdateCPosAll()
        {
            foreach (var it in m_CharaList)
            {
                if (it.m_bValid) it.UpdateCPos();
            }
        }

        public virtual void UpdateCPos()
        {
            GetCPos(out var cx,out var cy);
            if (m_nCX != cx || m_nCY != cy)
            {
                m_pParent.RemoveMapObject(m_nCX,m_nCY,this);
                m_pParent.AddMapObject(m_nCX = cx,m_nCY = cy,this);
            }
        }

        public static void KillAll()
        {
            foreach (var it in m_CharaList)
            {
                if (it.m_bValid) it.Kill();
            }
        }

        /// <summary>自発的相互作用(必ず自己完結すること)</summary>
        public virtual void Synergy() { }

        public virtual void GetCPos(out int x,out int y)
        {
            x = (int)Math.Floor(m_X / 32);
            y = (int)Math.Floor(m_Y / 32);
        }

        public float GetDistanceSquare(MapObjectBase obj)
        {
            obj.GetPos(out var x,out var y);
            return (x - m_X) * (x - m_X) + (y - m_Y) * (y - m_Y);
        }

        public static void Garbage()
        {
            foreach (var it in m_CharaList)
            {
                if (!it.m_bValid)
                {
                    // deleteの代替
                    it.Dispose();
                }
            }
        }

        public static void SetParent(Map lp) { m_pParent = lp; }

        public virtual void OnMove() { }

        public void Kill()
        {
            m_pParent.RemoveMapObject(m_nCX,m_nCY,this);
            m_bValid = false;
        }

        public bool IsValid() { return m_bValid; }

        public virtual void SetPos(float x,float y) { m_X = x; m_Y = y; }

        public void GetPos(out float x,out float y) { x = m_X; y = m_Y; }

        public int GetID() { return m_nID; }

        public MapObjectType GetMapObjectType() { return m_eType; }

        public MapObjectBase(MapObjectType eType)
        {
            m_bValid = true;
            m_eType = eType;
            m_X = 0;
            m_Y = 0;
            m_nCX = -1;
            m_nCY = -1;
            m_nID = m_nNextID++;
            m_pNext = null;
            m_CharaList.Add(this);
        }

        public virtual void Dispose()
        {
            m_pParent.RemoveMapObject(m_nCX,m_nCY,this);
            m_CharaList.Remove(this);
        }

        public MapObjectBase m_pNext;
    }
}
