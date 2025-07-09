using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Tida.CAD.DrawObjects;

namespace Tida.CAD.WPF.SimpleSample;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
 
    public MainWindow()
    {
        InitializeComponent();

        //使用反射获取所有测试命令;
        var types = Assembly.GetExecutingAssembly().GetTypes();
        var testCommandTypes = types.Where(type => type != typeof(ITestCommand) && typeof(ITestCommand).IsAssignableFrom(type));
        var testCommands = testCommandTypes.Select(commandType => Activator.CreateInstance(commandType) as ITestCommand).OrderBy(p => p.Order);
        foreach (var testCommand in testCommands)
        {
            var button = new Button
            {
                Content = new Viewbox { Child = new TextBlock { Text = testCommand.Name } }
            };
            button.Click += delegate
            {
                var executeContext = new TestExecuteContext
                {
                    MainWindow = this
                };
                testCommand.Execute(executeContext);
            };
            uniformGrid.Children.Add(button);
        }

    }
}
