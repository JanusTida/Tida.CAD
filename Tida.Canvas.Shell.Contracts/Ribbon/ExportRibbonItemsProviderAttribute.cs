using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.Contracts.Ribbon {
    [MetadataAttribute, AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ExportRibbonItemsProviderAttribute : ExportAttribute, IRibbonItemsProviderMetaData {
        public ExportRibbonItemsProviderAttribute():base(typeof(IRibbonItemsProvider)) {

        }
        public int Order { get; set; }
    }
}
