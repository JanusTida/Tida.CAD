using Tida.Application.Contracts.Common;
using System.Collections.Generic;
using System.Linq;

namespace Tida.Canvas.Shell.Contracts.StatusBar {
    /// <summary>
    /// 状态栏服务;
    /// </summary>
    public interface IStatusBarService {
        /// <summary>
        /// 常规显示文字;
        /// </summary>
        /// <param name="text">显示的文字</param>
        /// <param name="statusBarItemGUID">对应的状态栏项GUID,若为空则操作默认的项</param>
        //void Report(string text, string statusBarItemGUID = null);
        /// <summary>
        /// 添加状态栏项;
        /// </summary>
        /// <param name="item"></param>
        /// <param name="statusBarItemGUID"></param>
        void AddStatusBarItem(IStatusBarItem item);
        /// <summary>
        /// 移除状态栏项;
        /// </summary>
        /// <param name="item"></param>
        void RemoveStatusBarItem(IStatusBarItem item);
        /// <summary>
        /// 所有状态栏项;
        /// </summary>
        IEnumerable<IStatusBarItem> StatusBarItems { get; }
        /// <summary>
        /// 初始化;
        /// </summary>
        void Initialize();
        /// <summary>
        /// 根据内容创建状态栏项;
        /// </summary>
        /// <param name="content"></param>
        /// <param name="guid"></param>
        /// <returns></returns>
        //IStatusBarItem CreateStatusBarObjectItem(string guid,object content);
        /// <summary>
        /// 创建状态栏文字项;
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="gridChildLength"></param>
        /// <returns></returns>
        //IStatusBarTextItem CreateStatusBarTextItem(string guid);
    }

    public class StatusBarService : GenericServiceStaticInstance<IStatusBarService> {
        public static void Report(string text, string statusBarItemGUID = null) {
            if(Current == null) {
                return;
            }

            if (statusBarItemGUID == null) {
                statusBarItemGUID = Constants.StatusBarItem_Default;
            }

            var textItem = Current.StatusBarItems.FirstOrDefault(p => p.GUID == statusBarItemGUID) as StatusBarTextItem;
            if (textItem != null) {
                textItem.Text = text;
            }
        }

        //public static IStatusBarItem CreateStatusBarObjectItem(string guid, object content) =>
        //    Current.CreateStatusBarObjectItem(guid,content);
        //public static IStatusBarTextItem CreateStatusBarTextItem(string guid) =>
        //    Current.CreateStatusBarTextItem(guid);
    }
}
