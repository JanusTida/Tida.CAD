using System;
using System.ComponentModel.Composition;

namespace Tida.Canvas.Shell.Contracts.Ribbon {
    [MetadataAttribute, AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ExportRibbonItemsProviderAttribute : ExportAttribute, IRibbonItemsProviderMetaData {
        public ExportRibbonItemsProviderAttribute():base(typeof(IRibbonItemsProvider)) {

        }
        public int Order { get; set; }
    }
}
