using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Tida.CAD {

    /// <summary>
    /// The converter between screen coordinates and CAD coordinates;
    /// </summary>
    public interface ICanvasScreenConverter {

        /// <summary>
        /// The actual width of the canvas in pixel;
        /// </summary>
        double ActualWidth { get; }

        /// <summary>
        /// The actual height of the canvas in pixel;
        /// </summary>
        double ActualHeight { get; }
        
        /// <summary>
        /// Convert the point in CAD coordinates into a point in rendering coordinates;
        /// </summary>
        /// <param name="cadPoint"></param>
        /// <returns></returns>
        Point ToScreen(Point cadPoint);

        /// <summary>
        /// Convert a length in cad coordinates into a length in pixel;
        /// </summary>
        /// <param name="unitvalue"></param>
        /// <returns></returns>
        double ToScreen(double unitvalue);

        /// <summary>
        /// Convert a length in pixel into a length in cad coordinates;
        /// </summary>
        /// <param name="pixelLength"></param>
        /// <returns></returns>
        double ToCAD(double pixelLength);

        /// <summary>
        /// 将视图坐标转化为工程数学坐标;
        /// </summary>
        /// <param name="screenPoint"></param>
        /// <returns></returns>
        Point ToCAD(Point screenPoint);


        /// <summary>
        /// The current zoom of the canvas;
        /// </summary>
        double Zoom { get;  }
    }

}
