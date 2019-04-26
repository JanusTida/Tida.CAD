using Tida.Canvas.Contracts;
using Tida.Canvas.Infrastructure.DynamicInput;
using Tida.Canvas.Infrastructure.DrawObjects;
using Tida.Canvas.Infrastructure.EditTools;
using Tida.Canvas.Shell.Contracts.DynamicInput;
using Tida.Canvas.Shell.Contracts.NativePresentation;

namespace Tida.Canvas.Shell.DynamicInput {
    /// <summary>
    /// 编辑(单)线段工具所需动态输入处理器工厂;
    /// </summary>
    [ExportCanvasControlDynamicInputerProvider]
    public class SingleLineEditDynamicInputerProvider : ICanvasControlDynamicInputerProvider {
        public IDynamicInputer CreateInputer(ICanvasControl canvasControl) {
            if(!(canvasControl.CurrentEditTool is SingleLineEditTool singleLineEditTool)) {
                return null;
            }
            
            return new LengthAndAngleDynamicInputer<SingleLineEditTool>(singleLineEditTool,canvasControl,NumberBoxService.Current);
        }
    }

    

    
}
