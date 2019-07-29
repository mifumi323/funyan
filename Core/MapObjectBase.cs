class Cf3MapObjectBase
{
    //	friend class Cf3MapObjectmrframe;
    //	friend class Cf3MapObjectEffect;
    private:
	bool m_bValid;
    f3MapObjectType m_eType;
    int m_nID;

    static int m_nNextID;
    static MapObjectList m_CharaList;
    protected:
	static void RemoveCharaFromList(Cf3MapObjectBase* lp) { m_CharaList.erase(lp); }
    void SetViewPos(float offsetx = 0, float offsety = 0);
    //	void KillSimple() { m_bValid = false; }
    float m_X, m_Y;
    int m_nVX, m_nVY;
    //	float			m_nScrollX, m_nScrollY;	// スクロールにどれほど影響されるか(100%固定なので省略)
    int m_nCX, m_nCY;

    static Cf3Map* m_pParent;
    public:
	static int Count() { return m_CharaList.size(); }
    static void UpdateCPosAll();
    virtual void UpdateCPos();
    static void KillAll();
    virtual void Synergy() { }              // 自発的相互作用(必ず自己完結すること)
    virtual void GetCPos(int& x, int& y);
    float GetDistanceSquare(Cf3MapObjectBase& obj);
    //	float GetDistance(Cf3MapObjectBase& obj);
    static void Garbage();
    static void SetParent(Cf3Map* lp) { m_pParent = lp; }
    virtual void OnPreDraw() { }
    virtual void OnDraw(CDIB32* lp) =0;
	virtual void OnMove() { }
    void Kill();
    bool IsValid() { return m_bValid; }
    void SetPos(float x, float y) { m_X = x; m_Y = y; }
    void GetPos(float&x, float&y) { x = m_X; y = m_Y; }

    int GetID() { return m_nID; }
    int GetType() { return m_eType; }

    Cf3MapObjectBase(f3MapObjectType eType);
    virtual ~Cf3MapObjectBase();

    Cf3MapObjectBase* m_pNext;

};
