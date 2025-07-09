using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Tida.CAD.Events;

/// <summary>
/// The eventargs used after selecting drawobjects with clicking;
/// </summary>
public class ClickSelectedEventArgs : EventArgs
{
    public ClickSelectedEventArgs(Point position,IList<DrawObject> selectedDrawObjects)
    {
        this.HitPosition = position;
        this.SelectedDrawObjects = selectedDrawObjects;
    }

    /// <summary>
    /// The position where the mouse clicked;
    /// </summary>
    public Point HitPosition { get; }

    /// <summary>
    /// The drawobject to select;
    /// </summary>
    public IList<DrawObject> SelectedDrawObjects { get; }
}
