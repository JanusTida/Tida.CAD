using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using System;

namespace Tida.CAD.Avalonia.SimpleSample;

public partial class DrawObjectDemoWindow : Window
{
    public DrawObjectDemoWindow()
    {
        InitializeComponent();
        this.PointerWheelChanged +=     DrawObjectDemoWindow_PointerWheelChanged;
    }

    private void DrawObjectDemoWindow_PointerWheelChanged(object? sender, PointerWheelEventArgs e)
    {
    }

    private void UniformGrid_PointerWheelChanged(object? sender, PointerWheelEventArgs e)
    {

    }

    private void UniformGrid_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
    }
}