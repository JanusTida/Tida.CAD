using System.ComponentModel;

namespace Tida.CAD.Events;

/// <summary>
/// 即将更改值的事件参数;
/// </summary>
/// <typeparam name="TValue"></typeparam>
public class ValueChangingEventArgs<TValue> : CancelEventArgs {
    public ValueChangingEventArgs(TValue newValue, TValue oldValue) {
        this.NewValue = newValue;
        this.OldValue = oldValue;
    }

    /// <summary>
    /// 未更改前的值;
    /// </summary>
    public TValue OldValue { get; set; }

    /// <summary>
    /// 更改后的值;
    /// </summary>
    public TValue NewValue { get; set; }
}
