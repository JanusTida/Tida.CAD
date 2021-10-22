using System;
using System.Collections.Generic;
using System.Text;

namespace Tida.CAD
{
    /// <summary>
    /// Describes the width, height, and location of a rectangle.
    /// </summary>
    /// <remarks>
    /// Note that the y-axis is up forward,which is opposite in most rendering coordinate systems,
    /// the coordinate system which will be used here, is named "CAD Coordinates"
    /// </remarks>
    public struct CADRect
    {
        /// <summary>
        /// Gets or sets the x-axis value of the left side of the rectangle.
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Gets or sets the y-axis value of the bottom side of the rectangle.
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// Gets or sets the width of the rectangle.
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the rectangle.
        /// </summary>
        public double Height { get; set; }

        
    }
}
