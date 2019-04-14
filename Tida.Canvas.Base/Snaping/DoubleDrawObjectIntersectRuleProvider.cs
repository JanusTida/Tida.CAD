using Tida.Canvas.Base.Snaping.Intersect;
using Tida.Canvas.Infrastructure.Snaping;
using Tida.Canvas.Contracts;
using System.Collections.Generic;
using System.Linq;
using Tida.Canvas.Shell.Contracts.Snaping;
using System.ComponentModel.Composition;

namespace Tida.Canvas.Base.Snaping {
    /// <summary>
    /// 处理两个对象相交时的辅助规则提供器;
    /// </summary>
    [ExportSnapShapeRuleProvider(Order = Constants.SnapRuleOrder_DoubleDrawObjectIntersect)]
    class DoubleDrawObjectIntersectRuleProvider : ISnapShapeRuleProvider {
        [ImportingConstructor]
        public DoubleDrawObjectIntersectRuleProvider([ImportMany]IEnumerable<IDrawObjectIntersectRule> intersectRules) {
            _rules = intersectRules.Select(p => new DoubleDrawObjectIntersectSnapRule(p)).ToArray();
        }

        private readonly ISnapShapeRule[] _rules;
        public IEnumerable<ISnapShapeRule> Rules => _rules;

        
    }

}
