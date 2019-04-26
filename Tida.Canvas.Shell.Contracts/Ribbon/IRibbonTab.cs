using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.Contracts.Ribbon {
    /// <summary>
    /// Ribbon——Tab;
    /// </summary>
    public interface IRibbonTab {
        
    }

    public interface IRibbonTabMetaData {
        /// <summary>
        /// 标识;
        /// </summary>
        string GUID { get; }

        /// <summary>
        /// 资源文件中存储的资源的名称的键名;
        /// </summary>
        string TextLangaugeKey { get; }

        /// <summary>
        /// 排序;
        /// </summary>
        int Order { get; }
    }
}
