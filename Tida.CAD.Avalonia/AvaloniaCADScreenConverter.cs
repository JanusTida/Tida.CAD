using System;
using Tida.CAD;

namespace Tida.CAD.Avalonia;
/// <summary>
/// The <see cref="AvaloniaCADScreenConverter"/> class is used to convert CAD unit values to screen values and vice versa.
/// </summary>
class AvaloniaCADScreenConverter : ICADScreenConverter {
    /// <summary>
    /// The screen resolution, which is 96dpi.
    /// </summary>
    public const double ScreenResolution = 96;
    /// <summary>
    /// Default zoom value, which is 1.
    /// </summary>
    public const double DefaultZoom = 1;

    /// <summary>
    /// The zoom value of the view.
    /// </summary>
    private double _zoom = DefaultZoom;
    public double Zoom {
        get => _zoom;
        set {
            if(value <= 0) {
                throw new ArgumentException($"{nameof(Zoom)} should be larger than zero.");
            }

            _zoom = value;
        }
    } 

    /// <summary>
    /// The pan screen position of the view.
    /// </summary>
    public Point PanScreenPosition { get; set; }

    /// <summary>
    /// The actual width of the view.
    /// </summary>
    public  double ActualWidth { get; set; }

    /// <summary>
    /// The actual height of the view.
    /// </summary>
    public double ActualHeight { get; set; }


    public double ToScreen(double unitValue) {
        return unitValue * Zoom * ScreenResolution;
    }

    public Point ToScreen(Point unitpoint) {
        var screenX = ToScreen(unitpoint.X);
        var screenY = ToScreen(unitpoint.Y);

        return new Point(
            screenX + PanScreenPosition.X,
            -screenY + PanScreenPosition.Y
        );
    }

    public Point ToCAD(Point screenpoint) {
        var unitX = ToCAD(screenpoint.X   -  PanScreenPosition.X);
        var unitY = ToCAD(-screenpoint.Y +  PanScreenPosition.Y);

        return new Point(unitX, unitY);
    }

    public double ToCAD(double screenvalue) {
        return screenvalue / (ScreenResolution * Zoom);
    }

}
