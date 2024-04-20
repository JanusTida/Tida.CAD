using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Tida.CAD.Input
{
    /// <summary>
    /// CADMouseButtonEventArgs;
    /// </summary>
    public class CADMouseButtonEventArgs:CADRoutedEventArgs
    {
        public CADMouseButtonEventArgs(Point position)
        {
            Position = position;
        }

        /// <summary>
        /// The mouse position in CAD coordinates;
        /// </summary>
        public Point Position { get; }
#if WPF
        /// <summary>
        /// MouseButtonEventArgs in WPF;
        /// </summary>
        public MouseButtonEventArgs? MouseButtonEventArgs { get; set; }
#endif
    }
}
