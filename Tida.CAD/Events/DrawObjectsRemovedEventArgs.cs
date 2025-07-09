using System;
using System.Collections.Generic;

namespace Tida.CAD.Events;

/// <summary>
/// 绘制对象被移除事件参数;
/// </summary>
public class DrawObjectsRemovedEventArgs : EventArgs {
    public DrawObjectsRemovedEventArgs(IEnumerable<DrawObject> drawObject) {

        DrawObjects = drawObject ?? throw new ArgumentNullException(nameof(drawObject));

    }

    /// <summary>
    /// 对应的绘制对象;
    /// </summary>
    public IEnumerable<DrawObject> DrawObjects { get; }
}
