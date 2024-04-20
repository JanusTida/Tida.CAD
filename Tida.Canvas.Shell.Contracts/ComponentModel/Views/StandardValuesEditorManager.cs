using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

namespace Tida.Canvas.Shell.Contracts.ComponentModel.Views {
    /// <summary>
    /// 使用了下拉的标准值集合编辑器即<see cref="StandardValueLanguageInfo{TValue}"/>的管理器,本类旨在在具备不同参数类型的编辑器类中共用重要的静态成员;
    /// </summary>
    public static class StandardValuesEditorManager {
        /// <summary>
        /// 静态的创建选择器UI的工厂.在使用<see cref="StandardValuesEditor{TValue}"/>前,必须设定此值,否则将会抛出一个运行时异常;
        /// </summary>
        public static Func<Selector> SelectorFactory { get; set; }
    }
}
