using Tida.Canvas.Shell.Contracts.Docking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Tida.Canvas.Shell.Contracts.MainPage.Constants;
using static Tida.Canvas.Shell.CommandOutput.Constants;
using System.ComponentModel.Composition;
using Tida.Canvas.Shell.Contracts.App;

namespace Tida.Canvas.Shell.CommandOutput {

    /// <summary>
    /// 命令输出Pane;
    /// </summary>
    [ExportDockingPane(CanUserClose =  false,CanFloat = true,InitPaneGroupGUID = DockingPaneGroup_BottomLeft,GUID = DockingPane_CommandOutput)]
    class CommandOutPutPane : DockingPaneBase {
        [ImportingConstructor]
        public CommandOutPutPane(Views.CommandOutput commandOutput) {
            _commandOutput = commandOutput;
            Header = LanguageService.FindResourceString(PaneHeader_CommandOutput);
        }

        private readonly Views.CommandOutput _commandOutput;

        public override object UIObject => _commandOutput;
    }
}
