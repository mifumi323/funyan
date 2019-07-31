using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MifuminSoft.funyan.Core
{
    public class Cf3Setting
{
        protected struct tagSetting
    {
        public LPSTR name;
        public int* pdata;
        public bool check;
    } *m_Data;
	    protected int m_DataCount;

        protected Dictionary<string, int> m_Progress = new Dictionary<string, int>();

        // 普通の設定項目
        public static char SS_BGM[] =			"BGM";
        public static char SS_FULLSCREEN[] =	"FULLSCREEN";
        public static char SS_ZOOM[] =			"ZOOM";
        public static char SS_BACKGROUND[] =	"BACKGROUND";
        public static char SS_CHARACTER[] =	"CHARACTER";
        public static char SS_VIEWTIME[] =		"VIEWTIME";
        public static char SS_FPS[] =			"FPS";
        public static char SS_ESP[] =			"ESP";
        public static char SS_GRAVITY[] =		"GRAVITY";
        public static char SS_RECORDCLEAR[] =	"RECORDCLEAR";
        public static char SS_RECORDMISS[] =	"RECORDMISS";
        public static char SS_RECORDNUMBER[] =	"RECORDNUMBER";

        // プレイ記録
        public static char SS_BANANA[] =		"BANANA";
        public static char SS_PLAYTIME[] =		"PLAYTIME";
        public static char SS_SLEEPTIME[] =	"SLEEPTIME";
        public static char SS_SMILES[] =		"SMILES";
        public static char SS_TIMEMASTER[] =	"TIMEMASTER";
        public static char SS_EYEWITNESS[] =	"EYEWITNESS";
        public static char SS_FEATHERIRON[] =	"FEATHERIRON";
        public static char SS_GRAPECOLORED[] =	"GRAPECOLORED";
        public static char SS_ESREVER[] =		"ESREVER";
        public static char SS_DRAWMETHOD[] =	"DRAWMETHOD";
        public static char SS_OUTLINE[] =		"OUTLINE";
        public static char SS_COLDMAN[] =		"COLDMAN";
        public static char SS_HYPER[] =		"HYPER";
        public static char SS_ANDBALLOON[] =	"ANDBALLOON";

        // キー設定
        public static char SS_KEY_EXIT[] =		"KEY_EXIT";
        public static char SS_KEY_PAUSE[] =	"KEY_PAUSE";
        public static char SS_KEY_UP[] =		"KEY_UP";
        public static char SS_KEY_LEFT[] =		"KEY_LEFT";
        public static char SS_KEY_RIGHT[] =	"KEY_RIGHT";
        public static char SS_KEY_DOWN[] =		"KEY_DOWN";
        public static char SS_KEY_JUMP[] =		"KEY_JUMP";
        public static char SS_KEY_ATTACK[] =	"KEY_ATTACK";
        public static char SS_KEY_SMILE[] =	"KEY_SMILE";
        public static char SS_KEY_FPS[] =		"KEY_FPS";
        public static char SS_KEY_BGMNONE[] =	"KEY_BGMNONE";
        public static char SS_KEY_BGMDEF[] =	"KEY_BGMDEF";
        public static char SS_KEY_BGMUSER[] =	"KEY_BGMUSER";
        public static char SS_KEY_CAPTURE[] =	"KEY_CAPTURE";
        public static char SS_KEY_RECORD[] =	"KEY_RECORD";

        // その他
        public static char SS_PROGRESS[] =		"PROGRESS";
        public static char SS_CHECKSUM[] =		"CHECKSUM";


        public void SetProgress(string file, int stage)
{
	m_Progress[file] = stage;
}
        public int GetProgress(string file)
        {
            return m_Progress.TryGetValue(file,out var progress) ? progress : 0;
        }
        public void InitSaveData()
{
	for (int i=0; i<m_DataCount; i++) {
		if (m_Data[i].check) {
			*m_Data[i].pdata = 0;
		}
	}
}
        public int GetChecksum()
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
	foreach (var it in m_Progress) {
		int d = it.Value;
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
        public Cf3Setting()
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
            m_Progress = m_Progress.Where(it => File.Exists(it.Key)).ToDictionary(it => it.Key, it => it.Value);
	m_StartTime = timeGetTime();
	theSetting = this;
}
        public virtual ~Cf3Setting()
{
	m_PlayTime += (timeGetTime()-m_StartTime)/1000;
	CFile file;
	file.Open("funya3.ini","w");
	FILE* fp = file.GetFilePtr();
	for (int i=0; i<m_DataCount; i++) {
		if (*m_Data[i].pdata)
			fprintf(fp,"%s %d\n",m_Data[i].name,*m_Data[i].pdata);
	}
	foreach (var it in m_Progress) {
		if (it.Value > 0)
			fprintf(fp,"%s \"%s\" %d\n",SS_PROGRESS,it.Key,it.Value);
	}
	fprintf(fp,"%s %d\n",SS_CHECKSUM,GetChecksum());
	delete [] m_Data;
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
        public int m_Key[F3KEY_BUFSIZE];
}* theSetting;
}
