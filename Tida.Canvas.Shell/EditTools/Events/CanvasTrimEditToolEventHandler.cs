using Tida.Application.Contracts.App;
using Tida.Canvas.Base.EditTools;
using Tida.Canvas.Shell.Contracts.Canvas.Events;
using Tida.Canvas.Shell.Contracts.CommandOutput;
using System.ComponentModel.Composition;
using Tida.Canvas.Base.EditTools;

namespace Tida.Canvas.Shell.EditTools.Events {
    [Export(typeof(ICanvasEditToolChangedEventHandler))]
    class CanvasTrimEditToolEventHandler : CanvasEditToolChangedEventHandlerGenericBase<TrimEditTool> {
        /// <summary>
        /// 当当前工具变更为裁剪工具后,进行对应的UI响应(输出响应等);
        /// </summary>
        protected override void HandleNewEditTool(CanvasEditToolChangedEventArgs args, TrimEditTool newEditTool) {
            CommandOutputService.WriteLine(LanguageService.FindResourceString(Constants.EditToolTip_BeginText_Trim));
        }

        protected override void HandleOldEditTool(CanvasEditToolChangedEventArgs args, TrimEditTool oldEditTool) {
            CommandOutputService.WriteLine(LanguageService.FindResourceString(Constants.EditToolTip_EndText_Trim));
        }

        
    }

    
}
