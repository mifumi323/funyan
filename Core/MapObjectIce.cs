namespace MifuminSoft.funyan.Core
{
public class Cf3MapObjectIce : Cf3MapObjectIceBase  
{
        protected const float GRAVITY = 0.2f;
        protected const float FRICTION = 0.026f;
        protected const float REFRECTION = 0.9f;
        protected const int LIFE = 160;
        protected static set<Cf3MapObjectIce*> Cf3MapObjectIce::m_IceList;

        protected float m_DX, m_DY;
        protected int m_Life;

        public int GetSize()
{
	return 16*m_Life/LIFE;
}
        public static set<Cf3MapObjectIce*>::iterator IteratorBegin() { return m_IceList.begin(); }
        public static set<Cf3MapObjectIce*>::iterator IteratorEnd() { return m_IceList.end(); }
        public static void OnPreDrawAll()
{
	for(set<Cf3MapObjectIce*>::iterator it = m_IceList.begin();it!=m_IceList.end();it++){
		if ((*it)->IsValid()) (*it)->OnPreDraw();
	}
}
        public static void SynergyAll()
{
	for(set<Cf3MapObjectIce*>::iterator it = m_IceList.begin();it!=m_IceList.end();it++){
		if ((*it)->IsValid()) (*it)->Synergy();
	}
}
        public static void OnMoveAll()
{
	for(set<Cf3MapObjectIce*>::iterator it = m_IceList.begin();it!=m_IceList.end();it++){
		if ((*it)->IsValid()) (*it)->OnMove();
	}
}
        public static void OnDrawAll(CDIB32* lp)
{
	int sx, sy, ex, ey;
	sx = sy = 0;
	m_pParent->GetViewPos(sx,sy);
	sx = (-sx)>>5; sy = (-sy)>>5;
	ex = sx+320/32; ey = sy+224/32;
        TL.Saturate(sx,ref ex,m_pParent->GetWidth()-1);
        TL.Saturate(sy,ref ey,m_pParent->GetHeight()-1);
	for (Cf3MapObjectBase**it=m_pParent->GetMapObjects(sx-1, sy-1, ex+1, ey+1, MOT_ICE); (*it)!=NULL; it++) {
		if ((*it)->IsValid()) (*it)->OnDraw(lp);
	}
}
        public void OnDraw(CDIB32* lp)
{
	if (!IsValid()||m_Life<=0) return;
	RECT rc;
	rc.left =	64*(15-GetSize());	rc.top =	0;
	rc.right =	64*(16-GetSize());	rc.bottom =	64;
	SetViewPos(-32,-32);
	lp->BltNatural(m_Graphic,m_nVX,m_nVY,&rc);
}
        public void OnMove()
{
	if (--m_Life<=0) Kill();
	float Wind=m_pParent->GetWind(floor(m_X/32),floor(m_Y/32));
	m_DX -= (m_DX-Wind)*FRICTION;
	m_DY += GRAVITY;
	m_DY -= m_DY*FRICTION;
        TL.Saturate(-13.0f,ref m_DX,13.0f);
        TL.Saturate(-13.0f,ref m_DY,13.0f);
	m_X += m_DX;
	int s=GetSize();
	if (m_DX>0) {
		// 右側当たり判定
		if (m_pParent->GetHit(floor((m_X+s)/32),floor(m_Y/32),HIT_LEFT)) {
			if (floor((m_X+s)/32)!=floor((m_X+s-m_DX)/32)) {
				m_DX*=-REFRECTION;
				m_X = floor((m_X+s)/32)*32-s;
				m_Life--;
			}
		}
	}
        else if (m_DX<0) {
		// 左側当たり判定
		if (m_pParent->GetHit(floor((m_X-s)/32),floor(m_Y/32),HIT_RIGHT)) {
			if (floor((m_X-s)/32)!=floor((m_X-s-m_DX)/32)) {
				m_DX*=-REFRECTION;
				m_X = floor(m_X/32)*32+s;
				m_Life--;
			}
		}
	}
	m_Y += m_DY;
	if (m_DY>0) {
		// 下側当たり判定
		if (m_pParent->GetHit(floor(m_X/32),floor((m_Y+s)/32),HIT_TOP)) {
			if (floor((m_Y+s)/32)!=floor((m_Y+s-m_DY)/32)) {
				m_DY*=-REFRECTION;
				m_Y = floor((m_Y+s)/32)*32-s;
				m_Life--;
			}
		}
	}
        else if (m_DY<0) {
		// 上側当たり判定
		if (m_pParent->GetHit(floor(m_X/32),floor((m_Y-s)/32),HIT_BOTTOM)) {
			if (floor((m_Y-s)/32)!=floor((m_Y-s-m_DY)/32)) {
				m_DY*=-REFRECTION;
				m_Y = floor(m_Y/32)*32+s;
				m_Life--;
			}
		}
	}
}
        public Cf3MapObjectIce(float x, float y, float dx, float dy)
	:Cf3MapObjectIceBase(MOT_ICE)
{
	m_IceList.insert(this);
	SetPos(x,y);
	m_DX = dx; m_DY = dy;
	m_Life=LIFE;
}
        public virtual ~Cf3MapObjectIce()
{
	m_IceList.erase(this);
}

};
}
