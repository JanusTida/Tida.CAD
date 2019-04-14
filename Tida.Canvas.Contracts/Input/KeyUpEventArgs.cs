using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Input {
    /// <summary>
    /// 按键弹起事件;
    /// </summary>
    public class KeyUpEventArgs:KeyEventArgs {
        public KeyUpEventArgs(Key key):base(key) {

        }
    }
}
