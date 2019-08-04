namespace MifuminSoft.funyan.Core
{
    public abstract class Cf3GameInput
    {
        public abstract bool GetKeyPushed(int key);
        public abstract bool GetKeyPressed(int key);

        public static Cf3GameInputKey KeyInput { get; } = new Cf3GameInputKey();
        public static Cf3GameInputReplay ReplayInput { get; } = new Cf3GameInputReplay();
    };

    public class Cf3GameInputKey : Cf3GameInput
    {
        public override bool GetKeyPushed(int key) { return Cf3Input.f3Input.GetKeyPushed(key); }
        public override bool GetKeyPressed(int key) { return Cf3Input.f3Input.GetKeyPressed(key); }
    }

    public class Cf3GameInputReplay : Cf3GameInput
    {
        public byte pushed, pressed;
        public override bool GetKeyPushed(int key) { return 0 != (pushed & (1 << (key - 1))); }
        public override bool GetKeyPressed(int key) { return 0 != (pressed & (1 << (key - 1))); }
    }

    public abstract class Cf3MapObjectMain : Cf3MapObjectBase
    {
        protected static Cf3GameInput m_pInput = null;

        public static void SetInput(Cf3GameInput pInput) { m_pInput = pInput; }
        public static Cf3MapObjectMain Create(int x, int y)
        {
            return (Cf3Setting.theSetting.m_Gravity != 3 ?
                (Cf3MapObjectMain)new Cf3MapObjectfunya(x, y) :
                (Cf3MapObjectMain)new Cf3MapObjectfff(x, y));
        }
        public virtual bool IsFrozen() { return false; }
        public virtual void Die() { }
        public virtual bool IsDied() { return IsValid(); }
        public virtual void GetViewPos(out int vx, out int vy)
        {
            vx = (int)m_X;
            vy = (int)m_Y;
        }
        public Cf3MapObjectMain(f3MapObjectType eType) : base(eType) { }
    }
}
