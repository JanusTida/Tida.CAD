using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.Contracts.Docking {
    /// <summary>
    /// 停靠位置;
    /// </summary>
    public enum DockingPosition {
            //
        // Summary:
        //     The item is docked to the left.
        Left = 0,
        //
        // Summary:
        //     The item is docked to the bottom.
        Bottom = 1,
        //
        // Summary:
        //     The items is docked to the right.
        Right = 2,
        //
        // Summary:
        //     The item is docked to the top.
        Top = 3,
        //
        // Summary:
        //     The item is not docked, but is dockable.
        FloatingDockable = 4,
        //
        // Summary:
        //     The item is not dockable.
        FloatingOnly = 5
    }
}
