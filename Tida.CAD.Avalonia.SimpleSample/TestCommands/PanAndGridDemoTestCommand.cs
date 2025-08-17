using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tida.CAD.Avalonia.SimpleSample.ViewModels;

namespace Tida.CAD.Avalonia.SimpleSample.TestCommands;

class PanAndGridDemoTestCommand : ITestCommand
{
    public string Name => "Pan & Grid Demo";

    public int Order => 2;

    public async Task ExecuteAsync(TestExecuteContext testExecuteContext)
    {
        var vm = new PanAndGridDemoWindowViewModel
        {
            Zoom = CADControl.DefaultZoom,
            MinZoom = CADControl.DefaultMinZoom,
            IsMouseWheelingZoomEnabled = true,

            PanThickness = CADControl.DefaultPanThickness,
            PanLength = CADControl.DefaultPanLength,
            PanColor = CADControl.DefaultPanBrush.Color,
            
            GridsColor = CADControl.DefaultGridsBrush.Color,
            GridsThickness = CADControl.DefaultGridsThickness,
        };

        var window = new PanAndGridDemoWindow { DataContext = vm };
        await window.ShowDialog(testExecuteContext.MainWindow);
    }
}
