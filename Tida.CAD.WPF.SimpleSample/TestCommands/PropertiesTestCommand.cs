using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tida.CAD.WPF.SimpleSample.Views;

namespace Tida.CAD.WPF.SimpleSample.TestCommands
{
    internal class PropertiesTestCommand : ITestCommand
    {
        public string Name => "PropertiesTest";

        public int Order => 2;

        public void Execute(TestExecuteContext testExecuteContext) => new PropertiesTest().ShowDialog();
    }
}
