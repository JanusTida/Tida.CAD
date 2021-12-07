using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Tida.CAD
{
    /// <summary>
    /// A struct that describes a line in cad coordinates;
    /// </summary>
    public struct CADLine
    {
        public CADLine(Point start,Point end)
        {
            Start = start;
            End = end;
        }
        /// <summary>
        /// Start point;
        /// </summary>
        public Point Start { get; set; }

        /// <summary>
        /// End point;
        /// </summary>
        public Point End { get; set; }
    }
}
