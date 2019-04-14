using Tida.Canvas.Infrastructure.DynamicInput;
using Tida.Canvas.Contracts;
using Tida.Canvas.Base.EditTools;
using Tida.Canvas.Shell.Contracts.DynamicInput;

namespace Tida.Canvas.Base.DynamicInput {
    /// <summary>
    /// 通过圆的直径的两个端点创建圆的编辑工具的动态输入处理器;
    /// </summary>
    class RoundDiameterTwoPointsEditDynamicInputer : LengthAndAngleDynamicInputer<RoundDiameterTwoPointsEditTool> {
        public RoundDiameterTwoPointsEditDynamicInputer(RoundDiameterTwoPointsEditTool roundDiameterTwoPointsEditTool, ICanvasControl canvasControl) : base(roundDiameterTwoPointsEditTool, canvasControl) {

        }
    }

    [ExportCanvasControlDynamicInputerProvider]
    class RoundDiameterTwoPointsEditDynamicInputerProvider : EditToolDynamicInputerProviderGenericBase<RoundDiameterTwoPointsEditTool> {
        protected override IDynamicInputer OnCreateInputer(ICanvasControl canvasControl,RoundDiameterTwoPointsEditTool editTool) {
            return new RoundDiameterTwoPointsEditDynamicInputer(editTool,canvasControl);
        }
    }
}
