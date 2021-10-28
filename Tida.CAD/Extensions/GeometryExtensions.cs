using System;
using System.Windows;

namespace Tida.CAD.Extensions
{
    /// <summary>
    /// Some extented methods for geometry;
    /// </summary>
    public static class GeometryExtensions
    {
        public const double PI_DEG = 180;
        public const double SMALL_NUMBER = 1e-5;
        /// <summary>
        /// Convert angle in rad into angle in degree;
        /// </summary>
        /// <param name="rad">angle in rad</param>
        /// <returns></returns>
        public static double ConvertRadToDeg(double rad)
        {
            return (PI_DEG / Math.PI) * rad;
        }

        /// <summary>
        /// Get the center position of the rect;
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public static Point GetCenter(this Rect rect)
        {
            var centerX = rect.X + rect.Width / 2;
            var centerY = rect.Y - rect.Height / 2;
            return new Point(centerX, centerY);
        }

        private static double GetLengthSquared(this Vector vector) => vector.X * vector.X + vector.Y * vector.Y;

        /// <summary>
        /// Get the intersect point of two lines if it exists, return null when it doesn't exist;
        /// </summary>
        /// <param name="isSegement"></param>
        /// <param name="espilon"></param>
        /// <returns></returns>
        public static Point? GetIntersectPoint(Point point1,Point point2,Point point3,Point point4,bool isSegement = true, double espilon = SMALL_NUMBER)
        {
            espilon = espilon < 0 ? SMALL_NUMBER : espilon;
            var rxs = Vector.CrossProduct(point2 - point1,point4 - point3);
            if (Math.Abs(rxs) < espilon) return null;
            var r = Vector.CrossProduct(point3 - point1,point4 - point3) / rxs;
            var point = Evaluate(point1,point2,r);
            if (!isSegement) return point;
            var t = ClosestParameter(point1,point2,point);
            var u = ClosestParameter(point3,point4,point);
            var isOnline = t >= -espilon && t <= 1 + espilon && u >= -espilon && u <= 1 + espilon;
            if (isOnline) return point;
            return null;
        }

        private static double ClosestParameter(Point point1,Point point2,Point testPoint)
        {
            var v = point2 - point1;
            var ls = v.GetLengthSquared();
            var v1 = testPoint - point1;
            var v2 = testPoint - point2;
            var result = 0.0d;
            if (ls > 0)
            {
                if (v2.GetLengthSquared() <= v2.GetLengthSquared())
                    result = v1 * v / ls;
                else result = 1 + v2 * v / ls;
            }
            return result;
        }

        /// <summary>
        /// Get a point in a line (with a specified ratio);
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static Point Evaluate(Point start,Point end,double ratio)
        {
            return (end - start) * ratio + start;
        }
    }
}
