namespace MifuminSoft.funyan.Core
{
class Cf3Select : public CLayer  
{
protected:
	int m_Selected;
int m_Prev;
int m_Lines;
int m_CursorY;

int m_nFontSize;
int m_nLineHeight;

int m_nMaxWidth;
int m_nMaxHeight;
int m_nWidth;
int m_nHeight;

bool m_bDirty;
bool m_bInputDone;
int m_nScroll;
vector<string> m_LineList;//
CDIB32* m_Graphic;
RECT m_rcIcon;

vector<int> m_IDs;

void InnerOnDraw(CPlaneBase* lp);
public:
	void SetRect(int xLeft, int yTop, int xRight, int yBottom)
{ ::SetRect(&m_rcIcon, xLeft, yTop, xRight, yBottom); }
int VisibleLines() { return m_nMaxHeight / m_nLineHeight; }
void Dirty() { m_bDirty = true; }
void OnInput();
void Reset();
void SetMaxSize(int cx, int cy);
void SetHeight(int nHeight)
{
    m_nLineHeight = nHeight;
    m_bDirty = true;
}
void SetFontSize(int nSize)
{
    m_nFontSize = m_nLineHeight = nSize;
    m_bDirty = true;
}
void Update();
void Add(string item, int id);
void Add(string item) { Add(item, m_Lines); }
bool Changed() { OnInput(); return m_Prev != m_Selected; }
void SelectPos(int num);
void GetSize(int &sx, int &sy);
int GetSelected() { return m_IDs[m_Selected]; }
int GetSelectedPos() { return m_Selected; }
void Clear();

Cf3Select();
virtual ~Cf3Select();
};
}