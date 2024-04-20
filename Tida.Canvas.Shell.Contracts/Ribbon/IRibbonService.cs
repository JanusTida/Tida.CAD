using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.Contracts.Ribbon {
    /// <summary>
    /// Ribbon服务;
    /// </summary>
    public interface IRibbonService {
        /// <summary>
        /// 所有菜单项;
        /// </summary>
        //IEnumerable<IMenuItem> MenuItems { get; }

        /// <summary>
        /// 所有Ribbon——Tab项;
        /// </summary>
        //IEnumerable<IRibbonTab> RibbonTabs { get; }

        /// <summary>
        /// 所有Ribbon组项;
        /// </summary>
        //IEnumerable<IRibbonGroup> RibbonGroups { get; }

        /// <summary>
        /// 所有Ribbon项;
        /// </summary>
        //IEnumerable<IRibbonItem> RibbonItems { get; }
        

        /// <summary>
        /// 初始化;
        /// </summary>
        void Initialize();
    }
}
