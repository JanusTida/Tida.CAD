using Tida.Geometry.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Input {
    /// <summary>
    /// 鼠标弹起事件;
    /// </summary>
    public class MouseUpEventArgs:MouseButtonEventArgs {
        /// <summary>
        /// 鼠标事件参数构造;
        /// </summary>
        /// <param name="button">指示哪个按键被弹起</param>
        /// <param name="position">指针所在的工程数学坐标</param>
        /// <param name="location">指针所在的视图位置</param>
        public MouseUpEventArgs(MouseButton button,Vector2D position) :  base(button,position) {
        }
        
    }
}
