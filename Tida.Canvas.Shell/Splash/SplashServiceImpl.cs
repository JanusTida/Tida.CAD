using Tida.Canvas.Shell.Contracts.Common;
using Tida.Canvas.Shell.Contracts.Splash;
using System;
using System.ComponentModel.Composition;
using System.Threading;
using System.Windows.Threading;

namespace Tida.Canvas.Shell.Splash {
    [Export(typeof(ISplashService))]
    public class SplashService : ISplashService {
        [ImportingConstructor]
        public SplashService(ViewModels.SplashViewModel vm) {
            this._vm = vm;
        }

        private readonly ViewModels.SplashViewModel _vm;
    
        public void ReportMessage(string msg) {
            _vm.LoadingText = msg;
        }

        public void CloseSplash() {
            var splash = ServiceProvider.Current.GetInstance<Views.Splash>();
            splash.Dispatcher.BeginInvoke((Action)splash.Close);
        }

        public void ShowSplash() {
            var waitForCreation = new AutoResetEvent(false);
            var thread = new Thread(() => {
                Dispatcher.CurrentDispatcher.BeginInvoke(
                      (Action)(() => {
                          var splash = ServiceProvider.Current.GetInstance<Views.Splash>();
                          splash.Show();

                          waitForCreation.Set();
                      }));

                Dispatcher.Run();
            }) { Name = "Splash Thread", IsBackground = true };

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();

            waitForCreation.WaitOne();
        }
    }
}
