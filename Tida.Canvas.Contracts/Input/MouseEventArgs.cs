using Tida.Geometry.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Tida.Canvas.Input {
    /// <summary>
    /// 鼠标事件参数;
    /// </summary>
    public abstract class MouseEventArgs : HandleableEventArgs {
        /// <summary>
        /// 鼠标事件参数构造;
        /// </summary>
        /// <param name="button">鼠标按键状态</param>
        /// <param name="position">指针所在的工程数学坐标</param>
        public MouseEventArgs(Vector2D position) {
            Position = position ?? throw new ArgumentNullException(nameof(position));
        }
        
        /// <summary>
        /// 指针所在的工程数学坐标;
        /// </summary>
        public Vector2D Position { get; }
    }
}
