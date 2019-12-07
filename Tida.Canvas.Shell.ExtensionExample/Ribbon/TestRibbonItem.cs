using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tida.Canvas.Infrastructure.DrawObjects;
using Tida.Canvas.Shell.Contracts.Canvas;
using Tida.Canvas.Shell.Contracts.Ribbon;
using Tida.Geometry.Primitives;

namespace Tida.Canvas.Shell.ExtensionExample.Ribbon
{
    [ExportRibbonItem(GroupGUID = Constants.RibbonGroupGuid_Test,Order = 1)]
    class TestRibbonItem : IRibbonButtonItem
    {
        [ImportingConstructor]
        public TestRibbonItem(ICanvasService canvasService)
        {
            _canvasService = canvasService;
        }
        private readonly ICanvasService _canvasService;

        public string Icon => null;

        public ICommand Command => _testcommand ?? (_testcommand = new DelegateCommand(Test));

        private ICommand _testcommand;
        private void Test()
        {
            var canvasDataContext = _canvasService.CanvasDataContext;
            var layer = canvasDataContext.ActiveLayer;
            var lines = new Line[]
            {
                new Line(Vector2D.BasisX + Vector2D.BasisY,-Vector2D.BasisX - Vector2D.BasisY),
                new Line(-Vector2D.BasisX + Vector2D.BasisY,Vector2D.BasisX - Vector2D.BasisY)
            };
            layer.AddDrawObjects(lines);
        }
        public string HeaderLanguageKey => "Test Add DrawObjects";
    }
}
