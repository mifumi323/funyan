namespace MifuminSoft.funyan.Core
{
class Cf3MapObjectBanana : public Cf3MapObjectBase  
{
private:
//	CDIB32* m_Graphic;
	static set<Cf3MapObjectBanana*> m_BananaList;
public:
	void UpdateCPos() { }
static void OnPreDrawAll() { }
static void SynergyAll();
static void OnDrawAll(CDIB32* lp);
static set<Cf3MapObjectBanana*>::iterator IteratorBegin() { return m_BananaList.begin(); }
static set<Cf3MapObjectBanana*>::iterator IteratorEnd() { return m_BananaList.end(); }
void Reaction(Cf3MapObjectBase* obj);
void Synergy();
void OnDraw(CDIB32* lp);
Cf3MapObjectBanana(int nCX, int nCY);
~Cf3MapObjectBanana();

};
}