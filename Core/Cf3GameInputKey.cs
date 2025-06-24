namespace MifuminSoft.funyan.Core
{
    public class Cf3GameInputKey : Cf3GameInput
    {
        public override bool GetKeyPushed(int key) { return Cf3Input.f3Input.GetKeyPushed(key); }
        public override bool GetKeyPressed(int key) { return Cf3Input.f3Input.GetKeyPressed(key); }
    }
}
