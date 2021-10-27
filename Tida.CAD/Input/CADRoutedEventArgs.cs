using System;
using System.Collections.Generic;
using System.Text;

namespace Tida.CAD.Input
{
    /// <summary>
    ///  Contains state information and event data associated of a routed event;
    /// </summary>
    public abstract class CADRoutedEventArgs : EventArgs
    {
        public CADRoutedEventArgs()
        {
            
        }
        /// <summary>
        /// Gets or sets a value that indicates the present state of the event handling for
        ///     a routed event as it travels the route.
        /// </summary>
        public bool Handled { get; set; }
    }
}
