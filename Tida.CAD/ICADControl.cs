using System;
using System.Collections.Generic;
using System.Windows;
using Tida.CAD.Events;

namespace Tida.CAD;


/// <summary>
/// ICADControl;
/// </summary>
public interface ICADControl :  
#if WPF
    IInputElement
#endif
{
    /// <summary>
    /// 是否被聚焦;          
    /// </summary>
    bool IsFocused { get; }
    
    /// <summary>
    /// 画布内绘制对象选定状态发生了变化事件;
    /// </summary>
    event EventHandler<DrawObjectSelectedChangedEventArgs> DrawObjectIsSelectedChanged;

    /// <summary>
    /// 绘制对象被移除事件;
    /// </summary>
    event EventHandler<DrawObjectsRemovedEventArgs> DrawObjectsRemoved;

    /// <summary>
    /// 绘制对象被添加事件;
    /// </summary>
    event EventHandler<DrawObjectsAddedEventArgs> DrawObjectsAdded;

    /// <summary>
    /// 所有图层(内容图层);
    /// </summary>
    IEnumerable<CADLayer> Layers { get; set; }

    /// <summary>
    /// 坐标间进行转化契约实例;
    /// </summary>
    ICADScreenConverter CADScreenConverter { get; }

    /// <summary>
    /// 当前选定的活动图层;
    /// </summary>
    CADLayer ActiveLayer { get; set; }

    /// <summary>
    /// 活动图层发生变化时的事件;
    /// </summary>
    event EventHandler<ValueChangedEventArgs<CADLayer>> ActiveLayerChanged;

    /// <summary>
    /// 放大比例;
    /// </summary>
    double Zoom { get; set; }

    /// <summary>
    /// 原点所在的视图坐标位置;
    /// </summary>
    Point PanScreenPosition { get; set; }

    /// <summary>
    /// 拖拽选择事件;
    /// </summary>
    event EventHandler<DragSelectEventArgs> DragSelect;

    /// <summary>
    /// 拖拽选择鼠标移动事件;
    /// </summary>
    event EventHandler<DragSelectMouseMoveEventArgs> DrawSelectMouseMove;

    /// <summary>
    /// 点击选取事件;
    /// </summary>
    event EventHandler<ClickSelectingEventArgs> ClickSelecting;


#if WPF
    /// <summary>
    /// Add an instance of <see cref="UIElement"/> to the control;
    /// </summary>
    /// <param name="child"></param>
    void AddUIElement(UIElement child);

    /// <summary>
    /// Remove a instance of <see cref="UIElement"/> from the control;
    /// </summary>
    /// <param name="child"></param>
    void RemoveUIElement(UIElement child);
#endif

}
