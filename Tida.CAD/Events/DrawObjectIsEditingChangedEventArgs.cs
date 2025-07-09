using System;

namespace Tida.CAD.Events;

/// <summary>
/// 绘制对象是否正在被编辑发生了变化事件参数;
/// </summary>
public class DrawObjectIsEditingChangedEventArgs:ValueChangedEventArgs<bool> {
    public DrawObjectIsEditingChangedEventArgs(DrawObject drawObject,bool newValue,bool oldValue):base(newValue,oldValue) {

        DrawObject = drawObject ?? throw new ArgumentNullException(nameof(drawObject));

    }
    /// <summary>
    /// 对应的绘制对象;
    /// </summary>
    public DrawObject DrawObject { get; }
}
