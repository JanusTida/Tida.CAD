using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.Contracts.ComponentModel {
    /// <summary>
    /// 编辑器的显示类型;
    /// </summary>
    public enum EditorStyle {
        //
        // Summary:
        //     No special user interface element is used.
        None = 0,

        /// <summary>
        ///  A button that shows a modal dialog window with the custom editor inside is displayed.
        /// </summary>
        Modal = 1,
        /// <summary>
        ///  A drop down button which content is the custom editor is displayed.
        /// </summary>
        DropDown = 2,
     
    }
}
