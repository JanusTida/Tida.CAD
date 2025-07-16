using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tida.CAD.Avalonia.SimpleSample.ViewModels;

namespace Tida.CAD.Avalonia.SimpleSample.TestCommands;

class DrawObjectDemoCommand : ITestCommand
{
    public string Name => "DrawObject Demo";

    public int Order => 1;

    public async Task ExecuteAsync(TestExecuteContext testExecuteContext)
    {
        var vm = new DrawObjectDemoWindowViewModel();
        var window = new DrawObjectDemoWindow() { DataContext = vm };
        await window.ShowDialog(testExecuteContext.MainWindow);
    }
}
