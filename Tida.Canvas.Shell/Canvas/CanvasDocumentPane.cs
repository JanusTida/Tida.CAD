using Tida.Application.Contracts.App;
using Tida.Application.Contracts.Docking;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static Tida.Canvas.Shell.Canvas.Constants;

namespace Tida.Canvas.Shell.Canvas {
    [Export]
    class CanvasDocumentPane : DockingPaneBase,IDockingPaneMetaData {
        [ImportingConstructor]
        public CanvasDocumentPane(Views.Canvas canvas) {
            Header = LanguageService.FindResourceString(DocumentPaneHeader_Canvas);
            _canvas = canvas;
        }

        public string InitPaneGroupGUID => null;
        
        public string GUID => null;

        private readonly Views.Canvas _canvas;
        public override object UIObject => _canvas;

        public double InitialWidth => double.NaN;

        public double InitialHeight => double.NaN;

        public bool CanUserClose => false;

        public bool CanFloat => true;
    }
}
