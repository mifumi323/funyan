using System;

namespace MifuminSoft.funyan.Core
{
    [Flags]
    public enum Hit : byte
    {
        /// <summary>HIT_TOP</summary>
        Top = 0x01,
        /// <summary>HIT_BOTTOM</summary>
        Bottom = 0x02,
        /// <summary>HIT_LEFT</summary>
        Left = 0x04,
        /// <summary>HIT_RIGHT</summary>
        Right = 0x08,
        /// <summary>HIT_DEATH</summary>
        Death = 0x10,
    }
}
