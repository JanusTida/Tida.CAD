using Tida.Canvas.Base.EditTools;
using Tida.Canvas.Infrastructure.DynamicInput;
using Tida.Canvas.Contracts;
using Tida.Canvas.Shell.Contracts.DynamicInput;

namespace Tida.Canvas.Base.DynamicInput.Providers {
    /// <summary>
    /// 复制编辑工具的动态输入处理器;
    /// </summary>
    [ExportCanvasControlDynamicInputerProvider]
    class CopyEditToolDynamicInputerProvider : EditToolDynamicInputerProviderGenericBase<CopyEditTool> {
        protected override IDynamicInputer OnCreateInputer(ICanvasControl canvasControl, CopyEditTool editTool) {

            return new LengthAndAngleDynamicInputer<CopyEditTool>(editTool, canvasControl);
        }
    }
}
