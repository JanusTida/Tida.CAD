using Tida.Canvas.Shell.Contracts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.Contracts.Setting {
    /// <summary>
    /// 设定服务;
    /// </summary>
    public interface ISettingsService {
        /// <summary>
        /// 初始化;
        /// </summary>
        void Initialize();


        /// <summary>
        /// 所有设定节;
        /// </summary>
        IEnumerable<ISettingsSection> Sections { get; }

        /// <summary>
        /// 创建或获取设定节;
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        ISettingsSection GetOrCreateSection(string guid);

        /// <summary>
        /// 移除设定节;
        /// </summary>
        /// <param name="guid"></param>
        void RemoveSection(ISettingsSection settingsSection);
    }

    public class SettingsService:GenericServiceStaticInstance<ISettingsService> {
        public static ISettingsSection GetOrCreateSection(string guid) => Current?.GetOrCreateSection(guid);
        public static void RemoveSection(ISettingsSection settingsSection) => Current?.RemoveSection(settingsSection);
    }
}
