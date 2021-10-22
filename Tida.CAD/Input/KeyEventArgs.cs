using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Input {
    /// <summary>
    /// 按键事件参数;
    /// </summary>
    public abstract class KeyEventArgs:HandleableEventArgs {
        public KeyEventArgs(Key key) {
            this.Key = key;
        }

        /// <summary>
        /// 所按下的键;
        /// </summary>
        public Key Key { get; }
    }
}
