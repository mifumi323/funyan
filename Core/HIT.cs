using System;

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
}
