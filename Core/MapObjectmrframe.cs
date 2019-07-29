class Cf3MapObjectmrframe : public Cf3MapObjectBase 
{
protected:
	CDIB32* m_Graphic;

Cf3MapObjectfunya* m_funya;
int m_nLife;
public:
	void UpdateCPos();
bool IsFrozen();
static set<Cf3MapObjectmrframe*> m_EnemyList;
static void OnDrawAll(CDIB32* lp);
static void OnPreDrawAll();
static void SynergyAll();
static void OnMoveAll();
static set<Cf3MapObjectmrframe*>::iterator IteratorBegin() { return m_EnemyList.begin(); }
static set<Cf3MapObjectmrframe*>::iterator IteratorEnd() { return m_EnemyList.end(); }
void Synergy();
void OnPreDraw();
void OnMove();
void OnDraw(CDIB32* lp);
Cf3MapObjectmrframe(int nCX, int nCY);
virtual ~Cf3MapObjectmrframe();

};
