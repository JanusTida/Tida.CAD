using Tida.Canvas.Shell.Contracts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.Contracts.App {
    /// <summary>
    /// 本地资源管理器服务契约;
    /// </summary>
    public interface ILocalExplorerService {
        /// <summary>
        /// 打开指定目录并定位到文件;
        /// </summary>
        /// <param name="fileFullName"></param>
        void OpenFolderAndSelectFile(string fileFullName);
        /// <summary>
        /// 打开指定目录;
        /// </summary>
        /// <param name="folderPath"></param>
        void OpenFolder(string folderPath);
        /// <summary>
        /// 打开指定路径下文件，比如：Word、Excel、Dll、图片等都可以（前提是你已经安装打开程序的对应软件）
        /// </summary>
        /// <param name="NewFileName">eg：D:\Test\模版8.doc</param>
        /// <param name="NewFileName">eg：D:\Test\模版8.doc</param>
        void OpenFile(string fileFullName);
    }

    public class LocalExplorerService : GenericServiceStaticInstance<ILocalExplorerService> {
        public static void OpenFolderAndSelectFile(string fileFullName) => Current?.OpenFolderAndSelectFile(fileFullName);
        public static void OpenFile(string fileFullName) => Current?.OpenFile(fileFullName);

        public static void OpenFolder(string folderPath) => Current?.OpenFolder(folderPath);
    }
}
