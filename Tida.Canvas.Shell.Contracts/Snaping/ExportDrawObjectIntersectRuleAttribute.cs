using Tida.Canvas.Infrastructure.Snaping;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.Contracts.Snaping {

    [MetadataAttribute, AttributeUsage(AttributeTargets.Class | AttributeTargets.Field, AllowMultiple = false)]
    public class ExportDrawObjectIntersectRuleAttribute : ExportAttribute, IDrawObjectIntersectRuleMetaData {
        public ExportDrawObjectIntersectRuleAttribute() : base(typeof(IDrawObjectIntersectRule)) {

        }

        public int Order { get; set; }
    }
}
