using System;
using System.ComponentModel.Composition;

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
