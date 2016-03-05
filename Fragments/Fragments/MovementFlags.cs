using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fragments
{
    public enum MovementFlags
    {
        NONE = 0,
        UP = 1,
        DOWN = 2,
        LEFT = 4,
        RIGHT = 8,
        DAMAGE = 16
    }
}
