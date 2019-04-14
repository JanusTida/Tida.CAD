using Tida.Application.Contracts.Common;
using Tida.Canvas.Shell.Contracts.Canvas;
using Tida.Canvas.Shell.Contracts.Canvas.Events;
using Tida.Canvas.Shell.Contracts.EditTools;
using Tida.Canvas.Shell.Contracts.Shell;
using Tida.Canvas.Shell.Contracts.Shell.Events;
using Prism.Commands;
using Prism.Mef.Modularity;
using Prism.Modularity;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows.Input;

namespace Tida.Canvas.Base.EditTools {
    [Export(typeof(IShellInitializingEventHandler))]
    class ShellInitializingAddEditToolBindingHandler : IShellInitializingEventHandler {
        [ImportingConstructor]
        public ShellInitializingAddEditToolBindingHandler([ImportMany]IEnumerable<Lazy<IEditToolProvider, IEditToolProviderMetaData>> mefEditToolProviders) {
            this._mefEditToolProviders = mefEditToolProviders;
        }

        private readonly IEnumerable<Lazy<IEditToolProvider, IEditToolProviderMetaData>> _mefEditToolProviders;
        public int Sort => 239;

        public bool IsEnabled => true;

        public void Handle() {
            foreach (var provider in _mefEditToolProviders) {
                if (provider.Metadata.Key == Key.None) {
                    continue;
                }

                ShellService.Current.AddKeyBinding(
                    new DelegateCommand(
                        () => {
                            CanvasService.CanvasDataContext.CurrentEditTool = provider.Value.CreateEditTool();
                        }
                    ),
                    provider.Metadata.Key,
                    provider.Metadata.ModifierKeys
                );
            }
        }
    }

}
