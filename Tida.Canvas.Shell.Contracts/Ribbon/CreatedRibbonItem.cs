using System;

namespace Tida.Canvas.Shell.Contracts.Ribbon {
    /// <summary>
    /// 动态创建的Ribbon项;
    /// </summary>
    public class CreatedRibbonItem {
        public IRibbonItem RibbonItem { get; }
        public IRibbonItemMetaData RibbonItemMetaData { get; }

        public CreatedRibbonItem(IRibbonItem ribbonItem,IRibbonItemMetaData ribbonItemMetaData) {

            RibbonItem = ribbonItem ?? throw new ArgumentNullException(nameof(ribbonItem));

            RibbonItemMetaData = ribbonItemMetaData ?? throw new ArgumentNullException(nameof(ribbonItemMetaData));

        }
    }
}
