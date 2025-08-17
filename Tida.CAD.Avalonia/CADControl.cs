using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.VisualTree;
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

        PanBrushProperty.Changed.AddClassHandler<CADControl>((control, args) => control.HandlePanBrushChanged(args));
        PanThicknessProperty.Changed.AddClassHandler<CADControl>((control, args) => control.HandlePanThickness(args));
        PanScreenPositionProperty.Changed.AddClassHandler<CADControl>((control, args) => control.HandlePanScreenPositionChanged(args));

        GridsBrushProperty.Changed.AddClassHandler<CADControl>((control, args) => control.HandleGridsBrushChanged(args));
        GirdsThicknessProperty.Changed.AddClassHandler<CADControl>((control, args) => control.HandleGridsThicknessChanged(args));
        ShowGridsProperty.Changed.AddClassHandler<CADControl>((control, args) => control.HandleShowGridsChanged(args));
    }

    public CADControl()
    {
        _canvas = new AvaloniaCanvas(_cadScreenConverter);
        _gridsVisual = new GridsVisual(_cadScreenConverter);
        _panVisual = new PanVisual(_cadScreenConverter);
        _layersContainerVisual = new LayersContainerVisual(_canvas);
        VisualChildren.Add(_gridsVisual);
        VisualChildren.Add(_layersContainerVisual);
        VisualChildren.Add(_panVisual);
        
        _panVisual.RefreshPanPen();
        _gridsVisual.RefreshGridsPen();
    }

    private readonly AvaloniaCanvas _canvas;
    protected override Size ArrangeOverride(Size finalSize)
    {
        SuspendVisualInvalidation();
        InitializePanScreenPosition(finalSize);
        UpdateCADScreenConverter(finalSize);
        SetAllVisualInvalidationTypes();
        ResumeVisualInvalidation();
        return base.ArrangeOverride(finalSize);
    }

    public override void Render(DrawingContext context)
    {
        //This is necessary to receive mouse events;
        context.FillRectangle(Brushes.Transparent, new Rect(0, 0, Bounds.Width, Bounds.Height));
        base.Render(context);
    }

    private bool _isVisualInvalidationSuspended = false;

    /// <summary>
    /// The visual invalidation type.
    /// </summary>
    [Flags]
    enum VisualInvalidationType
    {
        None = 0,
        Pan = 1,
        Grids = 2,
        Layers = 4
    }
    private VisualInvalidationType _visualInvalidationType = VisualInvalidationType.None;
    private void SuspendVisualInvalidation()
    {
        _isVisualInvalidationSuspended = true;
    }
    private void ResumeVisualInvalidation()
    {
        _isVisualInvalidationSuspended = false;
        if (_visualInvalidationType.HasFlag(VisualInvalidationType.Pan))
        {
            _panVisual.InvalidateVisual();
        }
        if (_visualInvalidationType.HasFlag(VisualInvalidationType.Grids))
        {
            _gridsVisual.InvalidateVisual();
        }
        if (_visualInvalidationType.HasFlag(VisualInvalidationType.Layers))
        {
            _layersContainerVisual.InvalidateVisuals();
        }
        _visualInvalidationType = VisualInvalidationType.None;
    }
    private void SetAllVisualInvalidationTypes()
    {
        _visualInvalidationType = VisualInvalidationType.Pan | VisualInvalidationType.Grids | VisualInvalidationType.Layers;
    }

    #region Layers
    private readonly LayersContainerVisual _layersContainerVisual;
    public static readonly StyledProperty<IEnumerable<CADLayer>?> LayersProperty =
        AvaloniaProperty.Register<CADControl, IEnumerable<CADLayer>?>(nameof(Layers));
    public IEnumerable<CADLayer>? Layers
    {
        get => GetValue(LayersProperty);
        set => SetValue(LayersProperty, value);
    }

    private void HandleLayersChanged(AvaloniaPropertyChangedEventArgs args)
    {
        _layersContainerVisual.Layers = Layers;
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
        SetAllVisualInvalidationTypes();
        if (_isVisualInvalidationSuspended)
        {
            return;
        }
        ResumeVisualInvalidation();
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

    /// <summary>
    /// To change the zoom level on mouse wheeling;
    /// </summary>
    /// <param name="e"></param>
    private bool PointerWheelOnZoom(PointerWheelEventArgs e)
    {
        if (e.Delta.Y == 0)
        {
            return false;
        }
        if (!IsMouseWheelingZoomEnabled)
        {
            return false;
        }
        var mouseScreenPos = e.GetPosition(this);
        var mouseUnitPos = CADScreenConverter.ToCAD(mouseScreenPos);
        SuspendVisualInvalidation();
        if(e.Delta.Y > 0)
        {
            Zoom *= 1.1;
        }
        else
        {
            Zoom /= 1.1;
        }
        if (Zoom < MinZoom)
        {
            Zoom = MinZoom;
        }
        MoveUnitPositionToScreenPosition(mouseUnitPos, mouseScreenPos);
        ResumeVisualInvalidation();
        return false;
    }
    /// <summary>
    /// Put the mouse cursor at the specified screen position, and adjust the origin offset to the specified unit position.
    /// </summary>
    /// <param name="unitPos"></param>
    /// <param name="screenPos"></param>
    private void MoveUnitPositionToScreenPosition
    (
        Point unitPos,
        Point screenPos
    )
    {
        var unitPosInScreen = CADScreenConverter.ToScreen(unitPos);
        //通过调整原点的视图偏移实现;
        PanScreenPosition = new Point(
            PanScreenPosition.X + screenPos.X - unitPosInScreen.X,
            PanScreenPosition.Y + screenPos.Y - unitPosInScreen.Y
        );
    }
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
    private readonly PanVisual _panVisual;

    /// <summary>
    /// The length of the crosshair in the center of the canvas.(in pixels)
    /// </summary>
    public double PanLength
    {
        get => GetValue(PanLengthProperty);
        set => SetValue(PanLengthProperty, value);
    }

    public static readonly StyledProperty<double> PanLengthProperty =
        AvaloniaProperty.Register<CADControl, double>(nameof(PanLength), defaultValue: PanVisual.DefaultPanLength);

    /// <summary>
    /// The brush of the crosshair in the center of the canvas.
    /// </summary>
    public Brush PanBrush
    {
        get => GetValue(PanBrushProperty);
        set => SetValue(PanBrushProperty, value);
    }

    public static readonly StyledProperty<Brush> PanBrushProperty =
        AvaloniaProperty.Register<CADControl, Brush>(nameof(PanBrush), defaultValue: PanVisual.DefaultPanBrush);

    private void HandlePanBrushChanged(AvaloniaPropertyChangedEventArgs e)
    {
        _panVisual.PanBrush = PanBrush;
        _panVisual.RefreshPanPen();
        _panVisual.InvalidateVisual();
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
        AvaloniaProperty.Register<CADControl, double>(nameof(PanThickness), defaultValue: PanVisual.DefaultPanThickness);

    private void HandlePanThickness(AvaloniaPropertyChangedEventArgs _)
    {
        _panVisual.PanThickness = PanThickness;
        _panVisual.RefreshPanPen();
        _panVisual.InvalidateVisual();
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

    private void HandlePanScreenPositionChanged(AvaloniaPropertyChangedEventArgs _)
    {
        UpdateCADScreenConverter();
        SetAllVisualInvalidationTypes();
        if (_isVisualInvalidationSuspended)
        {
            return;
        }
        ResumeVisualInvalidation();
    }

    private bool _panInitialized;
    /// <summary>
    /// Initialize PanScreenPosition the first time control is being arranged;
    /// </summary>
    private void InitializePanScreenPosition(Size actualSize)
    {
        if (_panInitialized)
        {
            return;
        }
        PanScreenPosition = new Point(actualSize.Width / 2, actualSize.Height / 2);
        _panInitialized = true;
    }
    #endregion

    #region Grids
    private readonly GridsVisual _gridsVisual;
    /// <summary>
    /// The brush of grid lines;
    /// </summary>
    public Brush GridsBrush
    {
        get { return (Brush)GetValue(GridsBrushProperty); }
        set { SetValue(GridsBrushProperty, value); }
    }

    public static readonly StyledProperty<Brush> GridsBrushProperty =
        AvaloniaProperty.Register<CADControl,Brush>(nameof(GridsBrush), defaultValue:GridsVisual.DefaultGridsBrush);
    private void HandleGridsBrushChanged(AvaloniaPropertyChangedEventArgs _)
    {
        _gridsVisual.GridsBrush = GridsBrush;
        _gridsVisual.RefreshGridsPen();
        _gridsVisual.InvalidateVisual();
    }
    
    /// <summary>
    /// The thickness of grid lines;
    /// </summary>
    public double GridsThickness
    {
        get { return (double)GetValue(GirdsThicknessProperty); }
        set { SetValue(GirdsThicknessProperty, value); }
    }

    public static readonly StyledProperty<double> GirdsThicknessProperty =
        AvaloniaProperty.Register<CADControl,double>(nameof(GridsThickness),defaultValue:GridsVisual.DefaultGridsThickness);

    private void HandleGridsThicknessChanged(AvaloniaPropertyChangedEventArgs _)
    {
        _gridsVisual.GridsThickness = GridsThickness;
        _gridsVisual.RefreshGridsPen();
        _gridsVisual.InvalidateVisual();
    }


    /// <summary>
    /// Get or set whether grid lines should be shown;
    /// </summary>
    public bool ShowGrids
    {
        get { return GetValue(ShowGridsProperty); }
        set { SetValue(ShowGridsProperty, value); }
    }

    public static readonly StyledProperty<bool> ShowGridsProperty =
        AvaloniaProperty.Register<CADControl,bool>(nameof(ShowGrids), defaultValue: true);

    private void HandleShowGridsChanged(AvaloniaPropertyChangedEventArgs _)
    {
        _gridsVisual.IsVisible = ShowGrids;
        _gridsVisual.RefreshGridsPen();
        _gridsVisual.InvalidateVisual();
    }

    #endregion

    #region Mouse and keyboard events

    /// <summary>
    /// Handle the routed event by iterating through the handler collection and handling each element until a handler indicates that it has been handled;
    /// </summary>
    /// <typeparam name="TEventArgs">事件参数类型</typeparam>
    /// <param name="e">事件参数</param>
    /// <param name="handlers">处理器集合</param>
    private void HandleRoutedEvent<TEventArgs>(
        TEventArgs e,
        IEnumerable<Predicate<TEventArgs>> handlers
    ) where TEventArgs : RoutedEventArgs
    {
        if (handlers == null)
        {
            return;
        }

        foreach (var handler in handlers)
        {
            if (handler(e))
            {
                break;
            }
        }

    }
    
    /// <summary>
    /// Get all mouse wheel event handlers;
    /// </summary>
    /// <returns></returns>
    private IEnumerable<Predicate<PointerWheelEventArgs>> GetPointerWheelEventHandlers()
    {
        //缩放响应;
        yield return PointerWheelOnZoom;
    }
    protected override void OnPointerWheelChanged(PointerWheelEventArgs e)
    {
        HandleRoutedEvent(e, GetPointerWheelEventHandlers());
        base.OnPointerWheelChanged(e);
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
    }
    protected override void OnPointerMoved(PointerEventArgs e)
    {
        base.OnPointerMoved(e);
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
