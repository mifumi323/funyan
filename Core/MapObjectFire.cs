namespace MifuminSoft.funyan.Core
{
class Cf3MapObjectFire : public Cf3MapObjectIceBase  
{
private:
	static set<Cf3MapObjectFire*> m_FireList;

int m_Phase;
int m_Size;
int m_Delay;
public:
	void Synergy();
bool IsActive() { return m_Delay == 0; }
static set<Cf3MapObjectFire*>::iterator IteratorBegin() { return m_FireList.begin(); }
static set<Cf3MapObjectFire*>::iterator IteratorEnd() { return m_FireList.end(); }
void OnPreDraw();
void OnMove();
void OnDraw(CDIB32* lp);
int GetSize();
static void SynergyAll();
static void OnPreDrawAll();
static void OnMoveAll();
static void OnDrawAll(CDIB32* lp);
Cf3MapObjectFire(int x, int y);
virtual ~Cf3MapObjectFire();

};
}