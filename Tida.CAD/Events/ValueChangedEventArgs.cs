using System;

namespace Tida.CAD.Events {
    /// <summary>
    /// 值发生变化时的事件参数;
    /// </summary>
    public class ValueChangedEventArgs<TValue> : EventArgs {
        public ValueChangedEventArgs(TValue? newValue, TValue? oldValue) {
            this.NewValue = newValue;
            this.OldValue = oldValue;
        }

        /// <summary>
        /// 未更改前的值;
        /// </summary>
        public TValue? OldValue { get; }

        /// <summary>
        /// 更改后的值;
        /// </summary>
        public TValue? NewValue { get; }
    }
}
 