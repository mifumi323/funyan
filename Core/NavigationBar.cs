namespace MifuminSoft.funyan.Core
{
class TNavi
{
    public:
	TNavi() { dib = NULL; offset = 16; rect.left = rect.top = rect.right = rect.bottom = 0; }
    CDIB32* dib;
    float offset;
    RECT rect;
};

class TTextNavi : public TNavi {
public:
	TTextNavi() { text = NULL; }
void Update()
{
    if (text == NULL) return;
    text->UpdateText();
    int w, h;
    text->GetSize(w, h);
    rect.right = w; rect.bottom = h;
}
void Set(CTextDIB32* text)
{
    if ((dib = this->text = text) == NULL) return;
    this->text->GetFont()->SetSize(16);
    this->text->GetFont()->SetColor(0x000020);
    this->text->GetFont()->SetBackColor(0x303030);
}
CTextDIB32* text;
};

class Cf3NavigationBar
{
    protected:
	list<TNavi*> m_PlaneList;

    float m_Scroll;
    float m_ScrollTo;
    public:
	void Delete(TNavi* lpPlane) { m_PlaneList.remove(lpPlane); }
    void Clear() { m_ScrollTo = m_Scroll = 0; m_PlaneList.clear(); }
    void Add(TNavi* lpPlane);
    void OnDraw(CDIB32* lp);
    Cf3NavigationBar();
    virtual ~Cf3NavigationBar();

};

extern Cf3NavigationBar f3Navi;
}