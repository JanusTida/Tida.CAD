using System.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Input {
    /// <summary>
    /// 鼠标按下事件参数;
    /// </summary>
    public class MouseDownEventArgs:MouseButtonEventArgs {
        /// <summary>
        /// 鼠标事件参数构造;
        /// </summary>
        /// <param name="button">指示哪个按键被按下</param>
        /// <param name="position">指针所在的工程数学坐标</param>
        /// <param name="location">指针所在的视图位置</param>
        public MouseDownEventArgs(MouseButton button, Point position) : 
            base(button,position) {
            
        }

        
    }
}
