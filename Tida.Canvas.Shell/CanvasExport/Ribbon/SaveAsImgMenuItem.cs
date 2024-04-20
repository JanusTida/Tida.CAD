using Tida.Canvas.Shell.Contracts.App;

using Tida.Canvas.Shell.Contracts.Menu;
using Tida.Canvas.Shell.Contracts.Canvas;
using Tida.Canvas.Shell.Contracts.CanvasExport;
using Tida.Canvas.Shell.Contracts.Ribbon;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static Tida.Canvas.Shell.CanvasExport.Constants;
using static Tida.Canvas.Shell.Contracts.Ribbon.Constants;
using Tida.Canvas.Shell.Contracts.Common;

namespace Tida.Canvas.Shell.CanvasExport.Ribbon {
    /// <summary>
    /// 保存为图片菜单命令;
    /// </summary>
    [ExportMenuItem(
       GUID = MenuItemSaveAsImg,
       HeaderLanguageKey = MenuItemName_SaveAsImg,
       Icon = MenuItemIcon_SaveAsDoc,
        OwnerGUID = Menu_CanvasShellRibbon,
       Order = 12
   )]
    class SaveAsImgMenuItem : IMenuItem {
        public ICommand Command => _saveAsImgCommand ??
            (_saveAsImgCommand = new DelegateCommand(SaveCanvasAsImg));


        private DelegateCommand _saveAsImgCommand;

        private static void SaveCanvasAsImg() {
            var path = DialogService.Current.GetSaveFilePath(SaveAsImg_DefaultFileName);
            if (string.IsNullOrEmpty(path)) {
                return;
            }

            try {
                using (var fs = File.Create(path)) {
                    CanvasService.CanvasDataContext.CommitEdit();

                    var drawObjects = CanvasService.CanvasDataContext.GetAllVisibleDrawObjects();
                    var setting = new ExportImgSetting(fs);
                    ServiceProvider.GetInstance<IExportCanvasAsImgService>().ExportDrawObjectsAsImg(drawObjects, setting);
                }

                if (MsgBoxService.Show(
                            LanguageService.FindResourceString(
                                MsgText_ConfirmToShowExportedDirectory
                            ),
                            MessageBoxButton.YesNo
                        ) == MessageBoxResult.Yes) {

                    LocalExplorerService.OpenFolderAndSelectFile(path);
                }
            }
            catch (Exception ex) {
                LoggerService.WriteException(ex);
                MsgBoxService.Show(ex.Message);
            }
        }
    }
}
