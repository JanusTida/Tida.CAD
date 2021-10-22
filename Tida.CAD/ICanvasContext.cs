using Tida.Canvas.Events;
using Tida.Canvas.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Tida.CAD {
    /// <summary>
    /// 画布数据协约;
    /// </summary>
    public interface ICanvasContext {
        /// <summary>
        /// 所有图层(内容图层);
        /// </summary>
        IEnumerable<CanvasLayer> Layers { get; set; }

        /// <summary>
        /// 坐标间进行转化契约实例;
        /// </summary>
        ICanvasScreenConverter canvasScreenConverter { get; }

        /// <summary>
        /// 当前选定的活动图层;
        /// </summary>
        CanvasLayer ActiveLayer { get; set; }

        /// <summary>
        /// 活动图层发生变化时的事件;
        /// </summary>
        event EventHandler<ValueChangedEventArgs<CanvasLayer>> ActiveLayerChanged;

        /// <summary>
        /// 放大比例;
        /// </summary>
        double Zoom { get; set; }


        /// <summary>
        /// 上次编辑的标识位置;以工程数学坐标为准;
        /// </summary>
        Point LastEditPosition { get;  }


        /// <summary>
        /// 原点所在的视图坐标位置;
        /// </summary>
        Point PanScreenPosition { get; set; }

        /// <summary>
        /// 输入设备服务封装;
        /// </summary>
        IInputDevice InputDevice { get; }

        /// <summary>
        /// 拖拽选择事件;
        /// </summary>
        event EventHandler<DragSelectEventArgs> DragSelect;

        /// <summary>
        /// 拖拽选择鼠标移动事件;
        /// </summary>
        event EventHandler<DragSelectMouseMoveEventArgs> DrawSelectMouseMove;

        ///// <summary>
        ///// 当前的鼠标所在的工程数学坐标;
        ///// </summary>
        //Vector2D CurrentMousePosition { get; }

        ///// <summary>
        ///// 当前的鼠标所在的工程数学坐标发生变化事件;
        ///// </summary>
        //event EventHandler<ValueChangedEventArgs<Vector2D>> CurrentMousePositionChanged;

        /// <summary>
        /// 通知外部,将要针对指定的绘制对象集合,将要进行某种的类型输入交互的预处理事件;
        /// </summary>
        event EventHandler<PreviewDrawObjectsInteractionEventArgs> PreviewInteractionWithDrawObjects;

        /// <summary>
        /// 点击选取事件;
        /// </summary>
        event EventHandler<ClickSelectEventArgs> ClickSelect;
    }


}
