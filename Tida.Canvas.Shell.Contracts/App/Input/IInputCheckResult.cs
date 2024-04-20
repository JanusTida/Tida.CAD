using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.Contracts.App.Input {
    /// <summary>
    /// 输入检查结果;
    /// </summary>
    public class InputCheckResult {
        public InputCheckResult(bool isValid,string errorMessage = null) {
            this.IsValid = isValid;
            this.ErrorMessage = errorMessage;
        }

        /// <summary>
        /// 是否可用;
        /// </summary>
        public bool IsValid { get; }

        /// <summary>
        /// 错误信息;
        /// </summary>
        public string ErrorMessage { get; }
    }
}
