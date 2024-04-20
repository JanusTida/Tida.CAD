using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Tida.Canvas.Shell.Contracts.Controls {
    /// <summary>
    /// 通过此注解导出视图元数据;
    /// </summary>
    public class ExportViewAttribute:ExportAttribute {
        public ExportViewAttribute(string viewName):base(viewName,typeof(FrameworkElement)) {
            
        }

        
    }

    ///// <summary>
    ///// 视图元数据;
    ///// </summary>
    //public interface IViewMetaData {
    //    /// <summary>
    //    /// 视图名;
    //    /// </summary>
    //    string ViewName { get; }
    //}
}
