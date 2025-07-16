using Avalonia;
using Avalonia.Collections;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tida.CAD.DrawObjects;

namespace Tida.CAD.Avalonia.SimpleSample.ViewModels;

internal class DrawObjectDemoWindowViewModel : BindableBase
{
    public DrawObjectDemoWindowViewModel()
    {
        _layer = new CADLayer();
        Layers.Add(_layer);
    }
    public AvaloniaList<CADLayer> Layers { get; } = [];
    private readonly CADLayer _layer;

    private RelayCommand? _addLineCommand;
    public RelayCommand AddLineCommand => _addLineCommand ??= new RelayCommand(Addline);
    private void Addline()
    {
        var line = new Line
        {
            Start = new Point(0, 0),
            End = new Point(10, 10),
            Pen = new Pen { Thickness = 2, Brush = Brushes.White }
        };
        _layer.AddDrawObject(line);
    }


    private RelayCommand? _addRectCommand;
    public RelayCommand AddRectCommand => _addRectCommand ??= new RelayCommand(AddRect);
    private void AddRect()
    {
        var rectPen = new Pen(Brushes.White, 2);
        var rectBackground = Brushes.Orange;

        var rect = new Rectangle(new CADRect(new Point(-2, -2), new Size(4, 4)))
        {
            Pen = rectPen,
            IsSelected = true,
            Background = rectBackground
        };

        _layer.AddDrawObject(rect);
    }

    private RelayCommand? _clearCommand;
    public RelayCommand ClearCommand => _clearCommand ??= new RelayCommand(Clear);
    private void Clear()
    {
        _layer.Clear();
    }

    private RelayCommand? _changeLayerBackgroundCommand;
    public RelayCommand ChangeLayerBackgroundCommand => _changeLayerBackgroundCommand ??= new RelayCommand(ChangeLayerBackground);
    private void ChangeLayerBackground()
    {
        if (_layer.Background == null)
        {
            _layer.Background = Brushes.Blue;
        }
        else
        {
            _layer.Background = null;
        }
    }


    private RelayCommand? _addPolygonCommand;
    public RelayCommand AddPolygonCommand => _addPolygonCommand ??= new RelayCommand(AddPolygon);
    private void AddPolygon()
    {
        var polygon = new Polygon
        {
            Points = new[]
            {
                new Point(2,0),
                new Point(4,0),
                new Point(6,2),
                new Point(6,4),
                new Point(4,6),
                new Point(2,6),
                new Point(0,4),
                new Point(0,2),
                new Point(2,0)
            },
            Pen = new Pen(Brushes.White, 2),
            Brush = null
        };
        _layer.AddDrawObject(polygon);
    }

    private RelayCommand? _addArcCommand;
    public RelayCommand AddArcCommand => _addArcCommand ??= new RelayCommand(AddArc);
    private void AddArc()
    {
        _layer.AddDrawObject
        (
            new Arc
            {
                Pen = new Pen { Brush = Brushes.White, Thickness = 2 },
                Center = new Point(0, 0),
                Radius = 2,
                BeginAngle = 0,
                Angle = ConvertDegreesToRadians(185.0)
            }
        );
    }

    private static double ConvertDegreesToRadians(double v)
    {
        return v / 180 * Math.PI;
    }


    private RelayCommand? _addTextCommand;
    public RelayCommand AddTextCommand => _addTextCommand ??= new RelayCommand(AddText);
    private void AddText()
    {
        var text = new Text
        {
            Content = "Hello World",
            Position = new Point(0, 0),
            FontSize = 14
        };
        _layer.AddDrawObject(text);
    }
}
