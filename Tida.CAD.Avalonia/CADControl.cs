using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tida.CAD.Events;

namespace Tida.CAD.Avalonia;

public class CADControl : Control, ICADControl
{
    static CADControl()
    {
        LayersProperty.Changed.AddClassHandler<CADControl>((control, args) => control.HandleLayersChanged(args));
        ActiveLayerProperty.Changed.AddClassHandler<CADControl>((control, args) => control.HandleActiveLayerChanged(args));
        ZoomProperty.Changed.AddClassHandler<CADControl>((control, args) => control.HandleZoomChanged(args));
        PanBrushProperty.Changed.AddClassHandler<CADControl>((control, args) => control.HandlePanBrushPropertyChanged(args));
        PanThicknessProperty.Changed.AddClassHandler<CADControl>((control, args) => control.HandlePanThickness(args));
        PanScreenPositionProperty.Changed.AddClassHandler<CADControl>((control, args) => control.HandlePanScreenPositionyChanged(args));
    }

    public CADControl()
    {
        
    }
    private AvaloniaCanvas InternalCanvas => _canvas ??= new AvaloniaCanvas(CADScreenConverter);

    private AvaloniaCanvas? _canvas;

    #region Layers

    public static readonly StyledProperty<IEnumerable<CADLayer>?> LayersProperty =
        AvaloniaProperty.Register<CADControl, IEnumerable<CADLayer>?>(nameof(Layers));
    public IEnumerable<CADLayer>? Layers
    {
        get => GetValue(LayersProperty);
        set => SetValue(LayersProperty, value);
    }

    private void HandleLayersChanged(AvaloniaPropertyChangedEventArgs args)
    {
        var oldLayers = args.OldValue as IEnumerable<CADLayer>;
        var newLayers = args.NewValue as IEnumerable<CADLayer>;

        if (oldLayers != null)
        {
            foreach (var layer in oldLayers)
            {
                UnSetupLayer(layer);
            }

        }

        if (newLayers != null)
        {
            foreach (var layer in newLayers)
            {
                SetupLayer(layer);
            }
        }

        if (oldLayers is INotifyCollectionChanged oldLayerCollection)
        {
            oldLayerCollection.CollectionChanged -= Layers_CollectionChanged;
        }

        if (newLayers is INotifyCollectionChanged newLayerCollection)
        {
            newLayerCollection.CollectionChanged -= Layers_CollectionChanged;
            newLayerCollection.CollectionChanged += Layers_CollectionChanged;
        }
    }

    /// <summary>
    /// Setup the layer;
    /// </summary>
    /// <param name="layer"></param>
    private void SetupLayer(CADLayer layer)
    {
        if(layer == null)
        {
            return;
        }
        var cadLayerVisual = VisualChildren.OfType<CADLayerVisual>().FirstOrDefault(p => p.Layer == layer);
        if (cadLayerVisual != null)
        {
            return;
        }
        var layerContaienrVisual = new CADLayerVisual(layer, InternalCanvas);
        VisualChildren.Add(layerContaienrVisual);
    }

    /// <summary>
    /// Unsetup the layer;
    /// </summary>
    /// <param name="layer"></param>
    private void UnSetupLayer(CADLayer layer)
    {
        if (layer == null)
        {
            return;
        }

        var cadLayerVisual = VisualChildren.OfType<CADLayerVisual>().FirstOrDefault(p => p.Layer == layer);
        if (cadLayerVisual == null)
        {
            return;
        }
        VisualChildren.Remove(cadLayerVisual);
    }

    private void Layers_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (sender is not IEnumerable<CADLayer> layers)
        {
            return;
        }

        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                if (e.NewItems == null)
                {
                    break;
                }
                foreach (var item in e.NewItems)
                {
                    if (item is not CADLayer layer)
                    {
                        continue;
                    }

                    SetupLayer(layer);
                }
                break;


            case NotifyCollectionChangedAction.Remove:
                if (e.OldItems == null)
                {
                    break;
                }
                foreach (var item in e.OldItems)
                {
                    if (item is not CADLayer layer)
                    {
                        continue;
                    }

                    UnSetupLayer(layer);
                }
                break;

            case NotifyCollectionChangedAction.Reset:
                foreach (var layer in e.OldItems?.OfType<CADLayer>() ?? Enumerable.Empty<CADLayer>())
                {
                    UnSetupLayer(layer);
                }

                foreach (var layer in layers ?? Enumerable.Empty<CADLayer>())
                {
                    SetupLayer(layer);
                }
                break;

            case NotifyCollectionChangedAction.Replace:
                break;
            case NotifyCollectionChangedAction.Move:
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// The active layer of the CAD.
    /// </summary>
    public CADLayer? ActiveLayer
    {
        get { return GetValue(ActiveLayerProperty); }
        set { SetValue(ActiveLayerProperty, value); }
    }

    public static readonly StyledProperty<CADLayer?> ActiveLayerProperty =
        AvaloniaProperty.Register<CADControl, CADLayer?>(nameof(ActiveLayer));

    private void HandleActiveLayerChanged(AvaloniaPropertyChangedEventArgs e)
    {
        var oldLayer = e.OldValue as CADLayer;
        var newLayer = e.NewValue as CADLayer;
        var args = new ValueChangedEventArgs<CADLayer>(oldLayer, newLayer);
        ActiveLayerChanged?.Invoke(this, args);
    }
    #endregion

    #region Zoom
    /// <summary>
    /// The minimum value of <see cref="Zoom"/>
    /// </summary>
    public double MinZoom
    {
        get => GetValue(MinZoomProperty); 
        set => SetValue(MinZoomProperty, value);
    }

    /// <summary>
    /// The default value of <see cref="MinZoom"/>
    /// </summary>
    public const double DefaultMinZoom = 0.0005;

    /// <summary>
    /// The minimum zoom value for <see cref="Zoom"/>
    /// </summary>
    public static readonly StyledProperty<double> MinZoomProperty =
        AvaloniaProperty.Register<CADControl, double>(nameof(MinZoom),defaultValue: DefaultMinZoom);


    /// <summary>
    /// The value of zoom level;
    /// </summary>
    public double Zoom
    {
        get => GetValue(ZoomProperty); 
        set => SetValue(ZoomProperty, value);
    }

    /// <summary>
    /// The value of zoom level;
    /// </summary>
    public static readonly StyledProperty<double> ZoomProperty =
        AvaloniaProperty.Register<CADControl, double>(nameof(Zoom),defaultValue: 1D);
    private void HandleZoomChanged(AvaloniaPropertyChangedEventArgs e)
    {
        UpdateCADScreenConverter();
    }

    /// <summary>
    /// Get or set whether user can to change zoom by mouse wheeling;
    /// </summary>
    public bool IsMouseWheelingZoomEnabled
    {
        get { return (bool)GetValue(IsMouseWheelingOnZoomEnabledProperty); }
        set { SetValue(IsMouseWheelingOnZoomEnabledProperty, value); }
    }

    /// <summary>
    /// Get or set whether user can to change zoom by mouse wheeling;
    /// </summary>
    public static readonly StyledProperty<bool> IsMouseWheelingOnZoomEnabledProperty =
        AvaloniaProperty.Register<CADControl, bool>(nameof(IsMouseWheelingZoomEnabled), defaultValue: true);

    #endregion

    #region ICADScreenConverter implementation

    private readonly AvaloniaCADScreenConverter _cadScreenConverter = new();
    public ICADScreenConverter CADScreenConverter => _cadScreenConverter;

    /// <summary>
    /// Update the <see cref="ICADScreenConverter"/>'s properties.
    /// </summary>
    private void UpdateCADScreenConverter(Size? actualSize = null)
    {
        if (actualSize != null)
        {
            _cadScreenConverter.ActualHeight = actualSize.Value.Height;
            _cadScreenConverter.ActualWidth = actualSize.Value.Width;
        }

        _cadScreenConverter.Zoom = Zoom;
        _cadScreenConverter.PanScreenPosition = PanScreenPosition;
    }
    #endregion

    #region Pan(The crosshair in the center of the canvas)

    private Pen? _panPen;
    private void RefreshPanPen()
    {
        if (PanBrush == null || PanThickness <= 0)
        {
            _panPen = null;
        }
        else
        {
            _panPen = new Pen { Brush = PanBrush, Thickness = PanThickness };
        }
    }

    /// <summary>
    /// The length of the crosshair in the center of the canvas.(in pixels)
    /// </summary>
    public double PanLength
    {
        get => GetValue(PanLengthProperty);
        set => SetValue(PanLengthProperty, value);
    }

    public static readonly StyledProperty<double> PanLengthProperty =
        AvaloniaProperty.Register<CADControl, double>(nameof(PanLength), defaultValue: 72.0D);

    /// <summary>
    /// The brush of the crosshair in the center of the canvas.
    /// </summary>
    public Brush PanBrush
    {
        get => GetValue(PanBrushProperty);
        set => SetValue(PanBrushProperty, value);
    }

    public static readonly StyledProperty<Brush> PanBrushProperty =
        AvaloniaProperty.Register<CADControl, Brush>(nameof(PanBrush), defaultValue: Brushes.White);

    private void HandlePanBrushPropertyChanged(AvaloniaPropertyChangedEventArgs e)
    {
        RefreshPanPen();
    }

    /// <summary>
    /// The thickness of the crosshair in the center of the canvas.(in pixels)
    /// </summary>
    public double PanThickness
    {
        get => GetValue(PanThicknessProperty);
        set => SetValue(PanThicknessProperty, value);
    }

    public static readonly StyledProperty<double> PanThicknessProperty =
        AvaloniaProperty.Register<CADControl, double>(nameof(PanThickness), defaultValue: 2.0D);

    private void HandlePanThickness(AvaloniaPropertyChangedEventArgs e)
    {
        RefreshPanPen();
    }

    /// <summary>
    /// The position of the crosshair on the screen.(in pixels)
    /// </summary>
    public Point PanScreenPosition
    {
        get => GetValue(PanScreenPositionProperty);
        set => SetValue(PanScreenPositionProperty, value);
    }


    public static readonly StyledProperty<Point> PanScreenPositionProperty =
        AvaloniaProperty.Register<CADControl, Point>(nameof(PanScreenPosition));

    private void HandlePanScreenPositionyChanged(AvaloniaPropertyChangedEventArgs e)
    {
        UpdateCADScreenConverter();
    }

    #endregion

#pragma warning disable CS0067 // The event is never used
    public event EventHandler<DrawObjectSelectedChangedEventArgs>? DrawObjectIsSelectedChanged;
    public event EventHandler<DrawObjectsRemovedEventArgs>? DrawObjectsRemoved;
    public event EventHandler<DrawObjectsAddedEventArgs>? DrawObjectsAdded;
    public event EventHandler<ValueChangedEventArgs<CADLayer>>? ActiveLayerChanged;
    public event EventHandler<DragSelectEventArgs>? DragSelect;
    public event EventHandler<DragSelectMouseMoveEventArgs>? DrawSelectMouseMove;
    public event EventHandler<ClickSelectingEventArgs>? ClickSelecting;
#pragma warning restore CS0067 // The event is never used

    
}
