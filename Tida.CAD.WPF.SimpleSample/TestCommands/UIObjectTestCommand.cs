using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tida.CAD.WPF.SimpleSample.Views;

namespace Tida.CAD.WPF.SimpleSample.TestCommands;

internal class UIObjectTestCommand : ITestCommand
{
    public string Name => "UIObject";

    public int Order => 4;

    public void Execute(TestExecuteContext testExecuteContext)
    {
        new UIObjectTest().Show();
    }
}
