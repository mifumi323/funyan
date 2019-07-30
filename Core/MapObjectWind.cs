namespace MifuminSoft.funyan.Core
{
class Cf3MapObjectWind : public Cf3MapObjectBase  
{
protected:
	static set<Cf3MapObjectWind*> m_WindList;

struct tagWindParticle
{
    float x;
    int y;
    float dx;
    int color;
} * m_Particle;
int m_ParticleCount;

float m_Left, m_Right;
float m_Strength;

static CDIB32 m_Graphic[2];
static bool m_bGraphicInitialize;

public:
	void OnPreDraw();
void OnDraw(CDIB32* lp);
static void OnPreDrawAll();
static void OnDrawAll(CDIB32* lp);
Cf3MapObjectWind(int x, int y, int w, float s);
virtual ~Cf3MapObjectWind();

};
}