using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using System.ComponentModel.Composition;

namespace Tida.Canvas.Shell.CommandOutput.ViewModels {
    [Export]
    class CommandOutputViewModel:BindableBase {

        private string _text;
        public string Text {
            get { return _text; }
            set { SetProperty(ref _text, value); }
        }


        public InteractionRequest<Notification> ScrollToEndRequest { get; } = new InteractionRequest<Notification>();
    }
}
