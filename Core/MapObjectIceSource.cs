class Cf3MapObjectIceSource : public Cf3MapObjectIceBase  
{
protected:
	static set<Cf3MapObjectIceSource*> m_IceList;
int m_Phase;
int m_Size;
public:
	static set<Cf3MapObjectIceSource*>::iterator IteratorBegin() { return m_IceList.begin(); }
static set<Cf3MapObjectIceSource*>::iterator IteratorEnd() { return m_IceList.end(); }
int GetSize();
static void OnPreDrawAll();
static void OnDrawAll(CDIB32* lp);
void OnPreDraw();
void OnDraw(CDIB32* lp);
Cf3MapObjectIceSource(int x, int y);
virtual ~Cf3MapObjectIceSource();

};
