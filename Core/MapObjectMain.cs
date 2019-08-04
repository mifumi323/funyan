namespace MifuminSoft.funyan.Core
{
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
