using Tida.Canvas.Base.NativePresentation.Models;
using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace Tida.Canvas.Base.NativePresentation.ViewModels {
    class NumberBoxContainerViewModel : BindableBase {
        public ObservableCollection<NumberBoxModel> NumberBoxes { get; } = new ObservableCollection<NumberBoxModel>();
    }
}
