using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.Contracts.Ribbon {
    /// <summary>
    /// RiibbonGroups提供者注解;
    /// </summary>
    [MetadataAttribute, AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ExportRibbonGroupsProviderAttribute : ExportAttribute, IRibbonGroupsProviderMetaData {
        public ExportRibbonGroupsProviderAttribute():base(typeof(IRibbonGroupsProvider)) {

        }
        public int Order { get; set; }
    }
}
