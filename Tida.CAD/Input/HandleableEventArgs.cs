using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Input {

    /// <summary>
    /// 可指示处理的事件参数;
    /// </summary>
    public abstract class HandleableEventArgs:EventArgs {

        /// <summary>
        /// 指示是否已经处理;
        /// </summary>
        public bool Handled { get; set; }
    }
}
