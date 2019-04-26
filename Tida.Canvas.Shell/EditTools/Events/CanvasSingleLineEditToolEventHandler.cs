using Tida.Canvas.Infrastructure.EditTools;
using Tida.Canvas.Shell.Contracts.Canvas.Events;
using Tida.Canvas.Shell.Contracts.CommandOutput;
using System.ComponentModel.Composition;
using Tida.Application.Contracts.App;

namespace Tida.Canvas.Shell.EditTools.Events {
    [Export(typeof(ICanvasEditToolChangedEventHandler))]
     class CanvasSingleLineEditToolEventHandler:CanvasEditToolChangedEventHandlerGenericBase<SingleLineEditTool> {

        /// <summary>
        /// 当当前工具变更为复制工具后,进行对应的UI响应(输出响应等);
        /// </summary>
        protected override void HandleNewEditTool(CanvasEditToolChangedEventArgs args, SingleLineEditTool newEditTool) {
            CommandOutputService.WriteLine(LanguageService.FindResourceString(Constants.EditToolTip_BeginText_Line));
        }

        protected override void HandleOldEditTool(CanvasEditToolChangedEventArgs args, SingleLineEditTool oldEditTool) {
            CommandOutputService.WriteLine(LanguageService.FindResourceString(Constants.EditToolTip_EndText_Line));
        }
    }
}
