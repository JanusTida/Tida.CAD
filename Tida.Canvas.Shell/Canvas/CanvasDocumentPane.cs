using Tida.Canvas.Shell.Contracts.App;
using Tida.Canvas.Shell.Contracts.Docking;
using System.ComponentModel.Composition;
using static Tida.Canvas.Shell.Canvas.Constants;

namespace Tida.Canvas.Shell.Canvas {
    [Export]
    class CanvasDocumentPane : DockingPaneBase,IDockingPaneMetaData {
        [ImportingConstructor]
        public CanvasDocumentPane(Views.CanvasPresenter canvas) {
            Header = LanguageService.FindResourceString(DocumentPaneHeader_Canvas);
            _canvas = canvas;
        }

        public string InitPaneGroupGUID => null;
        
        public string GUID => null;

        private readonly Views.CanvasPresenter _canvas;
        public override object UIObject => _canvas;

        public double InitialWidth => double.NaN;

        public double InitialHeight => double.NaN;

        public bool CanUserClose => false;

        public bool CanFloat => true;
    }
}
