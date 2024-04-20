using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Tida.CAD
{

    /// <summary>
    /// ICanvas interface describes the drawing method in unit coordinate system,
    /// </summary>
    /// <remarks>Note that the y-axis is up forward,which is opposite in most rendering coordinate systems,
    /// the coordinate system which will be used here, is named "CAD Coordinates"
    /// </remarks>
    public interface ICanvas {
        /// <summary>
        /// The converter between screen coordinates and CAD Coordinates;
        /// </summary>
        ICADScreenConverter CADScreenConverter { get; }

        /// <summary>
        ///  Draws a line between the specified points using the specified <see cref="Pen"/>
        //     and applies the specified animation clocks.
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="pen"></param>
        /// <param name="point0">The position of first </param>
        /// <param name="point1"></param>
        void DrawLine(Pen? pen, Point point0, Point point1);
        
        /// <summary>
        /// Draw an arc ;
        /// </summary>
        /// <param name="pen"></param>
        /// <param name="center"></param>
        /// <param name="radius">The radius of the arc</param>
        /// <param name="beginangle">The start angle of the arc,this angle starts from the 3 o'clock</param>
        /// <param name="angle">The angle in unclockwise,unit in rad</param>
        void DrawArc(Pen? pen, Point center, double radius, double beginangle, double angle);

        /// <summary>
        /// Draw a Polygon;
        /// </summary>
        /// <param name="points">The collection of vertexs of the polygon</param>
        /// <param name="brush"></param>
        /// <param name="pen"></param>
        void DrawPolygon(IEnumerable<Point> points, Brush? brush, Pen? pen);

        /// <summary>
        /// Draw an ellipse;
        /// </summary>
        /// <param name="brush"></param>
        /// <param name="pen"></param>
        /// <param name="center"></param>
        void DrawEllipse(Brush? brush,Pen? pen, Point center,double radiusX,double radiusY);

        /// <summary>
        /// Draw text;
        /// </summary>
        /// <param name="formattedText"></param>
        /// <param name="origin"></param>
        void DrawText(FormattedText? formattedText, Point origin);

        /// <summary>
        /// Draw a path;
        /// </summary>
        /// <param name="pen"></param>
        /// <param name="points">The path vertexs of the curve</param>
        void DrawCurve(Pen? pen, IEnumerable<Point> points);

        /// <summary>
        /// Draw a rectangle;
        /// </summary>
        /// <param name="brush"></param>
        /// <param name="pen"></param>
        /// <param name="cadRect"></param>
        void DrawRectangle(CADRect cadRect,Brush? brush, Pen? pen);


#if WPF
        DrawingContext DrawingContext { get; }
#endif
    }


}
