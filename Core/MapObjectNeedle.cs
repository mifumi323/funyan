using System.Collections.Generic;

namespace MifuminSoft.funyan.Core
{
    public class MapObjectNeedle
    {
        protected void Reaction(MapObjectBase obj);
        protected static HashSet<MapObjectNeedle> m_EnemyList;

        protected float m_StartY;
        protected float m_Speed;
        protected enum NDT {
            UNDEFINED,
            HORIZONTAL,
            VERTICAL,
            DEAD,
        }
        protected NDT m_Type;
        protected enum NDS{
            WAIT,
            STOP,
            LEFT,
            RIGHT,
            DOWN,
            UP,
        }
        protected NDS m_State;

        public static void SynergyAll();
        public static void OnMoveAll();
        public static HashSet<MapObjectNeedle> All() { return m_EnemyList; }
        public void Synergy();
        public virtual void OnMove();
        public Cf3MapObjectNeedle(int nCX, int nCY, int nType = 0);
        public virtual ~Cf3MapObjectNeedle();
    }
}
