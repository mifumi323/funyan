using System;
using System.Collections.Generic;

namespace MifuminSoft.funyan.Core
{
    public class Setting
    {
        public static Setting Current { get; set; }

        // 普通の設定項目
        const string SS_BGM = "BGM";
        const string SS_FULLSCREEN = "FULLSCREEN";
        const string SS_ZOOM = "ZOOM";
        const string SS_BACKGROUND = "BACKGROUND";
        const string SS_CHARACTER = "CHARACTER";
        const string SS_VIEWTIME = "VIEWTIME";
        const string SS_FPS = "FPS";
        const string SS_ESP = "ESP";
        const string SS_GRAVITY = "GRAVITY";
        const string SS_RECORDCLEAR = "RECORDCLEAR";
        const string SS_RECORDMISS = "RECORDMISS";
        const string SS_RECORDNUMBER = "RECORDNUMBER";

        // プレイ記録
        const string SS_BANANA = "BANANA";
        const string SS_PLAYTIME = "PLAYTIME";
        const string SS_SLEEPTIME = "SLEEPTIME";
        const string SS_SMILES = "SMILES";
        const string SS_TIMEMASTER = "TIMEMASTER";
        const string SS_EYEWITNESS = "EYEWITNESS";
        const string SS_FEATHERIRON = "FEATHERIRON";
        const string SS_GRAPECOLORED = "GRAPECOLORED";
        const string SS_ESREVER = "ESREVER";
        const string SS_DRAWMETHOD = "DRAWMETHOD";
        const string SS_OUTLINE = "OUTLINE";
        const string SS_COLDMAN = "COLDMAN";
        const string SS_HYPER = "HYPER";
        const string SS_ANDBALLOON = "ANDBALLOON";

        // キー設定
        const string SS_KEY_EXIT = "KEY_EXIT";
        const string SS_KEY_PAUSE = "KEY_PAUSE";
        const string SS_KEY_UP = "KEY_UP";
        const string SS_KEY_LEFT = "KEY_LEFT";
        const string SS_KEY_RIGHT = "KEY_RIGHT";
        const string SS_KEY_DOWN = "KEY_DOWN";
        const string SS_KEY_JUMP = "KEY_JUMP";
        const string SS_KEY_ATTACK = "KEY_ATTACK";
        const string SS_KEY_SMILE = "KEY_SMILE";
        const string SS_KEY_FPS = "KEY_FPS";
        const string SS_KEY_BGMNONE = "KEY_BGMNONE";
        const string SS_KEY_BGMDEF = "KEY_BGMDEF";
        const string SS_KEY_BGMUSER = "KEY_BGMUSER";
        const string SS_KEY_CAPTURE = "KEY_CAPTURE";
        const string SS_KEY_RECORD = "KEY_RECORD";

        // その他
        const string SS_PROGRESS = "PROGRESS";
        const string SS_CHECKSUM = "CHECKSUM";

        protected int m_DataCount;

        protected Dictionary<string, int> m_Progress;

        public void SetProgress(string file, int stage)
        {
            m_Progress[file] = stage;
        }

        public int GetProgress(string file)
        {
            return m_Progress.TryGetValue(file, out var stage) ? stage : 0;
        }

        public Setting()
        {
            // 設定ファイル読み込みは別口で作ろう
            m_StartTime = DateTime.Now;
        }
        // 書き込みも別口で作ろう
        // public virtual ~Setting();
        public void Refresh()
        {
            // デストラクタでやっていた設定項目自動更新をここでやる
            m_PlayTime += (int)(DateTime.Now - m_StartTime).TotalSeconds;
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
        public DateTime m_StartTime;
        public int[] m_Key = new int[F3KEY_BUFSIZE];
    }
}
