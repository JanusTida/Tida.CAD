using System;

namespace Tida.CAD.Events
{
    /// <summary>
    /// 可以撤销状态变更处理参数;
    /// </summary>
    public class CanUndoChangedEventArgs:EventArgs {
        public CanUndoChangedEventArgs(bool canUndo) {
            this.CanUndo = canUndo;
        }
        public bool CanUndo { get; }
    }
}
