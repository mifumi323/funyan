using System.Collections.Generic;

namespace MifuminSoft.funyan.Core
{
public class Cf3MapObjectBase
{
        private bool m_bValid;
        private f3MapObjectType m_eType;
        private int m_nID;

        private static int m_nNextID = 0;
        private static HashSet<Cf3MapObjectBase> m_CharaList = new HashSet<Cf3MapObjectBase>();

        protected static void RemoveCharaFromList(Cf3MapObjectBase lp) { m_CharaList.Remove(lp); }
        protected void SetViewPos(float offsetx = 0, float offsety = 0)
    {
        m_nVX = m_X+offsetx;
        m_nVY = m_Y+offsety;
        if (m_pParent!=NULL) m_pParent->GetViewPos(m_nVX, m_nVY);
    }
        //	void KillSimple() { m_bValid = false; }
        protected float m_X, m_Y;
        protected int m_nVX, m_nVY;
        //	float			m_nScrollX, m_nScrollY;	// スクロールにどれほど影響されるか(100%固定なので省略)
        protected int m_nCX, m_nCY;

        protected static Cf3Map m_pParent = null;

        public static int Count() { return m_CharaList.Count; }
        public static void UpdateCPosAll()
        {
            foreach (var it in m_CharaList) {
                if (it.m_bValid) it.UpdateCPos();
            }
        }
        public virtual void UpdateCPos()
    {
        int cx, cy;
        GetCPos(cx, cy);
        if (m_nCX!=cx||m_nCY!=cy) {
            m_pParent->RemoveMapObject(m_nCX, m_nCY, this);
            m_pParent->AddMapObject(m_nCX=cx, m_nCY=cy, this);
        }
    }
        public static void KillAll()
        {
            foreach (var it in m_CharaList) {
                if (it.m_bValid) it.Kill();
            }
        }
        public virtual void Synergy() { }              // 自発的相互作用(必ず自己完結すること)
        public virtual void GetCPos(int& x, int& y)
    {
        x = floor(m_X/32); y = floor(m_Y/32);
    }
        public float GetDistanceSquare(Cf3MapObjectBase& obj)
    {
        float x,y;
        obj.GetPos(x,y);
        return (x-m_X)*(x-m_X)+(y-m_Y)*(y-m_Y);
    }
        //	float GetDistance(Cf3MapObjectBase& obj);
        public static void Garbage()
    {
            m_CharaList.RemoveWhere(it => !it.IsValid());
    }
        public static void SetParent(Cf3Map* lp) { m_pParent = lp; }
        public virtual void OnPreDraw() { }
        public virtual void OnDraw(CDIB32* lp) =0;
	    public virtual void OnMove() { }
        public void Kill()
    {
        m_pParent->RemoveMapObject(m_nCX, m_nCY, this);
        m_bValid = false;
    }
        public bool IsValid() { return m_bValid; }
        public void SetPos(float x, float y) { m_X = x; m_Y = y; }
        public void GetPos(float&x, float&y) { x = m_X; y = m_Y; }

        public int GetID() { return m_nID; }
        public int GetType() { return m_eType; }

        public Cf3MapObjectBase(f3MapObjectType eType):m_bValid(true)
	,m_eType(eType)
	,m_X(0) ,m_Y(0)
	,m_nCX(-1) ,m_nCY(-1)
	,m_nID(m_nNextID++)
	,m_pNext(NULL)
    {
        m_CharaList.Add(this);
    //	m_nScrollX = m_nScrollY = 1.0f;	// 標準でスクロールに完全についてゆく
    }
        public virtual ~Cf3MapObjectBase()
    {
        m_pParent->RemoveMapObject(m_nCX, m_nCY, this);
        m_CharaList.Remove(this);
    }

    Cf3MapObjectBase m_pNext;

};
}
