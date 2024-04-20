using System.ComponentModel.Composition;
using Tida.Canvas.Shell.App.Dialogs;
using Tida.Canvas.Shell.Contracts.App;
using Tida.Canvas.Shell.Contracts.App.Input;
using Ookii.Dialogs.Wpf;

namespace Tida.Canvas.Shell.App {
    /// <summary>
    /// 对话框服务;
    /// </summary>
    [Export(typeof(IDialogService))]
    class DialogServiceImpl : IDialogService {
        public string GetSaveFilePath(string defaultFileName) {
            return GetSaveFilePath(defaultFileName,null);
        }

        public string GetSaveFilePath() {
            return GetSaveFilePath(null,null);
        }

        public string GetSaveFilePath(string defaultFileName, string filter) {
            var dialog = new VistaSaveFileDialog {
                FileName = defaultFileName
            };

            if (dialog.ShowDialog() == true) {
                return dialog.FileName;
            }
            return null;
        }


        public string OpenFile() {
            return OpenFile(string.Empty);
        }

        //public ILoadingDialog CreateLoadingDialog() {
        //    return new Dialogs.ProgressDialog();
        //}

        //public IDoubleLoadingDialog CreateDoubleLoadingDialog() => new DoubleProcessDialog();

        public string OpenFile(string filter) {
            var dialog = new VistaOpenFileDialog();
            if (dialog.ShowDialog() == true) {
                return dialog.FileName;
            }
            return null;
        }

        public string OpenDirect() {
            var dialog = new VistaFolderBrowserDialog();
            if (dialog.ShowDialog() == true) {
                return dialog.SelectedPath;
            }

            return string.Empty;
        }

        public string GetInputValue(GetInputValueSetting getInputValueSetting = null) {
            return InputValueDialog.Show(getInputValueSetting);

        }

        //public string GetInputValue(string title = "", string desc = "", string val = "") {
        //    return InputValueDialog.Show(title, desc, val);
        //}

        //public ISingleSelectDialog<TOption> CreateSingleSelectOptionDialog<TOption>(
        //    IEnumerable<TOption> options,
        //    Func<TOption, string> getText
        //) where TOption : class {
        //    var dialog = new SingleSelectDialog<TOption>(options, getText);
        //    return dialog;
        //}
    }
}
