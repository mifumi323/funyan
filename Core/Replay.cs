using System;

namespace MifuminSoft.funyan.Core
{
    public class Cf3Replay : IDisposable
    {
        protected const int REPLAYBUFFER = 4096;
        protected class CKeyState
        {
            // こうしてある程度まとめたほうが速度的にもメモリ的にも効率がよろしい
            public byte[] pushed = new byte[REPLAYBUFFER];
            public byte[] pressed = new byte[REPLAYBUFFER];
        };
        protected list<CKeyState> m_State;
        protected list<CKeyState>::iterator m_iPointer;
        protected int m_nPointer;
        protected int m_nProgress;
        protected int m_nSize;
        protected class Cf3ReplayPlayerState : IDisposable
        {
            public Cf3ReplayPlayerState()
            {
                stage = null;
                map = null;
                oldgravity = Cf3Setting.theSetting.m_Gravity;
                oldhyper = Cf3Setting.theSetting.m_Hyper;
            }
            public void Dispose()
            {
                DELETE_SAFE(stage);
                DELETE_SAFE(map);
                Cf3Setting.theSetting.m_Gravity = oldgravity;
                Cf3Setting.theSetting.m_Hyper = oldhyper;
            }
            public Cf3StageFile stage;
            public Cf3Map map;
            public string stagetitle;
            public string maptitle;
            public long oldgravity;
            public long oldhyper;
        }
        protected Cf3ReplayPlayerState m_pPlayerState;
        protected string m_FileName;

        // 共通
        public int GetSize() { return m_nSize; }
        public string GetFileName() { return m_FileName; }
        public bool Finished() { return m_nProgress >= m_nSize; }
        public void Reset()
        {
            m_State.clear();
            m_State.push_back(CKeyState());
            m_nSize = 0;
            Seek();
            DELETE_SAFE(m_pPlayerState);
        }
        public void Seek(int position = 0)
        {
            m_iPointer = m_State.begin();
            m_nPointer = m_nProgress = 0;
            // position!=0にシークすべきではないのだが一応
            while (m_nProgress < position) Progress();
        }
        public void Progress()
        {
            m_nProgress++;
            m_nPointer++;
            if (m_nPointer >= REPLAYBUFFER) {
                if (++m_iPointer == m_State.end()) {
                    m_State.push_back(CKeyState());
                    m_iPointer = m_State.end();
                    m_iPointer--;
                }
                m_nPointer = 0;
            }
        }
        public Cf3Replay()
        {
            m_pPlayerState = null;
            Reset();
        }
        public void Dispose()
        {
            DELETE_SAFE(m_pPlayerState);
        }

        // Recorder
        public void Save(Cf3StageFile stage,int map)
        {
            if (CApp.theApp.MakeFileName(out m_FileName,"f3r",Cf3Setting.theSetting.m_RecordNumber,true)) {
                Cf3StageFile data;
                CT chunk;
                uint size;
                BYTE* ptr;
                // キー入力情報をこーんぽたーじゅ(謎)
                ptr = new BYTE[m_nSize * 2];
                Seek();
                while (!Finished()) {
                    ptr[m_nProgress * 2] = (*m_iPointer).pressed[m_nPointer];
                    ptr[m_nProgress * 2 + 1] = (*m_iPointer).pushed[m_nPointer];
                    Progress();
                }
                data.SetStageData(CT.CT_RPLY,m_nSize * 2,ptr);
                delete[] ptr;
                // 必要なステージ情報をコピーする
                if (ptr = stage->GetStageData(chunk = CT.CT_TITL,&size))
                    data.SetStageData(chunk,size,ptr);
                if (ptr = stage->GetStageData(chunk = CT.CT_HITS,&size))
                    data.SetStageData(chunk,size,ptr);
                if (ptr = stage->GetStageData(chunk = Cf3Map.GetChunkType(CT.CT_TL00,map),&size))
                    data.SetStageData(chunk,size,ptr);
                if (ptr = stage->GetStageData(chunk = Cf3Map.GetChunkType(CT.CT_M000,map),&size))
                    data.SetStageData(chunk,size,ptr);
                if (ptr = stage->GetStageData(chunk = Cf3Map.GetChunkType(CT.CT_M100,map),&size))
                    data.SetStageData(chunk,size,ptr);
                if (ptr = stage->GetStageData(chunk = Cf3Map.GetChunkType(CT.CT_M200,map),&size))
                    data.SetStageData(chunk,size,ptr);
                if (ptr = stage->GetStageData(chunk = CT.CT_MCD0 | (0 << 24),&size))
                    data.SetStageData(chunk,size,ptr);
                else if (ptr = stage->GetStageData(chunk = CT.CT_MCF0 | (0 << 24),&size))
                    data.SetStageData(chunk,size,ptr);
                if (ptr = stage->GetStageData(chunk = CT.CT_MCD0 | (1 << 24),&size))
                    data.SetStageData(chunk,size,ptr);
                else if (ptr = stage->GetStageData(chunk = CT.CT_MCF0 | (1 << 24),&size))
                    data.SetStageData(chunk,size,ptr);
                if (ptr = stage->GetStageData(chunk = CT.CT_MCD0 | (2 << 24),&size))
                    data.SetStageData(chunk,size,ptr);
                else if (ptr = stage->GetStageData(chunk = CT.CT_MCF0 | (2 << 24),&size))
                    data.SetStageData(chunk,size,ptr);
                // 追加の情報
                data.SetStageData(CT.CT_STGN,4,&map);
                data.SetStageData(CT.CT_GRVT,4,&Cf3Setting.theSetting.m_Gravity);
                data.SetStageData(CT.CT_HYPR,4,&Cf3Setting.theSetting.m_Hyper);
                data.Write(m_FileName);
            }
        }
        public void StartRecording()
        {
            Reset();
        }
        public void Record()
        {
            (*m_iPointer).pressed[m_nPointer] = (*m_iPointer).pushed[m_nPointer] = 0;
            for (int i = 0;i < 8;i++) {
                if (Cf3Input.f3Input.GetKeyPressed(i + 1)) (*m_iPointer).pressed[m_nPointer] |= (1 << i);
                if (Cf3Input.f3Input.GetKeyPushed(i + 1)) (*m_iPointer).pushed[m_nPointer] |= (1 << i);
            }
            Progress();
            m_nSize++;
        }

        // Player
        public void Load(string filename)
        {
            m_FileName = filename;
            Reset();
            BYTE* ptr;
            DWORD size;
            m_pPlayerState = new Cf3ReplayPlayerState();
            m_pPlayerState->stage = new Cf3StageFile();
            m_pPlayerState->stage->Read(m_FileName);
            // シークレットの状態と入力情報も読み込む
            Cf3Setting.theSetting.m_Gravity = *m_pPlayerState->stage->GetStageData(CT.CT_GRVT,null);
            Cf3Setting.theSetting.m_Hyper = *m_pPlayerState->stage->GetStageData(CT.CT_HYPR,null);
            ptr = m_pPlayerState->stage->GetStageData(CT.CT_RPLY,&size);
            m_nSize = size >> 1;
            for (int i = 0;i < m_nSize;i++) {
                (*m_iPointer).pressed[m_nPointer] = ptr[m_nProgress * 2];
                (*m_iPointer).pushed[m_nPointer] = ptr[m_nProgress * 2 + 1];
                Progress();
            }
            // 最後にマップを読み込む(設定を先に反映させる必要がある)
            ptr = m_pPlayerState->stage->GetStageData(CT.CT_STGN,null);
            m_pPlayerState->map = new Cf3Map(m_pPlayerState->stage,ptr ? *ptr : 0);
            Seek();
        }
        public void Replay()
        {
            if (Finished()) return;
            Cf3GameInput.ReplayInput.pressed = (*m_iPointer).pressed[m_nPointer];
            Cf3GameInput.ReplayInput.pushed = (*m_iPointer).pushed[m_nPointer];
            m_pPlayerState->map->OnMove();
            m_pPlayerState->map->OnPreDraw();
            Progress();
        }
        void OnDraw(CDIB32* lp) { m_pPlayerState->map->OnDraw(lp); }
        Cf3Map GetMap() { return m_pPlayerState->map; }

    }
}
