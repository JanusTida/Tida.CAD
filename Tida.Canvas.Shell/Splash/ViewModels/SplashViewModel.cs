using Prism.Mvvm;
using System.ComponentModel.Composition;

namespace Tida.Canvas.Shell.Splash.ViewModels {
    [Export]
    public class SplashViewModel:BindableBase {
        
        private string _loadingText;
        public string LoadingText {
            get { return _loadingText; }
            set { SetProperty(ref _loadingText, value); }
        }

    }
}
