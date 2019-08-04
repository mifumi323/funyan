using System;
using System.Collections.Generic;
using System.Drawing;

namespace MifuminSoft.funyan.Core
{
    [Flags]
    public enum HIT : byte {
        HIT_TOP = 0x01,
        HIT_BOTTOM = 0x02,
        HIT_LEFT = 0x04,
        HIT_RIGHT = 0x08,
        HIT_DEATH = 0x10,
    }

    public class Cf3Map : IDisposable
    {
        private CDIB32[] m_MapChip = new CDIB32[3];
        private byte[][] m_MapData = new byte[3][];
        private byte[] m_Width = new byte[3], m_Height = new byte[3];
        private HIT[] m_Hit = new HIT[240];
        private byte m_Stage;
        public int m_nGotBanana;
        private int m_nTotalBanana;

        private string m_Title;

        private bool m_bPlayable;

        private static readonly HIT[] m_defHit = {
            0x00,(HIT)0x0f,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,
            0x00,(HIT)0x0f,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,
            0x00,(HIT)0x0f,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,
            0x00,(HIT)0x0f,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,
            0x00,(HIT)0x0f,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,
            0x00,(HIT)0x0f,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,
            0x00,(HIT)0x0f,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,
            0x00,(HIT)0x0f,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,
            0x00,(HIT)0x0f,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,
            0x00,(HIT)0x0f,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,
            0x00,(HIT)0x0f,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,
            0x00,(HIT)0x0f,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,
            0x00,(HIT)0x0f,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,
            0x00,(HIT)0x0f,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,
            0x00,(HIT)0x0f,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,(HIT)0x10,
        };
        private static float[] m_Friction = {
            0.10f,0.00f,0.02f,0.05f,0.07f,0.15f,0.20f,1.00f,
        };

        private BGMNumber m_BGMNumber;

        private int m_ScrollX, m_ScrollY;
        private float m_ScrollRX, m_ScrollRY;

        private float[] m_Wind;
        private Cf3MapObjectBase[] m_pObject;
        private List<Cf3MapObjectBase> m_NearObject = new List<Cf3MapObjectBase>();

        private Cf3MapObjectMain m_MainChara;

        private static int m_nEffect = 0;
        private CDIB32 m_pDIBBuf;

        public IEnumerable<Cf3MapObjectBase> GetMapObjects(int x1, int y1, int x2, int y2, f3MapObjectType eType)
        {
            if (x1 < 0) x1 = 0;
            if (y1 < 0) y1 = 0;
            if (x2 >= m_Width[1]) x2 = m_Width[1] - 1;
            if (y2 >= m_Height[1]) y2 = m_Height[1] - 1;
            m_NearObject.Clear();
            Cf3MapObjectBase o;
            for (int x = x1; x <= x2; x++) {
                for (int y = y1; y <= y2; y++) {
                    o = m_pObject[GetIndex(x, y)];
                    while (o != null) {
                        if (o.GetMapObjectType() == eType && o.IsValid()) {
                            m_NearObject.Add(o);
                        }
                        o = o.m_pNext;
                    }
                }
            }
            return m_NearObject;
        }
        public int GetIndex(int level, int x, int y) { return x + y * m_Width[level]; }
        public int GetIndex(int x, int y) { return x + y * m_Width[1]; }
        public void AddMapObject(int x, int y, Cf3MapObjectBase p)
        {
            if (p == null) return;
            TL.Saturate(0, ref x, m_Width[1] - 1);
            TL.Saturate(0, ref y, m_Height[1] - 1);
            int i = GetIndex(x, y);
            var o = m_pObject[i];
            if (o == null) {
                m_pObject[i] = p;
                return;
            }
            while (o != p) {
                if (o.m_pNext == null) {
                    o.m_pNext = p;
                    return;
                }
                o = o.m_pNext;
            }
        }
        public void RemoveMapObject(int x, int y, Cf3MapObjectBase p)
        {
            if (p == null) return;
            TL.Saturate(0, ref x, m_Width[1] - 1);
            TL.Saturate(0, ref y, m_Height[1] - 1);
            int i = GetIndex(x, y);
            var o = m_pObject[i];
            if (o == p) o = m_pObject[i] = p.m_pNext;
            while (o != null) {
                if (o.m_pNext == p) o.m_pNext = p.m_pNext;
                o = o.m_pNext;
            }
            p.m_pNext = null;
        }
        public CDIB32 ReadMapChip(Cf3StageFile lp, int level)
        {
            byte[] buf;
            string text;
            var dib = CDIB32.Create();
            // ステージ内部データを読み込む
            if ((buf = lp.GetStageData(CT.CT_MCD0 | (CT)(level << 24))) != null) {
                if (dib.Load(buf, false) == 0) return dib;
            }
            // 駄目だったそうなのでファイル名から読み込む
            if ((text = lp.GetStageDataString(CT.CT_MCF0 | (CT)(level << 24))) != null) {
                if (dib.Load(text, false) == 0) return dib;
            }
            dib.Load("resource/Cave.bmp", false);
            return dib;
        }
        public bool IsPlayable() { return m_bPlayable; }
        public byte GetHeight(int level = 1) { return m_Height[level]; }
        public byte GetWidth(int level = 1) { return m_Width[level]; }
        public bool ItemCompleted() { return m_nGotBanana == m_nTotalBanana; }
        public static void SetEffect(int effect) { m_nEffect = effect; }
        public void GetMainCharaCPos(out int x, out int y)
        {
            m_MainChara.GetCPos(out x, out y);
        }
        public int SetMapData(int level, int x, int y, byte data)
        {
            if (level < 0 || 2 < level || x < 0 || m_Width[level] <= x || y < 0 || m_Height[level] <= y || data >= 0xf0) return 1;
            m_MapData[level][x + y * m_Width[level]] = data;
            return 0;
        }
        public void CreateTemparatureMap(CDIB32 dib)
        {
            float objX, objY, dX, dY, fX, fY;
            var pixel = dib.GetPtr();
            float offx = m_ScrollRX - 320 / 2, offy = m_ScrollRY - 224 / 2 - 2;
            TL.Saturate(0.0f, ref offx, m_Width[1] * 32 - 320.0f);
            TL.Saturate(0.0f, ref offy, m_Height[1] * 32 - 224.0f);
            uint i = 0;
            for (int y = 0; y < 224; y++) {
                for (int x = 0; x < 320; x++) {
                    fX = x + offx; fY = y + offy;   // GetViewPosとオフセットの掛け方が逆
                    var power = 0.0f;
                    // 氷ゾーン
                    foreach (var is_ in Cf3MapObjectIceSource.All()) {
                        is_.GetPos(out objX, out objY);
                        dX = objX - fX; dY = objY - fY;
                        power += 1.0f / (dX * dX + dY * dY);
                    }
                    // 炎ゾーン
                    foreach (var fr in Cf3MapObjectFire.All()) {
                        if (fr.IsActive()) {
                            fr.GetPos(out objX, out objY);
                            dX = objX - fX; dY = objY - fY;
                            power -= 1.0f / (dX * dX + dY * dY);
                        }
                    }
                    if (power > 1.0f / 256.0f) {
                        // 凍りつくゾーン
                        pixel[i] = 0x008080;
                    }
                    else if (power > 1.0f / 4096.0f) {
                        // パワーアップゾーン
                        pixel[i] = 0x00ffff;
                    }
                    else if (power < -1.0f / 256.0f) {
                        // 致死ゾーン
                        pixel[i] = 0x800000;
                    }
                    else if (power < -1.0f / 4096.0f) {
                        // 制限ゾーン
                        pixel[i] = 0xff0000;
                    } else {
                        // 普通ゾーン
                        pixel[i] = 0x000000;
                    }
                    i++;
                }
            }
            var lpSrc = dib;
            var lpDst = m_pDIBBuf;
            if ((m_nEffect & 1) != 0) {
                CPlaneTransBlt.MirrorBlt1(lpDst, lpSrc, 0, 0, 128);
                TL.swap(ref lpSrc, ref lpDst);
            }
            if ((m_nEffect & 2) != 0) {
                CPlaneTransBlt.MirrorBlt2(lpDst, lpSrc, 0, 0, 128);
                var rc = new Rectangle(0, 16, 320, 224);
                lpSrc.BltFast(lpDst, 0, 0, rc);
            }
            if (lpDst == dib) lpDst.BltFast(lpSrc, 0, 0);
        }
        public float GetWind(int x, int y)
        {
            if (m_Wind == null || x < 0 || m_Width[1] <= x || y < 0 || m_Height[1] <= y) return 0.0f;
            return m_Wind[GetIndex(x, y)];
        }
        public Cf3MapObjectMain GetMainChara() { return m_MainChara; }
        public BGMNumber GetBGM() { return m_BGMNumber; }
        public static CT GetChunkType(CT type, int stage)
        {
            uint r = (uint)stage & 0xf, l = (uint)stage & 0xf0;
            if (r >= 0xa) r += 0x7;
            if (l >= 0xa0) l += 0x70;
            return (CT)((uint)type + (r << 24) + (l << 12));
        }
        public bool IsMainCharaDied()
        {
            return m_MainChara != null && m_MainChara.IsDied();
        }
        public string GetTitle() { return m_Title; }
        public long GetTotalBanana() { return m_nTotalBanana; }
        public long GetGotBanana() { return m_nGotBanana; }
        public void KillAllMapObject()
        {
            Cf3MapObjectBase.KillAll();
        }
        public void GarbageMapObject()
        {
            if (m_MainChara != null && !m_MainChara.IsValid()) {
                m_MainChara = null;
            }
            Cf3MapObjectBase.Garbage();
        }
        public void OnPreDraw()
        {
            if (m_MainChara != null) {
                m_MainChara.OnPreDraw();
            }
            Cf3MapObjectBanana.OnPreDrawAll();
            Cf3MapObjectEelPitcher.OnPreDrawAll();
            Cf3MapObjectGeasprin.OnPreDrawAll();
            Cf3MapObjectmrframe.OnPreDrawAll();
            Cf3MapObjectNeedle.OnPreDrawAll();
            Cf3MapObjectIce.OnPreDrawAll();
            Cf3MapObjectIceSource.OnPreDrawAll();
            Cf3MapObjectFire.OnPreDrawAll();
            Cf3MapObjectEffect.OnPreDrawAll();
            Cf3MapObjectWind.OnPreDrawAll();
            if (m_MainChara != null) m_MainChara.GetViewPos(out m_ScrollX, out m_ScrollY);
            m_ScrollRX = (m_ScrollRX + m_ScrollX) / 2;
            m_ScrollRY = (m_ScrollRY + m_ScrollY) / 2;
        }
        public float GetFriction(int x, int y)
        {
            if (x < 0 || m_Width[1] <= x || y < 0 || m_Height[1] <= y) return 0.0f;
            return m_Friction[(int)m_Hit[GetMapData(1, x, y)] >> 5];
        }
        public void GetViewPos(ref int x, ref int y, float scrollx = 1.0f, float scrolly = 1.0f)
        {
            int offx = (int)(m_ScrollRX - 320 / 2), offy = (int)(m_ScrollRY - 224 / 2 - 2);
            TL.Saturate(0, ref offx, m_Width[1] * 32 - 320);
            TL.Saturate(0, ref offy, m_Height[1] * 32 - 224);
            x -= (int)(offx * scrollx); y -= (int)(offy * scrolly);
        }
        public void OnMove()
        {
            if (m_MainChara != null) m_MainChara.OnMove();
            Cf3MapObjectEelPitcher.OnMoveAll();
            Cf3MapObjectGeasprin.OnMoveAll();
            Cf3MapObjectmrframe.OnMoveAll();
            Cf3MapObjectNeedle.OnMoveAll();
            Cf3MapObjectIce.OnMoveAll();
            Cf3MapObjectFire.OnMoveAll();
            Cf3MapObjectBase.UpdateCPosAll();
            if (m_MainChara != null) m_MainChara.Synergy();
            Cf3MapObjectBanana.SynergyAll();
            Cf3MapObjectEelPitcher.SynergyAll();
            Cf3MapObjectGeasprin.SynergyAll();
            Cf3MapObjectmrframe.SynergyAll();
            Cf3MapObjectNeedle.SynergyAll();
            Cf3MapObjectIce.SynergyAll();
            Cf3MapObjectFire.SynergyAll();
        }
        public byte GetMapData(int level, int x, int y)
        {
            if (level < 0 || 2 < level || x < 0 || m_Width[level] <= x || y < 0 || m_Height[level] <= y) return 0;
            return m_MapData[level][GetIndex(level, x, y)];
        }
        public bool GetHit(int x, int y, HIT hit)
        {
            if (x < 0 || m_Width[1] <= x) return ((HIT)0x0f & hit) != 0;
            if (y < 0) return GetHit(x, 0, hit);
            if (y >= m_Height[1]) return GetHit(x, m_Height[1] - 1, hit);
            return (m_Hit[GetMapData(1, x, y)] & hit) != 0;
        }
        public void OnDraw(CDIB32 lp) { OnDraw(lp, false); }
        public void OnDraw(CDIB32 lp, bool bShowHit)
        {
            int x, y, z;
            int vx, vy;
            int sx, sy, ex, ey;
            Rectangle r;
            lp.Clear(0);
            if (m_MapData[0] != null) {
                float mx = 1;
                if (m_Width[1] - 10 > 0) mx = (float)(m_Width[0] - 10) / (float)(m_Width[1] - 10);
                float my = 1;
                if (m_Height[1] - 7 > 0) my = (float)(m_Height[0] - 7) / (float)(m_Height[1] - 7);
                sx = sy = 0;
                GetViewPos(ref sx, ref sy, mx, my);
                sx = (-sx) >> 5; sy = (-sy) >> 5;
                ex = sx + 320 / 32; ey = sy + 224 / 32;
                TL.Saturate(sx, ref ex, m_Width[0] - 1);
                TL.Saturate(sy, ref ey, m_Height[0] - 1);
                for (y = sy; y <= ey; y++) {
                    for (x = sx; x <= ex; x++) {
                        z = y * m_Width[0] + x;
                        r = new Rectangle((m_MapData[0][z] & 0xf) * 32, (m_MapData[0][z] >> 4) * 32, 32, 32);
                        vx = x * 32; vy = y * 32;
                        GetViewPos(ref vx, ref vy, mx, my);
                        lp.BltFast(m_MapChip[0], vx, vy, r);
                    }
                }
            }
            if (m_MapData[1] != null) {
                CDIB32 pHit = null;
                if (bShowHit) {
                    pHit = CDIB32.Create();
                    pHit.CreateSurface(384, 32);
                    pHit.BltFast(CResourceManager.ResourceManager.Get(RID.RID_HIT), 0, 0);
                    pHit.SubColorFast(CApp.theApp.random(0x1000000));
                }
                sx = sy = 0;
                GetViewPos(ref sx, ref sy);
                sx = (-sx) >> 5; sy = (-sy) >> 5;
                ex = sx + 320 / 32; ey = sy + 224 / 32;
                TL.Saturate(sx, ref ex, m_Width[1] - 1);
                TL.Saturate(sy, ref ey, m_Height[1] - 1);
                for (y = sy; y <= ey; y++) {
                    for (x = sx; x <= ex; x++) {
                        z = y * m_Width[1] + x;
                        r = new Rectangle((m_MapData[1][z] & 0xf) * 32, (m_MapData[1][z] >> 4) * 32, 32, 32);
                        vx = x * 32; vy = y * 32;
                        GetViewPos(ref vx, ref vy);
                        if (m_MapData[0] != null) lp.Blt(m_MapChip[1], vx, vy, r);
                        else lp.BltFast(m_MapChip[1], vx, vy, r);
                        if (bShowHit) {
                            // 当たり判定表示
                            if (GetHit(x, y, HIT.HIT_TOP)) {
                                int f = (byte)m_Hit[GetMapData(1, x, y)] & ~0x1f;
                                r = new Rectangle(f, 0, 32, 32);
                                lp.BlendBlt(pHit, vx, vy, 0x808080, 0x7f7f7f, r);
                            }
                            if (GetHit(x, y, HIT.HIT_BOTTOM)) {
                                r = new Rectangle(256, 0, 32, 32);
                                lp.BlendBlt(pHit, vx, vy, 0x808080, 0x7f7f7f, r);
                            }
                            if (GetHit(x, y, HIT.HIT_LEFT)) {
                                r = new Rectangle(288, 0, 32, 32);
                                lp.BlendBlt(pHit, vx, vy, 0x808080, 0x7f7f7f, r);
                            }
                            if (GetHit(x, y, HIT.HIT_RIGHT)) {
                                r = new Rectangle(320, 0, 32, 32);
                                lp.BlendBlt(pHit, vx, vy, 0x808080, 0x7f7f7f, r);
                            }
                            if (GetHit(x, y, HIT.HIT_DEATH)) {
                                r = new Rectangle(352, 0, 32, 32);
                                lp.BlendBlt(pHit, vx, vy, 0x808080, 0x7f7f7f, r);
                            }
                        }
                    }
                }
                if (bShowHit) {
                    pHit.Dispose();
                }
            }
            Cf3MapObjectBanana.OnDrawAll(lp);
            Cf3MapObjectmrframe.OnDrawAll(lp);
            if (m_MainChara != null) m_MainChara.OnDraw(lp);
            Cf3MapObjectGeasprin.OnDrawAll(lp);
            Cf3MapObjectNeedle.OnDrawAll(lp);
            Cf3MapObjectEelPitcher.OnDrawAll(lp);
            Cf3MapObjectIceSource.OnDrawAll(lp);
            Cf3MapObjectFire.OnDrawAll(lp);
            Cf3MapObjectIce.OnDrawAll(lp);
            Cf3MapObjectEffect.OnDrawAll(lp);
            Cf3MapObjectWind.OnDrawAll(lp);
            if (m_MapData[2] != null) {
                float mx = 1.0f;
                if (m_Width[1] - 10 > 0) mx = (float)(m_Width[2] - 10) / (m_Width[1] - 10);
                float my = 1.0f;
                if (m_Height[1] - 7 > 0) my = (float)(m_Height[2] - 7) / (m_Height[1] - 7);
                sx = sy = 0;
                GetViewPos(ref sx, ref sy, mx, my);
                sx = (-sx) >> 5; sy = (-sy) >> 5;
                ex = sx + 320 / 32; ey = sy + 224 / 32;
                TL.Saturate(sx, ref ex, m_Width[2] - 1);
                TL.Saturate(sy, ref ey, m_Height[2] - 1);
                for (y = sy; y <= ey; y++) {
                    for (x = sx; x <= ex; x++) {
                        z = y * m_Width[2] + x;
                        r = new Rectangle((m_MapData[2][z] & 0xf) * 32, (m_MapData[2][z] >> 4) * 32, 32, 32);
                        vx = (int)(x * 32 * mx); vy = (int)(y * 32 * my);
                        GetViewPos(ref vx, ref vy, mx, my);
                        lp.Blt(m_MapChip[2], vx, vy, r);
                    }
                }
            }
            var lpSrc = lp;
            var lpDst = m_pDIBBuf;
            if ((m_nEffect & 1) != 0) {
                CPlaneTransBlt.MirrorBlt1(lpDst, lpSrc, 0, 0, 128);
                TL.swap(ref lpSrc, ref lpDst);
            }
            if ((m_nEffect & 2) != 0) {
                CPlaneTransBlt.MirrorBlt2(lpDst, lpSrc, 0, 0, 128);
                var rc = new Rectangle(0, 16, 320, 224);
                lpSrc.BltFast(lpDst, 0, 0, rc);
            }
            if ((m_nEffect & 4) != 0) {
                CPlaneTransBlt.FlushBlt1(lpDst, lpSrc, 0, 0, 128);
                TL.swap(ref lpSrc, ref lpDst);
            }
            if (lpDst == lp) lpDst.BltFast(lpSrc, 0, 0);
        }
        public Cf3Map(Cf3StageFile lp, int stage, bool playable = true)
        {
            byte[] buf;
            string text;
            uint[] bgm = new uint[(int)BGMNumber.BGMN_SIZE];
            m_pDIBBuf = CDIB32.Create();
            m_pDIBBuf.CreateSurface(320, 240);
            m_Stage = (byte)stage;
            m_bPlayable = playable;
            Cf3MapObjectBase.SetParent(this);
            m_nGotBanana = m_nTotalBanana = 0;
            m_Wind = null;
            m_pObject = null;
            // キャラ
            m_MainChara = null;
            // タイトル
            m_Title = "";
            if ((text = lp.GetStageDataString(GetChunkType(CT.CT_TL00, stage))) != null) {
                m_Title = text;
            }
            // マップチップ
            m_MapChip[0] = ReadMapChip(lp, 0);
            m_MapChip[1] = ReadMapChip(lp, 1);
            m_MapChip[2] = ReadMapChip(lp, 2);
            // 当たり判定
            Array.Copy(m_defHit, m_Hit, 240);
            if ((buf = lp.GetStageData(CT.CT_HITS)) != null) {
                Array.Copy(buf, m_Hit, Math.Min(buf.Length, 240));
            }
            // マップデータ(下層)
            if ((buf = lp.GetStageData(GetChunkType(CT.CT_M000, stage))) != null) {
                m_Width[0] = buf[0];
                m_Height[0] = buf[1];
                m_MapData[0] = new byte[m_Width[0] * m_Height[0]];
                Array.Copy(buf, 2, m_MapData[0], 0, m_Width[0] * m_Height[0]);
            } else {
                m_MapData[0] = null;
            }
            // マップデータ(中層)
            if ((buf = lp.GetStageData(GetChunkType(CT.CT_M100, stage))) != null) {
                m_Width[1] = buf[0];
                m_Height[1] = buf[1];
                var stagesize = m_Width[1] * m_Height[1];
                m_MapData[1] = new byte[stagesize];
                m_Wind = new float[stagesize];
                m_pObject = new Cf3MapObjectBase[stagesize];
                var windmap = new byte[stagesize];
                Array.Copy(buf, 2, m_MapData[1], 0, stagesize);
                int x, y, z, n;
                z = 0;
                for (y = 0; y < m_Height[1]; y++) {
                    for (x = 0; x < m_Width[1]; x++) {
                        windmap[z] = 0;
                        n = m_MapData[1][z];
                        if (n >= 0xf0) {
                            if (n == 0xf0) {    // 主人公
                                if (m_MainChara == null) m_MainChara = Cf3MapObjectMain.Create(x, y);
                                bgm[(int)BGMNumber.BGMN_GAMEFUNYA] += 99;
                            }
                            else if (n == 0xf1) {   // バナナ
                                new Cf3MapObjectBanana(x, y);
                                bgm[(int)BGMNumber.BGMN_GAMEBANANA] += 1;
                                m_nTotalBanana++;
                            }
                            else if (n == 0xf2) {   // とげとげ
                                new Cf3MapObjectNeedle(x, y);
                                bgm[(int)BGMNumber.BGMN_GAMENEEDLE] += 4;
                            }
                            else if (n == 0xf3) {   // ギヤバネ左向き
                                new Cf3MapObjectGeasprin(x, y, f3MapObjectDirection.DIR_LEFT);
                                bgm[(int)BGMNumber.BGMN_GAMEGEASPRIN] += 10;
                            }
                            else if (n == 0xf4) {   // ギヤバネ右向き
                                new Cf3MapObjectGeasprin(x, y, f3MapObjectDirection.DIR_RIGHT);
                                bgm[(int)BGMNumber.BGMN_GAMEGEASPRIN] += 10;
                            }
                            else if (n == 0xf5) {   // 風ストップ
                                windmap[z] = 0xC;
                            }
                            else if (n == 0xf6) {   // 風左向き
                                windmap[z] = 0x1;
                                bgm[(int)BGMNumber.BGMN_GAMEWIND] += 1;
                            }
                            else if (n == 0xf7) {   // 風右向き
                                windmap[z] = 0x2;
                                bgm[(int)BGMNumber.BGMN_GAMEWIND] += 1;
                            }
                            else if (n == 0xf8) {   // ミスター・フレーム
                                new Cf3MapObjectmrframe(x, y);
                                bgm[(int)BGMNumber.BGMN_GAMEMRFRAME] += 40;
                            }
                            else if (n == 0xf9) {   // ウナギカズラ
                                new Cf3MapObjectEelPitcher(x, y);
                                bgm[(int)BGMNumber.BGMN_GAMEEELPITCHER] += 5;
                            }
                            else if (n == 0xfa) {   // 氷
                                new Cf3MapObjectIceSource(x, y);
                                bgm[(int)BGMNumber.BGMN_GAMEICE] += 8;
                            }
                            else if (n == 0xfb) {   // 火
                                new Cf3MapObjectFire(x, y);
                                bgm[(int)BGMNumber.BGMN_GAMEFIRE] += 8;
                            }
                            else if (n == 0xfc) {   // とげとげ
                                new Cf3MapObjectNeedle(x, y, 1);
                                bgm[(int)BGMNumber.BGMN_GAMENEEDLE] += 4;
                            }
                            else if (n == 0xfd) {   // とげとげ
                                new Cf3MapObjectNeedle(x, y, 2);
                                bgm[(int)BGMNumber.BGMN_GAMENEEDLE] += 4;
                            }
                            else if (n == 0xfe) {   // とげとげ
                                new Cf3MapObjectNeedle(x, y, 3);
                                bgm[(int)BGMNumber.BGMN_GAMENEEDLE] += 4;
                            }
                            m_MapData[1][z] = 0;
                        } else {
                            if (GetHit(x, y, HIT.HIT_LEFT)) windmap[z] = 0x4; else windmap[z] = 0;
                            if (GetHit(x, y, HIT.HIT_RIGHT)) windmap[z] |= 0x8;
                        }
                        z++;
                    }
                }
                z = 0;
                float wind;
                int dx;
                for (y = 0; y < m_Height[1]; y++) {
                    for (x = 0; x < m_Width[1]; x++) {
                        if ((windmap[z] & 0x10) != 0) {
                        } else {
                            wind = 0.0f;
                            for (dx = 0; x + dx < m_Width[1]; dx++) {
                                if ((windmap[z + dx] & 0x4) != 0) break;
                                if ((windmap[z + dx] & 0x1) != 0) wind -= 1.0f;
                                if ((windmap[z + dx] & 0x2) != 0) wind += 1.0f;
                                windmap[z + dx] |= 0x10;
                                if ((windmap[z + dx] & 0x8) != 0) break;
                            }
                            if (TL.IsIn(-0.01f, wind, 0.01f)) {
                                wind = 0.0f;
                            } else {
                                new Cf3MapObjectWind(x, y, dx, wind);
                            }
                        }
                        m_Wind[z] = wind;
                        z++;
                    }
                }
                windmap = null;
                Cf3MapObjectBase.UpdateCPosAll();
            } else {
                m_MapData[1] = null;
            }
            // マップデータ(上層)
            if ((buf = lp.GetStageData(GetChunkType(CT.CT_M200, stage))) != null) {
                m_Width[2] = buf[0];
                m_Height[2] = buf[1];
                m_MapData[2] = new byte[m_Width[2] * m_Height[2]];
                Array.Copy(buf, 2, m_MapData[2], 0, m_Width[2] * m_Height[2]);
            } else {
                m_MapData[2] = null;
            }
            // BGM
            uint bgmm = 0;
            m_BGMNumber = BGMNumber.BGMN_SIRENT;
            for (int i = (int)BGMNumber.BGMN_SIRENT; i < (int)BGMNumber.BGMN_SIZE; i++) {
                if (bgmm < bgm[i]) {
                    bgmm = bgm[i];
                    m_BGMNumber = (BGMNumber)i;
                }
            }
            m_ScrollX = m_ScrollY = 0;
            if (m_MainChara != null) m_MainChara.GetPos(out m_ScrollRX, out m_ScrollRY);
        }
        public void Dispose()
        {
            KillAllMapObject();
            GarbageMapObject();
            m_pObject = null;
            m_Wind = null;
            m_MapData[2] = null;
            m_MapData[1] = null;
            m_MapData[0] = null;
            m_MapChip[2].Dispose();
            m_MapChip[1].Dispose();
            m_MapChip[0].Dispose();
            TL.DELETE_SAFE(ref m_pDIBBuf);
        }
    }
}
