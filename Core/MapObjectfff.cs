namespace MifuminSoft.funyan.Core
{
class Cf3MapObjectfff : public Cf3MapObjectMain  
{
private:
	void Tire();
void BreatheOut();
void BreatheIn();
void Freeze(int level = 15);
void Die();
void HitCheck();
void Smile() { m_State = SMILE; }

float m_OldX, m_OldY;
float m_DX, m_DY, m_OldDX, m_OldDY; // 位置などの情報
float m_Angle, m_DAngle;            // 回転とか
float m_ChargePower;                // ジャンプチャージ係数(1.0f=100%から減ってゆく)
float m_Power, m_PowerX, m_PowerY;
int m_nPower;
enum f3fffState
{
    NORMAL,
    BREATHEIN,
    BREATHEOUT,
    TIRED,
    FROZEN,
    DEAD,
    SMILE,
}
m_State;

	int m_PoseCounter, m_PoseCounter2;

// 表示位置調整
int m_VOffsetX, m_VOffsetY;
int m_VOffsetToX, m_VOffsetToY;
public:
	bool IsFrozen() { return m_State == FROZEN; }
void Synergy();
bool IsDied() { return m_State == DEAD; }
void OnMove();
void OnDraw(CDIB32* lp);
Cf3MapObjectfff(int nCX, int nCY);
~Cf3MapObjectfff() { }

};
}