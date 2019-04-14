using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.CanvasExport {
    static class Constants {
        private const string Prefix = "pack://application:,,,/Tida.Canvas.Shell;component/Resources/";

        public const string MenuItemSaveAsImg = nameof(MenuItemSaveAsImg);

        public const string MenuItemName_SaveAsImg = nameof(MenuItemName_SaveAsImg);

        public const string MenuItemIcon_SaveAsDoc = Prefix + "Save.png";

        public const string SaveAsImg_DefaultFileName = "capture.png";


        public const string MsgText_ConfirmToShowExportedDirectory = nameof(MsgText_ConfirmToShowExportedDirectory);

    }
}
