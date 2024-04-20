using Tida.Canvas.Contracts;
using Tida.Canvas.Infrastructure.Snaping.Rules;
using Tida.Canvas.Shell.Contracts.Snaping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.Snaping {
    /// <summary>
    /// 基本图形辅助规则提供器;
    /// </summary>
    [ExportSnapShapeRuleProvider(Order = Constants.SnapRuleOrder_Basic)]
    class BasicSnapRulesProvider : ISnapShapeRuleProvider {
        public IEnumerable<ISnapShapeRule> Rules {
            get {
                if(_rules == null) {
                    _rules = new ISnapShapeRule[] {
                        new MeasureAngleSnapRule(),
                        new SingleEllipseSnapPointRule(),
                        new PointSnapPointRule(),
                        new LineSnapRule(),

                        new GridSnapPointRule(),
                        new AxisTrackingSnapRule(),
                        new LineSnapExtendRule(),
                        new LineSnapOnLineRule(),
                        
                    };
                }
                return _rules;
            }
        }

        private ISnapShapeRule[] _rules;
    }
}
