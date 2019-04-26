using System;
using Tida.Application.Contracts.Common;
using Tida.Application.Contracts.Controls;
using Tida.Canvas.Shell.Contracts.CommandOutput;
using Tida.Canvas.Shell.Contracts.Shell;
using Tida.Canvas.Shell.Contracts.Shell.Events;
using Prism.Mef.Modularity;
using Prism.Modularity;

namespace Tida.Canvas.Shell.CommandOutput {
    [ModuleExport(typeof(CommandOutputModule))]
    class CommandOutputModule : IModule {
        public void Initialize() {
            //CommandOutputService.WriteLine("欢迎来到王者荣耀");
        }

        
    }
}
