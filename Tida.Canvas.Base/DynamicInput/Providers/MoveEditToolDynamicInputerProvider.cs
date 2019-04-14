using Tida.Canvas.Base.EditTools;
using Tida.Canvas.Infrastructure.DynamicInput;
using Tida.Canvas.Contracts;
using Tida.Canvas.Shell.Contracts.DynamicInput;

namespace Tida.Canvas.Base.DynamicInput.Providers {
    /// <summary>
    /// 移动编辑工具的动态输入处理器;
    /// </summary>
    [ExportCanvasControlDynamicInputerProvider]
    class MoveEditToolDynamicInputerProvider : EditToolDynamicInputerProviderGenericBase<MoveEditTool>, ICanvasControlDynamicInputerProvider {
        protected override IDynamicInputer OnCreateInputer(ICanvasControl canvasControl,MoveEditTool editTool) {
            return new LengthAndAngleDynamicInputer<MoveEditTool>(editTool, canvasControl);
        }
    }
}
