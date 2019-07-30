namespace MifuminSoft.funyan.Core
{
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
	void SetProgress(string &file, int stage);
    int GetProgress(string &file);
    void InitSaveData();
    int GetChecksum();
    Cf3Setting();
    virtual ~Cf3Setting();

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