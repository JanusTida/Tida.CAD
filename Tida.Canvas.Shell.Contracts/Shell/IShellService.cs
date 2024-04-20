using System.Windows.Input;
using Tida.Canvas.Shell.Contracts.Common;
using Tida.Canvas.Shell.Contracts.Controls;

namespace Tida.Canvas.Shell.Contracts.Shell {
    /// <summary>
    /// Shell服务契约;
    /// </summary>
    public interface IShellService {
        /// <summary>
        /// 初始化;
        /// </summary>
        void Initialize();

        /// <summary>
        /// 是否被初始化;
        /// </summary>
        bool Initialized { get; }

        /// <summary>
        /// 更改标题栏文字;
        /// </summary>
        /// <param name="word"></param>
        /// <param name="saveBrandName">是否保留软件名称</param>
        void SetTitle(string word, bool saveBrandName = true);

        /// <summary>
        /// 聚焦;
        /// </summary>
        void Focus();

        /// <summary>
        /// 隐藏;
        /// </summary>
        void Hide();
        

        void ChangeLoadState(bool isLoading, string word = null);

        /// <summary>
        /// 添加热键绑定;
        /// </summary>
        /// <param name="command"></param>
        /// <param name="key"></param>
        /// <param name="modifier"></param>
        void AddKeyBinding(ICommand command, Key key, ModifierKeys modifier = ModifierKeys.None);

        /// <summary>
        /// 显示窗体(初始化时使用);
        /// </summary>
        void Show();

        /// <summary>
        /// 以对话框形式显示;
        /// </summary>
        /// <param name="owner">窗体的持有单位,可以是Win32的句柄,也可以是<see cref="System.Windows.Window"/>
        void ShowDialog(object owner = null);

        /// <summary>
        /// 关闭窗体;
        /// </summary>
        void Close();

        /// <summary>
        /// 主界面堆叠栈;
        /// </summary>
        //IStackGrid<IUIObjectProvider> StackGrid { get; }

        /// <summary>
        /// 窗体;
        /// </summary>
        object Shell { get; }
    }

    
    public class ShellService : GenericServiceStaticInstance<IShellService> {
        ///// <summary>
        ///// 尝试初始化;
        ///// </summary>
        //public static void TryInitialize() {
        //    if (Current.Initialized) {
        //        return;
        //    }

        //    Current.Initialize();
        //}
    }
}
