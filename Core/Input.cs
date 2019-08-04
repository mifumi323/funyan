using System;

namespace MifuminSoft.funyan.Core
{
    public enum F3KEY
    {
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
    }

    public struct tagButtonState
    {
        public int Button;     // 実際のボタン
        public bool Pressed;   // 今押されているか
        public bool Pushed;        // 今押されたか
        public bool Released;   // 今離されたか
    }

    /// <summary>
    /// Cf3Input
    /// インプット用のクラス
    /// 継承してキーボード用・マウス用・ジョイスティック用を用意する(つもりだった)
    /// </summary>
    public class Cf3Input
    {
        public string GetKeyName(int key) { return m_KeyName[m_ButtonState[key].Button]; }
        public void Init(int[] key)
        {
            for (int i = 0; i < (int)F3KEY.F3KEY_BUFSIZE; i++)
            {
                m_ButtonState[i].Button = key[i] != 0 ? key[i] : m_DefaultButton[i];
            }
        }
        public void Input()
        {
            for (int i = 0; i < (int)F3KEY.F3KEY_BUFSIZE; i++)
            {
                SetButtonState(i, GetAsyncKeyState(m_ButtonState[i].Button) != 0);
            }
        }

        /// <summary>
        /// リアルタイムのキー入力情報を取得する関数を設定します。
        /// int を引数として受け取り、押下状態なら非 0 を、非押下状態なら 0 を返します。
        /// </summary>
        public Func<int, int> GetAsyncKeyState { get; set; } = key => throw new NotImplementedException();

        public bool GetKeyReleased(int key) { return key < (int)F3KEY.F3KEY_BUFSIZE && m_ButtonState[key].Released; }
        public bool GetKeyPushed(int key) { return key < (int)F3KEY.F3KEY_BUFSIZE && m_ButtonState[key].Pushed; }
        public bool GetKeyPressed(int key) { return key < (int)F3KEY.F3KEY_BUFSIZE && m_ButtonState[key].Pressed; }

        private readonly tagButtonState[] m_ButtonState = new tagButtonState[(int)F3KEY.F3KEY_BUFSIZE];
        private static readonly int[] m_DefaultButton = new int[(int)F3KEY.F3KEY_BUFSIZE]{
            VK_ESCAPE,	// F3KEY_EXIT
            VK_RETURN,	// F3KEY_PAUSE
            VK_UP,		// F3KEY_UP
            VK_LEFT,	// F3KEY_LEFT
            VK_RIGHT,	// F3KEY_RIGHT
            VK_DOWN,	// F3KEY_DOWN
            'Z',		// F3KEY_JUMP
            'X',		// F3KEY_ATTACK
            0,			// F3KEY_SMILE
            'F',		// F3KEY_FPS
            '1',		// F3KEY_BGMNONE
            '2',		// F3KEY_BGMDEF
            '3',		// F3KEY_BGMUSER
            'C',		// F3KEY_CAPTURE
            0,			// F3KEY_RECORD
        };
        private static readonly string[] m_KeyName = new string[256]{
        // 00
        "なし",           "?",            "?",            "?",
        "?",            "?",            "?",            "?",
        "BackSpace",    "Tab",          "?",            "?",
        "Clear",        "Enter",        "?",            "?",
        // 10
        "Shift",        "Control",      "Alt",          "Pause",
        "Caps Lock",    "?",            "?",            "?",
        "?",            "?",            "?",            "Escape",
        "?",            "?",            "?",            "?",
        // 20
        "Space",        "?",            "?",            "End",
        "Home",         "←",            "↑",            "→",
        "↓",            "?",            "?",            "?",
        "?",            "Insert",       "Delete",       "?",
        // 30
        "0",            "1",            "2",            "3",
        "4",            "5",            "6",            "7",
        "8",            "9",            "?",            "?",
        "?",            "?",            "?",            "?",
        // 40
        "?",            "A",            "B",            "C",
        "D",            "E",            "F",            "G",
        "H",            "I",            "J",            "K",
        "L",            "M",            "N",            "O",
        // 50
        "P",            "Q",            "R",            "S",
        "T",            "U",            "V",            "W",
        "X",            "Y",            "Z",            "Windows(左)",
        "Windows(右)",   "Menu",         "?",            "?",
        // 60
        "0(テンキー)",  "1(テンキー)",  "2(テンキー)",  "3(テンキー)",
        "4(テンキー)",  "5(テンキー)",  "6(テンキー)",  "7(テンキー)",
        "8(テンキー)",  "9(テンキー)",  "*",            "+",
        "?",            "-",            ".",            "/",
        // 70
        "F1",           "F2",           "F3",           "F4",
        "F5",           "F6",           "F7",           "F8",
        "F9",           "F10",          "F11",          "F12",
        "F13",          "F14",          "F15",          "F16",
        // 80
        "F17",          "F18",          "F19",          "F20",
        "F21",          "F22",          "F23",          "F24",
        "?",            "?",            "?",            "?",
        "?",            "?",            "?",            "?",
        // 90
        "NumLock",      "Scroll",       "?",            "?",
        "?",            "?",            "?",            "?",
        "?",            "?",            "?",            "?",
        "?",            "?",            "?",            "?",
        // A0
        "Shift(左)", "Shift(右)", "Control(左)",   "Control(右)",
        "Menu(左)",      "Menu(右)",      "?",            "?",
        "?",            "?",            "?",            "?",
        "?",            "?",            "?",            "?",
        // B0
        "?",            "?",            "?",            "?",
        "?",            "?",            "?",            "?",
        "?",            "?",            "?",            "?",
        "?",            "?",            "?",            "?",
        // C0
        "?",            "?",            "?",            "?",
        "?",            "?",            "?",            "?",
        "?",            "?",            "?",            "?",
        "?",            "?",            "?",            "?",
        // D0
        "?",            "?",            "?",            "?",
        "?",            "?",            "?",            "?",
        "?",            "?",            "?",            "?",
        "?",            "?",            "?",            "?",
        // E0
        "?",            "?",            "?",            "?",
        "?",            "?",            "?",            "?",
        "?",            "?",            "?",            "?",
        "?",            "?",            "?",            "?",
        // F0
        "?",            "?",            "?",            "?",
        "?",            "?",            "?",            "?",
        "?",            "?",            "?",            "?",
        "?",            "?",            "?",            "?",
    };
        private static readonly int VK_ESCAPE = 0x1B;
        private static readonly int VK_RETURN = 0x0D;
        private static readonly int VK_UP = 0x26;
        private static readonly int VK_LEFT = 0x25;
        private static readonly int VK_RIGHT = 0x27;
        private static readonly int VK_DOWN = 0x28;

        protected int SetButtonState(int button, bool state)
        {
            if (button >= (int)F3KEY.F3KEY_BUFSIZE) return 1;  // そんなボタンないしぃ～
            m_ButtonState[button].Pushed = !m_ButtonState[button].Pressed && state;
            m_ButtonState[button].Released = m_ButtonState[button].Pressed && !state;
            m_ButtonState[button].Pressed = state;
            return 0;
        }

        public static Cf3Input f3Input { get; } = new Cf3Input();
    }
}
