using System;
using System.ComponentModel.Composition;

namespace Tida.Canvas.Shell.Contracts.Ribbon {
    /// <summary>
    /// 导出RibbonTab;
    /// </summary>
    [MetadataAttribute, AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ExportRibbonTabAttribute:ExportAttribute,IRibbonTabMetaData {
        public ExportRibbonTabAttribute():base(typeof(IRibbonTab)) {

        }

        public string GUID { get; set; }

        public string TextLangaugeKey { get; set; }

        public int Order { get; set; }
    }
}
