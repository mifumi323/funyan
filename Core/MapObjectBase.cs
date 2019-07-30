namespace MifuminSoft.funyan.Core
{
class Cf3MapObjectBase
{
    //	friend class Cf3MapObjectmrframe;
    //	friend class Cf3MapObjectEffect;
    private:
	bool m_bValid;
    f3MapObjectType m_eType;
    int m_nID;

    static int m_nNextID = 0;
    static MapObjectList m_CharaList;
    protected:
	static void RemoveCharaFromList(Cf3MapObjectBase* lp) { m_CharaList.erase(lp); }
    void SetViewPos(float offsetx = 0, float offsety = 0)
    {
        m_nVX = m_X+offsetx;
        m_nVY = m_Y+offsety;
        if (m_pParent!=NULL) m_pParent->GetViewPos(m_nVX, m_nVY);
    }
    //	void KillSimple() { m_bValid = false; }
    float m_X, m_Y;
    int m_nVX, m_nVY;
    //	float			m_nScrollX, m_nScrollY;	// スクロールにどれほど影響されるか(100%固定なので省略)
    int m_nCX, m_nCY;

    static Cf3Map* m_pParent = NULL;
    public:
	static int Count() { return m_CharaList.size(); }
    static void UpdateCPosAll()
    {
        for(MapObjectList::iterator it = m_CharaList.begin();it!=m_CharaList.end();it++){
            if ((*it)->m_bValid) (*it)->UpdateCPos();
        }
    }
    virtual void UpdateCPos()
    {
        int cx, cy;
        GetCPos(cx, cy);
        if (m_nCX!=cx||m_nCY!=cy) {
            m_pParent->RemoveMapObject(m_nCX, m_nCY, this);
            m_pParent->AddMapObject(m_nCX=cx, m_nCY=cy, this);
        }
    }
    static void KillAll()
    {
        for(MapObjectList::iterator it = m_CharaList.begin();it!=m_CharaList.end();it++){
            if ((*it)->m_bValid) (*it)->Kill();
        }
    }
    virtual void Synergy() { }              // 自発的相互作用(必ず自己完結すること)
    virtual void GetCPos(int& x, int& y)
    {
        x = floor(m_X/32); y = floor(m_Y/32);
    }
    float GetDistanceSquare(Cf3MapObjectBase& obj)
    {
        float x,y;
        obj.GetPos(x,y);
        return (x-m_X)*(x-m_X)+(y-m_Y)*(y-m_Y);
    }
    //	float GetDistance(Cf3MapObjectBase& obj);
    static void Garbage()
    {
        for(MapObjectList::iterator it = m_CharaList.begin();it!=m_CharaList.end();){
            if (!(*it)->IsValid()) {
                Cf3MapObjectBase* lp = *it;
                it++;
                DELETE_SAFE(lp);	//	イテレータはeraseするときにその要素を
                                    //	指していると不正になる
            } else {
                it++;
            }
        }
    }
    static void SetParent(Cf3Map* lp) { m_pParent = lp; }
    virtual void OnPreDraw() { }
    virtual void OnDraw(CDIB32* lp) =0;
	virtual void OnMove() { }
    void Kill()
    {
        m_pParent->RemoveMapObject(m_nCX, m_nCY, this);
        m_bValid = false;
    }
    bool IsValid() { return m_bValid; }
    void SetPos(float x, float y) { m_X = x; m_Y = y; }
    void GetPos(float&x, float&y) { x = m_X; y = m_Y; }

    int GetID() { return m_nID; }
    int GetType() { return m_eType; }

    Cf3MapObjectBase(f3MapObjectType eType):m_bValid(true)
	,m_eType(eType)
	,m_X(0) ,m_Y(0)
	,m_nCX(-1) ,m_nCY(-1)
	,m_nID(m_nNextID++)
	,m_pNext(NULL)
    {
        m_CharaList.insert(this);
    //	m_nScrollX = m_nScrollY = 1.0f;	// 標準でスクロールに完全についてゆく
    }
    virtual ~Cf3MapObjectBase()
    {
        m_pParent->RemoveMapObject(m_nCX, m_nCY, this);
        m_CharaList.erase(this);
    }

    Cf3MapObjectBase* m_pNext;

};
}