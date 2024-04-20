using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Input {
    /// <summary>
    /// 文字输入事件;
    /// </summary>
    public class TextInputEventArgs:HandleableEventArgs {
        public TextInputEventArgs(string text,string controlText) {
            this.Text = text;
            this.ControlText = controlText;
        }
        /// <summary>
        /// 键入的文字;
        /// </summary>
        public string Text { get; }

        public string ControlText { get; }
    }
}
