using Tida.Canvas.Contracts;
using Tida.Canvas.Infrastructure.DynamicInput;
using Tida.Canvas.Base.DrawObjects;
using Tida.Canvas.Base.EditTools;
using Tida.Canvas.Infrastructure.DrawObjects;
using Tida.Canvas.Shell.Contracts.DynamicInput;

namespace Tida.Canvas.Base.DynamicInput.Providers {
    /// <summary>
    /// 编辑(单)线段工具所需动态输入处理器工厂;
    /// </summary>
    [ExportCanvasControlDynamicInputerProvider]
    class SingleLineEditDynamicInputerProvider : ICanvasControlDynamicInputerProvider {
        public IDynamicInputer CreateInputer(ICanvasControl canvasControl) {
            if(!(canvasControl.CurrentEditTool is SingleLineEditTool singleLineEditTool)) {
                return null;
            }
            
            return new LengthAndAngleDynamicInputer<SingleLineEditTool>(singleLineEditTool,canvasControl);
        }
    }

    /// <summary>
    /// 编辑连续线段工具所需动态输入处理器工厂;
    /// </summary>
    [ExportCanvasControlDynamicInputerProvider]
    class MultiLineEditDynamicInputerProvider : EditToolDynamicInputerProviderGenericBase<MultiLineEditTool> {
        protected override IDynamicInputer OnCreateInputer(ICanvasControl canvasControl, MultiLineEditTool multiLineEditTool) {
            return new LengthAndAngleDynamicInputer<MultiLineEditTool>(multiLineEditTool, canvasControl);
        }
    }

    /// <summary>
    /// 某个<see cref="LineBase"/>在编辑时的动态输入;
    /// </summary>
    [ExportCanvasControlDynamicInputerProvider]
    class OneLineEditDynamicInputerProvider : OneEditingDrawObjectInputerProviderGenericBase<Line> {
        protected override IDynamicInputer OnCreateInputer(Line line, ICanvasControl canvasControl) {
            if (line.MousePositionTracker.LastMouseDownPosition.IsAlmostEqualTo(line.Line2D.Start) ||
                line.MousePositionTracker.LastMouseDownPosition.IsAlmostEqualTo(line.Line2D.End)) {

                var haveMousePositionWrapper = new HaveMousePositionTrackerForLineBase(line);
                var inputer = new LengthAndAngleDynamicInputer<HaveMousePositionTrackerForLineBase>(haveMousePositionWrapper, canvasControl);
                inputer.Disposed += (sender, e) => haveMousePositionWrapper.Dispose();

                return inputer;
            }

            return new LengthAndAngleDynamicInputer<Line>(
                line,
                canvasControl
            );
        }
    }

}
