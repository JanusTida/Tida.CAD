using Tida.Canvas.Shell.CommandOutput.ViewModels;
using Prism.Interactivity.InteractionRequest;
using System.ComponentModel.Composition;
using System.Text;

namespace Tida.Canvas.Shell.Contracts.CommandOutput {
    /// <summary>
    /// 命令输出服务;
    /// </summary>
    [Export(typeof(ICommandOutputService))]
    class CommandOutputServiceImpl: ICommandOutputService {
        [ImportingConstructor]
        public CommandOutputServiceImpl(CommandOutputViewModel vm) {
            _vm = vm;
        }

        private readonly CommandOutputViewModel _vm;
        private readonly StringBuilder _sb = new StringBuilder();
        /// <summary>
        /// 导出一行命令;
        /// </summary>
        /// <param name="line"></param>
        public void WriteLine(string line) {
            _sb.AppendLine(">>" + line);
            _vm.Text = _sb.ToString();
            _vm.ScrollToEndRequest.Raise(new Notification());
        }
    }

    

}
