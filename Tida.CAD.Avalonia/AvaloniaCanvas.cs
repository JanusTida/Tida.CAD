using Avalonia;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace Tida.CAD.Avalonia;

class AvaloniaCanvas : ICanvas
{
    /// <summary>
    /// Create an instance of WPFCanvas;
    /// </summary>
    /// <param name="cadScreenConverter">An converter instance</param>
    public AvaloniaCanvas(ICADScreenConverter cadScreenConverter)
    {
        CADScreenConverter = cadScreenConverter ?? throw new ArgumentNullException(nameof(cadScreenConverter));
    }

    /// <summary>
    /// An instance of DrawingContext,the value can be modified in run time;
    /// </summary>
    public DrawingContext DrawingContext => InernalDrawingContext ?? throw new Exception($"{nameof(InernalDrawingContext)} should not be null");

    internal DrawingContext? InernalDrawingContext { get; set; }

    /// <summary>
    /// The coverter instance;
    /// </summary>
    public ICADScreenConverter CADScreenConverter { get; }

    /// <summary>
    /// Draw a line;
    /// </summary>
    /// <param name="pen"></param>
    public void DrawLine(Pen? pen, Point point0, Point point1)
    {
        if (pen == null)
        {
            return;
        }

        var screenPoint1 = CADScreenConverter.ToScreen(point0);
        var screenPoint2 = CADScreenConverter.ToScreen(point1);

        //平台转换后再进行绘制;
        DrawingContext.DrawLine(
            pen,
            screenPoint1,
            screenPoint2
        );
    }


    /// <summary>
    /// Draw a arc;
    /// </summary>
    /// <param name="pen"></param>
    /// <param name="center"></param>
    /// <param name="radius"></param>
    /// <param name="beginangle"></param>
    /// <param name="angle"></param>
    public void DrawArc(Pen? pen, Point center, double radius, double beginangle, double angle)
    {
        beginangle %= (Math.PI * 2);
        angle %= (Math.PI * 2);

        var endAngle = beginangle + angle;

        var startPoint = new Point(center.X + Math.Cos(beginangle) * radius, center.Y + Math.Sin(beginangle) * radius);
        var endPoint = new Point(center.X + Math.Cos(endAngle) * radius, center.Y + Math.Sin(endAngle) * radius);

        var startScreenPoint = CADScreenConverter.ToScreen(startPoint);
        var endScreenPoint = CADScreenConverter.ToScreen(endPoint);


        var screenRadius = CADScreenConverter.ToScreen(radius);

        var arcGeometry = GetArcGeometry
        (
            startScreenPoint,
            endScreenPoint,
            screenRadius,
            SweepDirection.CounterClockwise
        );

        DrawingContext.DrawGeometry
        (
            null,
            pen,
            arcGeometry
        );

    }

    /// <summary>
    /// Create a <see cref="PathGeometry"/> from an arc;
    /// </summary>
    /// <param name="center"></param>
    /// <param name="screenRadius"></param>
    /// <param name="beginAngle"></param>
    /// <param name="angle"></param>
    /// <param name="smallAngle"></param>
    /// <returns></returns>
    private static PathGeometry GetArcGeometry(Point startScreenPoint, Point endScreenPoint, double screenRadius, SweepDirection sweepDirection)
    {
        //var arcSegment = new ArcSegment(endScreenPoint, new Size(screenRadius, screenRadius), 0D, false, sweepDirection, true);
        var arcSegment = new ArcSegment
        {
            Point = endScreenPoint,
            Size = new Size(screenRadius, screenRadius),
            SweepDirection = sweepDirection,
            RotationAngle = 0D,

        };

        var segments = new PathSegments { arcSegment };
        var pathFigure = new PathFigure
        {
            StartPoint = startScreenPoint,
            Segments = segments,
            IsClosed = false
        };

        var figures = new PathFigures { pathFigure };

        
        return new PathGeometry { Figures = figures, FillRule = FillRule.EvenOdd };
    }

    /// <summary>
    /// Draw a ellipse;
    /// </summary>
    /// <param name="brush"></param>
    /// <param name="pen"></param>
    /// <param name="center"></param>
    /// <param name="radiusX"></param>
    /// <param name="radiusY"></param>
    public void DrawEllipse(IBrush? brush, Pen? pen, Point center, double radiusX, double radiusY)
    {
        radiusX = CADScreenConverter.ToScreen(radiusX);
        radiusY = CADScreenConverter.ToScreen(radiusY);
        center = CADScreenConverter.ToScreen(center);
        NativeDrawEllipse(brush, pen, center, radiusX, radiusY);
    }

    /// <summary>
    /// Draw a text;
    /// </summary>
    /// <param name="text"></param>
    /// <param name="emSize"></param>
    /// <param name="foreground"></param>
    /// <param name="origin"></param>
    public void DrawText(FormattedText formattedText, Point origin)
    {
        var originScreenPoint = CADScreenConverter.ToScreen(origin);
        var nativeOriginScreenPoint = originScreenPoint;
        DrawingContext.DrawText(formattedText, nativeOriginScreenPoint);
    }

    /// <summary>
    /// Get bezier curve from points;
    /// </summary>
    /// <param name="points"></param>
    /// <returns></returns>
    private PathGeometry GetCurveGeometry(IEnumerable<Point> points)
    {
        if (points == null)
        {
            throw new ArgumentNullException(nameof(points));
        }

        var screenPoints = points.Select(x => CADScreenConverter.ToScreen(x)).ToArray();
        var bezier = new PolyBezierSegment(screenPoints, true);
        var pathFigure = new PathFigure();
        var pathGeometry = new PathGeometry();

        pathFigure.Segments = [bezier];
        pathGeometry.Figures = [pathFigure];

        if (screenPoints.Length >= 1)
        {
            pathFigure.StartPoint = screenPoints[0];

            //Because the PolyBezierSegment requires the number of points to be a multiple of 3,
            // we repeat the last point to make the number of points a multiple of 3.
            var repeatCount = (3 - (screenPoints.Length % 3)) % 3;

            var lastScreenPoint = screenPoints[screenPoints.Length - 1];
            for (int i = 0; i < repeatCount; i++)
            {
                if(bezier.Points == null)
                {
                    bezier.Points = [];
                }
                bezier.Points.Add(lastScreenPoint);
            }
        }

        return pathGeometry;
    }

    /// <summary>
    /// Draw a curve;
    /// </summary>
    /// <param name="pen"></param>
    /// <param name="points"></param>
    public void DrawCurve(Pen? pen, IEnumerable<Point> points)
    {
        //Point? lastScreenPoint = null;
        //foreach (var point in points)
        //{
        //    var thisScreenPoint = CanvasScreenConverter.ToScreen(point);
        //    if (lastScreenPoint != null)
        //    {
        //        //绘制的是上一次的节点到这一次的节点;
        //        DrawingContext.DrawLine(pen, lastScreenPoint.Value, thisScreenPoint);

        //    }
        //    lastScreenPoint = thisScreenPoint;
        //}

        var path = GetCurveGeometry(points);
        DrawingContext.DrawGeometry(null, pen, path);
    }

    /// <summary>
    /// Draw a rectangle;
    /// </summary>
    /// <param name="brush">The brush to fill the rect</param>
    /// <param name="pen">The pen to decorate the border of the rect</param>
    /// <param name="rect"></param>
    public void DrawRectangle(CADRect rect, IBrush? brush, Pen? pen)
    {
        var topLeftInScreen = CADScreenConverter.ToScreen(rect.TopLeft);
        var widthInScreen = CADScreenConverter.ToScreen(rect.Width);
        var heightInScreen = CADScreenConverter.ToScreen(rect.Height);
        var rectInScreen = new Rect(topLeftInScreen, new Size(widthInScreen, heightInScreen));
        DrawingContext.DrawRectangle(brush, pen, rectInScreen);
    }

    public void DrawPolygon(IEnumerable<Point> points, IBrush? brush, Pen? pen)
    {
        DrawFill(points, brush, pen);
    }

    /// <summary>
    /// Create a closed region by all points, and fill it with the specified brush and pen.
    /// </summary>
    /// <param name="points">The points to create the region</param>
    /// <param name="brush">The brush to fill the region</param>
    private void DrawFill(IEnumerable<Point> points, IBrush? brush, Pen? pen)
    {

        if (points == null)
        {
            throw new ArgumentNullException(nameof(points));
        }
        NativeDrawFill(
            points.Select(p => CADScreenConverter.ToScreen(p)),
            brush,
            pen
        );
    }


    /// <summary>
    /// Draw a closed region by all points, and fill it with the specified brush and pen.with native screen points;
    /// </summary>
    /// <param name="screenPoints"></param>
    /// <param name="brush"></param>
    /// <param name="pen"></param>
    private void NativeDrawFill(IEnumerable<Point> screenPoints, IBrush? brush, Pen? pen)
    {
        if (screenPoints == null)
        {
            throw new ArgumentNullException(nameof(screenPoints));
        }

        //操作PathGeometry中的Figures以绘制(封闭)区域
        var paths = new PathGeometry();

        var pfc = new PathFigures();
        var pf = new PathFigure();
        pfc.Add(pf);

        //存储一个点表示当前的PathFigure的StartPoint是否被指定;
        var startPointSet = false;

        foreach (var p in screenPoints)
        {
            //若StartPoint未被设定(第一个节点),设定后继续下一次循环;
            if (!startPointSet)
            {
                pf.StartPoint = p;
                startPointSet = true;
                continue;
            }

            //若若StartPoint被设定,则加入线段;
            var ps = new LineSegment();
            ps.Point = p;
            if(pf.Segments == null)
            {
                pf.Segments = [];
            }
            pf.Segments.Add(ps);
        }


        pf.IsClosed = true;
        paths.Figures = pfc;
        DrawingContext.DrawGeometry(brush, pen, paths);
    }

    /// <summary>
    /// Draw a ellipse;with native screen points;
    /// </summary>
    /// <param name="brush">The brush to fill the ellipse</param>
    /// <param name="pen">The pen to decorate the border of the ellipse</param>
    public void NativeDrawEllipse(IBrush? brush, Pen? pen, Point center, double radiusX, double radiusY)
    {
        DrawingContext.DrawEllipse(
            brush,
            pen,
            center,
            radiusX,
            radiusY
        );
    }
}
