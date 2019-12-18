using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.Contracts.Common {
    /// <summary>
    /// 具有排序特征的契约;
    /// </summary>
    public interface IHaveOrder {
        /// <summary>
        /// 排序;
        /// </summary>
        int Order { get; }
    }
}
