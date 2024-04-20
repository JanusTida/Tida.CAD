using Tida.Canvas.Media;
using Tida.Geometry.Primitives;
using System;
using System.Collections.Generic;

namespace Tida.Canvas.Contracts {
    
    /// <summary>
    /// 画布协约,以工程数学坐标为准提供最基础的绘制方法;
    /// </summary>
    public interface ICanvas {
        /// <summary>
        /// 绘制线段;
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="pen"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        void DrawLine(Pen pen, Line2D line);
        
        /// <summary>
        /// 绘制圆弧;
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="pen"></param>
        /// <param name="center"></param>
        /// <param name="radius">半径</param>
        /// <param name="beginangle">起始角度</param>
        /// <param name="angle">角度(逆时针方向)</param>
        void DrawArc(Pen pen, Vector2D center, double radius, double beginangle, double angle,bool smallAngle);

        /// <summary>
        /// 绘制多边形;
        /// </summary>
        /// <param name="points">顶点</param>
        /// <param name="brush"></param>
        /// <param name="pen"></param>
        void DrawPolygon(IEnumerable<Vector2D> points, Brush brush, Pen pen);

        /// <summary>
        /// 绘制圆;
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="brush"></param>
        /// <param name="point"></param>
        void DrawEllipse(Brush brush,Pen pen, Vector2D center,double radiusX,double radiusY);

        /// <summary>
        /// 绘制文字;
        /// </summary>
        /// <param name="text">文字内容</param>
        /// <param name="brush">画刷颜色</param>
        /// <param name="origin">起点</param>
        /// <param name="angle">偏转角度角度,单位为弧度</param>
        void DrawText(string text,double emSize,Brush foreground, Vector2D origin,double angle = 0);

        /// <summary>
        /// 更低级的文字绘制方法;
        /// </summary>
        /// <param name="formattedText"></param>
        /// <param name="origin"></param>
        /// <param name="angle">偏转角度角度，单位为弧度</param>
        void DrawFormattedText(FormattedText formattedText, Vector2D origin, double angle = 0);

        /// <summary>
        /// 绘制路径;
        /// </summary>
        /// <param name="pen"></param>
        /// <param name="points"></param>
        void DrawCurve(Pen pen, IEnumerable<Vector2D> points);

        /// <summary>
        /// 绘制矩形;
        /// </summary>
        /// <param name="brush"></param>
        /// <param name="pen"></param>
        /// <param name="rectangle"></param>
        void DrawRectangle(Rectangle2D2 rectangle,Brush brush, Pen pen);

        /// <summary>
        /// 以视图为标准坐标绘制线段;
        /// </summary>
        /// <param name="pen"></param>
        /// <param name="line2D"></param>
        void NativeDrawLine(Pen pen, Line2D line2D);

        /// <summary>
        /// 以视图坐标为标准,绘制矩形;
        /// </summary>
        /// <param name="rectangle">以视图坐标为准的矩形</param>
        /// <param name="brush">填充色</param>
        /// <param name="pen">笔</param>
        void NativeDrawRectangle(Rectangle2D2 rectangle, Brush brush, Pen pen);


        /// <summary>
        /// 以视图坐标为标准,绘制椭圆(圆);
        /// </summary>
        /// <param name="brush"></param>
        /// <param name="pen"></param>
        /// <param name="center"></param>
        /// <param name="radiusX"></param>
        /// <param name="radiusY"></param>
        void NativeDrawEllipse(Brush brush, Pen pen, Vector2D center, double radiusX, double radiusY);


        /// <summary>
        /// 推入透明度效果;
        /// </summary>
        /// <param name="opacity">最大值为1D</param>
        void PushOpacity(double opacity);

        /// <summary>
        /// 将上次的特效或者其他效果出栈;
        /// </summary>
        void Pop();

    }



    /// <summary>
    /// <see cref="ICanvas"/>拓展;
    /// </summary>
    public static class ICanvasExtensions {
        /// <summary>
        /// 以视图坐标为标准,绘制椭圆(圆);
        /// </summary>
        /// <param name="brush"></param>
        /// <param name="pen"></param>
        /// <param name="screenEllipse2D"></param>
        public static void NativeDrawEllipse(this ICanvas canvas,Brush brush, Pen pen, Ellipse2D screenEllipse2D) {

            if (canvas == null) {
                throw new ArgumentNullException(nameof(canvas));
            }


            if (screenEllipse2D == null) {
                throw new ArgumentNullException(nameof(screenEllipse2D));
            }

            canvas.NativeDrawEllipse(
               brush,
               pen,
               screenEllipse2D.Center,
               screenEllipse2D.RadiusX,
               screenEllipse2D.RadiusY
           );
        }

        /// <summary>
        /// 绘制椭圆(圆);
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="brush"></param>
        /// <param name="pen"></param>
        /// <param name="ellipse2D"></param>
        public static void DrawEllipse(this ICanvas canvas, Brush brush, Pen pen, Ellipse2D ellipse2D) {
            if (canvas == null) {
                throw new ArgumentNullException(nameof(canvas));
            }


            if (ellipse2D == null) {
                throw new ArgumentNullException(nameof(ellipse2D));
            }

            canvas.DrawEllipse(
               brush,
               pen,
               ellipse2D.Center,
               ellipse2D.RadiusX,
               ellipse2D.RadiusY
           );
        }
    }
}
