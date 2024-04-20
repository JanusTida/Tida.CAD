using Tida.Canvas.Contracts;
using Tida.Canvas.Infrastructure.Snaping;
using System.Collections.Generic;

namespace Tida.Canvas.Shell.Contracts.Snaping {
    /// <summary>
    /// 辅助规则提供器单元，适用于通过代码的方式动态提供辅助规则;
    /// </summary>
    public interface ISnapShapeRuleProvider {
        /// <summary>
        /// 所有辅助规则;
        /// </summary>
        IEnumerable<ISnapShapeRule> Rules { get; }
        
    }

}
