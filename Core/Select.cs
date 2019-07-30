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

void InnerOnDraw(CPlaneBase* lp)
{
	if (m_Lines) {
		m_Prev = m_Selected;
		if (f3Input.GetKeyPushed(F3KEY_UP)) {
			if (--m_Selected<0) m_Selected += m_Lines;
		}
		if (f3Input.GetKeyPushed(F3KEY_DOWN)) {
			if (++m_Selected>=m_Lines) m_Selected -= m_Lines;
		}
		lp->Blt(m_Text,m_nX, m_nY);
		RECT rc = { 288, 96, 320, 128 };
		//BringClose(m_CursorY, 32*m_Selected, 8);
		m_CursorY = (m_CursorY+32*m_Selected)/2;
		lp->Blt(m_Cursor,m_nX-32, m_nY+m_CursorY,&rc);
	}
}
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
void Add(string item, int id)
{
	m_Menu += item+"\n";
	m_Text->GetFont()->SetText(m_Menu);
	m_Text->UpdateText();
	m_IDs.push_back(id);
	m_Lines++;
}
void Add(string item) { Add(item, m_Lines); }
bool Changed() { OnInput(); return m_Prev != m_Selected; }
void SelectPos(int num)
{
	if (num<m_Lines) {
		m_Selected = num;
	}else {
		m_Selected = m_Lines-1;
	}
}
void GetSize(int &sx, int &sy)
{
	if (m_Lines) {
		m_Text->GetSize(sx,sy);
	}else {
		sx=0; sy=0;
	}
}
int GetSelected() { return m_IDs[m_Selected]; }
int GetSelectedPos() { return m_Selected; }
void Clear()
{
	m_Menu = "";
	m_Text->GetFont()->SetText(m_Menu);
	m_Text->UpdateText();
	m_Selected = m_Prev = m_Lines = 0;
	m_IDs.clear();
}

Cf3Select()
{
	m_Text = new CTextDIB32;
	m_Text->GetFont()->SetSize(32);
	m_Text->GetFont()->SetColor(0xffffff);
	m_Text->GetFont()->SetBackColor(0x303030);
	m_Cursor = ResourceManager.Get(RID_MAIN);
	m_CursorY = 0;
	Clear();
}
virtual ~Cf3Select()
{
	delete m_Text;
}
};
}