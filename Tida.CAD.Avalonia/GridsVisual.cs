using Avalonia;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.CAD.Avalonia;

class GridsVisual : Visual
{
    public GridsVisual(ICADScreenConverter cadScreenConverter)
    {
        _cadScreenConverter = cadScreenConverter;
    }
    private Pen? _gridPen;
    public void RefreshGridsPen()
    {
        if (GridsBrush == null || GridsThickness <= 0)
        {
            _gridPen = null;
            return;
        }

        _gridPen = new Pen { Brush = GridsBrush, Thickness = GridsThickness };
    }
    private readonly ICADScreenConverter _cadScreenConverter;
    /// <summary>
    /// The brush of grid lines;
    /// </summary>
    public Brush GridsBrush { get; set; } = DefaultGridsBrush;
    public static readonly Brush DefaultGridsBrush = new SolidColorBrush
    {
        Color = Color.FromArgb(230, 80, 80, 80)
    };

    public const double DefaultGridsThickness = 2;
    /// <summary>
    /// The thickness of grid lines;
    /// </summary>
    public double GridsThickness { get; set; } = DefaultGridsThickness;
    /// <summary>
    /// Draw the grid lines;
    /// </summary>
    public override void Render(DrawingContext drawingContext)
    {
        //Get the cell size in view coordinates.
        var unitLength = _cadScreenConverter.ToScreen(1);
        if (unitLength < 3)
        {
            return;
        }
        if (_gridPen == null)
        {
            return;
        }
        //Draw vertical lines;
        var horiPos = _cadScreenConverter.PanScreenPosition.X % unitLength;
        while (horiPos <= _cadScreenConverter.ActualWidth)
        {
            var point0 = new Point(horiPos, _cadScreenConverter.ActualHeight);
            var point1 = new Point(horiPos, 0);
            drawingContext.DrawLine(_gridPen, point0, point1);
            horiPos += unitLength;
        }
        
        //Draw horizontal lines;
        var vertiPos = _cadScreenConverter.PanScreenPosition.Y % unitLength;
        while (vertiPos <= _cadScreenConverter.ActualHeight)
        {
            var point0 = new Point(0,vertiPos);
            var point1 = new Point(_cadScreenConverter.ActualWidth,vertiPos);
            drawingContext.DrawLine(_gridPen, point0, point1);
            vertiPos += unitLength;
        }
    }
}
