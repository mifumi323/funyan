namespace MifuminSoft.funyan.Core
{
class Cf3GameInput
{
    public:
	virtual bool GetKeyPushed(int key)=0;
	virtual bool GetKeyPressed(int key)=0;
};

extern class Cf3GameInputKey : public Cf3GameInput
{
public:
	bool GetKeyPushed(int key) { return f3Input.GetKeyPushed(key); }
bool GetKeyPressed(int key) { return f3Input.GetKeyPressed(key); }
} KeyInput;

extern class Cf3GameInputReplay : public Cf3GameInput
{
public:
	BYTE pushed, pressed;
bool GetKeyPushed(int key) { return 0 != (pushed & (1 << (key - 1))); }
bool GetKeyPressed(int key) { return 0 != (pressed & (1 << (key - 1))); }
} ReplayInput;

class Cf3MapObjectMain : public Cf3MapObjectBase  
{
protected:
	static Cf3GameInput* m_pInput=NULL;
public:
	static SetInput(Cf3GameInput* pInput) { m_pInput = pInput; }
static Cf3MapObjectMain* Create(int x, int y)
{
	return (theSetting->m_Gravity!=3?
		(Cf3MapObjectMain*)new Cf3MapObjectfunya(x, y):
		(Cf3MapObjectMain*)new Cf3MapObjectfff(x, y));
}
virtual bool IsFrozen() { return false; }
virtual void Die() { }
virtual bool IsDied() { return IsValid(); }
virtual void GetViewPos(int &vx, int &vy)
{
	vx = m_X;
	vy = m_Y;
}
Cf3MapObjectMain(f3MapObjectType eType) : Cf3MapObjectBase(eType) { }
virtual ~Cf3MapObjectMain() { }

};
}