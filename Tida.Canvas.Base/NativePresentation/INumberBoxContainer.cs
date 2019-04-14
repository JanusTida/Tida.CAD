using Tida.Canvas.Events;
using System;
using System.Collections.Generic;

namespace Tida.Canvas.Base.NativePresentation {
    /// <summary>
    /// 数字输入框容器;
    /// </summary>
    public interface INumberBoxContainer:IDisposable {
        /// <summary>
        /// UI元素;
        /// </summary>
        object UIObject { get; }

        /// <summary>
        /// 所有输入框;
        /// </summary>
        IReadOnlyList<INumberBox> NumberBoxes { get; }
        
        /// <summary>
        /// 是否在输入;
        /// </summary>
        bool IsInputing { get; }

        /// <summary>
        /// 是否正在输入发生了变化;
        /// </summary>
        event EventHandler<ValueChangedEventArgs<bool>> IsInputingChanged;

        /// <summary>
        /// 创建输入框;
        /// </summary>
        /// <param name="numberBox"></param>
        void AddNumberBox(INumberBox numberBox);

        /// <summary>
        /// 移除输入框;
        /// </summary>
        /// <param name="numberBox"></param>
        void RemoveNumberBox(INumberBox numberBox);

        /// <summary>
        /// 复位;
        /// </summary>
        void Reset();

        
    }
}
