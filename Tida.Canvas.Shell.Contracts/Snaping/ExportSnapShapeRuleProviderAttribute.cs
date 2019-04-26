using System;
using System.ComponentModel.Composition;

namespace Tida.Canvas.Shell.Contracts.Snaping {
    /// <summary>
    /// 辅助规则提供器的导出注解;
    /// </summary>
    [MetadataAttribute, AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ExportSnapShapeRuleProviderAttribute : ExportAttribute, ISnapShapeRuleProviderMetaData {
        public ExportSnapShapeRuleProviderAttribute() : base(typeof(ISnapShapeRuleProvider)) {

        }
        public int Order { get; set; }
    }
}
