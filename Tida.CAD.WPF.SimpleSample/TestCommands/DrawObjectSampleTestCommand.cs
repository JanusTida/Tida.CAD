using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tida.CAD.WPF.SimpleSample.Views;

namespace Tida.CAD.WPF.SimpleSample.TestCommands
{
    class DrawObjectSampleTestCommand : ITestCommand
    {
        public string Name => "DrawObjectSample";

        public int Order => 2;

        public void Execute(TestExecuteContext testExecuteContext) => new DrawObjectSample().ShowDialog();
    }
}
