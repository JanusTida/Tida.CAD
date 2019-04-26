using Tida.Application.Contracts.App;
using Tida.Canvas.Shell.Contracts.Canvas.Events;
using Tida.Canvas.Shell.Contracts.CommandOutput;
using System;
using System.ComponentModel.Composition;
using static Tida.Canvas.Shell.EditTools.Constants;
using Tida.Canvas.Infrastructure.EditTools;
using Tida.Geometry.Primitives;

namespace Tida.Canvas.Shell.EditTools.Events {
    [Export(typeof(ICanvasEditToolChangedEventHandler))]
    class CanvasLengthMeasureEditToolEventHandler : CanvasEditToolChangedEventHandlerGenericBase<LengthMeasureEditTool> {
        protected override void HandleNewEditTool(CanvasEditToolChangedEventArgs args, LengthMeasureEditTool newEditTool) {
            CommandOutputService.WriteLine(LanguageService.FindResourceString(Constants.EditToolTip_BeginText_LengthMeasure));
        }

        protected override void HandleOldEditTool(CanvasEditToolChangedEventArgs args, LengthMeasureEditTool oldEditTool) {
            CommandOutputService.WriteLine(LanguageService.FindResourceString(Constants.EditToolTip_EndText_LengthMeasure));
        }
    }

    [Export(typeof(ICanvasEditToolChangedEventHandler))]
    class CanvasAngleMeasureEditToolEventHandler : CanvasEditToolChangedEventHandlerGenericBase<AngleMeasureEditTool> {
        protected override void HandleNewEditTool(CanvasEditToolChangedEventArgs args, AngleMeasureEditTool newEditTool) {
            if(newEditTool != null) {
                newEditTool.FirstMouseDownPositionConfirmed += NewEditTool_FirstMouseDownPositionConfirmed;
                newEditTool.SecondMouseDownPositionConfirmed += NewEditTool_SecondMouseDownPositionConfirmed;
                newEditTool.AngleCreated += NewEditTool_AngleCreated;
            }
            CommandOutputService.WriteLine(LanguageService.FindResourceString(EditToolTip_BeginText_AngleMeasure));
            CommandOutputService.WriteLine(LanguageService.FindResourceString(EditToolTip_AngleMeasure_ConfirmFirstMouseDownPosition));
        }

        private void NewEditTool_AngleCreated(object sender, EventArgs e) {
            CommandOutputService.WriteLine(LanguageService.FindResourceString(EditToolTip_AngleMeasure_AngleCreated));
            CommandOutputService.WriteLine(LanguageService.FindResourceString(EditToolTip_AngleMeasure_ConfirmFirstMouseDownPosition));
        }

        private void NewEditTool_SecondMouseDownPositionConfirmed(object sender, Vector2D e) {
            CommandOutputService.WriteLine(LanguageService.FindResourceString(EditToolTip_AngleMeasure_SecondMouseDownPositionConfirmed));
        }

        private void NewEditTool_FirstMouseDownPositionConfirmed(object sender, Vector2D e) {
            CommandOutputService.WriteLine(LanguageService.FindResourceString(EditToolTip_AngleMeasure_FirstMouseDownPositionConfirmed));
        }

        protected override void HandleOldEditTool(CanvasEditToolChangedEventArgs args, AngleMeasureEditTool oldEditTool) {
            if(oldEditTool != null) {
                oldEditTool.FirstMouseDownPositionConfirmed -= NewEditTool_FirstMouseDownPositionConfirmed;
                oldEditTool.SecondMouseDownPositionConfirmed -= NewEditTool_SecondMouseDownPositionConfirmed;
                oldEditTool.AngleCreated -= NewEditTool_AngleCreated;
            }
            CommandOutputService.WriteLine(LanguageService.FindResourceString(EditToolTip_EndText_AngleMeasure));
        }
    }
}
