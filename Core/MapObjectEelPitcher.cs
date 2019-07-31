using System.Collections.Generic;

namespace MifuminSoft.funyan.Core
{
public class Cf3MapObjectEelPitcher : Cf3MapObjectBase  
{
        private void Freeze() { m_State = EELFROZEN; m_Delay = 80; }
        private void Seed()
{
	int d=0;
	m_State = m_Delay?EELBUD:EELLEAF;
	m_RootX = m_X;
	m_RootY = floor(m_Y/32)*32;
	if (m_pParent->GetHit(floor((m_X-14)/32),floor(m_Y/32),HIT_TOP)) d|=1;
	if (m_pParent->GetHit(floor((m_X+14)/32),floor(m_Y/32),HIT_TOP)) d|=2;
	m_Direction = (d==1?DIR_RIGHT:(d==2?DIR_LEFT:((CApp::random(2))?DIR_LEFT:DIR_RIGHT)));
}
        //	CDIB32* m_Graphic;
        private static Dictionary<int, Cf3MapObjectEelPitcher> m_EnemyList = new Dictionary<int, Cf3MapObjectEelPitcher>();

        private f3MapObjectDirection m_Direction;
        private int m_Level;                        // 最大高さ
        private int m_Delay;                        // 待ち時間
        private enum f3EelPitcherState
{
    EELSEED,
    EELBUD,
    EELLEAF,
    EELFROZEN,
    EELDEAD,
}
m_State;
private float m_DX, m_DY;
        private float m_RootX, m_RootY;         // 根元
        private bool m_bBlinking;

        public bool IsLeaf() { return m_State == EELLEAF || m_State == EELFROZEN; }
        public static void OnDrawAll(CDIB32* lp)
        {
            foreach (var it in m_EnemyList) {
                if (it.Value.IsValid()) it.Value.OnDraw(lp);
            }
        }
        public static void OnPreDrawAll()
        {
            foreach (var it in m_EnemyList) {
                if (it.Value.IsValid()) it.Value.OnPreDraw();
            }
        }
        public static void SynergyAll()
        {
            foreach (var it in m_EnemyList) {
                if (it.Value.IsValid()) it.Value.Synergy();
            }
        }
        public static void OnMoveAll()
        {
            foreach (var it in m_EnemyList) {
                if (it.Value.IsValid()) it.Value.OnMove();
            }
        }
        public static Dictionary<int, Cf3MapObjectEelPitcher> All() { return m_EnemyList; }
        public void Reaction(Cf3MapObjectBase* obj)
{
	if (obj==NULL||obj==this) return;
	float objX, objY;
	obj->GetPos(objX,objY);
	switch(obj->GetType()) {
	case MOT_FUNYA:{
		if (TL.IsIn(m_X-16,objX,m_X+16)) {
			if (TL.IsIn(m_Y-16,objY,m_Y)) {
				// 踏まれた！！
				m_bBlinking = true;
			}
		}
		break;
				   }
	case MOT_NEEDLE:
	case MOT_GEASPRIN:{
		if (TL.IsIn(m_X-16,objX,m_X+16)) {
			if (TL.IsIn(m_Y,objY,m_Y+40)) {
				// 食べちゃった！！
				m_Level++;
			}
		}
		break;
				   }
	default:{
		return;
			}
	}
}
        public void Synergy()
{
	if (!IsValid()) return;
	m_bBlinking = false;
	int cx, cy;
	GetCPos(cx, cy);
	Cf3MapObjectBase**it;
	if (m_State==EELLEAF) {
		for(it=m_pParent->GetMapObjects(cx-2, cy-2, cx+2, cy+2, MOT_FUNYA); (*it)!=NULL; it++){
			if ((*it)->IsValid()) Reaction((*it));
		}
		if (m_RootY-m_Y>16) {
			for(it=m_pParent->GetMapObjects(cx-1, cy-1, cx+1, cy+1, MOT_ICE); (*it)!=NULL; it++){
				if ((*it)->IsValid()) {
					float objX, objY;
					(*it)->GetPos(objX,objY);
					if (TL.IsIn(m_X-16,objX,m_X+16)) {
						if (TL.IsIn(m_Y,objY,m_Y+40)) {
							Freeze();
						}
					}
				}
			}
		}
	}
	for(it=m_pParent->GetMapObjects(cx-1, cy-1, cx+1, cy+1, MOT_EELPITCHER); (*it)!=NULL; it++){
		if ((*it)->IsValid()&&(*it)!=this) {
			float objX, objY;
			(*it)->GetPos(objX,objY);
			if (m_State==EELLEAF||m_State==EELFROZEN) {
				if (TL.IsIn(m_X-16,objX,m_X+16)) {
					if (TL.IsIn(m_Y,objY,m_Y+16)) {
						if (((Cf3MapObjectEelPitcher*)(*it))->m_State!=EELLEAF) {
							// 食べちゃった！！
							m_Level+=((Cf3MapObjectEelPitcher*)(*it))->m_Level;
						}else {
							// 食べられちゃった！！
							m_State = EELDEAD;
						}
					}
				}
			}
                else if (m_State==EELSEED) {
				if (TL.IsIn(objX-16,m_X,objX+16)) {
					if (TL.IsIn(objY,m_Y,objY+16)) {
						if (((Cf3MapObjectEelPitcher*)(*it))->m_State!=EELDEAD) {
							// 食べられちゃった！！
							m_State = EELDEAD;
						}else {
							// 食べちゃった！！
							m_Level+=((Cf3MapObjectEelPitcher*)(*it))->m_Level;
						}
					}
				}
			}
		}
	}
	for(it=m_pParent->GetMapObjects(cx-2, cy-2, cx+2, cy+2, MOT_GEASPRIN); (*it)!=NULL; it++){
		if ((*it)->IsValid()) Reaction((*it));
	}
	for(it=m_pParent->GetMapObjects(cx-1, cy-1, cx+1, cy+1, MOT_NEEDLE); (*it)!=NULL; it++){
		if ((*it)->IsValid()) Reaction((*it));
	}
}
        public void OnMove()
{
	if (m_State==EELLEAF){
            TL.BringClose(ref m_Y,m_RootY-m_Level*32,4.0f);
		m_DX = m_DX + (m_pParent->GetWind(floor(m_X/32),floor(m_Y/32))-m_DX)*m_Level*0.1+(m_RootX-m_X)*0.025;
            TL.Saturate(-14.0f,ref m_DX,14.0f);
		m_X += m_DX;
		if (m_pParent->GetHit(floor((m_X-16)/32),floor(m_Y/32),HIT_RIGHT)) {
			m_DX = 0;
			m_X = floor(m_X/32)*32+16;
		}
            else if (m_pParent->GetHit(floor((m_X+16)/32),floor(m_Y/32),HIT_LEFT)) {
			m_DX = 0;
			m_X = floor(m_X/32)*32+16;
		}
	}
        else if (m_State==EELFROZEN) {
		if (--m_Delay==0) {
			m_Y += 16;
			m_State = EELSEED;
			m_Delay=200;
			new Cf3MapObjectEffect(m_X, m_Y, 0);
		}
	}
        else if (m_State==EELSEED) {
            TL.BringClose(ref m_DY,8.0f,1.0f);
		m_DX = m_DX + (m_pParent->GetWind(floor(m_X/32),floor(m_Y/32))-m_DX)*0.2;
            TL.Saturate(-14.0f,ref m_DX,14.0f);
		m_X += m_DX;
		if (m_pParent->GetHit(floor((m_X-16)/32),floor(m_Y/32),HIT_RIGHT)) {
			m_DX = 0;
			m_X = floor(m_X/32)*32+16;
		}
            else if (m_pParent->GetHit(floor((m_X+16)/32),floor(m_Y/32),HIT_LEFT)) {
			m_DX = 0;
			m_X = floor(m_X/32)*32+16;
		}
		m_Y += m_DY;
		if (floor(m_Y/32)!=floor((m_Y-m_DY)/32)) {
			// 32ドット境界をまたいだ！！
			if (m_pParent->GetHit(floor(m_X/32),floor(m_Y/32),HIT_TOP)
				|| floor(m_Y/32)>=m_pParent->GetHeight()) {
				Seed();
			}else {
				if (m_pParent->GetHit(floor((m_X-16)/32),floor(m_Y/32),HIT_RIGHT)) {
					m_DX = 0;
					m_X = floor(m_X/32)*32+16;
				}
                    else if (m_pParent->GetHit(floor((m_X+16)/32),floor(m_Y/32),HIT_LEFT)) {
					m_DX = 0;
					m_X = floor(m_X/32)*32+16;
				}
			}
		}
	}else if (m_State==EELBUD) {
		if (--m_Delay==0) m_State = EELLEAF;
	}
        else if (m_State==EELDEAD) {
		Kill();
	}
}
        public void OnDraw(CDIB32* lp)
{
	SetViewPos(-16,-16);
	int height = m_RootY-m_Y;
	RECT rc;
	CDIB32* graphic = ResourceManager.Get(RID_EELPITCHER);
	if (m_State==EELLEAF || m_State==EELFROZEN) {
		int offset1 = (m_State==EELLEAF?0:96);
		int offset2 = (m_Direction==DIR_LEFT?32:64);
		int offset3 = (m_bBlinking?32:0);
		// あたま
		int height1 = (height>=16?32:height+16);
		rc.left=offset1+offset2;	rc.top = offset3;
		rc.right=rc.left+32;		rc.bottom = rc.top+height1;
		lp->Blt(graphic,m_nVX,m_nVY,&rc);
		// 茎
		if (height>16) {
			int i, h, h2;
			h = floor((m_RootY-m_Y)/(abs(m_RootY-m_Y)+1)+1);
			if (h > 32) h = 32;
			for (i=16; i<height; i+=h) {
				h2 = h<=height-i?h:height-i;
				rc.left=offset1;		rc.top = offset2;
				rc.right=rc.left+32;	rc.bottom = rc.top+h2;
				// +0.5fは小数点以下四捨五入のため
				SetViewPos(-16+(m_RootX-m_X)*(i-16)/(height-16)+0.5f,-16);
				lp->Blt(graphic,m_nVX,m_nVY+16+i,&rc);
			}
		}
		SetViewPos(-16,-16);
		if (TL.IsIn(17,height,32)) {
			// あご
			int height3 = height-16;
			rc.left=offset1+offset2;	rc.top = 80;
			rc.right=rc.left+32;		rc.bottom = rc.top+height3;
			lp->Blt(graphic,m_nVX,m_nVY+32,&rc);
		}
            else if (TL.IsIn(33,height,48)) {
			int height2 = height-32;
			// ふくろ
			rc.left=offset1+offset2;	rc.top = 64;
			rc.right=rc.left+32;		rc.bottom = rc.top+height2;
			lp->Blt(graphic,m_nVX,m_nVY+32,&rc);
			// あご
			rc.left=offset1+offset2;	rc.top = 80;
			rc.right=rc.left+32;		rc.bottom = rc.top+16;
			lp->Blt(graphic,m_nVX,m_nVY+height,&rc);
		}
            else if (height>48) {
			// ふくろ
			rc.left=offset1+offset2;	rc.top = 64;
			rc.right=rc.left+32;		rc.bottom = rc.top+16;
			lp->Blt(graphic,m_nVX,m_nVY+32,&rc);
			// あご
			rc.left=offset1+offset2;	rc.top = 80;
			rc.right=rc.left+32;		rc.bottom = rc.top+16;
			lp->Blt(graphic,m_nVX,m_nVY+48,&rc);
		}
	}
        else if (m_State==EELSEED) {
		rc.left = 0;	rc.top = 0;
		rc.right = 32;	rc.bottom = 32;
		lp->Blt(graphic,m_nVX,m_nVY,&rc);
	}
}
        public Cf3MapObjectEelPitcher(int nCX, int nCY)
	:Cf3MapObjectBase(MOT_EELPITCHER)
	,m_Delay(0)
	,m_Level(1)
	,m_DX(0), m_DY(0)
	,m_State(EELSEED)
	,m_bBlinking(false)
{
	m_EnemyList.Add(GetID(), this);
//	m_Graphic = ResourceManager.Get(RID_EELPITCHER);
	SetPos(nCX*32+16,nCY*32+16);
}
        public ~Cf3MapObjectEelPitcher()
        {
            m_EnemyList.Remove(GetID());
        }

};
}
