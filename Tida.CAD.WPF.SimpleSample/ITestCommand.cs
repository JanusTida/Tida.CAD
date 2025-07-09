using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.CAD.WPF.SimpleSample;

/// <summary>
/// The command to test;
/// </summary>
interface ITestCommand
{
    /// <summary>
    /// The command name;
    /// </summary>
    string Name { get; }

    /// <summary>
    /// The method to execute the command;
    /// </summary>
    /// <param name="testExecuteContext"></param>
    void Execute(TestExecuteContext testExecuteContext);

    /// <summary>
    /// Sort;
    /// </summary>
    int Order { get; }
}
