using Tida.Canvas.Base.EditTools;
using Tida.Application.Contracts.App;
using Tida.Canvas.Base.EditTools;
using Tida.Canvas.Shell.Contracts.Canvas.Events;
using Tida.Canvas.Shell.Contracts.CommandOutput;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
