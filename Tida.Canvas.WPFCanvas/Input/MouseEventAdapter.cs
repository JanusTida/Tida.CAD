using Tida.Canvas.Input;
using Tida.Geometry.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemInput = System.Windows.Input;

namespace Tida.Canvas.WPFCanvas.Input {
    /// <summary>
    /// 鼠标参数适配器;
    /// </summary>
    static class MouseEventAdapter {
        /// <summary>
        /// 从WPF鼠标按下事件参数转化为<see cref="MouseDownEventArgs"/>
        /// </summary>
        /// <param name="e"></param>
        /// <param name="position">指针所在的工程数学坐标</param>
        /// <param name="viewLocation">指针所在的视图位置</param>
        /// <returns></returns>
        public static MouseDownEventArgs ConvertToMouseDownEventArgs(SystemInput.MouseButtonEventArgs e,Vector2D position) {
            if(e.ButtonState != SystemInput.MouseButtonState.Pressed) {
                throw new ArgumentException($"{nameof(e.ButtonState)} is not equal to {nameof(MouseButtonState.Pressed)}.");
            }

            var mouseBtn = e.ChangedButton.ConvertToMouseButton();
            return new MouseDownEventArgs(mouseBtn, position);
        }

        /// <summary>
        /// 从WPF鼠标弹起事件参数转化为<see cref="MouseUpEventArgs"/>
        /// </summary>
        /// <param name="e"></param>
        /// <param name="position"></param>
        /// <param name="viewLocation"></param>
        /// <returns></returns>
        public static MouseUpEventArgs ConvertToMouseUpEventArgs(SystemInput.MouseButtonEventArgs e,Vector2D position) {
            if(e.ButtonState != SystemInput.MouseButtonState.Released) {
                throw new ArgumentException($"{nameof(e.ButtonState)} is not equal to {nameof(MouseButtonState.Released)}.");
            }

            var mouseBtn = e.ChangedButton.ConvertToMouseButton();
            return new MouseUpEventArgs(mouseBtn, position);
        }

        /// <summary>
        /// 从WPF鼠标移动事件参数转化为<see cref="MouseMoveEventArgs"/>
        /// </summary>
        /// <param name="e"></param>
        /// <param name="position"></param>
        /// <param name="viewLocation"></param>
        /// <returns></returns>
        public static MouseMoveEventArgs ConvertToMouseMoveEventArgs(Vector2D position) {
            return new MouseMoveEventArgs(position);
        }

    }
}
