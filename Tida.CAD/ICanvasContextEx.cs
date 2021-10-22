using System;
using System.Collections.Generic;
using Tida.CAD.Events;

namespace Tida.CAD
{

    /// <summary>
    /// 画布上下文升级契约,此契约在<see cref="ICanvasContext"/>上加入了辅助规则内容;
    /// </summary>
    public interface ICanvasContextEx : ICanvasContext {

        /// <summary>
        /// 辅助规则集合;
        /// </summary>
        IEnumerable<ISnapShapeRule> SnapShapeRules { get; }

        /// <summary>
        /// 鼠标所处的辅助节点发生变化时;
        /// </summary>
        event EventHandler<ValueChangedEventArgs<ISnapShape>> MouseHoverSnapShapeChanged;

        /// <summary>
        /// 辅助是否可用;
        /// </summary>
        bool IsSnapingEnabled { get; set; }

        /// <summary>
        /// 正在判断辅助事件;
        /// </summary>
        event EventHandler<SnapingEventArgs> Snaping;

        /// <summary>
        /// 添加原生对象;
        /// </summary>
        /// <param name="nativeVisual"></param>
        void AddUIObject(object nativeVisual);

        /// <summary>
        /// 移除原生对象;
        /// </summary>
        /// <param name="nativeVisual"></param>
        void RemoveUIObject(object nativeVisual);

        
    }
}
