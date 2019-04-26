using Tida.Application.Contracts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.Contracts.CommandOutput {
    /// <summary>
    /// 命令输出服务;
    /// </summary>
    public interface ICommandOutputService {
        /// <summary>
        /// 导出一行命令;
        /// </summary>
        /// <param name="line"></param>
        void WriteLine(string line);
    }

    public class CommandOutputService : GenericServiceStaticInstance<ICommandOutputService> {
        public static void WriteLine(string line) => Current?.WriteLine(line);
    }

}
