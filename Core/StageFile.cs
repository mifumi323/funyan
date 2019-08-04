using System;

namespace MifuminSoft.funyan.Core
{
    public struct tagf3StageHeader
    {
        /// <summary>"funya3s1"(8 バイト)</summary>
        public ulong ident;
        /// <summary>展開サイズ(4 バイト)</summary>
        public int datasize;
        /// <summary>圧縮サイズ(datasizeと同じ場合無圧縮)</summary>
        public int packsize;
    }

    public enum CT : uint
    {
        CT_TITL = 0x4C544954,
        CT_AUTH = 0x48545541,
        CT_DESC = 0x43534544,
        CT_ENDM = 0x4D444E45,
        CT_STGC = 0x43475453,
        CT_REST = 0x54534552,
        CT_PNLT = 0x544C4E50,
        CT_HITS = 0x53544948,
        CT_TL00 = 0x30304C54,
        CT_M000 = 0x3030304D,
        CT_M100 = 0x3030314D,
        CT_M200 = 0x3030324D,
        CT_MCF0 = 0x3046434D,
        CT_MCD0 = 0x3044434D,

        CT_RPLY = 0x594C5052,
        CT_STGN = 0x4E475453,
        CT_GRVT = 0x54565247,
        CT_HYPR = 0x52505948,
    }

    public class Cf3StageFile : IDisposable
    {
        protected void ClearData()
        {
            // TODO: 一から実装し直したほうがよさそうだし、一度全部消すよ。
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns>0:成功、3:Open失敗</returns>
        public int Write(string filename)
        {
            // TODO: 一から実装し直したほうがよさそうだし、一度全部消すよ。
            throw new NotImplementedException();
        }
        public void SetStageData(CT dwType, byte[] lpData)
        {
            // TODO: 一から実装し直したほうがよさそうだし、一度全部消すよ。
            throw new NotImplementedException();
        }
        // データを取得。なければNULL
        public byte[] GetStageData(CT dwType)
        {
            // TODO: 一から実装し直したほうがよさそうだし、一度全部消すよ。
            throw new NotImplementedException();
        }

        public string GetStageDataString(CT dwType)
        {
            // TODO: 新規追加したやつ
            throw new NotImplementedException();
        }

        public int GetStageDataInt(CT dwType, int def = 0)
        {
            // TODO: 新規追加したやつ
            throw new NotImplementedException();
        }

        /// <summary>
        /// ステージファイルを読み込みメモリに格納する
        /// </summary>
        /// <param name="filename"></param>
        /// <returns>0:成功、2:未対応フォーマット(未知のヘッダ検出)、3:圧縮状態違反(サイズが大きくなっている)</returns>
        public int Read(string filename)
        {
            // TODO: 一から実装し直したほうがよさそうだし、一度全部消すよ。
            throw new NotImplementedException();
        }
        public Cf3StageFile()
        {
        }
        public void Dispose()
        {
            ClearData();
        }
    }
}
