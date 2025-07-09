using System;
using System.ComponentModel;
using System.Windows;

namespace Tida.CAD.Events;

/// <summary>
/// 拖放选取事件参数;
/// </summary>
public class DragSelectEventArgs:CancelEventArgs {
    public DragSelectEventArgs(Point position,CADRect rect,DrawObject[] hitedDrawObjects) {
        Position = position;
        Rect = rect;
        HitedDrawObjects = hitedDrawObjects ?? throw new ArgumentNullException(nameof(hitedDrawObjects));
    }
    
    /// <summary>
    /// 鼠标的位置;
    /// </summary>
    public Point Position { get; }

    /// <summary>
    /// 拖拽区域形成的矩形;
    /// </summary>
    public CADRect Rect { get; }

    /// <summary>
    /// 被命中的绘制单元;
    /// </summary>
    public DrawObject[] HitedDrawObjects { get; }
    
}
