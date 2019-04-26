using Tida.Canvas.Infrastructure.DrawObjects;
using Tida.Canvas.Infrastructure.DynamicInput;
using Tida.Canvas.Contracts;
using Tida.Canvas.Shell.Contracts.DynamicInput;
using Tida.Canvas.Shell.Contracts.NativePresentation;

namespace Tida.Canvas.Shell.DynamicInput {
    /// <summary>
    /// 某个点再被编辑时的动态输入处理器;
    /// </summary>
    [ExportCanvasControlDynamicInputerProvider]
    public class OnePointEditDynamicInputerProvider : OneEditingDrawObjectInputerProviderGenericBase<PointBase> {
        protected override IDynamicInputer OnCreateInputer(PointBase drawObject, ICanvasControl canvasControl) {
            return new LengthAndAngleDynamicInputer<PointBase>(
                drawObject,
                canvasControl,
                NumberBoxService.Current
            );
        }
    }
}
