using System;

namespace Tida.Canvas.Base.DynamicInput.Events {
    /// <summary>
    /// 数字被呈递事件参数;
    /// </summary>
    public class NumberCommitedEventArgs:EventArgs {
        public NumberCommitedEventArgs(double? number) {
            this.Number = number;
        }

        public double? Number { get; }
    }
}
