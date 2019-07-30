namespace MifuminSoft.funyan.Core
{
const float NaviSpeed = 0.5f;
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
    void Add(TNavi* lpPlane)
{
	m_PlaneList.remove(lpPlane);
	m_PlaneList.push_back(lpPlane);
	if (lpPlane->rect.bottom==0) {
		int sx, sy;
		lpPlane->dib->GetSize(sx,sy);
		lpPlane->rect.left=0;
		lpPlane->rect.top=0;
		lpPlane->rect.right=sx;
		lpPlane->rect.bottom=sy;
	}
}
    void OnDraw(CDIB32* lp)
{
	float t=0,o=0;
	list<TNavi*>::iterator it;
	for(it = m_PlaneList.begin();it!=m_PlaneList.end();it++){
		t+=(*it)->rect.right-(*it)->rect.left+(*it)->offset;
	}
	if (t>320) {
		m_ScrollTo+=NaviSpeed;
		if (m_ScrollTo>t) m_ScrollTo-=t;
		if (m_ScrollTo<0) m_ScrollTo+=t;
		if (m_Scroll-m_ScrollTo>t/2) m_Scroll = (m_Scroll+m_ScrollTo-t)/2;
		ef (m_ScrollTo-m_Scroll>t/2) m_Scroll = (m_Scroll+m_ScrollTo+t)/2;
		else m_Scroll = (m_Scroll+m_ScrollTo)/2;
	}else{
		m_ScrollTo = m_Scroll = 0;
	}
	lp->BltFast(ResourceManager.Get(RID_NAVI),0,224);
	for(it = m_PlaneList.begin();it!=m_PlaneList.end();it++){
		lp->Blt((*it)->dib,o-m_Scroll,224,&(*it)->rect);
		if (t>320) {
			lp->Blt((*it)->dib,o+t-m_Scroll,224,&(*it)->rect);
			lp->Blt((*it)->dib,o-t-m_Scroll,224,&(*it)->rect);
		}
		o+=(*it)->rect.right-(*it)->rect.left+(*it)->offset;
	}
}
    Cf3NavigationBar()
{
	m_ScrollTo = m_Scroll = 0;
}
    virtual ~Cf3NavigationBar()
{
}

};

extern Cf3NavigationBar f3Navi;
}