using System;
using System.Collections.Generic;

namespace Tida.CAD.Events;

/// <summary>
/// 绘制对象已被添加事件参数;
/// </summary>
public class DrawObjectsAddedEventArgs:EventArgs {
    public DrawObjectsAddedEventArgs(IEnumerable<DrawObject> drawObjects) {

        DrawObjects = drawObjects ?? throw new ArgumentNullException(nameof(drawObjects));

    }

    /// <summary>
    /// 对应的绘制对象;
    /// </summary>
    public IEnumerable<DrawObject> DrawObjects { get; }
}
