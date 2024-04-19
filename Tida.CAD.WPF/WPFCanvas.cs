using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Windows.Media;

namespace Tida.CAD.WPF
{
    /// <summary>
    /// A WPF canvas implemented with <see cref="DrawingContext"/>;
    /// </summary>
    public class WPFCanvas : ICanvas
    {
        /// <summary>
        /// Create an instance of WPFCanvas;
        /// </summary>
        /// <param name="cadScreenConverter">An converter instance</param>
        public WPFCanvas(ICADScreenConverter? cadScreenConverter)
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
        /// Validate <see cref="DrawingContext"/> is available at present;
        /// </summary>
        private void ValidateDrawingContext()
        {
            //如若DrawingContext为空,则不可执行动作;
            if (DrawingContext == null)
            {
                throw new InvalidOperationException($"The {nameof(DrawingContext)} should be set to perform this operation.");
            }

        }

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

            ValidateDrawingContext();

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
            ValidateDrawingContext();
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
                SweepDirection.Counterclockwise
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
        private static PathGeometry GetArcGeometry(Point startScreenPoint, Point endScreenPoint, double screenRadius , SweepDirection sweepDirection)
        {
            var arcSegment = new ArcSegment(endScreenPoint, new Size(screenRadius, screenRadius), 0D, false, sweepDirection, true);

            var segments = new PathSegment[] { arcSegment };
            var pathFigure = new PathFigure(startScreenPoint, segments, false);

            var figures = new PathFigure[] { pathFigure };

            arcSegment.Freeze();
            pathFigure.Freeze();

            return new PathGeometry(figures, FillRule.EvenOdd, null);
        }

        /// <summary>
        /// 绘制(椭)圆;
        /// </summary>
        /// <param name="brush"></param>
        /// <param name="pen"></param>
        /// <param name="center"></param>
        /// <param name="radiusX"></param>
        /// <param name="radiusY"></param>
        public void DrawEllipse(Brush? brush, Pen? pen, Point center, double radiusX, double radiusY)
        {
            ValidateDrawingContext();

            radiusX = CADScreenConverter.ToScreen(radiusX);
            radiusY = CADScreenConverter.ToScreen(radiusY);
            center = CADScreenConverter.ToScreen(center);
            NativeDrawEllipse(brush, pen, center, radiusX, radiusY);
        }

        /// <summary>
        /// 绘制文字;
        /// </summary>
        /// <param name="text"></param>
        /// <param name="emSize"></param>
        /// <param name="foreground"></param>
        /// <param name="origin"></param>
        public void DrawText(FormattedText? formattedText, Point origin)
        {
            ValidateDrawingContext();
            var originScreenPoint = CADScreenConverter.ToScreen(origin);
            var nativeOriginScreenPoint = originScreenPoint;
            DrawingContext.DrawText(formattedText, nativeOriginScreenPoint);
        }

        /// <summary>
        /// 通过点的集合获取三次贝赛尔曲线
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

            pathFigure.Segments.Add(bezier);
            pathGeometry.Figures.Add(pathFigure);

            if (screenPoints.Length >= 1)
            {
                pathFigure.StartPoint = screenPoints[0];

                //因为此处使用的三次贝塞尔曲线要求点的数量为3的倍数,所以在未能正处情况下,重复最后一项至下一个三的倍数;
                var repeatCount = (3 - (screenPoints.Length % 3)) % 3;

                var lastScreenPoint = screenPoints[screenPoints.Length - 1];
                for (int i = 0; i < repeatCount; i++)
                {
                    bezier.Points.Add(lastScreenPoint);
                }
            }

            return pathGeometry;
        }

        /// <summary>
        /// 绘制路径(未封闭区域);
        /// </summary>
        /// <param name="pen"></param>
        /// <param name="points"></param>
        public void DrawCurve(Pen? pen, IEnumerable<Point> points)
        {

            ValidateDrawingContext();

            ////使用一个变量存储上一次的视图点;
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
        public void DrawRectangle(CADRect rect, Brush? brush, Pen? pen)
        {
            ValidateDrawingContext();

            var topLeftInScreen = CADScreenConverter.ToScreen(rect.TopLeft);
            var widthInScreen = CADScreenConverter.ToScreen(rect.Width);
            var heightInScreen = CADScreenConverter.ToScreen(rect.Height);
            var rectInScreen = new Rect(topLeftInScreen, new Size(widthInScreen, heightInScreen));
            DrawingContext.DrawRectangle(brush, pen, rectInScreen);
        }

        public void DrawPolygon(IEnumerable<Point> points, Brush? brush, Pen? pen)
        {
            ValidateDrawingContext();
            DrawFill(points, brush, pen);
        }

        /// <summary>
        /// 根据所有的点，组成一个封闭区域，并且填充
        /// </summary>
        /// <param name="points">所有的顶点坐标</param>
        /// <param name="brush">区域颜色</param>
        private void DrawFill(IEnumerable<Point> points, Brush? brush, Pen? pen)
        {

            if (points == null)
            {
                throw new ArgumentNullException(nameof(points));
            }

            ValidateDrawingContext();

            NativeDrawFill(
                points.Select(p => CADScreenConverter.ToScreen(p)),
                brush,
                pen
            );
        }


        /// <summary>
        /// 直接根据视图位置,绘制WPF封闭区域;
        /// </summary>
        /// <param name="screenPoints"></param>
        /// <param name="brush"></param>
        /// <param name="pen"></param>
        private void NativeDrawFill(IEnumerable<Point> screenPoints, Brush? brush, Pen? pen)
        {
            if (screenPoints == null)
            {
                throw new ArgumentNullException(nameof(screenPoints));
            }

            ValidateDrawingContext();

            //操作PathGeometry中的Figures以绘制(封闭)区域
            var paths = new PathGeometry();

            var pfc = new PathFigureCollection();
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
                pf.Segments.Add(ps);
            }


            pf.IsClosed = true;
            paths.Figures = pfc;
            DrawingContext.DrawGeometry(brush, pen, paths);
        }

        /// <summary>
        /// 以视图坐标为标准,绘制椭圆;
        /// </summary>
        /// <param name="brush">填充色</param>
        /// <param name="pen">笔</param>
        public void NativeDrawEllipse(Brush? brush, Pen? pen, Point center, double radiusX, double radiusY)
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
}
