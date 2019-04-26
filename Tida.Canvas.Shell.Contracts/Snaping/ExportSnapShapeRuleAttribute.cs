using Tida.Canvas.Contracts;
using Tida.Canvas.Infrastructure.Snaping;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.Contracts.Snaping {
    /// <summary>
    /// 导出辅助图形规则,可通过此注解导出辅助图形规则;
    /// </summary>
    [MetadataAttribute, AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ExportSnapShapeRuleAttribute : ExportAttribute, ISnapShapeRuleMetaData {
        public ExportSnapShapeRuleAttribute() : base(typeof(ISnapShapeRule)) {

        }

        /// <summary>
        /// 排序;
        /// </summary>
        public int Order { get; set; }
    }


}
