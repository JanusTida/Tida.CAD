using Tida.Application.Contracts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.Contracts.Ribbon {
    /// <summary>
    /// Ribbon组动态提供者契约;
    /// </summary>
    public interface IRibbonGroupsProvider {
        /// <summary>
        /// Ribbon组集合;
        /// </summary>
        IEnumerable<CreatedRibbonGroup> Groups { get; }
    }

    public interface IRibbonGroupsProviderMetaData:IHaveOrder {

    }
}
