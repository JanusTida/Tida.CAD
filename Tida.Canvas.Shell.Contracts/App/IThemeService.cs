using Tida.Canvas.Shell.Contracts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Tida.Canvas.Shell.Contracts.App {
    /// <summary>
    /// 主题服务;
    /// </summary>
    public interface IThemeService {
        /// <summary>
        /// 添加资源字典到统一的全局资源中;
        /// </summary>
        void AddDictionary(ResourceDictionary res);

        /// <summary>
        /// 从统一的全局资源中移除资源字典;
        /// </summary>
        void RemoveDictionary(ResourceDictionary res);

    }

    /// <summary>
    /// 主题服务;
    /// </summary>
    public class ThemeService : GenericServiceStaticInstance<IThemeService> {
        /// <summary>
        /// 添加资源字典到统一的全局资源中;
        /// </summary>
        /// <param name="res"></param>
        public static void AddDictionary(ResourceDictionary res) => Current?.AddDictionary(res);


        /// <summary>
        /// 从统一的全局资源中移除资源字典;
        /// </summary>
        public static void RemoveDictionary(ResourceDictionary res) => Current?.RemoveDictionary(res);
    }
}
