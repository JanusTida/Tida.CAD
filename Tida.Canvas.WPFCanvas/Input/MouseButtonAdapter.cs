using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemInput = System.Windows.Input;

namespace Tida.Canvas.WPFCanvas.Input {
    static class MouseButtonAdapter {

        /// <summary>
        /// 从<see cref="SystemInput.MouseButton"/>转换至<see cref="Canvas.Input.MouseButton"/>
        /// </summary>
        /// <param name="sysMouseButton"></param>
        /// <returns></returns>
        public static Canvas.Input.MouseButton ConvertToMouseButton(this SystemInput.MouseButton sysMouseButton) {
            switch (sysMouseButton) {
                case SystemInput.MouseButton.Left:
                    return Canvas.Input.MouseButton.Left;
                case SystemInput.MouseButton.Middle:
                    return Canvas.Input.MouseButton.Middle;
                case SystemInput.MouseButton.Right:
                    return Canvas.Input.MouseButton.Right;
                case SystemInput.MouseButton.XButton1:
                    return Canvas.Input.MouseButton.XButton1;
                case SystemInput.MouseButton.XButton2:
                    return Canvas.Input.MouseButton.XButton2;
                default:
                    return Canvas.Input.MouseButton.UnKnown;
            }

        }
    }
}
