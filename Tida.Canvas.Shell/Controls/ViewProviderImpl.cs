using Tida.Canvas.Shell.Contracts.Common;
using Tida.Canvas.Shell.Contracts.Controls;
using System.Windows;


namespace Tida.Canvas.Shell.Controls {
    public class ViewProviderImpl : IViewProvider {
        public ViewProviderImpl(IServiceProvider serviceProvider) {
            this._serviceProvider = serviceProvider;
        }
        private IServiceProvider _serviceProvider;
        public object GetView(string viewName) => _serviceProvider.GetInstance<FrameworkElement>(viewName);
        
    }
}
