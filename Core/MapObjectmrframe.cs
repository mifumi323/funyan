using System.Collections.Generic;

namespace MifuminSoft.funyan.Core
{
    public class MapObjectmrframe
    {
        protected MapObjectfunya m_funya;
        protected int m_nLife;

        public void UpdateCPos();
        public bool IsFrozen();
        public static HashSet<MapObjectmrframe> m_EnemyList;
        public static void SynergyAll();
        public static void OnMoveAll();
        public static HashSet<MapObjectmrframe> All() { return m_EnemyList; }
        public void Synergy();
        public void OnMove();
        public Cf3MapObjectmrframe(int nCX, int nCY);
        public virtual ~Cf3MapObjectmrframe();
    }
}
