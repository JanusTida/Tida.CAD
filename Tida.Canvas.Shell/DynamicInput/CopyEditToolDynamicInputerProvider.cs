using Tida.Canvas.Infrastructure.DynamicInput;
using Tida.Canvas.Contracts;
using Tida.Canvas.Shell.Contracts.DynamicInput;
using Tida.Canvas.Infrastructure.EditTools;
using Tida.Canvas.Shell.Contracts.NativePresentation;
using System.ComponentModel.Composition;

namespace Tida.Canvas.Shell.DynamicInput {
    /// <summary>
    /// 复制编辑工具的动态输入处理器;
    /// </summary>
    [ExportCanvasControlDynamicInputerProvider]
    public class CopyEditToolDynamicInputerProvider : EditToolDynamicInputerProviderGenericBase<CopyEditTool> {
        protected override IDynamicInputer OnCreateInputer(ICanvasControl canvasControl, CopyEditTool editTool) {

            return new LengthAndAngleDynamicInputer<CopyEditTool>(editTool, canvasControl, NumberBoxService.Current);
        }
    }
}
