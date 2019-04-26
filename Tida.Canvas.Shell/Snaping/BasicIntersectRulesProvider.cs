using Tida.Canvas.Infrastructure.Snaping;
using Tida.Canvas.Infrastructure.Snaping.Intersect;
using Tida.Canvas.Shell.Contracts.Snaping;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.Snaping {
    /// <summary>
    /// 基本的相交规则提供器;
    /// </summary>
    [Export(typeof(IIntersectRuleProvider))]
    class BasicIntersectRulesProvider : IIntersectRuleProvider {
        private IDrawObjectIntersectRule[] _rules;
        public IEnumerable<IDrawObjectIntersectRule> Rules {
            get {
                if(_rules == null) {
                    _rules = new IDrawObjectIntersectRule[] {
                        new DoubleLineIntersectRule(),
                        new LineAndEllipseIntersectRule(),
                        new LineAndRectangleRule()
                    };
                }

                return _rules;
            }
        }
    }
}
