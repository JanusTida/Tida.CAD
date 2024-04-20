using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemInput = System.Windows.Input;

namespace Tida.Canvas.WPFCanvas.Input {
    static class MouseButtonStateAdapter {

        /// <summary>
        /// 从<see cref="SystemInput.MouseButtonState"/>转换至<see cref="Canvas.Input.MouseButtonState"/>
        /// </summary>
        /// <param name="sysMouseButtonState"></param>
        /// <returns></returns>
        public static Canvas.Input.MouseButtonState ConvertToMouseButtonState(this SystemInput.MouseButtonState sysMouseButtonState) {
            switch (sysMouseButtonState) {
                case SystemInput.MouseButtonState.Released:
                    return Canvas.Input.MouseButtonState.Released;
                case SystemInput.MouseButtonState.Pressed:
                    return Canvas.Input.MouseButtonState.Pressed;
                default:
                    break;
            }

            return Canvas.Input.MouseButtonState.Released;
        }
    }
}
