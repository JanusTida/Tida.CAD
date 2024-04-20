
using Tida.Canvas.Contracts;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Tida.Canvas.Shell.Contracts.Common;

namespace Tida.Canvas.Shell.Contracts.Canvas.Events {
    /// <summary>
    /// 绘制对象集合正在被移除事件参数;
    /// </summary>
    public class CanvasDrawObjectsRemovingEventArgs : CancelEventArgs {
        public CanvasDrawObjectsRemovingEventArgs(ICollection<DrawObject> removingDrawObjects,ICanvasDataContext canvasDataContext) {
            RemovingDrawObjects = removingDrawObjects ?? throw new ArgumentNullException(nameof(removingDrawObjects));
            CanvasDataContext = canvasDataContext ?? throw new ArgumentNullException(nameof(canvasDataContext));
        }

        /// <summary>
        /// 即将被移除的绘制对象集合;
        /// </summary>
        public ICollection<DrawObject> RemovingDrawObjects { get; }

        /// <summary>
        /// 画布上下文;
        /// </summary>
        public ICanvasDataContext CanvasDataContext { get; }
    }

    public class CanvasDrawObjectsRemovingEvent : PubSubEvent<CanvasDrawObjectsRemovingEventArgs> {

    }

    public interface ICanvasDrawObjectsRemovingEventHandler : IEventHandler<CanvasDrawObjectsRemovingEventArgs> {

    }
}
