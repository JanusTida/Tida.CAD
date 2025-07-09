using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Tida.CAD;
using Tida.CAD.Extensions;
using Tida.CAD.Input;

namespace Tida.CAD.DrawObjects; 
/// <summary>
/// DrawObject——Rectangle;
/// </summary>
public class Rectangle : DrawObject {
    static Rectangle()
    {
        _defaultSelectionPen = new Pen
        {
            Brush = Brushes.Blue,
            Thickness = 3
        };
        _defaultSelectionPen.Freeze();
    }

    private static Pen _defaultSelectionPen;

    private Pen? _pen;
    /// <summary>
    /// The pen used for borders;
    /// </summary>
    public Pen? Pen 
    {
        get => _pen;
        set
        {
            _pen = value;
            RaiseVisualChanged();
        }
    }

    private Pen _selectionPen = _defaultSelectionPen;
    /// <summary>
    /// The pen used for borders when selected;
    /// </summary>
    public Pen SelectionPen
    {
        get => _selectionPen;
        set
        {
            _selectionPen = value;
            RaiseVisualChanged();
        }
    }

    private Brush? _background;
    /// <summary>
    /// The background brush;
    /// </summary>
    public Brush? Background
    {
        get => _background;
        set
        {
            _background = value;
            RaiseVisualChanged();
        }
    }
    public Rectangle(CADRect rect) 
    {
        this.Rectangle2D = rect;
    }

    
    private CADRect _rectangle2D;

    public CADRect Rectangle2D 
    {
        get => _rectangle2D; 
        set
        {
            _rectangle2D = value;
            RaiseVisualChanged();
        }
    }

    
    public override CADRect? GetBoundingRect(ICADScreenConverter screenConverter)
    {
        return Rectangle2D;
    }

    public override bool ObjectInRectangle(CADRect rect, ICADScreenConverter cadScreenConverter, bool anyPoint) 
    {
        if (anyPoint) {
            return Rectangle2D.GetVertexes()?.Any(p => rect.Contains(p)) ?? false;
        }
        else {
            return Rectangle2D.GetVertexes()?.All(p => rect.Contains(p)) ?? false;
        }
    }

    public override bool PointInObject(Point point, ICADScreenConverter cadScreenConverter) 
    {
        return Rectangle2D.Contains(point);
    }

    public override void Draw(ICanvas canvas)
    {
        if (canvas == null) {
            throw new ArgumentNullException(nameof(canvas));
        }
        if (IsSelected)
        {
            canvas.DrawRectangle(Rectangle2D, Background, SelectionPen);
        }
        else
        {
            canvas.DrawRectangle(Rectangle2D, Background, Pen);
        }
        base.Draw(canvas);
    }

    protected override void OnMouseUpCore(CADMouseButtonEventArgs e)
    {
        base.OnMouseUpCore(e);
    }

}
