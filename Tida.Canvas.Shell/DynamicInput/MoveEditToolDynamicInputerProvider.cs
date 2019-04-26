using Tida.Canvas.Infrastructure.DynamicInput;
using Tida.Canvas.Contracts;
using Tida.Canvas.Infrastructure.EditTools;
using Tida.Canvas.Shell.Contracts.DynamicInput;
using Tida.Canvas.Shell.Contracts.NativePresentation;

namespace Tida.Canvas.Shell.DynamicInput {
    /// <summary>
    /// 移动编辑工具的动态输入处理器;
    /// </summary>
    [ExportCanvasControlDynamicInputerProvider]
    public class MoveEditToolDynamicInputerProvider : EditToolDynamicInputerProviderGenericBase<MoveEditTool>, ICanvasControlDynamicInputerProvider {
        protected override IDynamicInputer OnCreateInputer(ICanvasControl canvasControl,MoveEditTool editTool) {
            return new LengthAndAngleDynamicInputer<MoveEditTool>(editTool, canvasControl,NumberBoxService.Current);
        }
    }
}
