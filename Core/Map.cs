using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MifuminSoft.funyan.Core
{
    public class Map:IDisposable
    {
        // Coreでは描画は行わない。マップチップ画像はステージ固有なのでマップごとに持つ意味はない。
        // private CDIB32* m_MapChip[3];
        private Layer[] m_Layers = new Layer[3];
        private Hit[] m_Hit = new Hit[240];
        private byte m_Stage;
        private int m_nGotBanana, m_nTotalBanana;

        private string m_Title;

        private bool m_bPlayable;

        private static Hit[] m_defHit = new Hit[]{
            0x00,(Hit)0x0f,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,
            0x00,(Hit)0x0f,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,
            0x00,(Hit)0x0f,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,
            0x00,(Hit)0x0f,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,
            0x00,(Hit)0x0f,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,
            0x00,(Hit)0x0f,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,
            0x00,(Hit)0x0f,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,
            0x00,(Hit)0x0f,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,
            0x00,(Hit)0x0f,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,
            0x00,(Hit)0x0f,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,
            0x00,(Hit)0x0f,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,
            0x00,(Hit)0x0f,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,
            0x00,(Hit)0x0f,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,
            0x00,(Hit)0x0f,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,
            0x00,(Hit)0x0f,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,(Hit)0x10,
        };
        private static float[] m_Friction = new float[]{
            0.10f,0.00f,0.02f,0.05f,0.07f,0.15f,0.20f,1.00f,
        };

        private BGMNumber m_BGMNumber;

        private int m_ScrollX, m_ScrollY;
        private float m_ScrollRX, m_ScrollRY;

        private float[] m_Wind;
        private MapObjectBase[] m_pObject;
        private List<MapObjectBase> m_NearObject;

        private MapObjectMain m_MainChara;

        private static int m_nEffect = 0;

        // Coreでは描画は行わない。
        // private CDIB32* m_pDIBBuf;

        public int Width => m_Layers[1].Width;
        public int Height => m_Layers[1].Height;

        public List<MapObjectBase> GetMapObjects(int x1,int y1,int x2,int y2,MapObjectType eType) {
            if (x1 < 0) x1 = 0;
            if (y1 < 0) y1 = 0;
            if (x2 >= Width) x2 = Width - 1;
            if (y2 >= Height) y2 = Height - 1;
            m_NearObject.Clear();
            for (int x = x1;x <= x2;x++)
            {
                for (int y = y1;y <= y2;y++)
                {
                    var o = m_pObject[GetIndex(x,y)];
                    while (o != null)
                    {
                        if (o.GetMapObjectType() == eType && o.IsValid())
                        {
                            m_NearObject.Add(o);
                        }
                        o = o.m_pNext;
                    }
                }
            }
            return m_NearObject;
        }

        public int GetIndex(int level, int x, int y) { return x + y * m_Layers[level].Width; }

        public int GetIndex(int x, int y) { return x + y * Width; }

        public void AddMapObject(int x, int y, MapObjectBase p)
        {
            if (p == null) return;
            TL.Saturate(0,ref x,Width - 1);
            TL.Saturate(0,ref y,Width - 1);
            int i = GetIndex(x,y);
            MapObjectBase o = m_pObject[i];
            if (o == null)
            {
                m_pObject[i] = p;
                return;
            }
            while (o != p)
            {
                if (o.m_pNext == null)
                {
                    o.m_pNext = p;
                    return;
                }
                o = o.m_pNext;
            }
        }

        public void RemoveMapObject(int x, int y, MapObjectBase p)
        {
            if (p == null) return;
            TL.Saturate(0,ref x,Width - 1);
            TL.Saturate(0,ref y,Height - 1);
            int i = GetIndex(x,y);
            var o = m_pObject[i];
            if (o == p) o = m_pObject[i] = p.m_pNext;
            while (o != null)
            {
                if (o.m_pNext == p) o.m_pNext = p.m_pNext;
                o = o.m_pNext;
            }
            p.m_pNext = null;
        }

        public bool IsPlayable() { return m_bPlayable; }

        public byte GetHeight(int level = 1) { return m_Layers[level].Height; }

        public byte GetWidth(int level = 1) { return m_Layers[level].Width; }

        public bool ItemCompleted() { return m_nGotBanana == m_nTotalBanana; }

        public static void SetEffect(int effect) { m_nEffect = effect; }

        public void GetMainCharaCPos(out int x,out int y)
        {
            m_MainChara.GetCPos(out x,out y);
        }

        public int SetMapData(int level, int x, int y, byte data)
        {
            if (level < 0 || 2 < level || x < 0 || m_Layers[level].Width <= x || y < 0 || m_Layers[level].Height <= y || data >= 0xf0) return 1;
            m_Layers[level].Tiles[x + y * m_Layers[level].Width] = data;
            return 0;
        }

        public float GetWind(int x, int y)
        {
            if (m_Wind == null || x < 0 || Width <= x || y < 0 || Height <= y) return 0.0f;
            return m_Wind[GetIndex(x,y)];
        }

        public MapObjectMain GetMainChara() { return m_MainChara; }

        public BGMNumber GetBGM() { return m_BGMNumber; }

        public static uint GetChunkType(uint type,uint stage)
        {
            uint r = stage & 0xf, l = stage & 0xf0;
            if (r >= 0xa) r += 0x7;
            if (l >= 0xa0) l += 0x70;
            return type + (r << 24) + (l << 12);
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
            MapObjectBase.KillAll();
        }

        public void GarbageMapObject()
        {
            if (m_MainChara != null && !m_MainChara.IsValid())
            {
                m_MainChara = null;
            }
            MapObjectBase.Garbage();
        }

        public float GetFriction(int x, int y)
        {
            if (x < 0 || Width <= x || y < 0 || Height <= y) return 0.0f;
            return m_Friction[(int)m_Hit[GetMapData(1,x,y)] >> 5];
        }

        public void GetViewPos(ref int x,ref int y, float scrollx = 1.0f, float scrolly = 1.0f)
        {
            int offx = (int)(m_ScrollRX - 320 / 2), offy = (int)(m_ScrollRY - 224 / 2 - 2);
            TL.Saturate(0,ref offx,Width * 32 - 320);
            TL.Saturate(0,ref offy,Height * 32 - 224);
            x -= (int)(offx * scrollx); y -= (int)(offy * scrolly);
        }

        public void OnMove()
        {
            if (m_MainChara != null) m_MainChara.OnMove();
            MapObjectEelPitcher.OnMoveAll();
            MapObjectGeasprin.OnMoveAll();
            MapObjectmrframe.OnMoveAll();
            MapObjectNeedle.OnMoveAll();
            MapObjectIce.OnMoveAll();
            MapObjectFire.OnMoveAll();
            MapObjectBase.UpdateCPosAll();
            if (m_MainChara != null) m_MainChara.Synergy();
            MapObjectBanana.SynergyAll();
            MapObjectEelPitcher.SynergyAll();
            MapObjectGeasprin.SynergyAll();
            MapObjectmrframe.SynergyAll();
            MapObjectNeedle.SynergyAll();
            MapObjectIce.SynergyAll();
            MapObjectFire.SynergyAll();
        }

        public byte GetMapData(int level, int x, int y)
        {
            if (level < 0 || 2 < level || x < 0 || m_Layers[level].Width <= x || y < 0 || m_Layers[level].Height <= y) return 0;
            return m_Layers[level].Tiles[GetIndex(level,x,y)];
        }

        public bool GetHit(int x, int y, Hit hit)
        {
            if (x < 0 || Width <= x) return (0x0f & (byte)hit) != 0;
            if (y < 0) return GetHit(x,0,hit);
            if (y >= Height) return GetHit(x,Height - 1,hit);
            return (m_Hit[GetMapData(1,x,y)] & hit) != 0;
        }

        public Map(StageFile lp, byte stage, bool playable = true)
        {
            byte[] buf;
            uint s;
            int[] bgm = new int[(int)BGMNumber.Size];
            m_Stage = stage;
            m_bPlayable = playable;
            MapObjectBase.SetParent(this);
            m_nGotBanana = m_nTotalBanana = 0;
            m_Wind = null;
            m_pObject = null;
            // キャラ
            m_MainChara = null;
            // タイトル
            m_Title = "";
            if ((buf = lp.GetStageData(GetChunkType(ChunkType.TL00,stage),out s)) != null)
            {
                s = Math.Min(s,255);
                m_Title = Encoding.GetEncoding(932).GetString(buf,0,(int)s);
            }
            // TODO: マップチップ読み込みは外に任せたい
            //// マップチップ
            //m_MapChip[0] = ReadMapChip(lp,0);
            //m_MapChip[1] = ReadMapChip(lp,1);
            //m_MapChip[2] = ReadMapChip(lp,2);
            // 当たり判定
            Array.Copy(m_defHit,m_Hit,240);
            if ((buf = lp.GetStageData(ChunkType.HITS,out s)) != null)
            {
                if (s > 240) s = 240;
                Array.Copy(buf.Select(b => (Hit)b).ToArray(),m_Hit,s);
            }
            // マップデータ(下層)
            if ((buf = lp.GetStageData(GetChunkType(ChunkType.M000,stage),out s)) != null)
            {
                m_Layers[0].Width = buf[0];
                m_Layers[0].Height = buf[1];
                var stagesize = m_Layers[0].Width * m_Layers[0].Height;
                m_Layers[0].Tiles = new byte[stagesize];
                Array.Copy(buf.Skip(2).ToArray(),m_Layers[0].Tiles,stagesize);
            }
            else
            {
                m_Layers[0].Tiles = null;
            }
            // マップデータ(中層)
            if ((buf = lp.GetStageData(GetChunkType(ChunkType.M100,stage),out s)) != null)
            {
                m_Layers[1].Width = buf[0];
                m_Layers[1].Height = buf[1];
                var stagesize = m_Layers[1].Width * m_Layers[1].Height;
                m_Layers[1].Tiles = new byte[stagesize];
                m_Wind = new float[stagesize];
                m_pObject = new MapObjectBase[stagesize];
                var windmap = new byte[stagesize];
                Array.Copy(buf.Skip(2).ToArray(),m_Layers[1].Tiles,stagesize);
                int x, y, z, n;
                z = 0;
                for (y = 0;y < m_Layers[1].Height;y++)
                {
                    for (x = 0;x < m_Layers[1].Width;x++)
                    {
                        windmap[z] = 0;
                        n = m_Layers[1].Tiles[z];
                        if (n >= 0xf0)
                        {
                            if (n == 0xf0)
                            {   // 主人公
                                if (m_MainChara == null) m_MainChara = MapObjectMain.Create(x,y);
                                bgm[(int)BGMNumber.GameFunya] += 99;
                            }
                            else if (n == 0xf1){  // バナナ
                                new MapObjectBanana(x,y);
                                bgm[(int)BGMNumber.GameBanana] += 1;
                                m_nTotalBanana++;
                            }
                            else if (n == 0xf2){  // とげとげ
                                new MapObjectNeedle(x,y);
                                bgm[(int)BGMNumber.GameNeedle] += 4;
                            }
                            else if (n == 0xf3){  // ギヤバネ左向き
                                new MapObjectGeasprin(x,y,MapObjectDirection.Left);
                                bgm[(int)BGMNumber.GameGeasprin] += 10;
                            }
                            else if (n == 0xf4){  // ギヤバネ右向き
                                new MapObjectGeasprin(x,y,MapObjectDirection.Right);
                                bgm[(int)BGMNumber.GameGeasprin] += 10;
                            }
                            else if (n == 0xf5){  // 風ストップ
                                windmap[z] = 0xC;
                            }
                            else if (n == 0xf6){  // 風左向き
                                windmap[z] = 0x1;
                                bgm[(int)BGMNumber.GameWind] += 1;
                            }
                            else if (n == 0xf7){  // 風右向き
                                windmap[z] = 0x2;
                                bgm[(int)BGMNumber.GameWind] += 1;
                            }
                            else if (n == 0xf8){  // ミスター・フレーム
                                new MapObjectmrframe(x,y);
                                bgm[(int)BGMNumber.GameMrFrame] += 40;
                            }
                            else if (n == 0xf9){  // ウナギカズラ
                                new MapObjectEelPitcher(x,y);
                                bgm[(int)BGMNumber.GameEelPitcher] += 5;
                            }
                            else if (n == 0xfa){  // 氷
                                new MapObjectIceSource(x,y);
                                bgm[(int)BGMNumber.GameIce] += 8;
                            }
                            else if (n == 0xfb){  // 火
                                new MapObjectFire(x,y);
                                bgm[(int)BGMNumber.GameFire] += 8;
                            }
                            else if (n == 0xfc){  // とげとげ
                                new MapObjectNeedle(x,y,1);
                                bgm[(int)BGMNumber.GameNeedle] += 4;
                            }
                            else if (n == 0xfd){  // とげとげ
                                new MapObjectNeedle(x,y,2);
                                bgm[(int)BGMNumber.GameNeedle] += 4;
                            }
                            else if (n == 0xfe){  // とげとげ
                                new MapObjectNeedle(x,y,3);
                                bgm[(int)BGMNumber.GameNeedle] += 4;
                            }
                            m_Layers[1].Tiles[z] = 0;
                        }
                        else
                        {
                            if (GetHit(x,y,Hit.Left)) windmap[z] = 0x4; else windmap[z] = 0;
                            if (GetHit(x,y,Hit.Right)) windmap[z] |= 0x8;
                        }
                        z++;
                    }
                }
                z = 0;
                float wind = 0.0f;
                int dx;
                for (y = 0;y < m_Layers[1].Height;y++)
                {
                    for (x = 0;x < m_Layers[1].Width;x++)
                    {
                        if ((windmap[z] & 0x10)!=0)
                        {
                        }
                        else
                        {
                            wind = 0.0f;
                            for (dx = 0;x + dx < m_Layers[1].Width;dx++)
                            {
                                if ((windmap[z + dx] & 0x4)!=0) break;
                                if ((windmap[z + dx] & 0x1)!=0) wind -= 1.0f;
                                if ((windmap[z + dx] & 0x2)!=0) wind += 1.0f;
                                windmap[z + dx] |= 0x10;
                                if ((windmap[z + dx] & 0x8)!=0) break;
                            }
                            if (TL.IsIn(-0.01f,wind,0.01f))
                            {
                                wind = 0.0f;
                            }
                            else
                            {
                                new MapObjectWind(x,y,dx,wind);
                            }
                        }
                        m_Wind[z] = wind;
                        z++;
                    }
                }
                windmap = null;
                MapObjectBase.UpdateCPosAll();
            }
            else
            {
                m_Layers[1].Tiles = null;
            }
            // マップデータ(上層)
            if ((buf = lp.GetStageData(GetChunkType(ChunkType.M200,stage),out s)) != null)
            {
                m_Layers[2].Width = buf[0];
                m_Layers[2].Height = buf[1];
                var stagesize = m_Layers[2].Width * m_Layers[2].Height;
                m_Layers[2].Tiles = new byte[stagesize];
                Array.Copy(buf.Skip(2).ToArray(),m_Layers[2].Tiles,stagesize);
            }
            else
            {
                m_Layers[2].Tiles = null;
            }
            // BGM
            var bgmm = 0;
            m_BGMNumber = BGMNumber.Sirent;
            for (int i = (int)BGMNumber.Sirent;i < (int)BGMNumber.Size;i++)
            {
                if (bgmm < bgm[i])
                {
                    bgmm = bgm[i];
                    m_BGMNumber = (BGMNumber)i;
                }
            }
            m_ScrollX = m_ScrollY = 0;
            if (m_MainChara != null) m_MainChara.GetPos(out m_ScrollRX,out m_ScrollRY);
        }

        public void Dispose()
        {
            KillAllMapObject();
            GarbageMapObject();
            m_pObject=null;
            m_Wind = null;
            m_Layers = null;
        }
    }
}
