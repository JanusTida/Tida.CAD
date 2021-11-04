using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.CAD.WPF.SimpleSample
{
    /// <summary>
    /// 测试命令;
    /// </summary>
    interface ITestCommand
    {
        /// <summary>
        /// 命令名称;
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 执行命令;
        /// </summary>
        /// <param name="testExecuteContext"></param>
        void Execute(TestExecuteContext testExecuteContext);

        /// <summary>
        /// 排序;
        /// </summary>
        int Order { get; }
    }
}
