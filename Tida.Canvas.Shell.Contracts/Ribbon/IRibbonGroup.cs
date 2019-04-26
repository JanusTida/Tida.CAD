using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.Contracts.Ribbon {
    /// <summary>
    /// Ribbon组;
    /// </summary>
    public interface IRibbonGroup {
        
    }

    public interface IRibbonGroupMetaData {
        /// <summary>
        /// <see cref="IRibbonTab.GUID"/> 的标识;
        /// </summary>
        string ParentGUID { get; }

        /// <summary>
        /// 标识;
        /// </summary>
        string GUID { get; }

        /// <summary>
        /// 显示名;
        /// </summary>
        string HeaderLanguageKey { get; }

        /// <summary>
        /// 排序;
        /// </summary>
        int Order { get; }

        /// <summary>
        /// 图标;
        /// </summary>
        string Icon { get; }
    }
}
