using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Events {
    /// <summary>
    /// 可以撤销状态变更处理参数;
    /// </summary>
    public class CanRedoChangedEventArgs:EventArgs {
        public CanRedoChangedEventArgs(bool canRedo) {
            this.CanRedo = canRedo;
        }
        public bool CanRedo { get; }
    }
}
