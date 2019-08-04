using System;
using System.Collections.Generic;

namespace MifuminSoft.funyan.Core
{
    public class Cf3Replay : IDisposable
    {
        protected const int REPLAYBUFFER = 4096;
        protected List<byte> m_keyPressed = new List<byte>(REPLAYBUFFER);
        protected List<byte> m_keyPushed = new List<byte>(REPLAYBUFFER);
        protected int m_nProgress;
        protected uint m_nSize;
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
                TL.DELETE_SAFE(ref stage);
                TL.DELETE_SAFE(ref map);
                Cf3Setting.theSetting.m_Gravity = oldgravity;
                Cf3Setting.theSetting.m_Hyper = oldhyper;
            }
            public Cf3StageFile stage;
            public Cf3Map map;
            public string stagetitle;
            public string maptitle;
            public int oldgravity;
            public int oldhyper;
        }
        protected Cf3ReplayPlayerState m_pPlayerState;
        protected string m_FileName;

        // 共通
        public uint GetSize() { return m_nSize; }
        public string GetFileName() { return m_FileName; }
        public bool Finished() { return m_nProgress >= m_nSize; }
        public void Reset()
        {
            m_keyPushed.Clear();
            m_keyPressed.Clear();
            m_nSize = 0;
            Seek();
            TL.DELETE_SAFE(ref m_pPlayerState);
        }
        public void Seek(int position = 0)
        {
            m_nProgress = 0;
            while (m_nProgress < position) Progress();
        }
        public void Progress()
        {
            m_nProgress++;
        }
        public Cf3Replay()
        {
            m_pPlayerState = null;
            Reset();
        }
        public void Dispose()
        {
            TL.DELETE_SAFE(ref m_pPlayerState);
        }

        // Recorder
        public void Save(Cf3StageFile stage, int map)
        {
            if (CApp.theApp.MakeFileName(out m_FileName, "f3r", Cf3Setting.theSetting.m_RecordNumber, true)) {
                var data = new Cf3StageFile();
                CT chunk;
                uint size;
                byte[] ptr;
                // キー入力情報をこーんぽたーじゅ(謎)
                ptr = new byte[m_nSize * 2];
                Seek();
                while (!Finished()) {
                    ptr[m_nProgress * 2] = m_keyPressed[m_nProgress];
                    ptr[m_nProgress * 2 + 1] = m_keyPushed[m_nProgress];
                    Progress();
                }
                data.SetStageData(CT.CT_RPLY, ptr);
                // 必要なステージ情報をコピーする
                if (ptr = stage.GetStageData(chunk = CT.CT_TITL))
                    data.SetStageData(chunk, ptr);
                if (ptr = stage.GetStageData(chunk = CT.CT_HITS))
                    data.SetStageData(chunk, ptr);
                if (ptr = stage.GetStageData(chunk = Cf3Map.GetChunkType(CT.CT_TL00, map)))
                    data.SetStageData(chunk, ptr);
                if (ptr = stage.GetStageData(chunk = Cf3Map.GetChunkType(CT.CT_M000, map)))
                    data.SetStageData(chunk, ptr);
                if (ptr = stage.GetStageData(chunk = Cf3Map.GetChunkType(CT.CT_M100, map)))
                    data.SetStageData(chunk, ptr);
                if (ptr = stage.GetStageData(chunk = Cf3Map.GetChunkType(CT.CT_M200, map)))
                    data.SetStageData(chunk, ptr);
                if (ptr = stage.GetStageData(chunk = CT.CT_MCD0 | (0 << 24)))
                    data.SetStageData(chunk, ptr);
                else if (ptr = stage.GetStageData(chunk = CT.CT_MCF0 | (0 << 24)))
                    data.SetStageData(chunk, ptr);
                if (ptr = stage.GetStageData(chunk = CT.CT_MCD0 | (1 << 24)))
                    data.SetStageData(chunk, ptr);
                else if (ptr = stage.GetStageData(chunk = CT.CT_MCF0 | (1 << 24)))
                    data.SetStageData(chunk, ptr);
                if (ptr = stage.GetStageData(chunk = CT.CT_MCD0 | (2 << 24)))
                    data.SetStageData(chunk, ptr);
                else if (ptr = stage.GetStageData(chunk = CT.CT_MCF0 | (2 << 24)))
                    data.SetStageData(chunk, ptr);
                // 追加の情報
                data.SetStageData(CT.CT_STGN, map);
                data.SetStageData(CT.CT_GRVT, Cf3Setting.theSetting.m_Gravity);
                data.SetStageData(CT.CT_HYPR, Cf3Setting.theSetting.m_Hyper);
                data.Write(m_FileName);
            }
        }
        public void StartRecording()
        {
            Reset();
        }
        public void Record()
        {
            byte pressed = 0, pushed = 0;
            for (int i = 0; i < 8; i++) {
                if (Cf3Input.f3Input.GetKeyPressed(i + 1)) pressed |= (byte)(1 << i);
                if (Cf3Input.f3Input.GetKeyPushed(i + 1)) pushed |= (byte)(1 << i);
            }
            m_keyPressed[m_nProgress] = pressed;
            m_keyPushed[m_nProgress] = pushed;
            Progress();
            m_nSize++;
        }

        // Player
        public void Load(string filename)
        {
            m_FileName = filename;
            Reset();
            byte[] ptr;
            DWORD size;
            m_pPlayerState = new Cf3ReplayPlayerState();
            m_pPlayerState.stage = new Cf3StageFile();
            m_pPlayerState.stage.Read(m_FileName);
            // シークレットの状態と入力情報も読み込む
            Cf3Setting.theSetting.m_Gravity = m_pPlayerState.stage.GetStageData(CT.CT_GRVT);
            Cf3Setting.theSetting.m_Hyper = m_pPlayerState.stage.GetStageData(CT.CT_HYPR);
            ptr = m_pPlayerState.stage.GetStageData(CT.CT_RPLY);
            m_nSize = size >> 1;
            for (int i = 0; i < m_nSize; i++) {
                m_keyPressed[m_nProgress] = ptr[m_nProgress * 2];
                m_keyPushed[m_nProgress] = ptr[m_nProgress * 2 + 1];
                Progress();
            }
            // 最後にマップを読み込む(設定を先に反映させる必要がある)
            ptr = m_pPlayerState.stage.GetStageData(CT.CT_STGN);
            m_pPlayerState.map = new Cf3Map(m_pPlayerState.stage, ptr ? *ptr : 0);
            Seek();
        }
        public void Replay()
        {
            if (Finished()) return;
            Cf3GameInput.ReplayInput.pressed = m_keyPressed[m_nProgress];
            Cf3GameInput.ReplayInput.pushed = m_keyPushed[m_nProgress];
            m_pPlayerState.map.OnMove();
            m_pPlayerState.map.OnPreDraw();
            Progress();
        }
        public void OnDraw(CDIB32 lp) { m_pPlayerState.map.OnDraw(lp); }
        public Cf3Map GetMap() { return m_pPlayerState.map; }
    }
}
