namespace MifuminSoft.funyan.Core
{
    public class Cf3GameInput
    {
        public virtual bool GetKeyPushed(int key)=0;
	    public virtual bool GetKeyPressed(int key)=0;
};

extern public class Cf3GameInputKey : Cf3GameInput
    {
        public bool GetKeyPushed(int key) { return Cf3Input.f3Input.GetKeyPushed(key); }
        public bool GetKeyPressed(int key) { return Cf3Input.f3Input.GetKeyPressed(key); }
    } KeyInput;

extern public class Cf3GameInputReplay : Cf3GameInput
    {
        public byte pushed, pressed;
        public bool GetKeyPushed(int key) { return 0 != (pushed & (1 << (key - 1))); }
        public bool GetKeyPressed(int key) { return 0 != (pressed & (1 << (key - 1))); }
    } ReplayInput;

public abstract class Cf3MapObjectMain : Cf3MapObjectBase
    {
        protected static Cf3GameInput m_pInput = null;

        public static SetInput(Cf3GameInput* pInput) { m_pInput = pInput; }
        public static Cf3MapObjectMain* Create(int x, int y)
        {
            return (Cf3Setting.theSetting.m_Gravity != 3 ?
                (Cf3MapObjectMain*)new Cf3MapObjectfunya(x, y) :
                (Cf3MapObjectMain*)new Cf3MapObjectfff(x, y));
        }
        public virtual bool IsFrozen() { return false; }
        public virtual void Die() { }
        public virtual bool IsDied() { return IsValid(); }
        public virtual void GetViewPos(int &vx, int &vy)
        {
            vx = m_X;
            vy = m_Y;
        }
        public Cf3MapObjectMain(f3MapObjectType eType) : base(eType) { }
    }
}
