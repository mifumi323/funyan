namespace MifuminSoft.funyan.Core
{
struct tagf3StageHeader
{
    char ident[8];      // "funya3s1"
    long datasize;      // 展開サイズ
    long packsize;		// 圧縮サイズ(datasizeと同じ場合無圧縮)
};

enum {
    CT_TITL = 0x4C544954,
    CT_AUTH = 0x48545541,
    CT_DESC = 0x43534544,
    CT_ENDM = 0x4D444E45,
    CT_STGC = 0x43475453,
    CT_REST = 0x54534552,
    CT_PNLT = 0x544C4E50,
    CT_HITS = 0x53544948,
    CT_TL00 = 0x30304C54,
    CT_M000 = 0x3030304D,
    CT_M100 = 0x3030314D,
    CT_M200 = 0x3030324D,
    CT_MCF0 = 0x3046434D,
    CT_MCD0 = 0x3044434D,

    CT_RPLY = 0x594C5052,
    CT_STGN = 0x4E475453,
    CT_GRVT = 0x54565247,
    CT_HYPR = 0x52505948,
};

class Cf3StageFile
{
    protected:
	void ClearData()
{
	for (map<DWORD,HGLOBAL>::iterator it=m_Data.begin(); it!=m_Data.end(); it++) {
		::GlobalFree((*it).second);
	}
	m_Data.clear();
}
    void AnalyzeData(BYTE* data)
{
	DWORD NumberOfBytesRead = 0;
	DWORD size, type;
	HGLOBAL data2;
	while (NumberOfBytesRead+8<=m_StageHeader.datasize) {
		size = *(DWORD*)(data+NumberOfBytesRead);
		NumberOfBytesRead += 4;
		type = *(DWORD*)(data+NumberOfBytesRead);
		NumberOfBytesRead += 4;
		if (size) {
			data2 = ::GlobalAlloc(GMEM_FIXED | GMEM_NOCOMPACT,size);
			::CopyMemory(data2, data+NumberOfBytesRead,size);
			m_Data.insert(map<DWORD,HGLOBAL>::value_type(type,data2));
		}
		NumberOfBytesRead += size;
	}
}
    tagf3StageHeader m_StageHeader;
    map<DWORD, HGLOBAL> m_Data;
    public:
	LRESULT Write(string filename)
{
	// サイズ計算するのジャー
	DWORD dwSrcSize = 0;
	map<DWORD,HGLOBAL>::iterator it;
	for (it=m_Data.begin(); it!=m_Data.end(); it++) {
		dwSrcSize += 4+4+::GlobalSize((*it).second);
	}
	// 書き込むデータを用意する
	BYTE* lpSrcAdr=new BYTE[dwSrcSize];
	BYTE* lpPos = lpSrcAdr;
	for (it=m_Data.begin(); it!=m_Data.end(); it++) {
		DWORD chunksize = ::GlobalSize((*it).second);
		DWORD chunktype = (*it).first;
		::CopyMemory(lpPos, &chunksize, 4);
		lpPos += 4;
		::CopyMemory(lpPos, &chunktype, 4);
		lpPos += 4;
		::CopyMemory(lpPos, (BYTE*)(*it).second, chunksize);
		lpPos += chunksize;
	}

	// おもむろに圧縮(←「おもむろに」の使い方間違ってる)
	BYTE* lpDstAdr=NULL;
	CLZSS lzss;
	DWORD dwDstSize;
	if (lzss.Encode(lpSrcAdr,lpDstAdr,dwSrcSize,dwDstSize)) {
		dwDstSize = dwSrcSize;
		lpDstAdr = lpSrcAdr;
		lpSrcAdr = new BYTE[1];
	}

	// やーとこさ書き込みジャー
	HANDLE hFile = ::CreateFile(filename.c_str(),
		GENERIC_WRITE,0,NULL,TRUNCATE_EXISTING,FILE_ATTRIBUTE_NORMAL,NULL
	);
	if (hFile == INVALID_HANDLE_VALUE) { // あかんやん！
		hFile = ::CreateFile(filename.c_str(),
			GENERIC_WRITE,0,NULL,CREATE_NEW,FILE_ATTRIBUTE_NORMAL,NULL
		);
		if (hFile == INVALID_HANDLE_VALUE) return 3; // Open失敗		
	}
	DWORD NumberOfBytesWritten=0;
	WriteFile(hFile, (LPVOID)"funya3s1", strlen("funya3s1"), &NumberOfBytesWritten, NULL);
	WriteFile(hFile, (LPVOID)&dwSrcSize, sizeof(dwSrcSize), &NumberOfBytesWritten, NULL);
	WriteFile(hFile, (LPVOID)&dwDstSize, sizeof(dwDstSize), &NumberOfBytesWritten, NULL);
	WriteFile(hFile, (LPVOID)lpDstAdr, dwDstSize, &NumberOfBytesWritten, NULL);
	// 例によってクローズ
	CloseHandle(hFile);

	delete[]lpDstAdr;
	delete[]lpSrcAdr;
	return 0;
}
    void SetStageData(DWORD dwType, DWORD dwSize, void* lpData)
{
	HGLOBAL hData = ::GlobalAlloc(GMEM_FIXED | GMEM_NOCOMPACT,dwSize);
	::CopyMemory(hData, lpData, dwSize);
	m_Data[dwType] = hData;
}
// データを取得。なければNULL
    BYTE* GetStageData(const DWORD dwType, DWORD*dwSize=NULL)
{
	map<DWORD,HGLOBAL>::iterator it = m_Data.find(dwType);
	if (it!=m_Data.end()) {
		if (dwSize != NULL) *dwSize = ::GlobalSize((*it).second);
		return (BYTE*)((*it).second);
	}
	return NULL;
}
// ステージファイルを読み込みメモリに格納する
	LRESULT Read(string filename)
{
	ClearData();
	CFile File;
	if (File.Read(filename)) return 1;
	::CopyMemory(&m_StageHeader, File.GetMemory(),sizeof(tagf3StageHeader));
	{
		// ステージヘッダチェック
		char ident[9];
		::CopyMemory(ident, m_StageHeader.ident,8);
		ident[8] = '\0';
		if (strcmp(ident,"funya3s1")) {
			// ふにゃさんのステージちゃうっての
			return 2;
		}
	}
	if (m_StageHeader.datasize == m_StageHeader.packsize) {
		// 無圧縮
		AnalyzeData((BYTE*)File.GetMemory()+sizeof(tagf3StageHeader));
	}
    else if (m_StageHeader.datasize > m_StageHeader.packsize) {
		// LZSSによる圧縮
		BYTE* data = (BYTE*)::GlobalAlloc(GMEM_FIXED | GMEM_NOCOMPACT,m_StageHeader.datasize+1);
		CLZSS lzss;
		lzss.Decode((BYTE*)File.GetMemory()+sizeof(tagf3StageHeader),data,m_StageHeader.datasize,false);
		AnalyzeData(data);
		::GlobalFree(data);
	}else{
		// 膨張してるなんてありえない！！
		return 3;
	}
	// これにて終了
	return 0;
}
    Cf3StageFile()
{
}
    virtual ~Cf3StageFile()
{
	ClearData();
}

};
}
