namespace MifuminSoft.funyan.Core
{
    public abstract class Cf3MapObjectIceBase : Cf3MapObjectBase
    {
        protected CDIB32 m_Graphic;

        public Cf3MapObjectIceBase(f3MapObjectType eType) : base(eType)
        {
            m_Graphic = CApp.theApp.ResourceManager.Get(RID.RID_ICEFIRE);
        }
    }
}
