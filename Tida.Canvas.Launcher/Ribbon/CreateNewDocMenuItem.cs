using Tida.Canvas.Launcher.ViewModels;

using Tida.Canvas.Shell.Contracts.Menu;
using Tida.Canvas.Shell.Contracts.Canvas;
using Tida.Canvas.Shell.Contracts.Ribbon;
using Tida.Canvas.Shell.Contracts.Shell;
using Prism.Commands;
using System.Windows.Input;
using static Tida.Canvas.Shell.Contracts.Ribbon.Constants;
using Tida.Canvas.Shell.Contracts.Common;

namespace Tida.Canvas.Launcher.Ribbon {
    [ExportMenuItem(
        GUID = Constants.MenuItemGUID_CreateDoc,
        HeaderLanguageKey = Constants.MenuItemName_New,
        Icon = Constants.MenuItemIcon_CreateDoc,
        InputGestureText = "Ctrl + N",
        OwnerGUID = Menu_CanvasShellRibbon,
        Key = Key.N,
        ModifierKeys = ModifierKeys.Control,
        Order = 1
    )]
    class CreateNewDocMenuItem : IMenuItem {
        public ICommand Command => GenericServiceStaticInstance<CanvasSerializingViewModel>.Current.CreateNewCommand;
    }
}
