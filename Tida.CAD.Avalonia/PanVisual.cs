using Avalonia;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.CAD.Avalonia;

class PanVisual : Visual
{
    public PanVisual(ICADScreenConverter cadScreenConverter)
    {
        _cadScreenConverter = cadScreenConverter;
    }
    private readonly ICADScreenConverter _cadScreenConverter;
    public const double DefaultPanLength = 72;
    public double PanLength { get; set; } = DefaultPanLength;

    public static readonly Brush DefaultPanBrush = Brushes.White;
    
    public Brush PanBrush { get; set; } = DefaultPanBrush;

    public const double DefaultPanThickness = 2;
    public double PanThickness { get; set; } = DefaultPanThickness;

    private Pen? _panPen;
    public void RefreshPanPen()
    {
        if (PanBrush == null || PanThickness <= 0)
        {
            _panPen = null;
        }
        else
        {
            _panPen = new Pen { Brush = PanBrush, Thickness = PanThickness };
        }
    }

    public override void Render(DrawingContext drawingContext)
    {
        if (_panPen == null)
        {
            return;
        }
        drawingContext.PushClip(new Rect(new Size(_cadScreenConverter.ActualWidth, _cadScreenConverter.ActualHeight)));
        drawingContext.DrawLine(
            _panPen,
            new Point(_cadScreenConverter.PanScreenPosition.X - PanLength / 2, _cadScreenConverter.PanScreenPosition.Y),
            new Point(_cadScreenConverter.PanScreenPosition.X + PanLength / 2, _cadScreenConverter.PanScreenPosition.Y)
        );

        drawingContext.DrawLine(
            _panPen,
            new Point(_cadScreenConverter.PanScreenPosition.X, _cadScreenConverter.PanScreenPosition.Y - PanLength / 2),
            new Point(_cadScreenConverter.PanScreenPosition.X, _cadScreenConverter.PanScreenPosition.Y + PanLength / 2)
        );
    }

}
