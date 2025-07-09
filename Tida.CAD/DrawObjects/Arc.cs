using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Tida.CAD.DrawObjects;

public class Arc : DrawObject
{
    /// <summary>
    /// The pen that draws the arc,this property should be set so that the arc is visible;
    /// </summary>
    public Pen? Pen { get; set; }

    public Point Center { get; set; }

    public double Radius { get; set; }

    /// <summary>
    /// The angle from which the arc start;in radian;the corresponding direction of zero value is 3 o'clock;
    /// </summary>
    public double BeginAngle { get; set; }

    /// <summary>
    /// The angle byte which the arc was drawn;in radian,the sweep direction is unclock wise.
    /// </summary>
    public double Angle { get; set; }

    public override void Draw(ICanvas canvas)
    {
        if(Pen == null)
        {
            return;
        }
        canvas.DrawArc(Pen, Center,Radius, BeginAngle, Angle);
        base.Draw(canvas);
    }
}
