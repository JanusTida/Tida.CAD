using System;
using System.ComponentModel.Composition;

namespace Tida.Canvas.Shell.Contracts.Ribbon {
    /// <summary>
    /// 导出Ribbon项注解;
    /// </summary>
    [MetadataAttribute, AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ExportRibbonItemAttribute: ExportAttribute,IRibbonItemMetaData {
        public ExportRibbonItemAttribute():base(typeof(IRibbonItem)) {
            
        }

        public string GroupGUID { get; set; }

        public string GUID { get; set; }

        public int Order { get; set; }
    }
}
