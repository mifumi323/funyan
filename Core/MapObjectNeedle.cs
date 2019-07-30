namespace MifuminSoft.funyan.Core
{
class Cf3MapObjectNeedle : public Cf3MapObjectBase  
{
protected:
	void Reaction(Cf3MapObjectBase* obj)
{
	if (obj==NULL) return;
	float objX, objY;
	obj->GetPos(objX,objY);
	switch(obj->GetType()) {
	case MOT_FUNYA:
	case MOT_GEASPRIN:{
		if (m_Type!=NDT_VERTICAL || m_State!=NDS_STOP || m_Speed!=0) return;
		if (IsIn(m_X-16,objX,m_X+16)) {
			if (IsIn(m_Y+15,objY,m_Y+271)) {
				m_State = NDS_DOWN;
			}
		}
		break;
				   }
	default:{
		return;
			}
	}
}
CDIB32* m_Graphic;
static set<Cf3MapObjectNeedle*> m_EnemyList;

float m_StartY;
float m_Speed;
enum {
    NDT_UNDEFINED,
    NDT_HORIZONTAL,
    NDT_VERTICAL,
    NDT_DEAD,
}
m_Type;
	enum {
    NDS_WAIT,
    NDS_STOP,
    NDS_LEFT,
    NDS_RIGHT,
    NDS_DOWN,
    NDS_UP,
}
m_State;
public:
	static void OnDrawAll(CDIB32* lp)
{
	int sx, sy, ex, ey;
	sx = sy = 0;
	m_pParent->GetViewPos(sx,sy);
	sx = (-sx)>>5; sy = (-sy)>>5;
	ex = sx+320/32; ey = sy+224/32;
	Saturate(sx,ex,m_pParent->GetWidth()-1);
	Saturate(sy,ey,m_pParent->GetHeight()-1);
	for (Cf3MapObjectBase**it=m_pParent->GetMapObjects(sx, sy, ex, ey, MOT_NEEDLE); (*it)!=NULL; it++) {
		if ((*it)->IsValid()) (*it)->OnDraw(lp);
	}
/*	for(set<Cf3MapObjectNeedle*>::iterator it = m_EnemyList.begin();it!=m_EnemyList.end();it++){
		if ((*it)->m_bValid) (*it)->OnDraw(lp);
	}*/
}
static void OnPreDrawAll()
{
	for(set<Cf3MapObjectNeedle*>::iterator it = m_EnemyList.begin();it!=m_EnemyList.end();it++){
		if ((*it)->IsValid()) (*it)->OnPreDraw();
	}
}
static void SynergyAll()
{
	for(set<Cf3MapObjectNeedle*>::iterator it = m_EnemyList.begin();it!=m_EnemyList.end();it++){
		if ((*it)->IsValid()) (*it)->Synergy();
	}
}
static void OnMoveAll()
{
	for(set<Cf3MapObjectNeedle*>::iterator it = m_EnemyList.begin();it!=m_EnemyList.end();it++){
		if ((*it)->IsValid()) (*it)->OnMove();
	}
}
static set<Cf3MapObjectNeedle*>::iterator IteratorBegin() { return m_EnemyList.begin(); }
static set<Cf3MapObjectNeedle*>::iterator IteratorEnd() { return m_EnemyList.end(); }
void Synergy()
{
	if (!IsValid()) return;
	Cf3MapObjectBase**it;
	for(it=m_pParent->GetMapObjects(m_nCX-1, m_nCY, m_nCX+1, m_nCY+10, MOT_FUNYA); (*it)!=NULL; it++){
		if ((*it)->IsValid()) Reaction((*it));
	}
	for(it=m_pParent->GetMapObjects(m_nCX-1, m_nCY, m_nCX+1, m_nCY+10, MOT_GEASPRIN); (*it)!=NULL; it++){
		if ((*it)->IsValid()) Reaction((*it));
	}
	for(it=m_pParent->GetMapObjects(m_nCX-1, m_nCY-1, m_nCX+1, m_nCY+1, MOT_EELPITCHER); (*it)!=NULL; it++){
		if ((*it)->IsValid()) {
			float objX, objY;
			(*it)->GetPos(objX,objY);
			if (IsIn(objX-16,m_X,objX+16)) {
				if (IsIn(objY,m_Y,objY+40)) {
					// 食べられちゃった！！
					m_Type = NDT_DEAD;
					new Cf3MapObjectEffect(m_X, m_Y, 0);
				}
			}
		}
	}
}
virtual void OnMove()
{
	if (!IsValid()) return;
	if (m_Type==NDT_UNDEFINED) {
		// このタイミングで初期化
		if (m_pParent->GetHit(floor(m_X/32),floor((m_Y+16)/32),HIT_TOP)) {
			m_Type = NDT_HORIZONTAL;
			m_State=NDS_STOP;
		}else{
			m_Type = NDT_VERTICAL;
			m_StartY = m_Y;
			m_State=NDS_STOP;
		}
	}
	if (m_Type==NDT_HORIZONTAL) {
		if (m_State==NDS_STOP) {
			BringClose(m_Speed,0.0f,1.0f);
			if (m_Speed==0) {
				if (!m_pParent->GetHit(floor((m_X+15)/32),floor((m_Y+16)/32),HIT_TOP)||
					m_pParent->GetHit(floor((m_X+15)/32),floor((m_Y)/32),HIT_LEFT)) {
					m_State=NDS_LEFT;
				}else{
					m_State=NDS_RIGHT;
				}
			}
		}ef(m_State==NDS_LEFT) {
			m_X-=1;
			if (!m_pParent->GetHit(floor((m_X-16)/32),floor((m_Y+16)/32),HIT_TOP)||
				m_pParent->GetHit(floor((m_X-16)/32),floor((m_Y)/32),HIT_RIGHT)) {
				m_State=NDS_STOP;
				m_Speed=20;
			}
		}ef(m_State==NDS_RIGHT) {
			m_X+=1;
			if (!m_pParent->GetHit(floor((m_X+15)/32),floor((m_Y+16)/32),HIT_TOP)||
				m_pParent->GetHit(floor((m_X+15)/32),floor((m_Y)/32),HIT_LEFT)) {
				m_State=NDS_STOP;
				m_Speed=20;
			}
		}
	}ef(m_Type==NDT_VERTICAL) {
		if (m_State==NDS_STOP) {
			if (m_Speed!=0) {
				BringClose(m_Speed,0.0f,1.0f);
				if (m_Speed==0) {
					m_State=NDS_UP;
				}
			}
		}ef(m_State==NDS_UP) {
			BringClose(m_Y,m_StartY,1.0f);
			if (m_Y==m_StartY) {
				m_State=NDS_STOP;
			}
		}ef(m_State==NDS_DOWN) {
			m_Speed+=0.2f;
			Saturate(0.0f,m_Speed,10.0f);
			m_Y += m_Speed;
			if (m_pParent->GetHit(floor(m_X/32),floor((m_Y+16)/32),HIT_TOP)) {
				m_Y = floor((m_Y+16)/32)*32-15;
				m_Speed = 20;
				m_State=NDS_STOP;
			}ef(m_Y>m_pParent->GetHeight()*32+16) {
				m_Type = NDT_DEAD;
				new Cf3MapObjectEffect(m_X, m_Y, 1);
			}
		}
	}ef(m_Type==NDT_DEAD) {
		Kill();
	}
}
virtual void OnDraw(CDIB32* lp)
{
	if (!IsValid()) return;
	SetViewPos(-15,-15);
	lp->Blt(m_Graphic,m_nVX,m_nVY);
}
Cf3MapObjectNeedle(int nCX, int nCY, int nType = 0)
	:Cf3MapObjectBase(MOT_NEEDLE)
{
	m_EnemyList.insert(this);
	m_Graphic = ResourceManager.Get(RID_NEEDLE);
	SetPos(nCX*32+16,nCY*32+17);
	switch(nType){
	case 1:
		m_Type = NDT_HORIZONTAL;
		m_State=NDS_LEFT;
		break;
	case 2:
		m_Type = NDT_VERTICAL;
		m_StartY = m_Y;
		m_State=NDS_STOP;
		break;
	case 3:
		m_Type = NDT_HORIZONTAL;
		m_State=NDS_RIGHT;
		break;
	default:
		m_Type=NDT_UNDEFINED;
		break;
	}
	m_Speed = 0;
}
virtual ~Cf3MapObjectNeedle()
{
	m_EnemyList.erase(this);
}

};
}