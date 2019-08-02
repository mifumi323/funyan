using System;
using System.Collections.Generic;
using System.Drawing;

namespace MifuminSoft.funyan.Core
{
    public class Cf3MapObjectEffect : Cf3MapObjectBase, IDisposable
    {
        protected const float PI = 3.141592653589793238f;
        protected static HashSet<Cf3MapObjectEffect> m_EffectList = new HashSet<Cf3MapObjectEffect>();
        //	CDIB32* m_Graphic;
        protected static readonly Rectangle[] m_GraphicRect = new Rectangle[4 * 16]{
            new Rectangle( 0,0, 5,5), new Rectangle( 0,5, 5,10), new Rectangle( 0,10, 5,15), new Rectangle( 0,15, 5,20),
            new Rectangle( 5,0,10,5), new Rectangle( 5,5,10,10), new Rectangle( 5,10,10,15), new Rectangle( 5,15,10,20),
            new Rectangle(10,0,15,5), new Rectangle(10,5,15,10), new Rectangle(10,10,15,15), new Rectangle(10,15,15,20),
            new Rectangle(15,0,20,5), new Rectangle(15,5,20,10), new Rectangle(15,10,20,15), new Rectangle(15,15,20,20),
            new Rectangle(20,0,25,5), new Rectangle(20,5,25,10), new Rectangle(20,10,25,15), new Rectangle(20,15,25,20),
            new Rectangle(25,0,30,5), new Rectangle(25,5,30,10), new Rectangle(25,10,30,15), new Rectangle(25,15,30,20),
            new Rectangle(30,0,35,5), new Rectangle(30,5,35,10), new Rectangle(30,10,35,15), new Rectangle(30,15,35,20),
            new Rectangle(35,0,40,5), new Rectangle(35,5,40,10), new Rectangle(35,10,40,15), new Rectangle(35,15,40,20),
            new Rectangle(40,0,45,5), new Rectangle(40,5,45,10), new Rectangle(40,10,45,15), new Rectangle(40,15,45,20),
            new Rectangle(45,0,50,5), new Rectangle(45,5,50,10), new Rectangle(45,10,50,15), new Rectangle(45,15,50,20),
            new Rectangle(50,0,55,5), new Rectangle(50,5,55,10), new Rectangle(50,10,55,15), new Rectangle(50,15,55,20),
            new Rectangle(55,0,60,5), new Rectangle(55,5,60,10), new Rectangle(55,10,60,15), new Rectangle(55,15,60,20),
            new Rectangle(60,0,65,5), new Rectangle(60,5,65,10), new Rectangle(60,10,65,15), new Rectangle(60,15,65,20),
            new Rectangle(65,0,70,5), new Rectangle(65,5,70,10), new Rectangle(65,10,70,15), new Rectangle(65,15,70,20),
            new Rectangle(70,0,75,5), new Rectangle(70,5,75,10), new Rectangle(70,10,75,15), new Rectangle(70,15,75,20),
            new Rectangle(75,0,80,5), new Rectangle(75,5,80,10), new Rectangle(75,10,80,15), new Rectangle(75,15,80,20),
        };
        protected int m_nEffectType;
        protected struct tagStar
        {
            public float x, y, dx, dy, f;
            public int n, r;
        }
        protected tagStar[] m_Star;
        protected int m_StarNum;

        public void OnDraw(CDIB32* lp)
        {
            CDIB32* graphic = CResourceManager.ResourceManager.Get(RID.RID_EFFECT);
            for (int i = 0; i < m_StarNum; i++)
            {
                if (m_Star[i].n)
                {
                    SetViewPos(m_Star[i].x, m_Star[i].y);
                    lp->Blt(graphic, m_nVX, m_nVY, &m_GraphicRect[m_Star[i].r]);
                }
            }
        }
        public void OnPreDraw()
        {
            int n = m_StarNum;
            for (int i = 0; i < m_StarNum; i++)
            {
                if (m_Star[i].n) m_Star[i].n--; else n--;
                m_Star[i].dx *= m_Star[i].f;
                m_Star[i].dy *= m_Star[i].f;
                m_Star[i].x += m_Star[i].dx;
                m_Star[i].y += m_Star[i].dy;
            }
            if (!n) Kill();
        }
        public static void OnPreDrawAll()
        {
            foreach (var it in m_EffectList)
            {
                if (it.IsValid()) it.OnPreDraw();
            }
        }
        public static void OnDrawAll(CDIB32* lp)
        {
            int sx, sy, ex, ey;
            sx = sy = 0;
            m_pParent->GetViewPos(ref sx, ref sy);
            sx = (-sx) >> 5; sy = (-sy) >> 5;
            ex = sx + 320 / 32; ey = sy + 224 / 32;
            TL.Saturate(sx, ref ex, m_pParent->GetWidth() - 1);
            TL.Saturate(sy, ref ey, m_pParent->GetHeight() - 1);
            for (Cf3MapObjectBase** it = m_pParent->GetMapObjects(sx - 3, sy - 3, ex + 3, ey + 3, MOT_EFFECT); (*it) != null; it++)
            {
                if ((*it)->IsValid()) (*it)->OnDraw(lp);
            }
        }
        public Cf3MapObjectEffect(float x, float y, int EffectType) : base(f3MapObjectType.MOT_EFFECT)
        {
            m_StarNum = 0;
            m_Star = null;
            m_nEffectType = EffectType;
            m_EffectList.Add(this);
            SetPos(x, y);
            if (EffectType == 0)
            {
                m_StarNum = 12;
                m_Star = new tagStar[m_StarNum];
                for (int i = 0; i < m_StarNum; i++)
                {
                    float rad = 2.0 * PI * i / m_StarNum;
                    m_Star[i].dx = 4.0f * cos(rad) * (0.5 + 0.5 / 4096.0 * CApp.theApp.random(4096));
                    m_Star[i].dy = 4.0f * sin(rad) * (0.5 + 0.5 / 4096.0 * CApp.theApp.random(4096));
                    m_Star[i].x = 0;
                    m_Star[i].y = 0;
                    m_Star[i].f = 0.9f;
                    m_Star[i].n = 40;
                }
            }
            else if (EffectType == 1)
            {
                m_StarNum = 12;
                m_Star = new tagStar[m_StarNum];
                for (int i = 0; i < m_StarNum; i++)
                {
                    float rad = 2.0 * PI * i / m_StarNum;
                    m_Star[i].dx = 0;
                    m_Star[i].dy = -16.0f * (0.5 + 0.5 / 4096.0 * CApp.theApp.random(4096));
                    m_Star[i].x = 32 * (-0.5 + 1.0 / 4096.0 * CApp.theApp.random(4096));
                    m_Star[i].y = 0;
                    m_Star[i].f = 0.9f;
                    m_Star[i].n = 35 + CApp.theApp.random(10);
                }
            }
            else
            {
                Kill();
                return;
            }
            for (int i = 0; i < m_StarNum; i++)
            {
                m_Star[i].r = CApp.theApp.random(4 * 16);
            }
        }
        public override void Dispose()
        {
            m_EffectList.Remove(this);
            DELETEPTR_SAFE(m_Star);
            base.Dispose();
        }
    }
}
