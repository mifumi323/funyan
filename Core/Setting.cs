namespace MifuminSoft.funyan.Core
{
// 普通の設定項目
static char SS_BGM[] =			"BGM";
static char SS_FULLSCREEN[] =	"FULLSCREEN";
static char SS_ZOOM[] =			"ZOOM";
static char SS_BACKGROUND[] =	"BACKGROUND";
static char SS_CHARACTER[] =	"CHARACTER";
static char SS_VIEWTIME[] =		"VIEWTIME";
static char SS_FPS[] =			"FPS";
static char SS_ESP[] =			"ESP";
static char SS_GRAVITY[] =		"GRAVITY";
static char SS_RECORDCLEAR[] =	"RECORDCLEAR";
static char SS_RECORDMISS[] =	"RECORDMISS";
static char SS_RECORDNUMBER[] =	"RECORDNUMBER";

// プレイ記録
static char SS_BANANA[] =		"BANANA";
static char SS_PLAYTIME[] =		"PLAYTIME";
static char SS_SLEEPTIME[] =	"SLEEPTIME";
static char SS_SMILES[] =		"SMILES";
static char SS_TIMEMASTER[] =	"TIMEMASTER";
static char SS_EYEWITNESS[] =	"EYEWITNESS";
static char SS_FEATHERIRON[] =	"FEATHERIRON";
static char SS_GRAPECOLORED[] =	"GRAPECOLORED";
static char SS_ESREVER[] =		"ESREVER";
static char SS_DRAWMETHOD[] =	"DRAWMETHOD";
static char SS_OUTLINE[] =		"OUTLINE";
static char SS_COLDMAN[] =		"COLDMAN";
static char SS_HYPER[] =		"HYPER";
static char SS_ANDBALLOON[] =	"ANDBALLOON";

// キー設定
static char SS_KEY_EXIT[] =		"KEY_EXIT";
static char SS_KEY_PAUSE[] =	"KEY_PAUSE";
static char SS_KEY_UP[] =		"KEY_UP";
static char SS_KEY_LEFT[] =		"KEY_LEFT";
static char SS_KEY_RIGHT[] =	"KEY_RIGHT";
static char SS_KEY_DOWN[] =		"KEY_DOWN";
static char SS_KEY_JUMP[] =		"KEY_JUMP";
static char SS_KEY_ATTACK[] =	"KEY_ATTACK";
static char SS_KEY_SMILE[] =	"KEY_SMILE";
static char SS_KEY_FPS[] =		"KEY_FPS";
static char SS_KEY_BGMNONE[] =	"KEY_BGMNONE";
static char SS_KEY_BGMDEF[] =	"KEY_BGMDEF";
static char SS_KEY_BGMUSER[] =	"KEY_BGMUSER";
static char SS_KEY_CAPTURE[] =	"KEY_CAPTURE";
static char SS_KEY_RECORD[] =	"KEY_RECORD";

// その他
static char SS_PROGRESS[] =		"PROGRESS";
static char SS_CHECKSUM[] =		"CHECKSUM";


extern class Cf3Setting
{
    protected:
	struct tagSetting
    {
        LPSTR name;
        int* pdata;
        bool check;
    } *m_Data;
	int m_DataCount;

    map<string, int> m_Progress;

    public:
	void SetProgress(string &file, int stage)
{
	m_Progress[file] = stage;
}
    int GetProgress(string &file)
{
	map<string,int>::iterator it=m_Progress.find(file);
	if (it==m_Progress.end()) return 0; else return (*it).second;
}
    void InitSaveData()
{
	for (int i=0; i<m_DataCount; i++) {
		if (m_Data[i].check) {
			*m_Data[i].pdata = 0;
		}
	}
}
    int GetChecksum()
{
	int checksum=0;
	for (int i=0; i<m_DataCount; i++) {
		if (m_Data[i].check) {
			int d = *m_Data[i].pdata;
			checksum = checksum
				+((d>> 0)&1)+((d>> 1)&1)+((d>> 2)&1)+((d>> 3)&1)
				+((d>> 4)&1)+((d>> 5)&1)+((d>> 6)&1)+((d>> 7)&1)
				+((d>> 8)&1)+((d>> 9)&1)+((d>>10)&1)+((d>>11)&1)
				+((d>>12)&1)+((d>>13)&1)+((d>>14)&1)+((d>>15)&1)
				+((d>>16)&1)+((d>>17)&1)+((d>>18)&1)+((d>>19)&1)
				+((d>>20)&1)+((d>>21)&1)+((d>>22)&1)+((d>>23)&1)
				+((d>>24)&1)+((d>>25)&1)+((d>>26)&1)+((d>>27)&1)
				+((d>>28)&1)+((d>>29)&1)+((d>>30)&1)+((d>>31)&1)
				;
		}
	}
	for (map<string,int>::iterator it=m_Progress.begin(); it!=m_Progress.end(); it++) {
		int d = (*it).second;
		checksum = checksum
			+((d>> 0)&1)+((d>> 1)&1)+((d>> 2)&1)+((d>> 3)&1)
			+((d>> 4)&1)+((d>> 5)&1)+((d>> 6)&1)+((d>> 7)&1)
			+((d>> 8)&1)+((d>> 9)&1)+((d>>10)&1)+((d>>11)&1)
			+((d>>12)&1)+((d>>13)&1)+((d>>14)&1)+((d>>15)&1)
			+((d>>16)&1)+((d>>17)&1)+((d>>18)&1)+((d>>19)&1)
			+((d>>20)&1)+((d>>21)&1)+((d>>22)&1)+((d>>23)&1)
			+((d>>24)&1)+((d>>25)&1)+((d>>26)&1)+((d>>27)&1)
			+((d>>28)&1)+((d>>29)&1)+((d>>30)&1)+((d>>31)&1)
			;
	}
	return checksum;
}
    Cf3Setting()
{
	int checksum = 0;
	tagSetting DefSetting[] = {
		{ SS_BGM,			&m_BGMMode,				false},
		{ SS_FULLSCREEN,	&m_FullScreen,			false},
		{ SS_ZOOM,			&m_Zoom,				false},
		{ SS_BACKGROUND,	&m_Background,			false},
		{ SS_CHARACTER,		&m_Character,			false},
		{ SS_VIEWTIME,		&m_ViewTime,			false},
		{ SS_FPS,			&m_FPS,					false},
		{ SS_ESP,			&m_ESP,					false},
		{ SS_GRAVITY,		&m_Gravity,				false},
		{ SS_DRAWMETHOD,	&m_DrawMethod,			false},
		{ SS_HYPER,			&m_Hyper,				false},
		{ SS_RECORDCLEAR,	&m_RecordClear,			false},
		{ SS_RECORDMISS,	&m_RecordMiss,			false},
		{ SS_RECORDNUMBER,	&m_RecordNumber,		false},
		{ SS_BANANA,		&m_Banana,				true},
		{ SS_PLAYTIME,		&m_PlayTime,			true},
		{ SS_SLEEPTIME,		&m_SleepTime,			true},
		{ SS_SMILES,		&m_Smiles,				true},
		{ SS_TIMEMASTER,	&m_TimeMaster,			true},
		{ SS_EYEWITNESS,	&m_Eyewitness,			true},
		{ SS_FEATHERIRON,	&m_FeatherIron,			true},
		{ SS_GRAPECOLORED,	&m_GrapeColored,		true},
		{ SS_ESREVER,		&m_Esrever,				true},
		{ SS_OUTLINE,		&m_Outline,				true},
		{ SS_COLDMAN,		&m_ColdMan,				true},
		{ SS_ANDBALLOON,	&m_AndBalloon,			true},
		{ SS_KEY_EXIT,		&m_Key[F3KEY_EXIT],		false},
		{ SS_KEY_PAUSE,		&m_Key[F3KEY_PAUSE],	false},
		{ SS_KEY_UP,		&m_Key[F3KEY_UP],		false},
		{ SS_KEY_LEFT,		&m_Key[F3KEY_LEFT],		false},
		{ SS_KEY_RIGHT,		&m_Key[F3KEY_RIGHT],	false},
		{ SS_KEY_DOWN,		&m_Key[F3KEY_DOWN],		false},
		{ SS_KEY_JUMP,		&m_Key[F3KEY_JUMP],		false},
		{ SS_KEY_ATTACK,	&m_Key[F3KEY_ATTACK],	false},
		{ SS_KEY_SMILE,		&m_Key[F3KEY_SMILE],	false},
		{ SS_KEY_FPS,		&m_Key[F3KEY_FPS],		false},
		{ SS_KEY_BGMNONE,	&m_Key[F3KEY_BGMNONE],	false},
		{ SS_KEY_BGMDEF,	&m_Key[F3KEY_BGMDEF],	false},
		{ SS_KEY_BGMUSER,	&m_Key[F3KEY_BGMUSER],	false},
		{ SS_KEY_CAPTURE,	&m_Key[F3KEY_CAPTURE],	false},
		{ SS_KEY_RECORD,	&m_Key[F3KEY_RECORD],	false},
	};
	m_DataCount = NELEMS(DefSetting);
	m_Data = new tagSetting[m_DataCount];
	CopyMemory(m_Data,DefSetting,sizeof(DefSetting));
	for (int i=0; i<m_DataCount; i++) *m_Data[i].pdata = 0;
	CFile file;
	CLineParser parser;
	string line, s;
	int n;
	file.Read("funya3.ini");
	while (file.ReadLine(line)==0) {
		parser.SetLine((char*)line.c_str());
		if (parser.IsMatch(SS_PROGRESS)) {
			parser.GetStr(s);
			parser.GetNum(n);
			m_Progress[s] = n;
			continue;
		}
		if (parser.IsMatch(SS_CHECKSUM)) {
			parser.GetNum(checksum);
			continue;
		}
		for (int i=0; i<m_DataCount; i++) {
			if (parser.IsMatch(m_Data[i].name)) {
				parser.GetNum(*m_Data[i].pdata);
				break;
			}
		}
	}
	// チェックサム
	if (checksum != GetChecksum()) InitSaveData();
	// 存在しないファイルの進行状況を消す
	for (map<string,int>::iterator it=m_Progress.begin(); it!=m_Progress.end();) {
		if (!CDir().IsFileExist((*it).first)) {
			map<string,int>::iterator it2=it;
			it++;
			m_Progress.erase(it2);
		}else {
			it++;
		}
	}
	m_StartTime = timeGetTime();
	theSetting = this;
}
    virtual ~Cf3Setting()
{
	m_PlayTime += (timeGetTime()-m_StartTime)/1000;
	CFile file;
	file.Open("funya3.ini","w");
	FILE* fp = file.GetFilePtr();
	for (int i=0; i<m_DataCount; i++) {
		if (*m_Data[i].pdata)
			fprintf(fp,"%s %d\n",m_Data[i].name,*m_Data[i].pdata);
	}
	for (map<string,int>::iterator it=m_Progress.begin(); it!=m_Progress.end(); it++) {
		if ((*it).second)
			fprintf(fp,"%s \"%s\" %d\n",SS_PROGRESS,(*it).first.c_str(),(*it).second);
	}
	fprintf(fp,"%s %d\n",SS_CHECKSUM,GetChecksum());
	delete [] m_Data;
}

    int m_BGMMode;
    int m_FullScreen;
    int m_Zoom;
    int m_Background;
    int m_Character;
    int m_ViewTime;
    int m_FPS;
    int m_ESP;
    int m_Gravity;
    int m_RecordClear;
    int m_RecordMiss;
    int m_RecordNumber;
    int m_Banana;
    int m_PlayTime;
    int m_SleepTime;
    int m_Smiles;
    int m_TimeMaster;
    int m_Eyewitness;
    int m_FeatherIron;
    int m_GrapeColored;
    int m_Esrever;
    int m_DrawMethod;
    int m_Outline;
    int m_ColdMan;
    int m_Hyper;
    int m_AndBalloon;
    int m_StartTime;
    int m_Key[F3KEY_BUFSIZE];
}* theSetting;
}