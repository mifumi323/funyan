namespace MifuminSoft.funyan.Core
{
    public struct tagButtonState
    {
        public int Button;     // 実際のボタン
        public bool Pressed;   // 今押されているか
        public bool Pushed;        // 今押されたか
        public bool Released;   // 今離されたか
    }
}
