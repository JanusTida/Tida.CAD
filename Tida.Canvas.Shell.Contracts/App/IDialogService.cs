using Tida.Canvas.Shell.Contracts.App.Input;
using Tida.Canvas.Shell.Contracts.Common;

namespace Tida.Canvas.Shell.Contracts.App {
    /// <summary>
    /// 基础对话框服务契约;
    /// </summary>
    public interface IDialogService {
        /// <summary>
        /// 获得文件路径;
        /// </summary>6
        /// <returns></returns>
        string OpenFile();

        /// <summary>
        /// 获得文件路径;
        /// </summary>
        /// <param name="filter">后缀过滤</param>
        /// <returns></returns>
        string OpenFile(string filter);

        /// <summary>
        /// 获得保存文件路径;
        /// </summary>
        /// <param name="defaultFileName">默认名称</param>
        /// <returns></returns>
        string GetSaveFilePath();

        string GetSaveFilePath(string defaultFileName);

        string GetSaveFilePath(string defaultFileName, string filter);

        /// <summary>
        /// 获得目录;
        /// </summary>
        /// <returns></returns>
        string OpenDirect();

        /// <summary>
        /// 单行输入框;
        /// </summary>
        /// <param name="getInputValueSetting">获取输入设定</param>
        /// <returns>输入值</returns>
        string GetInputValue(GetInputValueSetting getInputValueSetting = null);

        
    }

   

    public class DialogService:GenericServiceStaticInstance<IDialogService> {
        /// <summary>
        /// 获得保存文件路径;
        /// </summary>
        /// <returns></returns>
        public static string GetSaveFilePath(string defaultFileName = null) => Current?.GetSaveFilePath(defaultFileName);

        public static string OpenFile() => Current?.OpenFile();

        public static string OpenDirect() => Current?.OpenDirect();
    }

}
