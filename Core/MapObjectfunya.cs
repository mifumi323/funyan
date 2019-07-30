namespace MifuminSoft.funyan.Core
{
const float RUNMAX = 13.0f;
const float RUNFRICTION = 0.43f;
const float WALKACCEL = 0.4f;
const float WALKFRICTION = 0.24f;
const float JUMPACCEL = 0.12f;
const float JUMPMAX = 13.0f;
const float FALLMAX = 13.0f;
const float ADDGRAVITY = 4.0f;
const float JUMPFRICTIONX = 0.026f;
const float JUMPFRICTIONY = 0.10f;
const float WINDFACTOR = 0.10f;
const float MAXDISTANCE = 1.6e5f;

#define SIN15	0.25881904510252076234889883762405f
#define SIN30	0.5f
#define SIN45	0.70710678118654752440084436210485f
#define SIN60	0.86602540378443864676372317075294f
#define SIN75	0.9659258262890682867497431997289f
#define COS15	SIN75
#define COS30	SIN60
#define COS45	SIN45
#define COS60	SIN30
#define COS75	SIN15

#define INVINCITY 0
class Cf3MapObjectfunya : public Cf3MapObjectMain  
{
	friend class Cf3MapObjectmrframe;
friend class CExplainScene;
protected:
	void Freeze(int level = 15)
{
	m_State = FROZEN;
	m_PoseCounter = level*8;
}
void Tire()
{
	m_State = TIRED;
	m_PoseCounter = 100;
}
void BreatheOut()
{
	if (m_bOriginal&&m_State==SLEEPING) theApp->GetBGM()->MusicEffect(MEN_AWAKE);
	int p=floor(m_ChargePower/40.0f)+1;
        TL.Saturate(1,ref p,m_nPower);
	struct { float x, y, dx, dy; } s[4];
	if (m_Direction==DIR_FRONT) {
		if (p&1) {
			s[0].x=0.0f;
			s[0].y=-16.0f;
			s[0].dx=0.0f;
			s[0].dy=-6.0f;
			s[1].x=16.0f*SIN30;
			s[1].y=-16.0f*COS30;
			s[1].dx=6.0f*SIN30;
			s[1].dy=-6.0f*COS30;
			s[2].x=-s[1].x;
			s[2].y=s[1].y;
			s[2].dx=-s[1].dx;
			s[2].dy=s[1].dy;
		}else {
			s[0].x=16.0f*SIN15;
			s[0].y=-16.0f*COS15;
			s[0].dx=6.0f*SIN15;
			s[0].dy=-6.0f*COS15;
			s[1].x=-s[0].x;
			s[1].y=s[0].y;
			s[1].dx=-s[0].dx;
			s[1].dy=s[0].dy;
			s[2].x=16.0f*SIN45;
			s[2].y=-16.0f*COS45;
			s[2].dx=6.0f*SIN45;
			s[2].dy=-6.0f*COS45;
			s[3].x=-s[2].x;
			s[3].y=s[2].y;
			s[3].dx=-s[2].dx;
			s[3].dy=s[2].dy;
		}
	}else{
		s[0].x=16.0f*COS15;
		s[0].y=-16.0f*SIN15;
		s[0].dx=6.0f*COS15;
		s[0].dy=-6.0f*SIN15;
		s[1].x=16.0f*COS30;
		s[1].y=-16.0f*SIN30;
		s[1].dx=6.0f*COS30;
		s[1].dy=-6.0f*SIN30;
		s[2].x=16.0f*COS45;
		s[2].y=-16.0f*SIN45;
		s[2].dx=6.0f*COS45;
		s[2].dy=-6.0f*SIN45;
		s[3].x=16.0f*COS60;
		s[3].y=-16.0f*SIN60;
		s[3].dx=6.0f*COS60;
		s[3].dy=-6.0f*SIN60;
	}
	for (int i=0; i<p; i++) {
		if (m_Direction==DIR_LEFT) { s[i].x=-s[i].x; s[i].dx=-s[i].dx; }
		new Cf3MapObjectIce(m_X+s[i].x,m_Y+s[i].y,m_DX+s[i].dx,m_DY+s[i].dy);
	}
	m_nPower -= p;
	m_State = BREATHEOUT;
	m_ChargePower=10.0f;
}
void BreatheIn()
{
	if (m_nPower) {
		if (m_bOriginal&&m_State==SLEEPING) theApp->GetBGM()->MusicEffect(MEN_AWAKE);
		m_State = BREATHEIN;
		m_ChargePower = 0.0f;
	}
}
void Sit()
{
	if (m_bOriginal&&m_State==SLEEPING) theApp->GetBGM()->MusicEffect(MEN_AWAKE);
	m_State = WALKING;
}
void Sleep()
{
	m_State = SLEEPING;
	if (m_bOriginal) theApp->GetBGM()->MusicEffect(MEN_SLEEP);
	m_PoseCounter = 0;
}
void Blink()
{
	m_State = BLINKING;
	m_PoseCounter = m_BananaDistance<MAXDISTANCE?2:12;
}
void HighJump()
{
	if (m_bOriginal&&m_State==SLEEPING) theApp->GetBGM()->MusicEffect(MEN_AWAKE);
	if (m_State!=FROZEN) m_State=JUMPING;
	m_DY = -6.35f;
}
void Smile()
{
	if (m_bOriginal&&m_State==SLEEPING) theApp->GetBGM()->MusicEffect(MEN_AWAKE);
	m_State = SMILING;
	m_DX = m_DY = 0;
}
void HitCheck()
{
	int CX=floor(m_X/32),
		CL=floor((m_X-14)/32),
		CR=floor((m_X+14)/32),
		CY=floor(m_Y/32),
		CT=floor((m_Y-14)/32),
		CB=floor((m_Y+14)/32);
	if (m_DX>=0) {	// 右へ
		if (CR!=CX) {
			if (m_pParent->GetHit(CR,CY,HIT_LEFT)) {
				m_X = 18+32*CX;
				m_DX = 0;
				m_HitRight = true;
			}
		}
	}
	if(m_DX<=0) {	// 左へ
		if (CL!=CX) {
			if (m_pParent->GetHit(CL,CY,HIT_RIGHT)) {
				m_X = 14+32*CX;
				m_DX = 0;
				m_HitLeft = true;
			}
		}ef ((double)CL==ceil((m_X-14)/32)) {
			// (m_X-14)/32が整数ということは境界値でギリギリ当たっているということ！！
			if (m_pParent->GetHit(CL-1,CY,HIT_RIGHT)) {
				m_X = 14+32*CX;
				m_DX = 0;
				m_HitLeft = true;
			}
		}
	}
	m_HitBottom = m_OnEnemy;
	if (m_DY>=0) {	// 落ちるとき
		if (CB!=CY) {
			if (m_pParent->GetHit(CX,CB,HIT_TOP)) {
				m_Y = 18+32*CY;
				m_DY = 0;
				m_HitBottom = true;
			}
		}
	}ef(m_DY<0){	// 飛ぶとき
		if (CT!=CY) {
			if (m_pParent->GetHit(CX,CT,HIT_BOTTOM)) {
				m_Y = 14+32*CY;
				m_DY = 0;
				m_HitTop = true;
			}
		}
	}
#if !INVINCITY
	if (m_pParent->GetHit(CX,CY,HIT_DEATH)) Die();
	if (m_Y-14 > 32*m_pParent->GetHeight()) Die();
#endif
}
void Stop()
{
	if (m_bOriginal&&m_State==SLEEPING) theApp->GetBGM()->MusicEffect(MEN_AWAKE);
	m_State = STANDING;
	m_Sleepy = 0;
}
void Run(f3MapObjectDirection direction)
{
	if (m_bOriginal&&m_State==SLEEPING) theApp->GetBGM()->MusicEffect(MEN_AWAKE);
	m_State = RUNNING;
	m_Direction = direction;
	m_PoseCounter = 0;
}
void Fall()
{
	if (m_bOriginal&&m_State==SLEEPING) theApp->GetBGM()->MusicEffect(MEN_AWAKE);
	m_State = JUMPING;
}
void Land()
{
	if (m_bOriginal&&m_State==SLEEPING) theApp->GetBGM()->MusicEffect(MEN_AWAKE);
	m_State = STANDING;
	m_DY = 0;
	m_Sleepy = 0;
}
void StartJump()
{
	if (m_bOriginal&&m_State==SLEEPING) theApp->GetBGM()->MusicEffect(MEN_AWAKE);
	m_State=CHARGING;
	m_ChargePower = 1.0f;
	m_DY = 0;
}
void Jump()
{
	if (m_bOriginal&&m_State==SLEEPING) theApp->GetBGM()->MusicEffect(MEN_AWAKE);
	m_State=JUMPING;
	m_OnEnemy = false;

	m_DY = -
		(m_ChargePower>=m_JumpFunc[0].Power?m_JumpFunc[0].DY:
		(m_ChargePower>=m_JumpFunc[1].Power?m_JumpFunc[1].DY:
		(m_ChargePower>=m_JumpFunc[2].Power?m_JumpFunc[2].DY:
		(m_ChargePower>=m_JumpFunc[3].Power?m_JumpFunc[3].DY:
		0))));
}

CDIB32* m_Graphic;
CDIB32* m_Graphic2;

float m_DX, m_DY;                           // 位置などの情報
float m_BananaDistance;
float m_ChargePower;                        // ジャンプチャージ係数(1.0f=100%から減ってゆく)
float m_ChargeDec;                          // ジャンプチャージ係数減衰値(設定で変化する予定)
static const f3JumpFunction m_JumpFunc[4] = {
	4.5f, 0.994f,
	3.6f, 0.980f,
	2.6f, 0.940f,
	0.9f, 0.0f,
};  // チャージ－ジャンプ力対応
float m_Power, m_PowerX, m_PowerY;
int m_nPower;
enum f3MainCharaState
{
    STANDING,
    RUNNING,
    WALKING,
    CHARGING,
    JUMPING,
    BREATHEIN,
    BREATHEOUT,
    TIRED,
    DAMAGED,
    FROZEN,
    DEAD,
    SMILING,
    SLEEPING,
    BLINKING,
}
m_State;
	f3MapObjectDirection m_Direction;
bool m_HitLeft, m_HitRight, m_HitTop, m_HitBottom, m_OnEnemy;
bool m_bOriginal;
bool m_bFirst;

int m_PoseCounter, m_PoseCounter2, m_Sleepy;

// 表示位置調整
int m_VOffsetX, m_VOffsetY;
int m_VOffsetToX, m_VOffsetToY;
public:
	float GetGravity() { int g = theSetting->m_Gravity; return g == 1 ? 0.2f : (g == 2 ? 0.05f : 0.1f); }
bool IsFrozen() { return m_State == FROZEN; }
void Die()
{
	m_State = DEAD;
	m_DX = m_DY = 0;
}
bool IsDied()
{
	return m_State==DEAD;
}
void GetViewPos(int &vx, int &vy)
{
	if (m_pParent->IsPlayable()) {
		int ox=0,oy=0,tx=0,ty=0;
		if (m_pInput->GetKeyPressed(F3KEY_RIGHT)) tx+=1;
		if (m_pInput->GetKeyPressed(F3KEY_LEFT)) tx+=2;
		if (m_pInput->GetKeyPressed(F3KEY_DOWN)) ty+=1;
		if (m_pInput->GetKeyPressed(F3KEY_UP)) ty+=2;
		if (tx==0) { ; }
		ef (tx==1) { m_VOffsetToX = 100; }
		ef (tx==2) { m_VOffsetToX = -100; }
		ef (tx==3) { m_VOffsetToX = 0; }
		if (ty==0) { ; }
		ef (ty==1) { m_VOffsetToY = 50; }
		ef (ty==2) { m_VOffsetToY = -50; }
		ef (ty==3) { m_VOffsetToY = 0; }
        TL.BringClose(ref m_VOffsetX,m_VOffsetToX,1+(int)(m_DX*m_VOffsetToX<0)+(int)(m_VOffsetX*m_VOffsetToX<0));
        TL.BringClose(ref m_VOffsetY,m_VOffsetToY,1+(int)(m_DY*m_VOffsetToY<0)+(int)(m_VOffsetY*m_VOffsetToY<0));
		ox = m_VOffsetX;	oy = m_VOffsetY;
		vx = m_X+ox;	vy = m_Y+oy;
	}else{
		vx = m_X;	vy = m_Y;
	}
}
void Synergy()
{
	if (m_State==DEAD||m_State==SMILING) return;
	m_OnEnemy = false;
	m_Power = m_PowerX = m_PowerY = 0.0f;
	Cf3MapObjectBase**it;
	// ギヤバネ
	for(it=m_pParent->GetMapObjects(m_nCX-2, m_nCY-2, m_nCX+2, m_nCY+2, MOT_GEASPRIN); (*it)!=NULL; it++){
		if ((*it)->IsValid()) {
			float objX, objY;
			(*it)->GetPos(objX,objY);
			if (!((Cf3MapObjectGeasprin*)(*it))->IsFrozen()) {
				if (TL.IsIn(objX-16,m_X,objX+15)) {
					if (TL.IsIn(objY-30,m_Y,objY+16)) {
						if (m_bOriginal) theApp->GetBGM()->MusicEffect(MEN_GEASPRIN);
						m_Y--;
						HighJump();
					}
				}ef(TL.IsIn(objX+16,m_X,objX+29)) {
					if (TL.IsIn(objY-16,m_Y,objY+15)) {
						if (m_bOriginal) theApp->GetBGM()->MusicEffect(MEN_GEASPRIN);
						m_DX=10;
					}
				}ef(TL.IsIn(objX-29,m_X,objX-16)) {
					if (TL.IsIn(objY-16,m_Y,objY+15)) {
						if (m_bOriginal) theApp->GetBGM()->MusicEffect(MEN_GEASPRIN);
						m_DX=-10;
					}
				}
			}else{
				if (TL.IsIn(objX-16,m_X,objX+15)) {
					if (TL.IsIn(objY-30,m_Y,objY)&&m_DY>=0) {
						m_OnEnemy = true;
						m_Y = objY-30;
						if (m_State==JUMPING) Land();
					}
				}ef(TL.IsIn(objX+16,m_X,objX+29)) {
					if (TL.IsIn(objY-16,m_Y,objY+15)) {
						m_X = objX+30;
						m_DX=0;
					}
				}ef(TL.IsIn(objX-29,m_X,objX-16)) {
					if (TL.IsIn(objY-16,m_Y,objY+15)) {
						m_X = objX-30;
						m_DX=-0;
					}
				}
			}
		}
	}
	// とげとげ
#if !INVINCITY
	for(it=m_pParent->GetMapObjects(m_nCX-2, m_nCY-2, m_nCX+2, m_nCY+2, MOT_NEEDLE); (*it)!=NULL; it++){
		if ((*it)->IsValid()) {
			float objX, objY;
			(*it)->GetPos(objX,objY);
			if ((objX-m_X)*(objX-m_X)+(objY-m_Y)*(objY-m_Y)<256) {
				Die();
				return;
			}
		}
	}
#endif
	// ウナギカズラ
	for(it=m_pParent->GetMapObjects(m_nCX-2, m_nCY-2, m_nCX+2, m_nCY+2, MOT_EELPITCHER); (*it)!=NULL; it++){
		if ((*it)->IsValid()&&((Cf3MapObjectEelPitcher*)(*it))->IsLeaf()) {
			float objX, objY;
			(*it)->GetPos(objX,objY);
			if (TL.IsIn(objX-16,m_X,objX+16)) {
				if (TL.IsIn(objY-14,m_Y,objY)) {
					if (m_DY>=0) {
						m_OnEnemy = true;
						m_Y = objY-14;
						if (m_State==JUMPING) Land();
					}
				}
			}
		}
	}
	if (m_State!=FROZEN) {
		// 氷
#if !INVINCITY
		for(it=m_pParent->GetMapObjects(m_nCX-2, m_nCY-2, m_nCX+2, m_nCY+2, MOT_ICE); (*it)!=NULL; it++){
			if ((*it)->IsValid()&&((Cf3MapObjectIce*)(*it))->GetSize()>10) {
				float objX, objY;
				(*it)->GetPos(objX,objY);
				if ((objX-m_X)*(objX-m_X)+(objY-m_Y)*(objY-m_Y)<256) {
					// あたった！
					Freeze(((Cf3MapObjectIce*)(*it))->GetSize());
				}
			}
		}
#endif
		// 氷ゾーン
		for(set<Cf3MapObjectIceSource*>::iterator is = Cf3MapObjectIceSource::IteratorBegin();
		is!=Cf3MapObjectIceSource::IteratorEnd();is++){
			float objX, objY;
			(*is)->GetPos(objX,objY);
			float dX = objX-m_X, dY = objY-m_Y,
				p=1.0f/(dX*dX+dY*dY), p3 = p*sqrt(p);
			m_Power += p;
			m_PowerX+= dX*p3;
			m_PowerY+= dY*p3;
		}
		// 炎ゾーン
		for(set<Cf3MapObjectFire*>::iterator fr = Cf3MapObjectFire::IteratorBegin();
		fr!=Cf3MapObjectFire::IteratorEnd();fr++){
			if ((*fr)->IsActive()) {
				float objX, objY;
				(*fr)->GetPos(objX,objY);
				float dX = objX-m_X, dY = objY-m_Y,
					p=1.0f/(dX*dX+dY*dY), p3 = p*sqrt(p);
				m_Power -= p;
				m_PowerX-= dX*p3;
				m_PowerY-= dY*p3;
			}
		}
		if (m_Power>1.0f/256.0f) {
#if !INVINCITY
			Freeze();
#endif
		}ef(m_Power>1.0f/4096.0f) {
			m_nPower=4;
			m_PowerX = m_PowerY = 0.0f;
		}ef(m_Power<-1.0f/256.0f) {
#if !INVINCITY
			Die();
		}ef(m_Power<-1.0f/4096.0f) {
#endif
		}else{
			m_PowerX = m_PowerY = 0.0f;
		}
	}
	// バナナ(BGMの調整用)
	if (m_pParent->GetMainChara()==this) {
		m_BananaDistance = MAXDISTANCE;
		float bd;
		int nBanana=0, nPosition=0;
		int cx, cy;
		for(set<Cf3MapObjectBanana*>::iterator bn = Cf3MapObjectBanana::IteratorBegin();bn!=Cf3MapObjectBanana::IteratorEnd();bn++){
			if ((*bn)->IsValid()) {
				(*bn)->GetCPos(cx, cy);
				bd = (cx*32+16-m_X)*(cx*32+16-m_X)+(cy*32+16-m_Y)*(cy*32+16-m_Y);
				if (bd<m_BananaDistance) m_BananaDistance=bd;
				nBanana++;
				nPosition += cx-m_nCX;
			}
		}
		theApp->GetBGM()->MusicEffect(MEN_BANANADISTANCE, m_BananaDistance);
		theApp->GetBGM()->MusicEffect(MEN_BANANAPOSITION, nBanana?(float)nPosition/nBanana:0.0f);
	}
	if (m_OnEnemy) HitCheck();
}
void OnPreDraw()
{
	if (!IsValid()) return;
	// PoseCounterの処理
	if (m_State==STANDING) {	// 立ってるとき
	}ef(m_State==RUNNING) {
		++m_PoseCounter %= 12;
	}ef(m_State==WALKING) {
	}ef(m_State==CHARGING) {
	}ef(m_State==JUMPING) {
	}ef(m_State==BREATHEIN) {
	}ef(m_State==BREATHEOUT) {
	}ef(m_State==TIRED) {
		m_PoseCounter2 = m_PoseCounter-1;
	}ef(m_State==DAMAGED) {
	}ef(m_State==DEAD) {
	}ef(m_State==SLEEPING) {
		++m_PoseCounter %= 40;
		m_PoseCounter2 = m_PoseCounter-1;
		theSetting->m_SleepTime++;
	}ef(m_State==BLINKING) {
	}
	if (m_Power<-1.0f/4096.0f) { ++m_PoseCounter2 %= 40; }
}
void OnMove()
{
	if (!IsValid()) return;
	if (!m_pParent->IsPlayable()) return;
	if (m_bFirst) { HitCheck(); m_bFirst = false; }
	float Wind = m_pParent->GetWind(floor(m_X/32),floor(m_Y/32));
	float Friction = m_pParent->GetFriction(floor(m_X/32),floor((m_Y+14)/32));
	float Gravity = GetGravity();
	if (m_pParent->ItemCompleted()) Smile();
	if (theSetting->m_Hyper) m_nPower=4;
	// 動かしま～す
	if (m_State==STANDING||m_State==SLEEPING||m_State==BLINKING) {
		// 立ってるとき
		m_DX -= WINDFACTOR*(m_DX-Wind)*RUNFRICTION;
        TL.BringClose(ref m_DX,0.0f,Friction);
		if (m_DX==0) m_Direction = DIR_FRONT;
		if (m_State==STANDING && ++m_Sleepy>=30*40/3) Sleep();
		if (m_State==BLINKING && --m_PoseCounter==0) m_State = STANDING;
		if (m_State==STANDING && CApp::random(120)==0) Blink();
		if (m_PowerY<=0 && m_pInput->GetKeyPressed(F3KEY_JUMP)) StartJump();
		if (m_PowerX<=0 && m_pInput->GetKeyPressed(F3KEY_LEFT)) Run(DIR_LEFT);
		if (m_PowerX>=0 && m_pInput->GetKeyPressed(F3KEY_RIGHT)) Run(DIR_RIGHT);
		if (m_pInput->GetKeyPressed(F3KEY_DOWN)) Sit();
		if (m_pInput->GetKeyPushed(F3KEY_ATTACK)) BreatheIn();
		if (!m_HitBottom) {
			Fall();
		}
	}ef(m_State==RUNNING) {
		// 走ってるとき
		int AXL = 0, AXR = 0;
		if (m_PowerX<=0 && m_pInput->GetKeyPressed(F3KEY_LEFT)) AXL = 1;
		if (m_PowerX>=0 && m_pInput->GetKeyPressed(F3KEY_RIGHT)) AXR = 1;
		m_DX -= Friction*(m_DX-Wind)*RUNFRICTION;
		m_DX += Friction*2.0f*(AXR-AXL);
		if (AXL&&!AXR) m_Direction = DIR_LEFT;
		if (AXR&&!AXL) m_Direction = DIR_RIGHT;
		if (!AXL&&!AXR) Stop();
		if (m_PowerY<=0 && m_pInput->GetKeyPressed(F3KEY_JUMP)) StartJump();
		if (m_pInput->GetKeyPressed(F3KEY_DOWN)) Sit();
		if (m_pInput->GetKeyPushed(F3KEY_ATTACK)) BreatheIn();
		if (!m_HitBottom) {
			Fall();
		}
	}ef(m_State==WALKING) {
		// 歩いてるとき
		int AXL = 0, AXR = 0;
		if (m_PowerX<=0 && m_pInput->GetKeyPressed(F3KEY_LEFT)) AXL = 1;
		if (m_PowerX>=0 && m_pInput->GetKeyPressed(F3KEY_RIGHT)) AXR = 1;
		m_DX += WALKACCEL*(AXR-AXL);
		m_DX -= m_DX*WALKFRICTION;
		if (AXL&!AXR) m_Direction = DIR_LEFT;
		if (AXR&!AXL) m_Direction = DIR_RIGHT;
		if (!AXL&!AXR) m_Direction = DIR_FRONT;
		if (!m_pInput->GetKeyPressed(F3KEY_DOWN)) Stop();
		if (m_PowerY<=0 && m_pInput->GetKeyPushed(F3KEY_JUMP)) StartJump();
		if (m_pInput->GetKeyPushed(F3KEY_ATTACK)) BreatheIn();
		if (!m_HitBottom) Fall();
	}ef(m_State==CHARGING) {
		// パワー充填中
		if (m_ChargePower>0) {
			m_ChargePower -= m_ChargeDec;
			if (m_ChargePower<0) m_ChargePower = 0;
		}
		m_X -= m_DX;
		if (m_pInput->GetKeyPushed(F3KEY_DOWN)) Sit();
		if (m_pInput->GetKeyPushed(F3KEY_ATTACK)) BreatheIn();
		if (!m_pInput->GetKeyPressed(F3KEY_JUMP)) Jump();
		if (!m_HitBottom) Fall();
	}ef(m_State==JUMPING) {
		// 空中
		if (m_DY>=0) {
			int AXL = 0, AXR = 0;
			if (m_PowerX<=0 && m_pInput->GetKeyPressed(F3KEY_LEFT)) AXL = 1;
			if (m_PowerX>=0 && m_pInput->GetKeyPressed(F3KEY_RIGHT)) AXR = 1;
			m_DX -= (m_DX-Wind)*JUMPFRICTIONX;
			m_DX += JUMPACCEL*(AXR-AXL);
			if (AXL&!AXR) m_Direction = DIR_LEFT;
			if (AXR&!AXL) m_Direction = DIR_RIGHT;
		}
		if (m_HitLeft||m_HitRight) m_Direction = DIR_FRONT;
		m_DY += Gravity;
		if (m_DY>=0) {
			if (m_PowerY>=0 && m_pInput->GetKeyPressed(F3KEY_DOWN)) m_DY += Gravity*ADDGRAVITY;
			m_DY -= m_DY*JUMPFRICTIONY;
			if (m_pInput->GetKeyPressed(F3KEY_UP)) {
				m_DY += Gravity;
				m_DY -= m_DY*JUMPFRICTIONY;
			}
		}
		if (m_pInput->GetKeyPushed(F3KEY_ATTACK)) BreatheIn();
		if (m_HitBottom) Land();
	}ef(m_State==BREATHEIN) {
		// 冷気充填中
		m_ChargePower+=1.0f;
		if (m_pInput->GetKeyPushed(F3KEY_LEFT)) m_Direction = DIR_LEFT;
		if (m_pInput->GetKeyPushed(F3KEY_RIGHT)) m_Direction = DIR_RIGHT;
		if (m_pInput->GetKeyPushed(F3KEY_UP)) m_Direction = DIR_FRONT;
		if (m_HitBottom) {
			m_DX -= WINDFACTOR*(m_DX-Wind)*RUNFRICTION;
            TL.BringClose(ref m_DX,0.0f,Friction);
			if (m_pInput->GetKeyPushed(F3KEY_DOWN)) Sit();
		}else {
			m_ChargePower+=1.0f;
			if (m_DY>=0) {
				m_DX -= (m_DX-Wind)*JUMPFRICTIONX;
			}
			m_DY += Gravity;
			if (m_DY>=0) {
				m_DY -= m_DY*JUMPFRICTIONY;
			}
		}
		if (!m_pInput->GetKeyPressed(F3KEY_ATTACK)) BreatheOut();
	}ef(m_State==BREATHEOUT) {
		// 冷気放出！！
		m_ChargePower-=1.0f;
		if (m_HitBottom) {
			m_DX -= WINDFACTOR*(m_DX-Wind)*RUNFRICTION;
            TL.BringClose(ref m_DX,0.0f,Friction);
		}else {
			if (m_DY>=0) {
				m_DX -= (m_DX-Wind)*JUMPFRICTIONX;
			}
			m_DY += Gravity;
			if (m_DY>=0) {
				m_DY -= m_DY*JUMPFRICTIONY;
			}
		}
		if (m_ChargePower<=0.0f) {
			if (m_nPower) {
				if (m_HitBottom) Land(); else Fall();
			}else {
				Tire();
			}
		}
	}ef(m_State==TIRED) {
		// ちかれたー！
		m_PoseCounter--;
		if (m_HitBottom) {
			m_DX -= WINDFACTOR*(m_DX-Wind)*RUNFRICTION;
            TL.BringClose(ref m_DX,0.0f,Friction);
		}else {
			m_DX -= (m_DX-Wind)*JUMPFRICTIONX;
			if (m_HitLeft||m_HitRight) m_Direction = DIR_FRONT;
			m_DY += Gravity;
			m_DY -= m_DY*JUMPFRICTIONY;
		}
		if (m_PoseCounter==0) Land();
	}ef(m_State==FROZEN) {
		// 凍っちゃった…
		m_PoseCounter--;
		if (m_HitBottom) {
			m_DX -= WINDFACTOR*(m_DX-Wind)*RUNFRICTION/5;
            TL.BringClose(ref m_DX,0.0f,Friction/5);
		}else {
			m_DX -= (m_DX-Wind)*JUMPFRICTIONX/5;
			m_DY += Gravity*(1+ADDGRAVITY);
			m_DY -= m_DY*JUMPFRICTIONY/5;
		}
		if (m_PoseCounter==0) Land();
	}
    // 速度飽和(めり込み防止)
    TL.Saturate(-RUNMAX,ref m_DX,RUNMAX);
    TL.Saturate(-JUMPMAX,ref m_DY,FALLMAX);
	// 実際の移動+当たり判定
	// １回の移動ごとに当たり判定
	// という手順ですり抜けバグは解消されるはず
	m_HitLeft = m_HitRight = m_HitTop = m_HitBottom = false;
	m_X += m_DX;
	HitCheck();
	if (!m_HitTop&&!m_HitBottom) {
		m_Y += m_DY;
		HitCheck();
	}
}
virtual void OnDraw(CDIB32* lp)
{
	if (!IsValid()) return;
	if (m_pParent->ItemCompleted()) Smile();
	int CX=0, CY=m_Direction;
	SetViewPos(-16,-15);
	if (m_State==STANDING) {	// 立ってるとき
		if (m_pInput->GetKeyPressed(F3KEY_SMILE)) CX = 18;
	}ef(m_State==RUNNING) {
		CX = m_PoseCounter<6?m_PoseCounter+2:14-m_PoseCounter;
	}ef(m_State==WALKING) {
		CX = 11;
	}ef(m_State==CHARGING) {
		CX=	(m_ChargePower>=m_JumpFunc[0].Power?24:
			(m_ChargePower>=m_JumpFunc[1].Power?11:
			(m_ChargePower>=m_JumpFunc[2].Power?25:
			(m_ChargePower>=m_JumpFunc[3].Power?12:
			12))));
	}ef(m_State==JUMPING) {
		CX = ((m_DY>=0)?10:9);
	}ef(m_State==BREATHEIN) {
		CX=	(m_ChargePower< 40.0f?15:
			(m_ChargePower<120.0f?16:
			17));
		if (!m_HitBottom) CX += 12;
	}ef(m_State==BREATHEOUT) {
		CX = 14;
		if (!m_HitBottom) CX += 12;
	}ef(m_State==TIRED) {
		CX = ((m_PoseCounter+1)%40<20)?21:22;
	}ef(m_State==DAMAGED) {
		CX = 13;
	}ef(m_State==FROZEN) {
		CX = 23;
	}ef(m_State==DEAD) {
		CX = 13;	CY = 0;
	}ef(m_State==SMILING) {
		CX = 18;	CY = 0;
	}ef(m_State==SLEEPING) {
		CX = 19+(int)(m_PoseCounter>=20);
		if (m_Power<-1.0f/4096.0f) CX += 2;
		CY = 0;
	}ef(m_State==BLINKING) {
		CX = m_BananaDistance<MAXDISTANCE?1:30;
		if (m_pInput->GetKeyPressed(F3KEY_SMILE)) CX = 18;
	}
	// あせあせ
	RECT rc = {CX*32, CY*32, CX*32+32, CY*32+32,};
	lp->BltNatural(m_nPower==0?m_Graphic:m_Graphic2,m_nVX,m_nVY,&rc);
	if (m_Power<-1.0f/4096.0f) {
		rc.left=(m_PoseCounter2<20?0:64)+
			((m_Direction!=DIR_RIGHT&&floor(m_X/32)<m_pParent->GetWidth()-1)?0:128);
		rc.top=96;
		rc.right=rc.left+64;
		rc.bottom=rc.top+32;
		lp->BltNatural(m_Graphic,m_nVX-16,m_nVY,&rc);
	}
}
Cf3MapObjectfunya(int nCX, int nCY)
	:Cf3MapObjectMain(MOT_FUNYA)
{
	m_Graphic = ResourceManager.Get(RID_MAIN);
	m_Graphic2 = ResourceManager.Get(RID_MAINICY);
	m_DX = m_DY = 0.0f;
	m_ChargePower = 0.0f;
	m_ChargeDec = 0.001f;
	m_nPower = 0;
	m_Power = m_PowerX = m_PowerY = 0.0f;
	m_Direction = DIR_FRONT;
	m_HitLeft = m_HitRight = m_HitTop = m_OnEnemy = false; m_HitBottom = true;
	Land();
	SetPos(nCX*32+16,nCY*32+18);
	m_VOffsetX = m_VOffsetY = m_VOffsetToX = m_VOffsetToY = 0;
	m_bOriginal = true;
	m_bFirst = true;
	m_PoseCounter2=0;
	m_BananaDistance = 0.0f;
}
virtual ~Cf3MapObjectfunya() { }

};
}
