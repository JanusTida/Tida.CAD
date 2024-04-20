using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Input {
    //
    // Summary:
    //     Specifies the set of modifier keys.
    [Flags]
    public enum ModifierKeys {
        //
        // Summary:
        //     No modifiers are pressed.
        None = 0,
        //
        // Summary:
        //     The ALT key.
        Alt = 1,
        //
        // Summary:
        //     The CTRL key.
        Control = 2,
        //
        // Summary:
        //     The SHIFT key.
        Shift = 4,
        //
        // Summary:
        //     The Windows logo key.
        Windows = 8
    }
}
