﻿namespace MifuminSoft.funyan.Core
{
public class Cf3MapObjectWind : Cf3MapObjectBase  
{
        protected static set<Cf3MapObjectWind*> m_WindList;

        protected struct tagWindParticle
{
    public float x;
    public int y;
    public float dx;
    public int color;
} * m_Particle;
protected int m_ParticleCount;

        protected float m_Left, m_Right;
        protected float m_Strength;

        protected static CDIB32 m_Graphic[2];
        protected static bool m_bGraphicInitialize = false;

        public void OnPreDraw()
{
	for (int i=0; i<m_ParticleCount; i++) {
		m_Particle[i].x+=m_Particle[i].dx;
		if (m_Particle[i].x<m_Left) {
			m_Particle[i].x += m_Right-m_Left;
			m_Particle[i].y = CApp::random(32);
			m_Particle[i].color = 0xffffff*CApp::random(2);
			m_Particle[i].dx = m_Strength*(0.5f+(float)CApp::random(4096)/4096);
		}
		if (m_Particle[i].x>m_Right) {
			m_Particle[i].x -= m_Right-m_Left;
			m_Particle[i].y = CApp::random(32);
			m_Particle[i].color = 0xffffff*CApp::random(2);
			m_Particle[i].dx = m_Strength*(0.5f+(float)CApp::random(4096)/4096);
		}
	}
}
        public void OnDraw(CDIB32* lp)
{
	SetViewPos();
	for (int i=0; i<m_ParticleCount; i++) {
		lp->BltFast(&m_Graphic[m_Particle[i].color&1],m_nVX+m_Particle[i].x,m_nVY+m_Particle[i].y);
	}
}
        public static void OnPreDrawAll()
{
	for(set<Cf3MapObjectWind*>::iterator it = m_WindList.begin();it!=m_WindList.end();it++){
		if ((*it)->IsValid()) (*it)->OnPreDraw();
	}
}
        public static void OnDrawAll(CDIB32* lp)
{
	for(set<Cf3MapObjectWind*>::iterator it = m_WindList.begin();it!=m_WindList.end();it++){
		if ((*it)->IsValid()) (*it)->OnDraw(lp);
	}
}
        public Cf3MapObjectWind(int x, int y, int w, float s)
	:Cf3MapObjectBase(MOT_EFFECT)
{
	m_WindList.insert(this);
	if (!m_bGraphicInitialize) {
		m_Graphic[0].CreateSurface(1,1,false);
		m_Graphic[0].SetPixel(0,0,0);
		m_Graphic[1].CreateSurface(1,1,false);
		m_Graphic[1].SetPixel(0,0,0xffffff);
		m_bGraphicInitialize = true;
	}
	m_ParticleCount = (int)floor(w*abs(s)*0.5);
	if (m_ParticleCount==0) { Kill(); return; }
	m_Particle = new tagWindParticle[m_ParticleCount];
	m_Left = x*32;
	m_Right = (x+w)*32;
	m_Y = y*32;
	m_Strength = s;
	for (int i=0; i<m_ParticleCount; i++) {
		m_Particle[i].x = m_Left + CApp::random(w*32);
		m_Particle[i].y = CApp::random(32);
		m_Particle[i].color = CApp::random(2);
		m_Particle[i].dx = m_Strength*(0.5f+(float)CApp::random(4096)/4096);
	}
}
        public virtual ~Cf3MapObjectWind()
{
	m_WindList.erase(this);
}

};
}
