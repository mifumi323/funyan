using System;

namespace MifuminSoft.funyan.Core
{
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
