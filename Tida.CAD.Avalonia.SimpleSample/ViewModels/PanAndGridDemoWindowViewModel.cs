using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.CAD.Avalonia.SimpleSample.ViewModels;

class PanAndGridDemoWindowViewModel : BindableBase
{
	private double _minZoom;
	public double MinZoom
    {
		get => _minZoom;
		set => SetProperty(ref _minZoom, value);
	}

	private double _maxZoom = 10;
	public double MaxZoom
	{
		get => _maxZoom;
		set => SetProperty(ref _maxZoom, value);
	}

	private double _zoom = 1.0;
	public  double Zoom
	{
		get => _zoom;
		set => SetProperty(ref _zoom, value);
	}

    private bool _isMouseWheelingZoomEnabled;
    public bool IsMouseWheelingZoomEnabled
    {
        get => _isMouseWheelingZoomEnabled;
        set => SetProperty(ref _isMouseWheelingZoomEnabled, value);
    }

    private double _panThickness;
	public double PanThickness
	{
		get => _panThickness;
		set => SetProperty(ref _panThickness, value);
	}

	private IBrush? _panBrush;
	public IBrush? PanBrush => _panBrush;
	private Color _panColor;
	public Color PanColor
	{
		get => _panColor;
		set
		{
			SetProperty(ref _panColor, value);
			_panBrush = new SolidColorBrush(value).ToImmutable();
			OnPropertyChanged(nameof(PanBrush));
        }
	}

	private double _panLength;
	public double PanLength
	{
		get => _panLength;
		set => SetProperty(ref _panLength, value);
    }

	private IBrush? _gridsBrush;
	public IBrush? GridsBrush => _gridsBrush;
	private Color _gridsColor;
	public Color GridsColor
	{
		get => _gridsColor;
		set
		{
			SetProperty(ref _gridsColor, value);
			_gridsBrush = new SolidColorBrush(value).ToImmutable();
			OnPropertyChanged(nameof(GridsBrush));
		}
    }
	private double _gridsThickness;
	public double GridsThickness
	{
		get => _gridsThickness;
		set => SetProperty(ref _gridsThickness, value);
    }

}
