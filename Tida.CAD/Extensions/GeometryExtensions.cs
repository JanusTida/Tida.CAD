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
    }
}
