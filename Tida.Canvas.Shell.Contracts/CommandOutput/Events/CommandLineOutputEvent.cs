using Tida.Application.Contracts.Common;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.Contracts.CommandOutput.Events {
    /// <summary>
    /// 命令被输出事件;
    /// </summary>
    public class CommandLineOutPutEventArgs:EventArgs {
        public CommandLineOutPutEventArgs(string text) {
            this.Text = text;
        }
        /// <summary>
        /// 命令内容;
        /// </summary>
        public string Text { get; }
    }

    public class CommandLineOutputEvent:PubSubEvent<CommandLineOutPutEventArgs> {

    }

    public interface ICommandLineOutputEventHandler:IEventHandler<CommandLineOutPutEventArgs> {

    }
}
