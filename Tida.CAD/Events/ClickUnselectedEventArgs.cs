using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;

namespace Tida.CAD.Events
{
    /// <summary>
    /// The eventargs  used after unselecting drawobjects with clicking;
    /// </summary>
    public class ClickUnselectedEventArgs : EventArgs
    {
        public ClickUnselectedEventArgs(Point position, IList<DrawObject> unselectedDrawObjects)
        {
            this.HitPosition = position;
            this.UnselectedDrawObjects = unselectedDrawObjects;
        }

        /// <summary>
        /// The position where the mouse clicked;
        /// </summary>
        public Point HitPosition { get; }

        /// <summary>
        /// The drawobject to select;
        /// </summary>
        public IList<DrawObject> UnselectedDrawObjects { get; }
    }
}
