using Tida.Canvas.Shell.NativePresentation.Models;
using Tida.Canvas.Infrastructure.NativePresentation;
using System.ComponentModel.Composition;

namespace Tida.Canvas.Shell.NativePresentation {
    /// <summary>
    /// <see cref="INumberBoxService"/>的实现;
    /// </summary>
    [Export(typeof(INumberBoxService))]
    class NumberBoxServiceImpl : INumberBoxService {
        public INumberBoxContainer CreateContainer() => new Views.NumberBoxContainer();

        public INumberBox CreateNumberBox() => new NumberBoxModel {
            IsReadOnly = true
        };
    }
}
