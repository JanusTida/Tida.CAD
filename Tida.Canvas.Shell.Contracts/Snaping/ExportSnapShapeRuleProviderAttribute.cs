using System;
using System.ComponentModel.Composition;
using Tida.Canvas.Infrastructure.Snaping;

namespace Tida.Canvas.Shell.Contracts.Snaping {
    [MetadataAttribute, AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ExportSnapShapeRuleProviderAttribute : ExportAttribute, ISnapShapeRuleProviderMetaData {
        public ExportSnapShapeRuleProviderAttribute() : base(typeof(ISnapShapeRuleProvider)) {

        }
        public int Order { get; set; }
    }
}
