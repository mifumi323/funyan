namespace MifuminSoft.funyan.Core
{
    public class Cf3GameInputReplay : Cf3GameInput
    {
        public byte pushed, pressed;
        public override bool GetKeyPushed(int key) { return 0 != (pushed & (1 << (key - 1))); }
        public override bool GetKeyPressed(int key) { return 0 != (pressed & (1 << (key - 1))); }
    }
}
