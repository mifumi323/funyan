using System;

namespace MifuminSoft.funyan.Core
{
    public enum BGMMode
    {
        BGMM_NONE,
        BGMM_DEFAULT,
        BGMM_USER,
    };

    public enum BGMNumber
    {
        BGMN_SIRENT,
        BGMN_TITLE,
        BGMN_GAMEFUNYA,
        BGMN_GAMEBANANA,
        BGMN_GAMENEEDLE,
        BGMN_GAMEGEASPRIN,
        BGMN_GAMEWIND,
        BGMN_GAMEICE,
        BGMN_GAMEFIRE,
        BGMN_GAMEMRFRAME,
        BGMN_GAMEEELPITCHER,
        BGMN_CLEAR,
        BGMN_ENDING,
        BGMN_MISS,
        BGMN_GAMEOVER,
        BGMN_EXPLAIN,
        BGMN_SIZE,              // この番号のBGMは存在しない。配列のサイズを決めるため使う
    };

    public enum MENumber
    {
        MEN_SLEEP,
        MEN_AWAKE,
        MEN_GEASPRIN,
        MEN_BANANADISTANCE,
        MEN_BANANAPOSITION,
        MEN_SIZE,               // この番号のBGMは存在しない。配列のサイズを決めるため使う
    };

    public class CBGMBase : IDisposable
    {
        protected BGMNumber m_PlayNo;
        protected virtual void InnerPlay(BGMNumber no) { }
        protected virtual void InnerStop() { }

        public virtual void MusicEffect(MENumber no,float param = 0.0f) { }
        public virtual void Update() { }
        public virtual string GetClassName() { return "CBGMBase"; }

        public BGMNumber GetBGM() { return m_PlayNo; }

        public void Stop()
        {
            InnerStop();
            m_PlayNo = BGMNumber.BGMN_SIRENT;
        }

        public void Play(BGMNumber no)
        {
            if (m_PlayNo == no) return;
            Stop();
            m_PlayNo = no;
            InnerPlay(no);
        }

        /// <returns>成功したらtrue</returns>
        public virtual bool Init() { return true; }

        public CBGMBase()
        {
            m_PlayNo = BGMNumber.BGMN_SIRENT;
        }

        public virtual void Dispose() { }
    }
}
