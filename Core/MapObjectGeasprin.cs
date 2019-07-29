﻿class Cf3MapObjectGeasprin : public Cf3MapObjectBase  
{
protected:
	void Freeze();
void Back(f3MapObjectDirection dir);
void Jump();
void Blow(f3MapObjectDirection dir = DIR_FRONT);
void Laugh();
void Stop();
void Walk();
CDIB32* m_Graphic;
static map<int, Cf3MapObjectGeasprin*> m_EnemyList;
//	static set<Cf3MapObjectGeasprin*> m_EnemyList;

enum f3GeasprinState
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
m_State;
	int m_GX, m_GY;
int m_DY;
f3MapObjectDirection m_Direction;
int m_Delay;
unsigned int m_Spring[3];
unsigned int m_Spring2[3];
public:
	bool IsFrozen() { return m_State == FROZEN; }
void OnPreDraw();
static void OnDrawAll(CDIB32* lp);
static void OnPreDrawAll();
static void SynergyAll();
static void OnMoveAll();
static map<int, Cf3MapObjectGeasprin*>::iterator IteratorBegin() { return m_EnemyList.begin(); }
static map<int, Cf3MapObjectGeasprin*>::iterator IteratorEnd() { return m_EnemyList.end(); }
void Reaction(Cf3MapObjectBase* obj);
void Synergy();
void SetPos(float x, float y);
void OnMove();
void OnDraw(CDIB32* lp);
Cf3MapObjectGeasprin(int nCX, int nCY, f3MapObjectDirection direction = DIR_LEFT);
virtual ~Cf3MapObjectGeasprin();

};
