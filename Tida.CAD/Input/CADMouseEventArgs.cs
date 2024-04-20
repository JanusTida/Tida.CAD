using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Tida.CAD.Input
{
    /// <summary>
    /// CADMouseEventArgs;
    /// </summary>
    public class CADMouseEventArgs:CADRoutedEventArgs
    {
        public CADMouseEventArgs(Point position)
        {
            Position = position;
        }

        /// <summary>
        /// The mouse position in CAD coordinates;
        /// </summary>
        public Point Position { get; }
#if WPF
        /// <summary>
        /// The native mouseeventargs in WPF;
        /// </summary>
        public MouseEventArgs? MouseEventArgs { get; set; }
#endif
    }
}
