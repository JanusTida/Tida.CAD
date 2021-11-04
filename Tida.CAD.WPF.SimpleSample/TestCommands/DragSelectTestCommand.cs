using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tida.CAD.WPF.SimpleSample.Views;

namespace Tida.CAD.WPF.SimpleSample.TestCommands
{
    internal class DragSelectTestCommand : ITestCommand
    {
        public string Name => "DragSelect";

        public int Order => 4;

        public void Execute(TestExecuteContext testExecuteContext) => new DragSelect().ShowDialog();
    }
}
