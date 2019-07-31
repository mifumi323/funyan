namespace MifuminSoft.funyan.Core
{
class Cf3MapObjectFire : public Cf3MapObjectIceBase  
{
private:
    const int PHASEMAX = 32;
	static set<Cf3MapObjectFire*> m_FireList;

int m_Phase;
int m_Size;
int m_Delay;
public:
	void Synergy()
{
	if (m_Delay==0) {
		// ふにゃ
		Cf3MapObjectBase**it;
		for(it=m_pParent->GetMapObjects(m_nCX-1, m_nCY-1, m_nCX+1, m_nCY+1, MOT_FUNYA); (*it)!=NULL; it++){
			if ((*it)->IsValid()&&((Cf3MapObjectMain*)(*it))->IsFrozen()) {
				float objX, objY;
				(*it)->GetPos(objX,objY);
				// あたった！
				if ((objX-m_X)*(objX-m_X)+(objY-m_Y)*(objY-m_Y)<256) m_Delay = 200;
			}
		}
		// 氷
		for(it=m_pParent->GetMapObjects(m_nCX-1, m_nCY-1, m_nCX+1, m_nCY+1, MOT_ICE); (*it)!=NULL; it++){
			if ((*it)->IsValid()) {
				float objX, objY;
				(*it)->GetPos(objX,objY);
				// あたった！
				if ((objX-m_X)*(objX-m_X)+(objY-m_Y)*(objY-m_Y)<256) m_Delay = 200;
			}
		}
	}
}
bool IsActive() { return m_Delay == 0; }
static set<Cf3MapObjectFire*>::iterator IteratorBegin() { return m_FireList.begin(); }
static set<Cf3MapObjectFire*>::iterator IteratorEnd() { return m_FireList.end(); }
void OnPreDraw()
{
	if (CApp::random(40)) { m_Phase++; m_Phase%=PHASEMAX; }
	if (m_Size<GetSize()) { m_Size++; }
        else if (m_Size>GetSize()) { m_Size--; }
}
void OnMove()
{
	if (m_Delay>0) m_Delay--;
        else if (m_Delay<0) m_Delay++;
}
void OnDraw(CDIB32* lp)
{
	if (!IsValid()) return;
	RECT rc = { (15-m_Size)*64, 64, (16-m_Size)*64, 128, };
	SetViewPos(-32,-32);
	lp->BltNatural(m_Graphic,m_nVX,m_nVY,&rc);
}
int GetSize()
{
	int s=abs((PHASEMAX/2)-m_Phase)*6/PHASEMAX;
	if (m_Delay==0) s+=10;
	return s;
}
static void SynergyAll()
{
	for(set<Cf3MapObjectFire*>::iterator it = m_FireList.begin();it!=m_FireList.end();it++){
		if ((*it)->IsValid()) (*it)->Synergy();
	}
}
static void OnPreDrawAll()
{
	for(set<Cf3MapObjectFire*>::iterator it = m_FireList.begin();it!=m_FireList.end();it++){
		if ((*it)->IsValid()) (*it)->OnPreDraw();
	}
}
static void OnMoveAll()
{
	for(set<Cf3MapObjectFire*>::iterator it = m_FireList.begin();it!=m_FireList.end();it++){
		if ((*it)->IsValid()) (*it)->OnMove();
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
	for (Cf3MapObjectBase**it=m_pParent->GetMapObjects(sx-1, sy-1, ex+1, ey+1, MOT_FIRE); (*it)!=NULL; it++) {
		if ((*it)->IsValid()) (*it)->OnDraw(lp);
	}
}
Cf3MapObjectFire(int x, int y)
	:Cf3MapObjectIceBase(MOT_FIRE)
	,m_Delay(0)
{
	m_FireList.insert(this);
	SetPos(x*32+16,y*32+16);
	m_Phase = CApp::random(PHASEMAX);
	m_Size = GetSize();
}
virtual ~Cf3MapObjectFire()
{
	m_FireList.erase(this);
}

};
}
