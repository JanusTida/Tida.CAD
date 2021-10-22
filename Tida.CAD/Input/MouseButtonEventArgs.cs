using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Tida.Canvas.Input {
    /// <summary>
    /// 鼠标按键事件;
    /// </summary>
    public abstract class MouseButtonEventArgs:MouseEventArgs {
        public MouseButtonEventArgs(MouseButton button, Point position) :base(position) {
            Button = button;
        }
        /// <summary>
        /// 应该关注的按键;
        /// </summary>
        public MouseButton Button { get; }
    }
}
