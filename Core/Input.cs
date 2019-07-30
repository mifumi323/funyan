namespace MifuminSoft.funyan.Core
{
enum {
    F3KEY_EXIT,
    F3KEY_PAUSE,
    F3KEY_UP,
    F3KEY_LEFT,
    F3KEY_RIGHT,
    F3KEY_DOWN,
    F3KEY_JUMP,
    F3KEY_ATTACK,
    F3KEY_SMILE,
    F3KEY_FPS,
    F3KEY_BGMNONE,
    F3KEY_BGMDEF,
    F3KEY_BGMUSER,
    F3KEY_CAPTURE,
    F3KEY_RECORD,

    F3KEY_BUFSIZE
};

struct tagButtonState
{
    int Button;     // 実際のボタン
    bool Pressed;   // 今押されているか
    bool Pushed;        // 今押されたか
    bool Released;	// 今離されたか
};

extern class Cf3Input
{
    public:
	LPSTR GetKeyName(int key) { return m_KeyName[m_ButtonState[key].Button]; }
    void Init(int* key);
    void Input();
    bool GetKeyReleased(int key) { return key < F3KEY_BUFSIZE && m_ButtonState[key].Released; }
    bool GetKeyPushed(int key) { return key < F3KEY_BUFSIZE && m_ButtonState[key].Pushed; }
    bool GetKeyPressed(int key) { return key < F3KEY_BUFSIZE && m_ButtonState[key].Pressed; }
    Cf3Input() { ZERO(m_ButtonState); }
    virtual ~Cf3Input() { }

    private:
	tagButtonState m_ButtonState[F3KEY_BUFSIZE];
    static const int m_DefaultButton[F3KEY_BUFSIZE];
    static const LPSTR m_KeyName[256];
    protected:
	LRESULT SetButtonState(int button, bool state);
}
f3Input;
}