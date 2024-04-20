using Tida.Canvas.Contracts;
using Tida.Canvas.Infrastructure.DynamicInput;
using Tida.Canvas.Infrastructure.DrawObjects;
using Tida.Canvas.Infrastructure.EditTools;
using Tida.Canvas.Shell.Contracts.DynamicInput;
using Tida.Canvas.Shell.Contracts.NativePresentation;

namespace Tida.Canvas.Shell.DynamicInput {
    /// <summary>
    /// 编辑连续线段工具所需动态输入处理器工厂;
    /// </summary>
    [ExportCanvasControlDynamicInputerProvider]
    public class MultiLineEditDynamicInputerProvider : EditToolDynamicInputerProviderGenericBase<MultiLineEditTool> {
        protected override IDynamicInputer OnCreateInputer(ICanvasControl canvasControl, MultiLineEditTool multiLineEditTool) {
            return new LengthAndAngleDynamicInputer<MultiLineEditTool>(multiLineEditTool, canvasControl, NumberBoxService.Current);
        }
    }
}
