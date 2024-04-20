using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Contracts {
    /// <summary>
    /// 可绘制对象协约;
    /// </summary>
    public interface IDrawable {
        /// <summary>
        /// 本身图像已经发生了变化的事件;
        /// </summary>
        event EventHandler VisualChanged;
        
        /// <summary>
        /// 绘制自身;
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="canvasProxy">视图与工程数学坐标的转换器</param>
        void Draw(ICanvas canvas,ICanvasScreenConvertable canvasProxy);
    }
}
