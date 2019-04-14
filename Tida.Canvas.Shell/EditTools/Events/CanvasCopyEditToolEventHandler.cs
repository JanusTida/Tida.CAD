using Tida.Canvas.Base.EditTools;
using Tida.Application.Contracts.App;
using Tida.Canvas.Shell.Contracts.Canvas.Events;
using Tida.Canvas.Shell.Contracts.CommandOutput;
using System.ComponentModel.Composition;

namespace Tida.Canvas.Shell.EditTools.Events {
    [Export(typeof(ICanvasEditToolChangedEventHandler))]
    class CopyEditToolEventHandler: CanvasEditToolChangedEventHandlerGenericBase<CopyEditTool> {
        public override int Sort => 128;

        protected override void HandleNewEditTool(CanvasEditToolChangedEventArgs args, CopyEditTool newEditTool) {
            CommandOutputService.WriteLine(LanguageService.FindResourceString(Constants.EditToolTip_BeginText_Copy));
        }

        protected override void HandleOldEditTool(CanvasEditToolChangedEventArgs args, CopyEditTool oldEditTool) {
            CommandOutputService.WriteLine(LanguageService.FindResourceString(Constants.EditToolTip_EndText_Copy));
        }
    }
    
}
