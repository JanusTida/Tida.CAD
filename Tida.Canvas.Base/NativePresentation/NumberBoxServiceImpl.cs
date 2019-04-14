using System.ComponentModel.Composition;
using Tida.Canvas.Base.NativePresentation.Models;

namespace Tida.Canvas.Base.NativePresentation {
    [Export(typeof(INumberBoxService))]
    class NumberBoxServiceImpl : INumberBoxService {
        public INumberBoxContainer CreateContainer() => new Views.NumberBoxContainer();

        public INumberBox CreateNumberBox() => new NumberBoxModel {
            IsReadOnly = true
        };
    }
}
