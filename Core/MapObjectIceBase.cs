namespace MifuminSoft.funyan.Core
{
public class Cf3MapObjectIceBase : Cf3MapObjectBase  
{
        protected CDIB32* m_Graphic;

        public Cf3MapObjectIceBase(f3MapObjectType eType)
	:Cf3MapObjectBase(eType)
{
	m_Graphic = ResourceManager.Get(RID_ICEFIRE);
}
        public virtual ~Cf3MapObjectIceBase() { }

};
}
