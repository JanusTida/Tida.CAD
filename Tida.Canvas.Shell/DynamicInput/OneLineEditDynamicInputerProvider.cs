using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tida.Canvas.Contracts;
using Tida.Canvas.Infrastructure.DrawObjects;
using Tida.Canvas.Infrastructure.DynamicInput;
using Tida.Canvas.Shell.Contracts.DynamicInput;
using Tida.Canvas.Shell.Contracts.NativePresentation;

namespace Tida.Canvas.Shell.DynamicInput {
    /// <summary>
    /// 某个<see cref="LineBase"/>在编辑时的动态输入;
    /// </summary>
    [ExportCanvasControlDynamicInputerProvider]
    public class OneLineEditDynamicInputerProvider : OneEditingDrawObjectInputerProviderGenericBase<LineBase> {
        protected override IDynamicInputer OnCreateInputer(LineBase line, ICanvasControl canvasControl) {
            if (line.MousePositionTracker.LastMouseDownPosition.IsAlmostEqualTo(line.Line2D.Start) ||
                line.MousePositionTracker.LastMouseDownPosition.IsAlmostEqualTo(line.Line2D.End)) {

                var haveMousePositionWrapper = new HaveMousePositionTrackerForLineBase(line);
                var inputer = new LengthAndAngleDynamicInputer<HaveMousePositionTrackerForLineBase>(haveMousePositionWrapper, canvasControl, NumberBoxService.Current);
                inputer.Disposed += (sender, e) => haveMousePositionWrapper.Dispose();

                return inputer;
            }

            return new LengthAndAngleDynamicInputer<LineBase>(
                line,
                canvasControl,
                NumberBoxService.Current
            );
        }
    }

}
