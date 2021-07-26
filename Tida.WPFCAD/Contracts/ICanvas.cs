using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Tida.WPFCAD {
    
    /// <summary>
    /// 画布协约,以工程数学坐标为准提供最基础的绘制方法;
    /// </summary>
    public interface ICanvas {
        /// <summary>
        /// 绘制线段;
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="pen"></param>
        /// <param name="point0"></param>
        /// <param name="point1"></param>
        void DrawLine(Pen pen,Point point0,Point point1);
        
        /// <summary>
        /// 绘制圆弧;
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="pen"></param>
        /// <param name="center"></param>
        /// <param name="radius">半径</param>
        /// <param name="beginangle">起始角度</param>
        /// <param name="angle">角度(逆时针方向)</param>
        void DrawArc(Pen pen, Point center, double radius, double beginangle, double angle,bool smallAngle);

        /// <summary>
        /// 绘制多边形;
        /// </summary>
        /// <param name="points">顶点</param>
        /// <param name="brush"></param>
        /// <param name="pen"></param>
        void DrawPolygon(IEnumerable<Point> points, Brush brush, Pen pen);

        /// <summary>
        /// 绘制圆;
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="brush"></param>
        /// <param name="point"></param>
        void DrawEllipse(Brush brush,Pen pen, Point center,double radiusX,double radiusY);

        /// <summary>
        /// 绘制文字;
        /// </summary>
        /// <param name="text">文字内容</param>
        /// <param name="brush">画刷颜色</param>
        /// <param name="origin">起点</param>
        /// <param name="angle">偏转角度角度,单位为弧度</param>
        void DrawText(FormattedText formattedText, Point origin, double angle = 0);

        /// <summary>
        /// 绘制矩形;
        /// </summary>
        /// <param name="brush"></param>
        /// <param name="pen"></param>
        /// <param name="rectangle"></param>
        void DrawRectangle(Rect rectangle, Pen pen, Brush brush);

        /// <summary>
        /// WPF绘制核心本体;
        /// </summary>
        DrawingContext DrawingContext { get; }
    }



  
}
