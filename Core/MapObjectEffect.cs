class Cf3MapObjectEffect : public Cf3MapObjectBase  
{
protected:
	static set<Cf3MapObjectEffect*> m_EffectList;
//	CDIB32* m_Graphic;
static RECT m_GraphicRect[4 * 16];
int m_nEffectType;
struct tagStar
{
    float x, y, dx, dy, f;
    int n, r;
} * m_Star;
int m_StarNum;
public:
	void OnDraw(CDIB32* lp);
void OnPreDraw();
static void OnPreDrawAll();
static void OnDrawAll(CDIB32* lp);
Cf3MapObjectEffect(float x, float y, int EffectType);
~Cf3MapObjectEffect();
};
