namespace MifuminSoft.funyan.Core
{
public class Cf3Replay
{
        protected const int REPLAYBUFFER = 4096;
        protected class CKeyState
    {
		public CKeyState() { ZERO(pushed); ZERO(pressed); }
        // こうしてある程度まとめたほうが速度的にもメモリ的にも効率がよろしい
        public BYTE pushed[REPLAYBUFFER], pressed[REPLAYBUFFER];
    };
        protected list<CKeyState> m_State;
        protected list<CKeyState>::iterator m_iPointer;
        protected int m_nPointer;
        protected int m_nProgress;
        protected int m_nSize;
        protected class Cf3ReplayPlayerState
    {
		public Cf3ReplayPlayerState()
        {
            stage = NULL;
            map = NULL;
            oldgravity = theSetting->m_Gravity;
            oldhyper = theSetting->m_Hyper;
        }
        public virtual ~Cf3ReplayPlayerState()
        {
            DELETE_SAFE(stage);
            DELETE_SAFE(map);
            theSetting->m_Gravity = oldgravity;
            theSetting->m_Hyper = oldhyper;
        }
        public Cf3StageFile* stage;
        public Cf3Map* map;
        public string stagetitle;
        public string maptitle;
        public long oldgravity;
        public long oldhyper;
    }*m_pPlayerState;
	    protected string m_FileName;

        // 共通
        public int GetSize() { return m_nSize; }
        public string GetFileName() { return m_FileName; }
        public bool Finished() { return m_nProgress >= m_nSize; }
        public void Reset()
{
	m_State.clear();
	m_State.push_back(CKeyState());
	m_nSize=0;
	Seek();
	DELETE_SAFE(m_pPlayerState);
}
        public void Seek(int position = 0)
{
	m_iPointer = m_State.begin();
	m_nPointer=m_nProgress=0;
	// position!=0にシークすべきではないのだが一応
	while(m_nProgress<position) Progress();
}
        public void Progress()
{
	m_nProgress++;
	m_nPointer++;
	if (m_nPointer>=REPLAYBUFFER) {
		if (++m_iPointer==m_State.end()) {
			m_State.push_back(CKeyState());
			m_iPointer = m_State.end();
			m_iPointer--;
		}
		m_nPointer=0;
	}
}
        public Cf3Replay()
{
	m_pPlayerState=NULL;
	Reset();
}
        public virtual ~Cf3Replay()
{
	DELETE_SAFE(m_pPlayerState);
}

        // Recorder
        public void Save(Cf3StageFile* stage, int map)
{
	if (CApp::MakeFileName(m_FileName,"f3r",theSetting->m_RecordNumber,true)) {
		Cf3StageFile data;
		DWORD chunk, size;
		BYTE* ptr;
		// キー入力情報をこーんぽたーじゅ(謎)
		ptr = new BYTE[m_nSize*2];
		Seek();
		while(!Finished()) {
			ptr[m_nProgress*2	] = (*m_iPointer).pressed	[m_nPointer];
			ptr[m_nProgress*2+1	] = (*m_iPointer).pushed	[m_nPointer];
			Progress();
		}
		data.SetStageData(CT_RPLY, m_nSize*2, ptr);
		delete[]ptr;
		// 必要なステージ情報をコピーする
		if (ptr=stage->GetStageData(chunk=CT_TITL,&size))
			data.SetStageData(chunk,size,ptr);
		if (ptr=stage->GetStageData(chunk=CT_HITS,&size))
			data.SetStageData(chunk,size,ptr);
		if (ptr=stage->GetStageData(chunk=Cf3Map::GetChunkType(CT_TL00,map),&size))
			data.SetStageData(chunk,size,ptr);
		if (ptr=stage->GetStageData(chunk=Cf3Map::GetChunkType(CT_M000,map),&size))
			data.SetStageData(chunk,size,ptr);
		if (ptr=stage->GetStageData(chunk=Cf3Map::GetChunkType(CT_M100,map),&size))
			data.SetStageData(chunk,size,ptr);
		if (ptr=stage->GetStageData(chunk=Cf3Map::GetChunkType(CT_M200,map),&size))
			data.SetStageData(chunk,size,ptr);
		if (ptr=stage->GetStageData(chunk=CT_MCD0|(0<<24),&size))
			data.SetStageData(chunk,size,ptr);
                else if (ptr=stage->GetStageData(chunk=CT_MCF0|(0<<24),&size))
			data.SetStageData(chunk,size,ptr);
		if (ptr=stage->GetStageData(chunk=CT_MCD0|(1<<24),&size))
			data.SetStageData(chunk,size,ptr);
                else if (ptr=stage->GetStageData(chunk=CT_MCF0|(1<<24),&size))
			data.SetStageData(chunk,size,ptr);
		if (ptr=stage->GetStageData(chunk=CT_MCD0|(2<<24),&size))
			data.SetStageData(chunk,size,ptr);
                else if (ptr=stage->GetStageData(chunk=CT_MCF0|(2<<24),&size))
			data.SetStageData(chunk,size,ptr);
		// 追加の情報
		data.SetStageData(CT_STGN, 4, &map);
		data.SetStageData(CT_GRVT, 4, &theSetting->m_Gravity);
		data.SetStageData(CT_HYPR, 4, &theSetting->m_Hyper);
		data.Write(m_FileName);
	}
}
        public void StartRecording()
{
	Reset();
}
        public void Record()
{
	(*m_iPointer).pressed[m_nPointer]=(*m_iPointer).pushed[m_nPointer]=0;
	for (int i=0; i<8; i++) {
		if (f3Input.GetKeyPressed(i+1))	(*m_iPointer).pressed	[m_nPointer] |= (1<<i);
		if (f3Input.GetKeyPushed(i+1))	(*m_iPointer).pushed	[m_nPointer] |= (1<<i);
	}
	Progress();
	m_nSize++;
}

        // Player
        public void Load(const string& filename)
{
	m_FileName = filename;
	Reset();
	BYTE* ptr;
	DWORD size;
	m_pPlayerState = new Cf3ReplayPlayerState();
	m_pPlayerState->stage = new Cf3StageFile();
	m_pPlayerState->stage->Read(m_FileName);
	// シークレットの状態と入力情報も読み込む
	theSetting->m_Gravity = *m_pPlayerState->stage->GetStageData(CT_GRVT, NULL);
	theSetting->m_Hyper = *m_pPlayerState->stage->GetStageData(CT_HYPR, NULL);
	ptr = m_pPlayerState->stage->GetStageData(CT_RPLY, &size);
	m_nSize = size>>1;
	for (int i=0; i<m_nSize; i++) {
		(*m_iPointer).pressed	[m_nPointer] = ptr[m_nProgress*2	];
		(*m_iPointer).pushed	[m_nPointer] = ptr[m_nProgress*2+1	];
		Progress();
	}
	// 最後にマップを読み込む(設定を先に反映させる必要がある)
	ptr=m_pPlayerState->stage->GetStageData(CT_STGN, NULL);
	m_pPlayerState->map = new Cf3Map(m_pPlayerState->stage, ptr?*ptr:0);
	Seek();
}
public void Replay()
{
	if (Finished()) return;
	ReplayInput.pressed	= (*m_iPointer).pressed	[m_nPointer];
	ReplayInput.pushed	= (*m_iPointer).pushed	[m_nPointer];
	m_pPlayerState->map->OnMove();
	m_pPlayerState->map->OnPreDraw();
	Progress();
}
    void OnDraw(CDIB32* lp) { m_pPlayerState->map->OnDraw(lp); }
    Cf3Map* GetMap() { return m_pPlayerState->map; }

};
}
