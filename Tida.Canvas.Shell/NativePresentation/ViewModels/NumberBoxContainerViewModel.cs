using Tida.Canvas.Shell.NativePresentation.Models;
using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace Tida.Canvas.Shell.NativePresentation.ViewModels {
    class NumberBoxContainerViewModel : BindableBase {
        public ObservableCollection<NumberBoxModel> NumberBoxes { get; } = new ObservableCollection<NumberBoxModel>();
    }
}
