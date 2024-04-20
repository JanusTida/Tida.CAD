using Tida.Canvas.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Tida.Canvas.WPFCanvas.Input {
    /// <summary>
    /// 键入文字事件参数适配器;
    /// </summary>
    static class TextInputAdapter {
        /// <summary>
        /// 从WPF<see cref="TextCompositionEventArgs"/> 转化为<see cref="TextInputEventArgs"/>
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static TextInputEventArgs ConverterToTextInputEventArgs(TextCompositionEventArgs e) {
            return new TextInputEventArgs(e.Text,e.ControlText);
        }
    }
}
