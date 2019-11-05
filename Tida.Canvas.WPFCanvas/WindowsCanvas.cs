using System;
using System.Collections.Generic;
using SystemMedia = System.Windows.Media;
using Tida.Geometry.Primitives;
using Tida.Canvas.WPFCanvas.Media;
using Tida.Canvas.WPFCanvas.Geometry;
using Tida.Canvas.Contracts;
using Tida.Canvas.Media;
using System.Windows;
using System.Linq;
using Tida.Geometry.External;

namespace Tida.Canvas.WPFCanvas {
    /// <summary>
    /// 本类为根据<see cref="SystemMedia.DrawingContext"/>为基础所封装的WPF画布实现;
    /// </summary>
    public class WindowsCanvas : ICanvas {
        /// <summary>
        /// 绘制文字所需用到的一个参数;
        /// </summary>
        public static readonly SystemMedia.Typeface TypeFace = new SystemMedia.Typeface("微软雅黑");
        private static SystemMedia.GlyphTypeface _glyphTypeFace;
        public static SystemMedia.GlyphTypeface GlyphTypeFace {
            get {
                if(_glyphTypeFace == null) {
                    TypeFace.TryGetGlyphTypeface(out _glyphTypeFace);
                }
                return _glyphTypeFace;
            }
        }

        /// <summary>
        /// 根据一个画布-视图转化实现构建;
        /// </summary>
        /// <param name="canvasProxy">转化实现</param>
        public WindowsCanvas(ICanvasScreenConvertable canvasProxy) {
            this._canvasProxy = canvasProxy ?? throw new ArgumentNullException(nameof(canvasProxy));
        }

        /// <summary>
        /// 被操作的DrawingContext实例;外部可更改本对象,以达到复用对象的目的,避免反复构建画布;
        /// </summary>
        public SystemMedia.DrawingContext DrawingContext { get; set; }

        /// <summary>
        /// 因为绘制需要使用到坐标转换等的功能，以处理工程数学与WPF间的转换;
        /// </summary>
        private readonly ICanvasScreenConvertable _canvasProxy;

        /// <summary>
        /// 验证DrawingContext是否可用;
        /// </summary>
        private void ValidateDrawingContext() {
            //如若DrawingContext为空,则不可执行动作;
            if (DrawingContext == null) {
                throw new InvalidOperationException($"The {nameof(DrawingContext)} should be set to perform this operation.");
            }
            
        }

        /// <summary>
        /// 将线段限制到可见范围内;
        /// </summary>
        /// <param name="line">原坐标</param>
        /// <returns>返回可见的线段</returns>
        private static Line2D GetLimitedLine2D(Line2D line, Rectangle2D2 rect) {
            var visibleLine = new Line2D(line.Start, line.End);

            return visibleLine;
        }

        /// <summary>
        /// 绘制线段;
        /// </summary>
        /// <param name="pen"></param>
        /// <param name="line"></param>
        public void DrawLine(Pen pen, Line2D line) {
            if (pen == null) {
                return;
            }
            
            if (line == null) {
                throw new ArgumentNullException(nameof(line));
            }

            ValidateDrawingContext();

            var screenPoint1 = _canvasProxy.ToScreen(line.Start);
            var screenPoint2 = _canvasProxy.ToScreen(line.End);
            
            //平台转换后再进行绘制;
            DrawingContext.DrawLine(
                PenAdapter.ConverterToSystemPen(pen),
                Vector2DAdapter.ConvertToSystemPoint(screenPoint1),
                Vector2DAdapter.ConvertToSystemPoint(screenPoint2)
            );
        }

     
        /// <summary>
        /// 绘制圆弧;
        /// </summary>
        /// <param name="pen"></param>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <param name="beginangle"></param>
        /// <param name="angle"></param>
        public void DrawArc(Pen pen, Vector2D center, double radius, double beginangle, double angle,bool smallAngle) {
            if(pen == null) {
                throw new ArgumentNullException(nameof(pen));
            }

            ValidateDrawingContext();
            beginangle %= (Math.PI * 2);
            angle  %= (Math.PI * 2);
            
            var centerPoint = ConvertVectorToScreenPoint(center);
            var endAngle = beginangle + angle;

            var startPoint = new Vector2D(center.X + Math.Cos(beginangle) * radius, center.Y + Math.Sin(beginangle) * radius);
            var endPoint = new Vector2D(center.X + Math.Cos(endAngle) * radius, center.Y + Math.Sin(endAngle) * radius);

            var startScreenPoint = ConvertVectorToScreenPoint(startPoint);
            var endScreenPoint = ConvertVectorToScreenPoint(endPoint);

            //求两边之叉积,由叉积的符号决定是顺时针和逆时针;
            var cross = Math.Cos(beginangle) * Math.Sin(endAngle) - Math.Sin(beginangle) * Math.Cos(endAngle);

            var screenRadius = _canvasProxy.ToScreen(radius);

            //因为数学坐标中，
            var arcGeometry = GetArcGeometry(
                startScreenPoint,
                endScreenPoint,
                screenRadius,
                smallAngle,
                cross <0 ? SystemMedia.SweepDirection.Clockwise: SystemMedia.SweepDirection.Counterclockwise
            );
            
            DrawingContext.DrawGeometry(
                null,
                PenAdapter.ConverterToSystemPen(pen),
                arcGeometry
            );
            
        }
        
        /// <summary>
        /// 得到圆弧的几何图形;
        /// </summary>
        /// <param name="center"></param>
        /// <param name="screenRadius"></param>
        /// <param name="beginAngle"></param>
        /// <param name="angle"></param>
        /// <param name="smallAngle"></param>
        /// <returns></returns>
        private static SystemMedia.PathGeometry GetArcGeometry(Point startScreenPoint,Point endScreenPoint, double  screenRadius ,bool smallAngle,SystemMedia.SweepDirection sweepDirection) {
            var arcSegment = new SystemMedia.ArcSegment(endScreenPoint, new System.Windows.Size(screenRadius, screenRadius), 0D, !smallAngle,sweepDirection,true);
            
            var segments = new SystemMedia.PathSegment[] { arcSegment };
            var pathFigure = new SystemMedia.PathFigure(startScreenPoint, segments, false);
            
            var figures = new SystemMedia.PathFigure[] { pathFigure };

            arcSegment.Freeze();
            pathFigure.Freeze();

            return new SystemMedia.PathGeometry(figures, SystemMedia.FillRule.EvenOdd, null);
        }

        /// <summary>
        /// 绘制(椭)圆;
        /// </summary>
        /// <param name="brush"></param>
        /// <param name="pen"></param>
        /// <param name="center"></param>
        /// <param name="radiusX"></param>
        /// <param name="radiusY"></param>
        public void DrawEllipse(Brush brush, Pen pen, Vector2D center, double radiusX, double radiusY) {
            ValidateDrawingContext();

            radiusX = _canvasProxy.ToScreen(radiusX);
            radiusY = _canvasProxy.ToScreen(radiusY);
            center = _canvasProxy.ToScreen(center);
            NativeDrawEllipse(brush, pen, center, radiusX, radiusY);
        }
        
        /// <summary>
        /// 绘制文字;
        /// </summary>
        /// <param name="text"></param>
        /// <param name="emSize"></param>
        /// <param name="foreground"></param>
        /// <param name="origin"></param>
        public void DrawText(string text, double emSize, Brush foreground, Vector2D origin ,double angle = 0) {
            ValidateDrawingContext();

            var originScreenPoint = _canvasProxy.ToScreen(origin);
            var nativeOriginScreenPoint = Vector2DAdapter.ConvertToSystemPoint(originScreenPoint);

            var ft = new SystemMedia.FormattedText(
                text,
                System.Globalization.CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight, TypeFace,
                emSize,
                BrushAdapter.ConvertToSystemBrush(foreground)
            );

            DrawFormattedTextCore(ft, origin, angle);
        }

        /// <summary>
        /// 通过点的集合获取三次贝赛尔曲线
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        private SystemMedia.PathGeometry GetCurveGeometry(IEnumerable<Vector2D> points)
        {
            if (points == null) {
                throw new ArgumentNullException(nameof(points));
            }
            
            var screenPoints = points.Select(x => ConvertVectorToScreenPoint(x)).ToArray();
            var bezier = new SystemMedia.PolyBezierSegment(screenPoints, true);
            var pathFigure = new SystemMedia.PathFigure();
            var pathGeometry = new SystemMedia.PathGeometry();

            pathFigure.Segments.Add(bezier);
            pathGeometry.Figures.Add(pathFigure);

            if(screenPoints.Length >= 1) {
                pathFigure.StartPoint = screenPoints[0];

                //因为此处使用的三次贝塞尔曲线要求点的数量为3的倍数,所以在未能正处情况下,重复最后一项至下一个三的倍数;
                var repeatCount = (3 - (screenPoints.Length % 3)) % 3;

                var lastScreenPoint = screenPoints[screenPoints.Length - 1];
                for (int i = 0; i < repeatCount; i++) {
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
        public void DrawCurve(Pen pen, IEnumerable<Vector2D> points) {
            if (pen == null) {
                throw new ArgumentNullException(nameof(pen));
            }

            ValidateDrawingContext();


            //使用一个变量存储上一次的视图点;
            Point? lastScreenPoint = null;
            var sysPen = PenAdapter.ConverterToSystemPen(pen);
            foreach (var point in points)
            {
                var thisScreenPoint = ConvertVectorToScreenPoint(point);
                if (lastScreenPoint != null)
                {
                    //绘制的是上一次的节点到这一次的节点;
                    DrawingContext.DrawLine(sysPen, lastScreenPoint.Value, thisScreenPoint);

                }
                lastScreenPoint = thisScreenPoint;
            }

            //var path = GetCurveGeometry(points);
            //DrawingContext.DrawGeometry(null, sysPen, path);
        }

        /// <summary>
        /// 从坐标节点转换为以当前画布视图为标准的屏幕(系统)节点;
        /// </summary>
        /// <returns></returns>
        private Point ConvertVectorToScreenPoint(Vector2D point) => Vector2DAdapter.ConvertToSystemPoint(_canvasProxy.ToScreen(point));

        /// <summary>
        /// 绘制矩形;
        /// </summary>
        /// <param name="brush">填充颜色</param>
        /// <param name="pen"></param>
        /// <param name="rectangle"></param>
        public void DrawRectangle(Rectangle2D2 rectangle,Brush brush, Pen pen) {
            if (rectangle == null) {
                throw new ArgumentNullException(nameof(rectangle));
            }
            
            ValidateDrawingContext();
            DrawFill(rectangle.GetVertexes(), brush, pen);
        }

        /// <summary>
        /// 根据所有的点，组成一个封闭区域，且可以填充，并且填充
        /// </summary>
        /// <param name="points">所有的顶点坐标</param>
        /// <param name="brush">区域颜色</param>
        private void DrawFill(IEnumerable<Vector2D> points,Brush brush,Pen pen) {
            
            if(points == null) {
                throw new ArgumentNullException(nameof(points));
            }
            if(pen == null) {
                throw new ArgumentNullException(nameof(pen));
            }

            ValidateDrawingContext();

            NativeDrawFill(
                points.Select(p => ConvertVectorToScreenPoint(p)),
                BrushAdapter.ConvertToSystemBrush(brush),
                PenAdapter.ConverterToSystemPen(pen)
            );
        }

        /// <summary>
        /// 直接根据视图位置,绘制WPF封闭区域;
        /// </summary>
        /// <param name="screenPoints"></param>
        /// <param name="brush"></param>
        /// <param name="pen"></param>
        private void NativeDrawFill(IEnumerable<Point> screenPoints,SystemMedia.Brush brush,SystemMedia.Pen pen) {
            if (screenPoints == null) {
                throw new ArgumentNullException(nameof(screenPoints));
            }
            if (pen == null) {
                throw new ArgumentNullException(nameof(pen));
            }

            ValidateDrawingContext();

            pen.Freeze();
            


            //操作SystemMedia.PathGeometry中的Figures以绘制(封闭)区域
            var paths = new SystemMedia.PathGeometry();

            var pfc = new SystemMedia.PathFigureCollection();
            var pf = new SystemMedia.PathFigure();
            pfc.Add(pf);

            //存储一个点表示当前的PathFigure的StartPoint是否被指定;
            var startPointSet = false;

            foreach (var p in screenPoints) {
                //若StartPoint未被设定(第一个节点),设定后继续下一次循环;
                if (!startPointSet) {
                    pf.StartPoint = p;
                    startPointSet = true;
                    continue;
                }

                //若若StartPoint被设定,则加入线段;
                var ps = new SystemMedia.LineSegment();
                ps.Point = p;
                pf.Segments.Add(ps);
            }


            pf.IsClosed = true;
            paths.Figures = pfc;
            DrawingContext.DrawGeometry(brush, pen, paths);
        }

        /// <summary>
        /// 以视图坐标为标准,绘制矩形;
        /// </summary>
        /// <param name="rectangle">以视图坐标为准的矩形</param>
        /// <param name="brush">填充色</param>
        /// <param name="pen">笔</param>
        public void NativeDrawRectangle(Rectangle2D2 rectangle, Brush brush, Pen pen) {
            if (rectangle == null) {
                throw new ArgumentNullException(nameof(rectangle));
            }
            
            ValidateDrawingContext();

            var vertexes = rectangle.GetVertexes();
            NativeDrawFill(
                vertexes.Select(p => Vector2DAdapter.ConvertToSystemPoint(p)),
                BrushAdapter.ConvertToSystemBrush(brush),
                PenAdapter.ConverterToSystemPen(pen)
            );
        }

        /// <summary>
        /// 以视图坐标为标准,绘制椭圆;
        /// </summary>
        /// <param name="rectangle">以视图坐标为准的矩形</param>
        /// <param name="brush">填充色</param>
        /// <param name="pen">笔</param>
        public void NativeDrawEllipse(Brush brush, Pen pen, Vector2D center, double radiusX, double radiusY) {
            if(center == null) {
                throw new ArgumentNullException(nameof(center));
            }

            var centerPoint = Vector2DAdapter.ConvertToSystemPoint(center);
            
            DrawingContext.DrawEllipse(
                BrushAdapter.ConvertToSystemBrush(brush),
                PenAdapter.ConverterToSystemPen(pen),
                centerPoint,
                radiusX,
                radiusY
            );
        }

        /// <summary>
        /// 以视图坐标为标准,绘制线段;
        /// </summary>
        /// <param name="rectangle">以视图坐标为准的矩形</param>
        /// <param name="brush">填充色</param>
        /// <param name="pen">笔</param>
        public void NativeDrawLine(Pen pen, Line2D line2D) {
            if(pen == null) {
                return;
            }

            if(line2D == null) {
                throw new ArgumentNullException(nameof(line2D));
            }

            ValidateDrawingContext();
            
            //平台转换后再进行绘制;
            DrawingContext.DrawLine(
                PenAdapter.ConverterToSystemPen(pen),
                Vector2DAdapter.ConvertToSystemPoint(line2D.Start),
                Vector2DAdapter.ConvertToSystemPoint(line2D.End)
            );
        }

        /// <summary>
        /// 推入不透明度效果;
        /// </summary>
        /// <param name="opacity">最大值为1D</param>
        public void PushOpacity(double opacity) {
            DrawingContext.PushOpacity(opacity);
        }

        /// <summary>
        /// 将上次的特效获取其他效果出栈;
        /// </summary>
        public void Pop() {
            DrawingContext.Pop();
        }

        public void DrawFormattedText(FormattedText formattedText, Vector2D origin, double angle = 0) {

            if (formattedText == null) {
                throw new ArgumentNullException(nameof(formattedText));
            }

            ValidateDrawingContext();
            var ft = FormattedTextAdapter.ConvertToSystemFormattedText(formattedText);
            DrawFormattedTextCore(ft, origin, angle);
        }

        private void DrawFormattedTextCore(SystemMedia.FormattedText formattedText,Vector2D origin,double angle = 0) {
            
            var originScreenPoint = _canvasProxy.ToScreen(origin);
            var nativeOriginScreenPoint = Vector2DAdapter.ConvertToSystemPoint(originScreenPoint);
            
            //若偏转角度数不为圆周整数整除,则应用偏转角变化;
            if (angle % Math.PI * 2 != 0) {
                
                var rotateTransform = new SystemMedia.RotateTransform {
                    
                    Angle = Extension.RadToDeg(angle),
                    
                    CenterX = nativeOriginScreenPoint.X,
                    CenterY = nativeOriginScreenPoint.Y
                };

                //推入;
                DrawingContext.PushTransform(rotateTransform);

                DrawingContext.DrawText(formattedText, nativeOriginScreenPoint);

                //出栈;
                DrawingContext.Pop();
            }
            else {
                DrawingContext.DrawText(formattedText, nativeOriginScreenPoint);
            }
        }
    }
}
