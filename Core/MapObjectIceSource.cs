namespace MifuminSoft.funyan.Core
{
class Cf3MapObjectIceSource : public Cf3MapObjectIceBase  
{
protected:
    const int PHASEMAX = 32;
    static set<Cf3MapObjectIceSource*> m_IceList;
int m_Phase;
int m_Size;
public:
	static set<Cf3MapObjectIceSource*>::iterator IteratorBegin() { return m_IceList.begin(); }
static set<Cf3MapObjectIceSource*>::iterator IteratorEnd() { return m_IceList.end(); }
int GetSize()
{
	int s=abs((PHASEMAX/2)-m_Phase)*6/PHASEMAX+4;
	return s;
}
static void OnPreDrawAll()
{
	for(set<Cf3MapObjectIceSource*>::iterator it = m_IceList.begin();it!=m_IceList.end();it++){
		if ((*it)->IsValid()) (*it)->OnPreDraw();
	}
}
static void OnDrawAll(CDIB32* lp)
{
	int sx, sy, ex, ey;
	sx = sy = 0;
	m_pParent->GetViewPos(sx,sy);
	sx = (-sx)>>5; sy = (-sy)>>5;
	ex = sx+320/32; ey = sy+224/32;
        TL.Saturate(sx,ref ex,m_pParent->GetWidth()-1);
        TL.Saturate(sy,ref ey,m_pParent->GetHeight()-1);
	for (Cf3MapObjectBase**it=m_pParent->GetMapObjects(sx-1, sy-1, ex+1, ey+1, MOT_ICESOURCE); (*it)!=NULL; it++) {
		if ((*it)->IsValid()) (*it)->OnDraw(lp);
	}
}
void OnPreDraw()
{
	if (CApp::random(40)) { m_Phase++; m_Phase%=PHASEMAX; }
	m_Size = GetSize();
}
void OnDraw(CDIB32* lp)
{
	if (!IsValid()) return;
	RECT rc = { (7-m_Size)*64, 0, (8-m_Size)*64, 64, };
	SetViewPos(-32,-32);
	lp->BltNatural(m_Graphic,m_nVX,m_nVY,&rc);
}
Cf3MapObjectIceSource(int x, int y)
	:Cf3MapObjectIceBase(MOT_ICESOURCE)
{
	m_IceList.insert(this);
	SetPos(x*32+16,y*32+16);
	m_Phase = CApp::random(PHASEMAX);
	m_Size = GetSize();
}
virtual ~Cf3MapObjectIceSource()
{
	m_IceList.erase(this);
}

};
}
