using System;

namespace MifuminSoft.funyan.Core
{
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
        public void SetStageData(CT dwType, int value)
        {
            // TODO: 新規追加したやつ
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
