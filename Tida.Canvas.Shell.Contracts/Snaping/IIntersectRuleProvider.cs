using Tida.Canvas.Infrastructure.Snaping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.Contracts.Snaping {
    /// <summary>
    /// 相交规则的提供器契约;
    /// </summary>
    public interface IIntersectRuleProvider {
        IEnumerable<IDrawObjectIntersectRule> Rules { get; }
    }
}
