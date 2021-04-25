using Prism.Interactivity.InteractionRequest;
using Prism.Ioc;
using Prism.Mvvm;
using System.ComponentModel.Composition;
using Tida.Canvas.Shell.CommandOutput.IViews;
using Tida.Canvas.Shell.Contracts.Common;

namespace Tida.Canvas.Shell.CommandOutput.ViewModels {
    [Export]
    class CommandOutputViewModel:BindableBase {
        [ImportingConstructor]
        public CommandOutputViewModel(IContainerExtension containerProvider)
        {
            _containerProvider = containerProvider;
        }
        private readonly IContainerProvider _containerProvider;
        private string _text;
        public string Text {
            get { return _text; }
            set { SetProperty(ref _text, value); }
        }

        public void ScrollToEnd()
        {
           ServiceProvider.GetInstance<ICommandOutput>().ScrollToEnd();
        }
        

    }
}
