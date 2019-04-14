using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Input {
    /// <summary>
    /// 按键拓展;
    /// </summary>
    public static class KeyExtensions {
        /// <summary>
        /// 尝试将某个按键的值尝试转化为数字;
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static byte? ConvertToByte(this Key key) {
            if(key >= Key.D0 && key <= Key.D9) {
                return (byte)(key - Key.D0);
            }
            else if(key >= Key.NumPad0 && key <= Key.NumPad9) {
                return (byte)(key - Key.NumPad0);
            }

            return null;
        }
    }
}
