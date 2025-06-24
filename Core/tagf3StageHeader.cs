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
}
