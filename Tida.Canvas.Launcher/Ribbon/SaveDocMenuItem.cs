using Tida.Canvas.Launcher.ViewModels;

using Tida.Canvas.Shell.Contracts.Menu;
using Tida.Canvas.Shell.Contracts.Ribbon;
using System.Windows.Input;
using static Tida.Canvas.Shell.Contracts.Ribbon.Constants;
using Tida.Canvas.Shell.Contracts.Common;

namespace Tida.Canvas.Launcher.Ribbon {
    [ExportMenuItem(
            GUID = Constants.MenuItemGUID_SaveDoc,
            OwnerGUID = Menu_CanvasShellRibbon,
            HeaderLanguageKey = Constants.MenuItemName_Save,
            Icon = Constants.MenuItemIcon_SaveDoc,
            InputGestureText = "Ctrl + S",
            Key = Key.S,
            ModifierKeys = ModifierKeys.Control ,
            Order = 8
     )]
    class SaveDocMenuItem : IMenuItem {
        public ICommand Command => GenericServiceStaticInstance<CanvasSerializingViewModel>.Current.SaveCommand;
    }
}
