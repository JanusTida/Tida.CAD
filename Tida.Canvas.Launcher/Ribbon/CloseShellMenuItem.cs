using Tida.Canvas.Shell.Contracts.Menu;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static Tida.Canvas.Shell.Contracts.Ribbon.Constants;
using Tida.Canvas.Shell.Contracts.Shell;

namespace Tida.Canvas.Launcher.Ribbon {
    [ExportMenuItem(
        GUID = Constants.MenuItemGUID_CloseShell,
        HeaderLanguageKey = Constants.MenuItemName_CloseShell,
        OwnerGUID = Menu_CanvasShellRibbon,
        Order = 4096
    )]
    class CloseShellMenuItem : IMenuItem {
        public ICommand Command => _closeShellCommand = (_closeShellCommand = new DelegateCommand(CloseShell));
        private DelegateCommand _closeShellCommand;
        private void CloseShell() {
            ShellService.Current.Close();
            
        }
    }
}
