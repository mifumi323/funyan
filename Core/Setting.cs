using System;

namespace MifuminSoft.funyan.Core
{
    public class Cf3Setting : IDisposable
    {
        // 普通の設定項目
        public const string SS_BGM = "BGM";
        public const string SS_FULLSCREEN = "FULLSCREEN";
        public const string SS_ZOOM = "ZOOM";
        public const string SS_BACKGROUND = "BACKGROUND";
        public const string SS_CHARACTER = "CHARACTER";
        public const string SS_VIEWTIME = "VIEWTIME";
        public const string SS_FPS = "FPS";
        public const string SS_ESP = "ESP";
        public const string SS_GRAVITY = "GRAVITY";
        public const string SS_RECORDCLEAR = "RECORDCLEAR";
        public const string SS_RECORDMISS = "RECORDMISS";
        public const string SS_RECORDNUMBER = "RECORDNUMBER";

        // プレイ記録
        public const string SS_BANANA = "BANANA";
        public const string SS_PLAYTIME = "PLAYTIME";
        public const string SS_SLEEPTIME = "SLEEPTIME";
        public const string SS_SMILES = "SMILES";
        public const string SS_TIMEMASTER = "TIMEMASTER";
        public const string SS_EYEWITNESS = "EYEWITNESS";
        public const string SS_FEATHERIRON = "FEATHERIRON";
        public const string SS_GRAPECOLORED = "GRAPECOLORED";
        public const string SS_ESREVER = "ESREVER";
        public const string SS_DRAWMETHOD = "DRAWMETHOD";
        public const string SS_OUTLINE = "OUTLINE";
        public const string SS_COLDMAN = "COLDMAN";
        public const string SS_HYPER = "HYPER";
        public const string SS_ANDBALLOON = "ANDBALLOON";

        // キー設定
        public const string SS_KEY_EXIT = "KEY_EXIT";
        public const string SS_KEY_PAUSE = "KEY_PAUSE";
        public const string SS_KEY_UP = "KEY_UP";
        public const string SS_KEY_LEFT = "KEY_LEFT";
        public const string SS_KEY_RIGHT = "KEY_RIGHT";
        public const string SS_KEY_DOWN = "KEY_DOWN";
        public const string SS_KEY_JUMP = "KEY_JUMP";
        public const string SS_KEY_ATTACK = "KEY_ATTACK";
        public const string SS_KEY_SMILE = "KEY_SMILE";
        public const string SS_KEY_FPS = "KEY_FPS";
        public const string SS_KEY_BGMNONE = "KEY_BGMNONE";
        public const string SS_KEY_BGMDEF = "KEY_BGMDEF";
        public const string SS_KEY_BGMUSER = "KEY_BGMUSER";
        public const string SS_KEY_CAPTURE = "KEY_CAPTURE";
        public const string SS_KEY_RECORD = "KEY_RECORD";

        // その他
        public const string SS_PROGRESS = "PROGRESS";
        public const string SS_CHECKSUM = "CHECKSUM";


        public void SetProgress(string file, int stage)
        {
            // TODO: 一から実装し直したほうがよさそうだし、一度全部消すよ。
            throw new NotImplementedException();
        }
        public int GetProgress(string file)
        {
            // TODO: 一から実装し直したほうがよさそうだし、一度全部消すよ。
            throw new NotImplementedException();
        }
        public void InitSaveData()
        {
            // TODO: 一から実装し直したほうがよさそうだし、一度全部消すよ。
            throw new NotImplementedException();
        }
        public int GetChecksum()
        {
            // TODO: 一から実装し直したほうがよさそうだし、一度全部消すよ。
            throw new NotImplementedException();
        }
        public Cf3Setting()
        {
            // TODO: 一から実装し直したほうがよさそうだし、一度全部消すよ。
            throw new NotImplementedException();
        }
        public void Dispose()
        {
            // TODO: 一から実装し直したほうがよさそうだし、一度全部消すよ。
            throw new NotImplementedException();
        }

        public int m_BGMMode;
        public int m_FullScreen;
        public int m_Zoom;
        public int m_Background;
        public int m_Character;
        public int m_ViewTime;
        public int m_FPS;
        public int m_ESP;
        public int m_Gravity;
        public int m_RecordClear;
        public int m_RecordMiss;
        public int m_RecordNumber;
        public int m_Banana;
        public int m_PlayTime;
        public int m_SleepTime;
        public int m_Smiles;
        public int m_TimeMaster;
        public int m_Eyewitness;
        public int m_FeatherIron;
        public int m_GrapeColored;
        public int m_Esrever;
        public int m_DrawMethod;
        public int m_Outline;
        public int m_ColdMan;
        public int m_Hyper;
        public int m_AndBalloon;
        public int m_StartTime;
        public int[] m_Key = new int[(int)F3KEY.F3KEY_BUFSIZE];

        public static Cf3Setting theSetting { get; } = new Cf3Setting();
    }
}
