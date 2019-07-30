namespace MifuminSoft.funyan.Core
{
class Cf3MapObjectIceBase : public Cf3MapObjectBase  
{
protected:
	CDIB32* m_Graphic;
public:
	Cf3MapObjectIceBase(f3MapObjectType eType)
	:Cf3MapObjectBase(eType)
{
	m_Graphic = ResourceManager.Get(RID_ICEFIRE);
}
virtual ~Cf3MapObjectIceBase() { }

};
}