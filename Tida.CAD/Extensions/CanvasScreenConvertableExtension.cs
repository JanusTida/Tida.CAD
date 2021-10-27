using System;
using System.Windows;

namespace Tida.CAD.Extensions
{

    /// <summary>
    /// Some extended methods for <see cref="ICADScreenConverter"/>;
    /// </summary>
    public static class CanvasScreenConvertableExtension
    {
        /// <summary>
        /// Get the right bottom point of the screen in CAD coordinates;
        /// </summary>
        /// <param name="canvasScreenConverter"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static Point GetBottomRightCADPoint(this ICADScreenConverter canvasScreenConverter)
        {

            if (canvasScreenConverter == null)
            {
                throw new ArgumentNullException(nameof(canvasScreenConverter));
            }

            return canvasScreenConverter.ToCAD(new Point(canvasScreenConverter.ActualWidth, canvasScreenConverter.ActualHeight));
        }

        /// <summary>
        /// Get the top left point of the screen in CAD coordinates;
        /// </summary>
        /// <param name="canvasScreenConverter"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static Point GetTopLeftCADPoint(this ICADScreenConverter canvasScreenConverter)
        {
            if (canvasScreenConverter == null)
            {
                throw new ArgumentNullException(nameof(canvasScreenConverter));
            }

            return canvasScreenConverter.ToCAD(new Point(0, 0));
        }


    }
}
