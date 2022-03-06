using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;

namespace Tida.CAD.Events
{
    /// <summary>
    /// The eventargs  used while unselecting drawobjects with clicking;
    /// </summary>
    public class ClickUnselectingEventArgs : CancelEventArgs
    {
        public ClickUnselectingEventArgs(Point position, IList<DrawObject> hitedDrawObjects)
        {
            this.HitPosition = position;
            this.HitedDrawObjects = hitedDrawObjects;
        }

        /// <summary>
        /// The position where the mouse clicked;
        /// </summary>
        public Point HitPosition { get; }

        /// <summary>
        /// The drawobject to select;
        /// </summary>
        public IList<DrawObject> HitedDrawObjects { get; }
    }
}
