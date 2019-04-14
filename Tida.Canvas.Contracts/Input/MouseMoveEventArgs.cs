
using Tida.Geometry.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Tida.Canvas.Input {
    /// <summary>
    /// 鼠标移动事件;
    /// </summary>
    public class MouseMoveEventArgs:MouseEventArgs {
        /// <summary>
        /// 鼠标事件参数构造;
        /// </summary>
        /// <param name="button"></param>
        public MouseMoveEventArgs(Vector2D position) : 
            base(position) {

        }
    }
}
