using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.Contracts.App.Input {
    /// <summary>
    /// 获取输入设定;
    /// </summary>
    public class GetInputValueSetting {
        /// <summary>
        /// 标题;
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// 详细;
        /// </summary>
        public string Desc { get; set; }
        
        /// <summary>
        /// 初始值;
        /// </summary>
        public string Val { get; set; }

        /// <summary>
        /// 值检查器;
        /// </summary>
        public IInputChecker InputChecker { get; set; }

        /// <summary>
        /// 主窗体拥有者;
        /// </summary>
        public object Owner { get; set; }
    }
}
