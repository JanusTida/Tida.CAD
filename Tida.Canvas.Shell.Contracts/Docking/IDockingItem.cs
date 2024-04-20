using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.Contracts.Docking {
    /// <summary>
    /// 停靠基项契约;
    /// </summary>
    public interface IDockingItem {
        
    }

    /// <summary>
    /// 停靠基项元数据;
    /// </summary>
    public interface IDockingItemMetaData {
        /// <summary>
        /// 唯一标识;
        /// </summary>
        string GUID { get; }
    }
}
