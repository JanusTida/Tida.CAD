using Tida.Canvas.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemInput = System.Windows.Input;

namespace Tida.Canvas.WPFCanvas.Input {
    /// <summary>
    /// 键盘事件参数适配器;
    /// </summary>
    static class KeyAdapter {
        /// <summary>
        /// 从WPF鼠标按下参数转化为鼠标按下参数;
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static KeyDownEventArgs ConvertToKeyDownEventArgs(SystemInput.KeyEventArgs e) {
            return new KeyDownEventArgs(ConvertToKey(e.Key));
        }

        public static KeyUpEventArgs ConvertToKeyUpEventArgs(SystemInput.KeyEventArgs e) {
            return new KeyUpEventArgs(ConvertToKey(e.Key));
        }

        /// <summary>
        /// 从WPF系统按键转化为按键;
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static Key ConvertToKey(SystemInput.Key e) {
            
            return (Key)((int)e);
        }

        /// <summary>
        /// 从按键转化为WPF系统按键;
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static SystemInput.Key ConvertToSystemKey(Key e) {
            return (SystemInput.Key)((int)e);
        }

        /// <summary>
        /// 从WPF系统修饰键转换为修饰键;
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static ModifierKeys ConvertToModifierKeys(SystemInput.ModifierKeys e) {
            return (ModifierKeys)((int)e);
        }

    }
}
