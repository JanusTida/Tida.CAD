using System;

namespace Tida.CAD
{
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
        void Draw(ICanvas canvas);
    }
}
