using Tida.Canvas.Contracts;
using System;

namespace Tida.Canvas.Infrastructure.DynamicInput {
    /// <summary>
    /// 动态输入处理器基类;本类在<see cref="DynamicInputerBase"/>上,增加了<see cref="ICanvasControl"/>
    /// </summary>
    public class CanvasControlDynamicInputerBase:DynamicInputerBase {
        public CanvasControlDynamicInputerBase(ICanvasControl canvasControl) {

            CanvasControl = canvasControl ?? throw new ArgumentNullException(nameof(canvasControl));

        }

        public ICanvasControl CanvasControl { get; }
    }
}
