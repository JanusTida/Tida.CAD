using System;

namespace Tida.Canvas.Shell.Contracts.Canvas.Events {
    /// <summary>
    /// 画布事件参数泛型基类;
    /// </summary>
    /// <typeparam name="TEventArgs">所包装的原生事件参数</typeparam>
    public abstract class CanvasEventArgs<TEventArgs>:EventArgs where TEventArgs:EventArgs{

        public CanvasEventArgs(ICanvasDataContext canvasDataContext,TEventArgs eventArgs) {

            CanvasDataContext = canvasDataContext ?? throw new ArgumentNullException(nameof(canvasDataContext));

            EventArgs = eventArgs ?? throw new ArgumentNullException(nameof(eventArgs));

        }

        /// <summary>
        /// 画布上下文;
        /// </summary>
        public ICanvasDataContext CanvasDataContext { get; }

        public TEventArgs EventArgs { get; }
    }
}
