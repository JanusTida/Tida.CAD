using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Tida.CAD.Events
{
    /// <summary>
    /// 通知外部,将要针对指定的绘制对象集合,将要进行某种的类型输入交互的预处理事件的参数;
    /// </summary>
    /// <typeparam name="TEventArgs"></typeparam>
    public class PreviewDrawObjectsInteractionEventArgs : CancelEventArgs {
        public PreviewDrawObjectsInteractionEventArgs(IEnumerable<DrawObject> drawObjects) {
            DrawObjects = drawObjects ?? throw new ArgumentNullException(nameof(drawObjects));
        }

       
        /// <summary>
        /// 将要与之交互的绘制对象集合;
        /// </summary>
        public IEnumerable<DrawObject> DrawObjects { get; }
    }

    ///// <summary>
    ///// 通知绘制对象集合,将要进行某种类型输入交互的预处理事件参数;
    ///// </summary>
    ///// <typeparam name="TEventArgs"></typeparam>
    //public abstract class DrawObjectsInteractionEventArgs<TEventArgs> : CancelEventArgs where TEventArgs : EventArgs {
    //    public DrawObjectsInteractionEventArgs(TEventArgs eventArgs, IEnumerable<DrawObject> drawObjects) {

    //        EventArgs = EventArgs ?? throw new ArgumentNullException(nameof(EventArgs));

    //        DrawObjects = drawObjects ?? throw new ArgumentNullException(nameof(drawObjects));

    //    }

    //    /// <summary>
    //    /// 将要用于交互的事件参数;
    //    /// </summary>
    //    public TEventArgs EventArgs { get; }

    //    /// <summary>
    //    /// 将要与之交互的绘制对象集合;
    //    /// </summary>
    //    public IEnumerable<DrawObject> DrawObjects { get; }
    //}
}
