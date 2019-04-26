using Tida.Canvas.Infrastructure.Snaping;
using Tida.Canvas.Contracts;
using Tida.Canvas.Shell.Contracts.Snaping;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tida.Canvas.Infrastructure.Snaping.Intersect;

namespace Tida.Canvas.Shell.Snaping {
    /// <summary>
    /// 处理两个对象相交时的辅助规则提供器;
    /// </summary>
    [ExportSnapShapeRuleProvider(Order = Constants.SnapRuleOrder_DoubleDrawObjectIntersect)]
    class DoubleDrawObjectIntersectRuleProvider : ISnapShapeRuleProvider {
        [ImportingConstructor]
        public DoubleDrawObjectIntersectRuleProvider(
            [ImportMany]IEnumerable<IDrawObjectIntersectRule> intersectRules,
            [ImportMany]IEnumerable<IIntersectRuleProvider> intersectRuleProviders) {

            _rules = intersectRules.Union(intersectRuleProviders.SelectMany(p => p.Rules)).Select(p => new DoubleDrawObjectIntersectSnapRule(p)).ToArray();
        }

        private readonly ISnapShapeRule[] _rules;
        public IEnumerable<ISnapShapeRule> Rules => _rules;


    }

}
