using System;
using System.ComponentModel.Composition;
using Tida.Canvas.Infrastructure.Snaping;

namespace Tida.Canvas.Shell.Contracts.Snaping {
    [MetadataAttribute, AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ExportDrawObjectIntersectRuleAttribute : ExportAttribute, IDrawObjectIntersectRuleMetaData {
        public ExportDrawObjectIntersectRuleAttribute() : base(typeof(IDrawObjectIntersectRule)) {

        }

        public int Order { get; set; }
    }
}
