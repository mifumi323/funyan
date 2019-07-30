namespace MifuminSoft.funyan.Core
{
class Cf3MapObjectEelPitcher : public Cf3MapObjectBase  
{
private:
	void Freeze() { m_State = EELFROZEN; m_Delay = 80; }
void Seed();
//	CDIB32* m_Graphic;
static map<int, Cf3MapObjectEelPitcher*> m_EnemyList;

f3MapObjectDirection m_Direction;
int m_Level;                        // 最大高さ
int m_Delay;                        // 待ち時間
enum f3EelPitcherState
{
    EELSEED,
    EELBUD,
    EELLEAF,
    EELFROZEN,
    EELDEAD,
}
m_State;
	float m_DX, m_DY;
float m_RootX, m_RootY;         // 根元
bool m_bBlinking;
public:
	bool IsLeaf() { return m_State == EELLEAF || m_State == EELFROZEN; }
static void OnDrawAll(CDIB32* lp);
static void OnPreDrawAll();
static void SynergyAll();
static void OnMoveAll();
static map<int, Cf3MapObjectEelPitcher*>::iterator IteratorBegin() { return m_EnemyList.begin(); }
static map<int, Cf3MapObjectEelPitcher*>::iterator IteratorEnd() { return m_EnemyList.end(); }
void Reaction(Cf3MapObjectBase* obj);
void Synergy();
void OnMove();
void OnDraw(CDIB32* lp);
Cf3MapObjectEelPitcher(int nCX, int nCY);
~Cf3MapObjectEelPitcher();

};
}