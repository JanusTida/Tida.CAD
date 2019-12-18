using Tida.Canvas.Shell.Contracts.App;
using Tida.Canvas.Shell.Contracts.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.App {
    [Export(typeof(ILocalExplorerService))]
    public class LocalExplorerServiceImpl : ILocalExplorerService {
        /// <summary>
        /// 打开指定目录并定位到文件;
        /// </summary>
        /// <param name="fileFullName"></param>
        public void OpenFolderAndSelectFile(string fileFullName) {
            var psi = new ProcessStartInfo("explorer.exe") {
                Arguments = $"/e,/select,\"{Path.GetFullPath(fileFullName)}\"",
                UseShellExecute = false
            };
            try {
                Process.Start(psi);
            }
            catch (Exception ex) {
                LoggerService.WriteCallerLine(ex.Message);
            }
        }

        /// <summary>
        /// 打开指定目录;
        /// </summary>
        /// <param name="folderPath"></param>
        public void OpenFolder(string folderPath) {
            var psi = new ProcessStartInfo("explorer.exe") {
                Arguments = $"\"{folderPath}\"",
                UseShellExecute = false
            };
            try {
                Process.Start(psi);
            }
            catch (Exception ex) {
                LoggerService.WriteCallerLine(ex.Message);
            }
        }

        /// <summary>
        /// 打开指定路径下文件，比如：Word、Excel、Dll、图片等都可以（前提是你已经安装打开程序的对应软件）
        /// </summary>
        /// <param name="fileFullName">eg：D:\Test\模版8.doc</param>
        public void OpenFile(string fileFullName) {
            var psi = new ProcessStartInfo("Explorer.exe") {
                Arguments = $"\"{fileFullName}\""
            };
            try {
                Process.Start(psi);
            }
            catch (Exception ex) {
                LoggerService.WriteCallerLine(ex.Message);
            }
        }
    }
}
