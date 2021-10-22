using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Tida.CAD {
    /// <summary>
    /// 感应坐标的辅助图形(通常跟鼠标位置有关);
    /// </summary>
    public interface ISnapShape:IDrawable {
        /// <summary>
        /// 所属绘制对象;
        /// </summary>
        //DrawObject Owner { get; }

        /// <summary>
        /// 辅助图形所在的标识位置,以工程数学坐标为准;
        /// </summary>
        Point Position { get; }


        /// <summary>
        /// 所处的有效矩形区域,改矩形区域将以视图坐标为准;
        /// </summary>
        /// <param name="canvasScreenConverter">视图——工程数学坐标转换器</param>
        Rect GetNativeBoundingRect(ICanvasScreenConverter canvasScreenConverter);
    }
}
