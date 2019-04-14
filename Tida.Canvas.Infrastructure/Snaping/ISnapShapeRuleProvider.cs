using Tida.Canvas.Infrastructure.Contracts;
using Tida.Canvas.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Infrastructure.Snaping {
    /// <summary>
    /// 辅助规则提供器单元，适用于通过代码的方式动态提供辅助规则;
    /// </summary>
    public interface ISnapShapeRuleProvider  {
        /// <summary>
        /// 所有辅助规则;
        /// </summary>
        IEnumerable<ISnapShapeRule> Rules { get; }
    }

    /// <summary>
    /// 辅助规则提供器元数据;
    /// </summary>
    public interface ISnapShapeRuleProviderMetaData : IHaveOrder {

    }


    
}
