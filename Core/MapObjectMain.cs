namespace MifuminSoft.funyan.Core
{
    public class MapObjectMain : MapObjectBase
    {

        protected static GameInput m_pInput;

        public static void SetInput(GameInput pInput) { m_pInput = pInput; }
        public static MapObjectMain Create(int x, int y)
        {
            return Setting.Current.m_Gravity != 3 ?
                (MapObjectMain)new MapObjectfunya(x, y) :
                (MapObjectMain)new MapObjectfff(x, y);
        }
        public virtual bool IsFrozen() { return false; }
        public virtual void Die() { }
        public virtual bool IsDied() { return IsValid(); }
        public virtual void GetViewPos(out int vx, out int vy)
        {
            vx = (int)m_X;
            vy = (int)m_Y;
        }

        public MapObjectMain(MapObjectType eType) : base(eType) { }
    }
}
