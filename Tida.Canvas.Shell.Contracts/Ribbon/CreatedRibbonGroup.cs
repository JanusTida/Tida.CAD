using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.Contracts.Ribbon {

    /// <summary>
    /// 动态创建的Ribbon组;
    /// </summary>
    public class CreatedRibbonGroup {
        public IRibbonGroup RibbonGroup { get; }

        public IRibbonGroupMetaData RibbonGroupMetaData { get; }
        public CreatedRibbonGroup(IRibbonGroup ribbonGroup,IRibbonGroupMetaData ribbonGroupMetaData) {
            RibbonGroup = ribbonGroup ?? throw new ArgumentNullException(nameof(ribbonGroup));
            RibbonGroupMetaData = ribbonGroupMetaData ?? throw new ArgumentNullException(nameof(ribbonGroupMetaData));
        }
    }
}
