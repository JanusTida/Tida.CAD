using System;

namespace Tida.CAD.Events
{
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
