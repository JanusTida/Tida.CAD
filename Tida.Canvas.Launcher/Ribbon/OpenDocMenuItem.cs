using Tida.Canvas.Launcher.ViewModels;

using Tida.Canvas.Shell.Contracts.Menu;
using Tida.Canvas.Shell.Contracts.Ribbon;
using System;
using System.ComponentModel.Composition;
using System.Windows.Input;
using static Tida.Canvas.Shell.Contracts.Ribbon.Constants;
using Tida.Canvas.Shell.Contracts.Common;

namespace Tida.Canvas.Launcher.Ribbon {
    [ExportMenuItem(
        GUID = Constants.MenuItemGUID_OpenDoc,
        HeaderLanguageKey = Constants.MenuItemName_OpenDoc,
        Icon = Constants.MenuItemIcon_OpenDoc,
        InputGestureText = "Ctrl + 0",
        OwnerGUID = Menu_CanvasShellRibbon,
        Key = Key.O,
        ModifierKeys = ModifierKeys.Control,
        Order = 4
    )]
    class OpenDocMenuItem : IMenuItem {
        public ICommand Command => GenericServiceStaticInstance<CanvasSerializingViewModel>.Current.OpenDocCommand;
        
    }
}
