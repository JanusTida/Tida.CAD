
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tida.Canvas.Shell.Contracts.Common;

namespace Tida.Canvas.Shell.Contracts.Ribbon {
    /// <summary>
    /// Ribbon项基本契约;
    /// </summary>
    public interface IRibbonItem {
        
    }

    /// <summary>
    /// Ribbon项元数据;
    /// </summary>
    public interface IRibbonItemMetaData:IHaveOrder {
        /// <summary>
        /// <see cref="IRibbonGroup"/>的标识;
        /// </summary>
        string GroupGUID { get; }


        /// <summary>
        /// 唯一标识;
        /// </summary>
        string GUID { get; }
    }
}
