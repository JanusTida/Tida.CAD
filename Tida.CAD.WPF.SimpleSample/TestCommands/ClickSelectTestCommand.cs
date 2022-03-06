using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tida.CAD.WPF.SimpleSample.Views;

namespace Tida.CAD.WPF.SimpleSample.TestCommands
{
    internal class ClickSelectTestCommand : ITestCommand
    {
        public string Name => "ClickSelectTest";

        public int Order => 3;

        public void Execute(TestExecuteContext testExecuteContext)
        {
            new ClickSelectTest().Show();
        }
    }
}
