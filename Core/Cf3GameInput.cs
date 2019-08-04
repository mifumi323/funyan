namespace MifuminSoft.funyan.Core
{
    public abstract class Cf3GameInput
    {
        public abstract bool GetKeyPushed(int key);
        public abstract bool GetKeyPressed(int key);

        public static Cf3GameInputKey KeyInput { get; } = new Cf3GameInputKey();
        public static Cf3GameInputReplay ReplayInput { get; } = new Cf3GameInputReplay();
    }
}
