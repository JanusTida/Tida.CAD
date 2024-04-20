using System;
using System.ComponentModel.Composition;
using System.Windows.Input;

namespace Tida.Canvas.Shell.Contracts.EditTools {
    /// <summary>
    /// 导出编辑工具提供器注解,需通过此注解才能导出编辑工具创建工具;
    /// </summary>
    [MetadataAttribute,AttributeUsage(AttributeTargets.Class,AllowMultiple =false)]
    public class ExportEditToolProviderAttribute : ExportAttribute,IEditToolProviderMetaData {
        public ExportEditToolProviderAttribute():base(typeof(IEditToolProvider)) {

        }

        public string GroupGUID { get; set; }

        public string EditToolLanguageKey { get; set; }

        public string GUID { get; set; }

        public string IconResource { get; set; }

        public int Order { get; set; }

        public Key Key { get; set; }

        public ModifierKeys ModifierKeys { get; set; }
        
    }
}
