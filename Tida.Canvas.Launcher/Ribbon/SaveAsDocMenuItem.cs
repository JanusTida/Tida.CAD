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
       GUID = Constants.MenuItemGUID_SaveAsDoc,
       HeaderLanguageKey = Constants.MenuItemName_SaveAs,
       Icon = Constants.MenuItemIcon_SaveAsDoc,
       OwnerGUID = Menu_CanvasShellRibbon,
       InputGestureText = "Ctrl + Shift + S",
       Key = Key.S,
       ModifierKeys = ModifierKeys.Control | ModifierKeys.Shift,
       Order = 12
   )]
    class SaveAsDocMenuItem : IMenuItem {
        public ICommand Command => GenericServiceStaticInstance<CanvasSerializingViewModel>.Current.SaveAsCommand;
    }
}
