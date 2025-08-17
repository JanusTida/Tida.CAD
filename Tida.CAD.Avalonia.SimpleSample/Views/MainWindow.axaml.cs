using Avalonia.Controls;
using System;
using System.Linq;
using System.Reflection;

namespace Tida.CAD.Avalonia.SimpleSample.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        //使用反射获取所有测试命令;
        var types = Assembly.GetExecutingAssembly().GetTypes();
        var testCommandTypes = types.Where(type => type != typeof(ITestCommand) && typeof(ITestCommand).IsAssignableFrom(type) && !type.IsAbstract && !type.IsInterface);
        var testCommands = testCommandTypes.Select(commandType => Activator.CreateInstance(commandType) as ITestCommand).OrderBy(p => p.Order);
        foreach (var testCommand in testCommands.OfType<ITestCommand>())
        {
            var button = new Button
            {
                Content = new Viewbox { Child = new TextBlock { Text = testCommand.Name } }
            };
            button.Click += async(_,_) =>
            {
                var executeContext = new TestExecuteContext
                {
                    MainWindow = this
                };
                await testCommand.ExecuteAsync(executeContext);
            };
            uniformGrid.Children.Add(button);
        }
    }
}
