using Tida.Canvas.Infrastructure.DrawObjects;
using Tida.Canvas.Infrastructure.DynamicInput;
using Tida.Canvas.Contracts;
using Tida.Canvas.Shell.Contracts.DynamicInput;

namespace Tida.Canvas.Base.DynamicInput.Providers {
    /// <summary>
    /// 某个点再被编辑时的动态输入处理器;
    /// </summary>
    [ExportCanvasControlDynamicInputerProvider]
    class OnePointEditDynamicInputerProvider : OneEditingDrawObjectInputerProviderGenericBase<PointBase> {
        protected override IDynamicInputer OnCreateInputer(PointBase drawObject, ICanvasControl canvasControl) {
            return new LengthAndAngleDynamicInputer<PointBase>(
                drawObject,
                canvasControl
            );
        }
    }
}
