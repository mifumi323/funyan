class Cf3MapObjectIce : public Cf3MapObjectIceBase  
{
protected:
	static set<Cf3MapObjectIce*> Cf3MapObjectIce::m_IceList;

float m_DX, m_DY;
int m_Life;
public:
	int GetSize();
static set<Cf3MapObjectIce*>::iterator IteratorBegin() { return m_IceList.begin(); }
static set<Cf3MapObjectIce*>::iterator IteratorEnd() { return m_IceList.end(); }
static void OnPreDrawAll();
static void SynergyAll();
static void OnMoveAll();
static void OnDrawAll(CDIB32* lp);
void OnDraw(CDIB32* lp);
void OnMove();
Cf3MapObjectIce(float x, float y, float dx, float dy);
virtual ~Cf3MapObjectIce();

};
