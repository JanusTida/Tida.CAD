using Tida.Canvas.Base.EditTools;
using Tida.Application.Contracts.App;
using Tida.Canvas.Shell.Contracts.Canvas.Events;
using Tida.Canvas.Shell.Contracts.CommandOutput;
using System.ComponentModel.Composition;

namespace Tida.Canvas.Shell.EditTools.Events {
    [Export(typeof(ICanvasEditToolChangedEventHandler))]
    class CanvasMoveEditToolEventHandler:CanvasEditToolChangedEventHandlerGenericBase<MoveEditTool> {

        /// <summary>
        /// 当当前工具变更为复制工具后,进行对应的UI响应(输出响应等);
        /// </summary>
        protected override void HandleNewEditTool(CanvasEditToolChangedEventArgs args, MoveEditTool newEditTool) {
            CommandOutputService.WriteLine(LanguageService.FindResourceString(Constants.EditToolTip_BeginText_Move));
        }

        protected override void HandleOldEditTool(CanvasEditToolChangedEventArgs args, MoveEditTool oldEditTool) {
            CommandOutputService.WriteLine(LanguageService.FindResourceString(Constants.EditToolTip_EndText_Move));
        }
        
    }
}
