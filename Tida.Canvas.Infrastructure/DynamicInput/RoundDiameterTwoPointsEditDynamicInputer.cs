using Tida.Canvas.Contracts;
using Tida.Canvas.Infrastructure.EditTools;
using Tida.Canvas.Infrastructure.NativePresentation;

namespace Tida.Canvas.Infrastructure.DynamicInput {
    /// <summary>
    /// 通过圆的直径的两个端点创建圆的编辑工具的动态输入处理器;
    /// </summary>
    class RoundDiameterTwoPointsEditDynamicInputer : LengthAndAngleDynamicInputer<RoundDiameterTwoPointsEditTool> {
        
        public RoundDiameterTwoPointsEditDynamicInputer(RoundDiameterTwoPointsEditTool roundDiameterTwoPointsEditTool, ICanvasControl canvasControl, INumberBoxService numberBoxService) : 
            base(roundDiameterTwoPointsEditTool, canvasControl,numberBoxService) {

        }
    }
    
}
