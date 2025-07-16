using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Tida.CAD.DrawObjects;

/// <summary>
/// A draw object that draws a polygon;
/// </summary>
public class Polygon:DrawObject
{
    private IEnumerable<Point>? _points;
    public IEnumerable<Point>? Points 
    { 
        get => _points;
        set
        {
            _points = value;
            RaiseVisualChanged();
        }
    }

    private Pen? _pen;
    public Pen? Pen
    {
        get => _pen;
        set
        {
            _pen = value;
            RaiseVisualChanged();
        }
    }

    private Brush? _brush;

    public Brush? Brush
    {
        get { return _brush; }
        set 
        { 
            _brush = value;
            RaiseVisualChanged();
        }
    }


    public override void Draw(ICanvas canvas)
    {
        if(Points == null)
        {
            return;
        }
        canvas.DrawPolygon(Points, Brush, Pen);
        base.Draw(canvas);  
    }
}
