using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Linq;
using System.Collections.Specialized;
using System.Windows.Input;
using System.Windows.Controls;
using Tida.CAD;
using Tida.CAD.Events;
using Tida.CAD.Extensions;
using System.Diagnostics;
using Tida.CAD.Input;

namespace Tida.CAD.WPF {

    /// <summary>
    /// <see cref="ICADControl"/> implemented with WPF;
    /// </summary>
    public partial class CADControl : Panel, ICADControl {
        static CADControl() {
            
            BackgroundProperty.OverrideMetadata(typeof(CADControl), new FrameworkPropertyMetadata(
                Constants.DefaultCanvasBackground,
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits
            ));
            
        }

        public CADControl() {
            this.Children.Add(VisualContainer);
            VisualContainer.AddVisual(_dragSelectionContainerVisual);
            this.Focusable = true;
            
            RefreshPanPen();
            RefreshGridLinePen();
        }
        
        protected readonly VisualContainer VisualContainer = new VisualContainer();
        private readonly Dictionary<CADLayer, ContainerVisual> _layerContainerVisualDict = new Dictionary<CADLayer, ContainerVisual>();
        private readonly ContainerVisual _dragSelectionContainerVisual = new ContainerVisual();
        
        /// <summary>
        /// <see cref="ICanvas"/> implemented with WPF;
        /// </summary>
        private WPFCanvas InternalCanvas => _canvas ?? (_canvas = new WPFCanvas(this.CADScreenConverter));
        
        private WPFCanvas _canvas;
        
        /// <summary>
        /// 记录鼠标按下的位置,拖拽画布时使用;
        /// </summary>
        private Point _lastMouseDownPointForPan;
        /// <summary>
        /// 记录是否正在被拖拽;
        /// </summary>
        private bool _isDragging = false;
        /// <summary>
        /// 记录未拖动前原点的视图坐标(用于拖拽进行中的时候);
        /// </summary>
        private Point _lastPanOffsetBeforeDragging;

        /// <summary>
        /// 可绘制对象与WPF DrawingVisual缓存;
        /// </summary>
        private readonly Dictionary<IDrawable, DrawingVisual> _visualDict = new Dictionary<IDrawable, DrawingVisual>();

        /// <summary>
        /// 当前被悬停的绘制对象集合;
        /// </summary>
        private readonly List<DrawObject> _hoveredDrawObjects = new List<DrawObject>();

        /// <summary>
        /// 内部存储维护的所有图层集合;
        /// </summary>
        private readonly List<CADLayer> _CADLayers = new List<CADLayer>();

        /// <summary>
        /// 记录上一次鼠标按下的位置,在拖放选择时使用;
        /// </summary>
        private Point? _lastMouseDownPositionForDragSelecting;

        /// <summary>
        /// 拖放选择时所呈现的矩形对象;
        /// </summary>
        private readonly SimpleRectangle _dragSelectRectangle = new SimpleRectangle();

        /// <summary>
        /// 拖放选取时是否为任意选中;
        /// </summary>
        private bool _anyPointSelectForDragSelect;


        /// <summary>
        /// 画布内绘制对象选定状态发生了变化事件;
        /// </summary>
        public event EventHandler<DrawObjectSelectedChangedEventArgs> DrawObjectIsSelectedChanged;
        

        /// <summary>
        /// 绘制对象被移除;
        /// </summary>
        public event EventHandler<DrawObjectsRemovedEventArgs> DrawObjectsRemoved;

        /// <summary>
        /// 绘制对象被添加;
        /// </summary>
        public event EventHandler<DrawObjectsAddedEventArgs> DrawObjectsAdded;

        /// <summary>
        /// 拖拽选择事件;
        /// </summary>
        public event EventHandler<DragSelectEventArgs> DragSelect;

        /// <summary>
        /// 拖拽选择鼠标移动事件;
        /// </summary>
        public event EventHandler<DragSelectMouseMoveEventArgs> DrawSelectMouseMove;

        /// <summary>
        /// 点击选取事件;
        /// </summary>
        public event EventHandler<ClickSelectEventArgs> ClickSelect;

        private bool _panInitialized;
        /// <summary>
        /// Initialize PanScreenPosition the first time control is being arranged;
        /// </summary>
        private void InitializePanScreenPosition(Size actualSize) {
            if (_panInitialized)
            {
                return;
            }
            PanScreenPosition = new Point(actualSize.Width / 2, actualSize.Height / 2);
            _panInitialized = true;
        }

        /// <summary>
        /// 通过调整原点的视图偏移,使得某个某个视图坐标的某工程坐标点处于视图中心的位置;
        /// </summary>
        /// <param name="screenPoint"></param>
        protected void SetCenterScreen(Point screenPoint) {
            PanScreenPosition = new Point(
                PanScreenPosition.X + this.ActualWidth / 2 - screenPoint.X,
                PanScreenPosition.Y + this.ActualHeight / 2 - screenPoint.Y
            );
        }

        /// <summary>
        /// 重写绘制函数;
        /// </summary>
        /// <param name="drawingContext"></param>
        protected override void OnRender(DrawingContext drawingContext) {
            base.OnRender(drawingContext);
            
            //绘制背景;
            //DrawBackground(drawingContext);
            //绘制网格;
            DrawGridLines(drawingContext);
            //绘制原点十字;
            DrawPan(drawingContext);

            AddSelectRectangleToDict();
            /*重绘元素*/

            //因为尺寸可能发生了变化,故需重新设定裁剪区域;
            var clipGeometry = new RectangleGeometry(
                new Rect {
                    Width = this.ActualWidth,
                    Height = this.ActualHeight
                }
            );
            
            foreach (var pair in _visualDict) {
                pair.Value.Clip = clipGeometry;
                DrawDrawableCore(pair.Key, pair.Value);
            }
        }
        
        protected override Size ArrangeOverride(Size arrangeSize) {
            InitializePanScreenPosition(arrangeSize);
            UpdateCanvasScreenConverter(arrangeSize);
            return base.ArrangeOverride(arrangeSize);
        }
    }

    /// <summary>
    /// <see cref="ICanvasScreenConvertable"/>相关成员;
    /// </summary>
    public partial class CADControl {

        private static readonly DependencyPropertyKey CADScreenConverterPropertyKey = DependencyProperty.RegisterReadOnly(nameof(CADScreenConverter), typeof(ICADScreenConverter), typeof(CADControl), new PropertyMetadata());
        public static readonly DependencyProperty CADScreenConverterProperty = CADScreenConverterPropertyKey.DependencyProperty;

     
        private readonly WPFCADScreenConverter _cadScreenConverter = new WPFCADScreenConverter();

        /// <summary>
        /// 画布坐标转化实例;
        /// </summary>
        public ICADScreenConverter CADScreenConverter {
            get => _cadScreenConverter;
            set => throw new NotSupportedException($"The {nameof(CADScreenConverter)} property is readonly!");
        }

        /// <summary>
        /// 更新<see cref="_cadScreenConverter"/>中的关键参数;
        /// </summary>
        private void UpdateCanvasScreenConverter(Size? actualSize = null) {
            if(actualSize != null)
            {
                _cadScreenConverter.ActualHeight = actualSize.Value.Height;
                _cadScreenConverter.ActualWidth = actualSize.Value.Width;
            }

            _cadScreenConverter.Zoom = this.Zoom;
            _cadScreenConverter.PanScreenPosition = this.PanScreenPosition;
        }

    }

    /// <summary>
    /// 鼠标,键盘动作的重写;
    /// </summary>
    public partial class CADControl
    {
        /// <summary>
        /// 处理路由事件的方法,遍历处理器集合中的每一个元素进行处理,直到某一个处理器指示已被处理;
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
        /// 鼠标滚动;
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
        {
            HandleRoutedEvent(e, GetMouseWheelEventHandlers());
            base.OnPreviewMouseWheel(e);
        }

        /// <summary>
        /// 获取内部所有鼠标滚动的处理器;
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Predicate<MouseWheelEventArgs>> GetMouseWheelEventHandlers()
        {
            //缩放响应;
            yield return MouseWheelOnZoom;
        }

        /// <summary>
        /// 鼠标按下;
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            this.Focus();
            HandleRoutedEvent(e, GetMouseDownEventHandlers());
            base.OnPreviewMouseDown(e);
        }

        /// <summary>
        /// 获取内部所有鼠标按下的处理器;
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Predicate<MouseButtonEventArgs>> GetMouseDownEventHandlers()
        {
            //通知外部;
            yield return MouseDownOnPreview;
            //拖拽响应;
            yield return MouseDownOnDrag;
            //被选取对象的交互操作;
            yield return MouseDownOnSelectedDrawObjects;
            //拖放选中响应;
            yield return MouseDownOnDragingSelectDrawObject;
            //选中响应;
            yield return MouseDownOnSelectDrawObject;
        }

        /// <summary>
        /// 鼠标移动;
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            HandleRoutedEvent(e, GetMouseMoveEventHandlers());
            base.OnPreviewMouseMove(e);
        }

        /// <summary>
        /// 获取内部所有鼠标移动的处理器;
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Predicate<MouseEventArgs>> GetMouseMoveEventHandlers()
        {
            //通知外界;
            yield return MouseMoveOnPreview;

            //通知鼠标当前的位置节点;
            yield return MouseMoveOnCurrentMousePosition;

            //拖拽响应;
            yield return MouseMoveOnDrag;

            //被选取对象的交互操作;
            yield return MouseMoveOnSelectedDrawObjects;

            //选取响应;
            yield return MouseMoveOnSelectDrawObject;

            //拖拽选取响应;
            yield return MouseMoveOnDragingSelectDrawObject;

        }

        /// <summary>
        /// 鼠标弹起;
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreviewMouseUp(MouseButtonEventArgs e)
        {
            HandleRoutedEvent(e, GetMouseUpEventHandlers());
            base.OnPreviewMouseUp(e);
        }

        /// <summary>
        /// 获取内部所有鼠标弹起的处理器;
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Predicate<MouseButtonEventArgs>> GetMouseUpEventHandlers()
        {
            //通知外界;
            yield return MouseUpOnPreview;

            //拖拽响应;
            yield return MouseUpOnDrag;
        }

        /// <summary>
        /// 键盘按下;
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            HandleRoutedEvent(e, GetKeyDownEventHandlers());
            base.OnPreviewKeyDown(e);

        }

        protected override void OnKeyDown(KeyEventArgs e) {
            //因为Tab键会导致键盘焦点转移,所以需指示处理了Tab键;
            if (e.Key == Key.Tab) {
                e.Handled = true;
            }
            base.OnKeyDown(e);
        }

        /// <summary>
        /// 获取内部所有键盘按下的处理器;
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Predicate<KeyEventArgs>> GetKeyDownEventHandlers()
        {
            //通知外界;
            yield return KeyDownOnPreview;

            //被选取对象的交互操作;
            yield return KeyDownOnSelectedDrawObjects;

            //拖拽选取响应;
            yield return KeyDownOnDragingSelectDrawObject;

            //选取响应;
            yield return KeyDownOnSelectDrawObject;
        }

        /// <summary>
        /// 键盘弹起;
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreviewKeyUp(KeyEventArgs e)
        {
            HandleRoutedEvent(e, GetKeyUpEventHandlers());
            base.OnPreviewKeyUp(e);
        }

        /// <summary>
        /// 获取内部所有键盘弹起的处理器;
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Predicate<KeyEventArgs>> GetKeyUpEventHandlers()
        {
            //通知外界;
            yield return KeyUpOnPreview;

            //与被选取对对象的交互操作;
            yield return KeyUpOnSelectedDrawObjects;

        }

        protected override void OnPreviewTextInput(TextCompositionEventArgs e) {
            base.OnPreviewTextInput(e);
        }
        protected override void OnTextInput(TextCompositionEventArgs e) {
            HandleRoutedEvent(e, GetTextInputEventHandlers());
            base.OnTextInput(e);
        }

        /// <summary>
        /// 获取内部所有键盘键入文字事件;
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Predicate<TextCompositionEventArgs>> GetTextInputEventHandlers() 
        {
            
            //通知外界;
            yield return TextInputOnPreview;

        }
    }

    /// <summary>
    /// 网格部分;
    /// </summary>
    public partial class CADControl
    {
        private Pen _gridLinePen;
        private void RefreshGridLinePen()
        {
            if(GridLineBrush == null || GridLineThickness <= 0 || !ShowGridLines)
            {
                _gridLinePen = null;
                return;
            }

            _gridLinePen = new Pen { Brush = GridLineBrush,Thickness = GridLineThickness };
            _gridLinePen.Freeze();
        }

        /// <summary>
        /// The brush of grid lines;
        /// </summary>
        public Brush GridLineBrush
        {
            get { return (Brush)GetValue(GridLineBrushProperty); }
            set { SetValue(GridLineBrushProperty, value); }
        }

        private static readonly Brush DefaultGridLineBrush = new SolidColorBrush
        {
            Color = Color.FromArgb(230, 80, 80, 80)
        };
        public static readonly DependencyProperty GridLineBrushProperty =
            DependencyProperty.Register(nameof(GridLineBrush), typeof(Brush), typeof(CADControl), new FrameworkPropertyMetadata(DefaultGridLineBrush, FrameworkPropertyMetadataOptions.AffectsRender, GridLineBrush_PropertyChanged));

        private static void GridLineBrush_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is CADControl canvasControl))
            {
                return;
            }
            canvasControl.RefreshGridLinePen();
        }

        /// <summary>
        /// The thickness of grid lines;
        /// </summary>
        public double GridLineThickness
        {
            get { return (double)GetValue(GirdLineThicknessProperty); }
            set { SetValue(GirdLineThicknessProperty, value); }
        }

        public static readonly DependencyProperty GirdLineThicknessProperty =
            DependencyProperty.Register(nameof(GridLineThickness), typeof(double), typeof(CADControl), new FrameworkPropertyMetadata(0.2D,FrameworkPropertyMetadataOptions.AffectsRender,GridLineThickness_PropertyChanged));

        private static void GridLineThickness_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is CADControl canvasControl))
            {
                return;
            }
            canvasControl.RefreshGridLinePen();
        }


        /// <summary>
        /// Get or set whether grid lines should be shown;
        /// </summary>
        public bool ShowGridLines
        {   
            get { return (bool)GetValue(ShowGridLinesProperty); }
            set { SetValue(ShowGridLinesProperty, value); }
        }

        public static readonly DependencyProperty ShowGridLinesProperty =
            DependencyProperty.Register(nameof(ShowGridLines), typeof(bool), typeof(CADControl), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsRender, ShowGridLines_PropertyChanged));

        private static void ShowGridLines_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is CADControl canvasControl))
            {
                return;
            }
            canvasControl.RefreshGridLinePen();
        }

        /// <summary>
        /// 绘制网格;
        /// </summary>
        private void DrawGridLines(DrawingContext drawingContext)
        {
            //获得单元格的边长视图大小;
            var unitLength = CADScreenConverter.ToScreen(1);
            if (unitLength < 3)
            {
                return;
            }
            if(_gridLinePen == null)
            {
                return;
            }
            
            //为避免多次构造Point，只构造两个实例以重复利用;
            var point0 = new Point();
            var point1 = new Point();

            #region  绘制竖线;
            point0.Y = 0;
            point1.Y = this.ActualHeight;


            var horiPos = PanScreenPosition.X % unitLength;
            //递增至超出可见范围;
            while (horiPos <= this.ActualWidth)
            {
                point0.X = horiPos;
                point1.X = horiPos;
                drawingContext.DrawLine(_gridLinePen, point0, point1);
                horiPos += unitLength;
            }
            #endregion

            #region 绘制横线;
            point0.X = 0;
            point1.X = this.ActualWidth;

            var vertiPos = PanScreenPosition.Y % unitLength;
            //递增至超出可见范围;
            while (vertiPos <= this.ActualHeight)
            {
                point0.Y = vertiPos;
                point1.Y = vertiPos;
                drawingContext.DrawLine(_gridLinePen, point0, point1);
                vertiPos += unitLength;
            }
            #endregion
        }
    }

    /// <summary>
    /// 原点部分;
    /// </summary>
    public partial class CADControl
    {
        private Pen _panPen;
        private void RefreshPanPen()
        {
            if(PanBrush == null || PanThickness <= 0)
            {
                _panPen = null;
            }
            else
            {
                _panPen = new Pen { Brush = PanBrush, Thickness = PanThickness };
                _panPen.Freeze();
            }
        }
        /// <summary>
        /// 原点的十字边长;
        /// </summary>
        public double PanLength
        {
            get { return (double)GetValue(PanLengthProperty); }
            set { SetValue(PanLengthProperty, value); }
        }


        public static readonly DependencyProperty PanLengthProperty =
            DependencyProperty.Register(nameof(PanLength), typeof(double), typeof(CADControl), new PropertyMetadata(72.0D));


        /// <summary>
        /// 原点的十字画刷;
        /// </summary>
        public Brush PanBrush
        {
            get { return (Brush)GetValue(PanBrushProperty); }
            set { SetValue(PanBrushProperty, value); }
        }

        public static readonly DependencyProperty PanBrushProperty =
            DependencyProperty.Register(nameof(PanBrush), typeof(Brush), typeof(CADControl), new PropertyMetadata(Brushes.White,PanBrush_PropertyChanged));

        private static void PanBrush_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is CADControl canvasControl))
            {
                return;
            }
            canvasControl.RefreshPanPen();
        }


        /// <summary>
        /// 原点的十字宽度;
        /// </summary>
        public double PanThickness
        {
            get { return (double)GetValue(PanThicknessProperty); }
            set { SetValue(PanThicknessProperty, value); }
        }

        public static readonly DependencyProperty PanThicknessProperty =
            DependencyProperty.Register(nameof(PanThickness), typeof(double), typeof(CADControl), new PropertyMetadata(2D,PanThickness_PropertyChanged));

        private static void PanThickness_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is CADControl canvasControl))
            {
                return;
            }
            canvasControl.RefreshPanPen();
        }


        /// <summary>
        /// 原点所在的视图坐标;
        /// </summary>
        public Point PanScreenPosition {
            get { return (Point)GetValue(PanScreenPositionProperty); }
            set { SetValue(PanScreenPositionProperty, value); }
        }

        
        public static readonly DependencyProperty PanScreenPositionProperty =
            DependencyProperty.Register(nameof(PanScreenPosition), typeof(Point), typeof(CADControl),
                new FrameworkPropertyMetadata(default(Point),
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsRender,
                    PanScreenPosition_PropertyChanged));

        private static void PanScreenPosition_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) 
        {
            if(!(d is CADControl canvasControl)) {
                return;
            }

            canvasControl.UpdateCanvasScreenConverter();
        }


        /// <summary>
        /// 绘制原点十字;
        /// </summary>
        /// <param name="drawingContext"></param>
        private void DrawPan(DrawingContext drawingContext)
        {
            if(_panPen == null)
            {
                return;
            }

            drawingContext.DrawLine(
                _panPen,
                new Point(PanScreenPosition.X - PanLength / 2, PanScreenPosition.Y),
                new Point(PanScreenPosition.X + PanLength / 2, PanScreenPosition.Y)
            );

            drawingContext.DrawLine(
                _panPen,
                new Point(PanScreenPosition.X, PanScreenPosition.Y - PanLength / 2),
                new Point(PanScreenPosition.X, PanScreenPosition.Y + PanLength / 2)
            );

        }
    }

    /// <summary>
    /// 缩放部分;
    /// </summary>
    public partial class CADControl
    {
        /// <summary>
        /// 最小的放大等级;
        /// </summary>
        public double MinZoom
        {
            get { return (double)GetValue(MinZoomProperty); }
            set { SetValue(MinZoomProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MinZoom.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinZoomProperty =
            DependencyProperty.Register(nameof(MinZoom), typeof(double), typeof(CADControl), new PropertyMetadata(0.0005));


        /// <summary>
        /// 缩放等级;
        /// </summary>
        public double Zoom
        {
            get { return (double)GetValue(ZoomProperty); }
            set { SetValue(ZoomProperty, value); }
        }

        public static readonly DependencyProperty ZoomProperty =
            DependencyProperty.Register(
                nameof(Zoom),
                typeof(double),
                typeof(CADControl),
                new FrameworkPropertyMetadata(1D, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Zoom_PropertyChanged)
            );

        private static void Zoom_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs args) {
            if(!(d is CADControl canvasControl)) {
                return;
            }

            canvasControl.UpdateCanvasScreenConverter();
        }


        /// <summary>
        /// Get or set is zoom enabled;
        /// </summary>
        public bool IsZoomEnabled
        {
            get { return (bool)GetValue(IsZoomEnabledProperty); }
            set { SetValue(IsZoomEnabledProperty, value); }
        }

        public static readonly DependencyProperty IsZoomEnabledProperty =
            DependencyProperty.Register(nameof(IsZoomEnabled), typeof(bool), typeof(CADControl), new PropertyMetadata(true));


        /// <summary>
        /// 鼠标滚动时的缩放响应;
        /// </summary>
        /// <param name="e"></param>
        private bool MouseWheelOnZoom(MouseWheelEventArgs e)
        {
            if(e.Delta == 0)
            {
                return false;
            }
            if (!IsZoomEnabled)
            {
                return false;
            }
            //根据鼠标的位置响应缩放;
            var mouseUnitPos = CADScreenConverter.ToCAD(e.GetPosition(this));

            double wheeldeltatick = 120;
            double zoomdelta = (1.25f * (Math.Abs(e.Delta) / wheeldeltatick));

            if (e.Delta < 0)
            {
                //缩小时设定上限;
                if (Zoom >= MinZoom)
                {
                    Zoom /= zoomdelta;
                }
            }
            //放大时设定上限;
            else if (CADScreenConverter.ToScreen(1) < 1200)
            {
                Zoom *= zoomdelta;
            }

            MoveUnitPositionToScreenPosition(mouseUnitPos, e.GetPosition(this));

            return false;
        }

        /// <summary>
        /// 通过调整原点的偏移,将某个工程坐标节点的位置对应至视图上某个坐标;
        /// </summary>
        /// <param name="unitPos"></param>
        /// <param name="screenPos"></param>
        private void MoveUnitPositionToScreenPosition(
            Point unitPos,
            Point screenPos)
        {
            var unitPosInScreen = CADScreenConverter.ToScreen(unitPos);
            //通过调整原点的视图偏移实现;
            PanScreenPosition = new Point(
                PanScreenPosition.X + screenPos.X - unitPosInScreen.X,
                PanScreenPosition.Y + screenPos.Y - unitPosInScreen.Y
            );
        }
    }

    /// <summary>
    /// 画布拖拽处理部分;
    /// </summary>
    public partial class CADControl
    {

        /// <summary>
        /// Get or set is drag behavior enabled;
        /// </summary>
        public bool IsDragEnabled
        {
            get { return (bool)GetValue(IsDragEnabledProperty); }
            set { SetValue(IsDragEnabledProperty, value); }
        }

        public static readonly DependencyProperty IsDragEnabledProperty =
            DependencyProperty.Register(nameof(IsDragEnabled), typeof(bool), typeof(CADControl), new PropertyMetadata(true));


        /// <summary>
        /// 鼠标按下时的拖拽响应;
        /// </summary>
        /// <param name="e"></param>
        private bool MouseDownOnDrag(MouseButtonEventArgs e)
        {
            if (!IsDragEnabled)
            {
                return false;
            }
            //拖拽视图时使用;
            _lastMouseDownPointForPan = e.GetPosition(this);

            if (e.MiddleButton == MouseButtonState.Pressed)
            {
                UpdateCursor();
                _lastPanOffsetBeforeDragging = PanScreenPosition;
                _isDragging = true;
                return true;
            }

            return false;
        }

        /// <summary>
        /// 根据当前是否处于拖拽状态设定Cursor;
        /// </summary>
        /// <param name="e"></param>
        private Cursor GetCursorOnDrag()
        {
            if (Mouse.MiddleButton == MouseButtonState.Pressed)
            {
                return DragCursor;
            }

            return null;
        }

        /// <summary>
        /// 鼠标移动时的拖拽处理;
        /// </summary>
        /// <param name="e"></param>
        private bool MouseMoveOnDrag(MouseEventArgs e)
        {
            if (!IsDragEnabled)
            {
                return false;
            }
            //若未处于正在拖动状态,则退出;
            if (!_isDragging)
            {
                return false;
            }
            //若中键被按住,则为拖动操作;
            if (e.MiddleButton == MouseButtonState.Pressed)
            {
                UpdateCursor();
                var mousePos = e.GetPosition(this);

                PanScreenPosition = new Point(
                    _lastPanOffsetBeforeDragging.X + mousePos.X - _lastMouseDownPointForPan.X,
                    _lastPanOffsetBeforeDragging.Y + mousePos.Y - _lastMouseDownPointForPan.Y
                );
#if DEBUG
                Debug.WriteLine($"PanScreenPosition:{PanScreenPosition}");
#endif
                return true;

            }

            return false;
        }

        /// <summary>
        /// 鼠标键弹起时的拖拽处理;
        /// </summary>
        /// <param name="e"></param>
        private bool MouseUpOnDrag(MouseButtonEventArgs e)
        {
            if (!IsDragEnabled)
            {
                return false;
            }
            if (!_isDragging)
            {
                return false;
            }

            if (e.MiddleButton == MouseButtonState.Released)
            {
                _isDragging = false;
                UpdateCursor();
                
                return true;
            }

            return false;
        }

        /// <summary>
        /// 进行拖动时,显示的鼠标指针;
        /// </summary>
        public Cursor DragCursor
        {
            get { return (Cursor)GetValue(DragCursorProperty); }
            set { SetValue(DragCursorProperty, value); }
        }

        public static readonly DependencyProperty DragCursorProperty =
            DependencyProperty.Register(nameof(DragCursor), typeof(Cursor), typeof(CADControl), new PropertyMetadata(Cursors.Hand));


    }

    /// <summary>
    /// Cursor(鼠标形状)部分;
    /// </summary>
    public partial class CADControl
    {
        /// <summary>
        /// 更新Cursor;
        /// </summary>
        private void UpdateCursor()
        {
            //遍历所有内部Cursor设定器,直到值被设定;
            foreach (var getter in GetAllCursorGetters())
            {
                var cursor = getter();
                if (cursor != null)
                {
                    this.Cursor = cursor;
                    return;
                }
            }
            
            //若Cursor仍未被设定,将重置为默认Cursor;
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 获取所有设定Cursor的设置器;
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Func<Cursor>> GetAllCursorGetters()
        {
            //拖拽判断;
            yield return GetCursorOnDrag;
            //选取判断;
            yield return GetCursorOnSelect;
        }
    }

    /// <summary>
    /// 内容图层,绘制对象部分;
    /// </summary>
    public partial class CADControl
    {
        /// <summary>
        /// 当前活动图层;
        /// </summary>
        public CADLayer ActiveLayer
        {
            get { return (CADLayer)GetValue(ActiveLayerProperty); }
            set { SetValue(ActiveLayerProperty, value); }
        }

        public static readonly DependencyProperty ActiveLayerProperty =
            DependencyProperty.Register(
                nameof(ActiveLayer),
                typeof(CADLayer),
                typeof(CADControl),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    ActiveLayer_PropertyChanged
                )
            );

        private static void ActiveLayer_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            if(d is CADControl ctrl) {
                ctrl.ActiveLayerChanged?.Invoke(
                    ctrl,
                    new ValueChangedEventArgs<CADLayer>(e.OldValue as CADLayer, e.NewValue as CADLayer)
                );
            }
        }

        public event EventHandler<ValueChangedEventArgs<CADLayer>> ActiveLayerChanged;

        /// <summary>
        /// 所有内容图层;
        /// </summary>
        public IEnumerable<CADLayer> Layers
        {
            get { return (IEnumerable<CADLayer>)GetValue(LayersProperty); }
            set { SetValue(LayersProperty, value); }
        }

        public static readonly DependencyProperty LayersProperty =
            DependencyProperty.Register(nameof(Layers), typeof(IEnumerable<CADLayer>), typeof(CADControl), new PropertyMetadata(null, Layers_PropertyChanged));

        /// <summary>
        /// 图层集合发生变更时;
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void Layers_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is CADControl ctrl))
            {
                return;
            }

            var oldLayers = e.OldValue as IEnumerable<CADLayer>;
            var newLayers = e.NewValue as IEnumerable<CADLayer>;

            //卸载/装载新/旧图层;
            if (oldLayers != null)
            {
                foreach (var layer in oldLayers)
                {
                    ctrl.UnSetupLayer(layer);
                }

            }

            if (newLayers != null)
            {
                foreach (var layer in newLayers)
                {
                    ctrl.SetupLayer(layer);
                }
            }

            if (oldLayers is INotifyCollectionChanged oldLayerCollection)
            {
                oldLayerCollection.CollectionChanged -= ctrl.Layers_CollectionChanged;
            }

            if (newLayers is INotifyCollectionChanged newLayerCollection)
            {
                newLayerCollection.CollectionChanged += ctrl.Layers_CollectionChanged;
            }
        }

        /// <summary>
        /// 图层集合发生变化时的响应;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Layers_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if(!(sender is IEnumerable<CADLayer> layers)) {
                return;
            }

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var item in e.NewItems)
                    {
                        if (!(item is CADLayer layer))
                        {
                            continue;
                        }

                        SetupLayer(layer);
                    }
                    break;


                case NotifyCollectionChangedAction.Remove:
                    foreach (var item in e.OldItems)
                    {
                        if (!(item is CADLayer layer))
                        {
                            continue;
                        }

                        UnSetupLayer(layer);
                    }
                    break;

                case NotifyCollectionChangedAction.Reset:
                    ///若是复位(可能是清除操作),则需将现有所有图层清除;
                    ///再逐次添加图层;
                    ///为避免遍历中移除元素,故使用<see cref="Enumerable.ToList{TSource}(IEnumerable{TSource})"/>
                    _CADLayers.ToList().ForEach(layer => {
                        UnSetupLayer(layer);
                    });

                    if (layers != null)
                    {
                        foreach (var layer in layers)
                        {
                            SetupLayer(layer);
                        }
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
        /// 安装图层,绘制对象,事件注册等操作;
        /// </summary>
        /// <param name="CADLayer"></param>
        private void SetupLayer(CADLayer CADLayer)
        {
            if (_layerContainerVisualDict.ContainsKey(CADLayer))
            {
                return;
            }
            var layerContainerVisual = new ContainerVisual();
            _layerContainerVisualDict.Add(CADLayer, layerContainerVisual);
            VisualContainer.InsertVisual(_layerContainerVisualDict.Count - 1, layerContainerVisual);
            AddDrawable(CADLayer,layerContainerVisual);
            AddDrawObjects(CADLayer.DrawObjects,CADLayer);
          
            //图层内绘制对象增减清除时,延长/缩减/清除绘制对象的缓冲池;
            CADLayer.DrawObjectsAdded += CADLayer_DrawObjectsAdded;
            CADLayer.DrawObjectsRemoved += CADLayer_DrawObjectsRemoved;
            CADLayer.DrawObjectsClearing += CADLayer_DrawObjectClearing;

            //可见状态变化时进行响应;
            CADLayer.IsVisibleChanged += CADLayer_IsVisibleChanged;

            _CADLayers.Add(CADLayer);
            
        }
        
        /// <summary>
        /// 卸载图层;
        /// </summary>
        /// <param name="CADLayer"></param>
        private void UnSetupLayer(CADLayer CADLayer)
        {
            if(CADLayer == null)
            {
                return;
            }

            if (_layerContainerVisualDict.TryGetValue(CADLayer,out var layerContaienrVisual))
            {
                return;
            }
            
            RemoveDrawable(CADLayer,layerContaienrVisual);
            //卸载该图层内所有绘制对象;
            RemoveAllVisualsOfLayer(CADLayer);

            CADLayer.DrawObjectsAdded -= CADLayer_DrawObjectsAdded;
            CADLayer.DrawObjectsRemoved -= CADLayer_DrawObjectsRemoved;
            CADLayer.DrawObjectsClearing -= CADLayer_DrawObjectClearing;

            CADLayer.IsVisibleChanged -= CADLayer_IsVisibleChanged;

            _CADLayers.Remove(CADLayer);
        }
        
        /// <summary>
        /// 图层内可绘制元素被添加时的响应;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CADLayer_DrawObjectsAdded(object sender,IEnumerable<DrawObject> e)
        {
            if (e == null)
            {
                return;
            }
            if(!(sender is CADLayer CADLayer))
            {
                return;
            }

            AddDrawObjects(e,CADLayer);
        }
        
        /// <summary>
        /// 被清除响应;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CADLayer_DrawObjectClearing(object sender, EventArgs e) {
            if(!(sender is CADLayer layer)) {
                return;
            }

            RemoveAllVisualsOfLayer(layer);
        }


        /// <summary>
        /// 添加绘制对象;
        /// </summary>
        /// <param name="drawObject"></param>
        private void AddDrawObjects(IEnumerable<DrawObject> drawObjects, CADLayer CADLayer) {
            if (drawObjects == null) {
                return;
            }
            if (CADLayer == null)
            {
                return;
            }
            if (!_layerContainerVisualDict.TryGetValue(CADLayer, out var containerVisual))
            {
                return;
            }

            foreach (var drawObject in drawObjects) {
                AddDrawable(drawObject,containerVisual);

                drawObject.IsVisibleChanged += DrawObject_IsVisibleChanged;
                drawObject.IsSelectedChanged += DrawObject_IsSelectedChanged;
            }

            DrawObjectsAdded?.Invoke(this, new DrawObjectsAddedEventArgs(drawObjects));
        }


        /// <summary>
        /// 移除绘制元素;
        /// </summary>
        /// <param name="drawObject"></param>
        private void RemoveDrawObjects(IEnumerable<DrawObject> drawObjects, CADLayer CADLayer)
        {
            if(drawObjects == null) 
            {
                return;
            }
            if(CADLayer == null)
            {
                return;
            }
            if (!_layerContainerVisualDict.TryGetValue(CADLayer,out var containerVisual))
            {
                return;
            }

            foreach (var drawObject in drawObjects) {
                RemoveDrawable(drawObject, containerVisual);

                drawObject.IsVisibleChanged -= DrawObject_IsVisibleChanged;
                drawObject.IsSelectedChanged -= DrawObject_IsSelectedChanged;
            }

            DrawObjectsRemoved?.Invoke(this, new DrawObjectsRemovedEventArgs(drawObjects));
        }

        /// <summary>
        /// 绘制对象的可见状态发生变化时的动作;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DrawObject_IsVisibleChanged(object sender, EventArgs e)
        {
            if (!(sender is DrawObject drawObject))
            {
                return;
            }

            DrawDrawable(drawObject);
        }


        /// <summary>
        /// 绘制对象选中状态发生变化时,触发相关事件;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DrawObject_IsSelectedChanged(object sender, ValueChangedEventArgs<bool> e) {
            if(!(sender is DrawObject drawObject)) {
                return;
            }

            DrawObjectIsSelectedChanged?.Invoke(this,new DrawObjectSelectedChangedEventArgs(drawObject,e.NewValue,e.OldValue));
        }

        

        /// <summary>
        /// 图层内可绘制元素被移除时的响应;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CADLayer_DrawObjectsRemoved(object sender, IEnumerable<DrawObject> e)
        {
            if (e == null)
            {
                return;
            }
            if(!(sender is CADLayer CADLayer))
            {
                return;
            }

            RemoveDrawObjects(e,CADLayer);
            
        }

        /// <summary>
        /// Add a drawble object to cache <see cref="_visualDict"/> and visual tree;
        /// </summary>
        /// <param name="drawable"></param>
        private void AddDrawable(IDrawable drawable,ContainerVisual containerVisual)
        {
            if (_visualDict.ContainsKey(drawable))
            {
                return;
            }
            
            var drawingVisual = new DrawingVisual
            {
                Clip = new RectangleGeometry(new Rect
                {
                    Width = ActualWidth,
                    Height = ActualHeight
                })
            };

            _visualDict.Add(drawable, drawingVisual);
            containerVisual.Children.Add(drawingVisual);
            drawable.VisualChanged += Drawable_VisualChanged;
            DrawDrawable(drawable);
        }

        /// <summary>
        /// remove a drawable object from cache and visual tree;
        /// </summary>
        /// <param name="drawable"></param>
        private void RemoveDrawable(IDrawable drawable,ContainerVisual containerVisual)
        {
            if (!_visualDict.TryGetValue(drawable,out var drawingVisual))
            {
                return;
            }
            _visualDict.Remove(drawable);
            containerVisual.Children.Remove(drawingVisual);
            drawable.VisualChanged -= Drawable_VisualChanged;
        }


        /// <summary>
        /// 绘制对象;
        /// </summary>
        /// <param name="drawable">负责绘制逻辑的单元</param>
        /// <remarks>该对象必须</remarks>
        private void DrawDrawable(IDrawable drawable)
        {
            if (!_visualDict.ContainsKey(drawable))
            {
                return;
            }

            var drawingVisual = _visualDict[drawable];
            DrawDrawableCore(drawable, drawingVisual);
        }


        /// <summary>
        /// 绘制可绘制对象核心;
        /// </summary>
        /// <param name="drawable">可绘制对象</param>
        /// <param name="drawingVisual">对应的WPF-DrawingVisual</param>
        private void DrawDrawableCore(IDrawable drawable, DrawingVisual drawingVisual)
        {
            var dc = drawingVisual.RenderOpen();
            InternalCanvas.DrawingContext = dc;

            if (!(drawable is CADElement canvasElement) || canvasElement.IsVisible)
            {
                drawable.Draw(InternalCanvas);
            }


            dc.Close();
            InternalCanvas.DrawingContext = null;
        }

        
        /// <summary>
        /// 移除来自对应图层内的所有绘制元素;
        /// </summary>
        /// <param name="CADLayer"></param>
        private void RemoveAllVisualsOfLayer(CADLayer CADLayer)
        {
            //遍历缓存中所有的Visual,移除该图层内的所有元素;
            var removeVisualPairs = _visualDict.Where(p =>
            {
                if (!(p.Key is DrawObject drawObject))
                {
                    return false;
                }
                return drawObject.Layer == CADLayer;
            });

            RemoveDrawObjects(removeVisualPairs.Select(p => p.Key).OfType<DrawObject>().ToList(),CADLayer);
        }

        /// <summary>
        /// 图层可见状态发生变化时的响应;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CADLayer_IsVisibleChanged(object sender, EventArgs e)
        {
            if (!(sender is CADLayer CADLayer))
            {
                return;
            }
            if(!_layerContainerVisualDict.TryGetValue(CADLayer,out var layerContainerVisual))
            {
                return;
            }

            //若可见,则加入来自该图层内的所有元素;
            if (CADLayer.IsVisible)
            {
                AddDrawObjects(CADLayer.DrawObjects, CADLayer);
            }
            //若不可见,则移除制来自该图层内的所有元素;
            else
            {
                RemoveDrawObjects(CADLayer.DrawObjects, CADLayer);
            }

        }

        /// <summary>
        /// 某个图像单元内容发生变化时的响应;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Drawable_VisualChanged(object sender, EventArgs e)
        {
            if (!(sender is IDrawable drawable))
            {
                return;
            }

            if (!_visualDict.ContainsKey(drawable))
            {
                return;
            }

            DrawDrawable(drawable);
        }

    }

    /// <summary>
    /// 绘制对象的选取(点击,按键)部分;
    /// </summary>
    public partial class CADControl
    {

        /// <summary>
        /// 鼠标移动时绘制对象的可选取状态的感应处理;
        /// </summary>
        /// <param name="e"></param>
        private bool MouseMoveOnSelectDrawObject(MouseEventArgs e)
        {
            if (Layers == null)
            {
                return false;
            }

            //检查是否存在某个图层，该图层内存在元素符合悬停条件,若满足条件,需高亮感应;
            var mousePosition = CADScreenConverter.ToCAD(e.GetPosition(this));

            _hoveredDrawObjects.Clear();

            _hoveredDrawObjects.AddRange(
                this.GetVisibleLayers().SelectMany(p => p.DrawObjects.Where(
                    q => q.PointInObject(mousePosition, CADScreenConverter)
                )
            ));

            //变更Cursor;
            UpdateCursor();

            return false;
        }

        /// <summary>
        /// 鼠标按下时选取绘制对象的处理;
        /// </summary>
        /// <param name="e"></param>
        private bool MouseDownOnSelectDrawObject(MouseEventArgs e)
        {
            if (Layers == null)
            {
                return false;
            }

            if (e.LeftButton == MouseButtonState.Released)
            {
                return false;
            }

            //将所有被悬停的元素置为被选择状态;
            var mousePosition = CADScreenConverter.ToCAD(e.GetPosition(this));

            var clickedDrawObjects = this.GetAllVisibleDrawObjects().
                Where(p => p.PointInObject(mousePosition, CADScreenConverter)).ToArray();

            foreach (var drawObject in clickedDrawObjects)
            {
                drawObject.IsSelected = !drawObject.IsSelected;
                return true;
            }

            return false;
        }


        /// <summary>
        /// 键盘按下时选取的处理;
        /// </summary>
        /// <param name="e"></param>
        private bool KeyDownOnSelectDrawObject(KeyEventArgs e)
        {
            if (Layers == null)
            {
                return false;
            }
            
            //若按下的键为Tab,则可以交替选择单元;
            if (e.Key == Key.Tab && e.IsDown && _hoveredDrawObjects.Count != 0)
            {
                TabKeyDownOnSelectDrawObject();
                //因为Tab键按下会导致本控件失去焦点,所以需指定路由事件参数为已处理;
                e.Handled = true;
                return true;
            }
            
            return false;
        }


        /// <summary>
        /// 键盘按下了Tab时的处理,交替选择单元;
        /// </summary>
        private void TabKeyDownOnSelectDrawObject()
        {
            //若不存在多个被悬停的对象,则不必处理;
            if (_hoveredDrawObjects.Count <= 1)
            {
                return;
            }

            var selectedCount = _hoveredDrawObjects.Count(p => p.IsSelected);

            //若悬停对象中,只有一个对象被选中,则进行交替选择;
            if (selectedCount == 1)
            {
                for (int i = 0; i < _hoveredDrawObjects.Count; i++)
                {
                    var drawObject = _hoveredDrawObjects[i];
                    if (drawObject.IsSelected)
                    {
                        _hoveredDrawObjects[i == _hoveredDrawObjects.Count - 1 ? 0 : i + 1].IsSelected = true;
                        drawObject.IsSelected = false;
                        break;
                    }
                }
            }
            else
            {
                for (int i = 1; i < _hoveredDrawObjects.Count; i++)
                {
                    _hoveredDrawObjects[i].IsSelected = false;
                }

                _hoveredDrawObjects[0].IsSelected = true;
            }
            
        }


        /// <summary>
        /// 根据选取状态设定Cursor;
        /// </summary>
        /// <param name="e"></param>
        private Cursor GetCursorOnSelect()
        {
            //若存在被悬停的可绘制对象,变更Cursor;
            if (_hoveredDrawObjects.Any())
            {
                return Cursors.Hand;
            }

            return null;
        }
    }

    /// <summary>
    /// Drag select with left mouse button;
    /// </summary>
    public partial class CADControl
    {
        /// <summary>
        /// This value indicate the drag selection behavior is enabled or not,default value is true;
        /// </summary>
        public bool IsDragSelectEnabled
        {
            get { return (bool)GetValue(DragSelectEnabledProperty); }
            set { SetValue(DragSelectEnabledProperty, value); }
        }

        public static readonly DependencyProperty DragSelectEnabledProperty =
            DependencyProperty.Register(nameof(IsDragSelectEnabled), typeof(bool), typeof(CADControl), new PropertyMetadata(true));

        /// <summary>
        /// Add selected rectangle to <see cref="_visualDict"/>;
        /// </summary>
        private void AddSelectRectangleToDict() {
            if (!_visualDict.ContainsKey(_dragSelectRectangle)) {
                //Add highlighted rect to visual tree;
                AddDrawable(_dragSelectRectangle,_dragSelectionContainerVisual);
            }
        }

        /// <summary>
        /// Handle mousedown operation for dargged drawobjects while mouse is pressed;
        /// </summary>
        /// <param name="e"></param>
        private bool MouseDownOnDragingSelectDrawObject(MouseEventArgs e)
        {
            if (!IsDragSelectEnabled)
            {
                return false;
            }
            var mousePosition = CADScreenConverter.ToCAD(e.GetPosition(this));
            //若上次点击位置不为空,则进行拖放选中操作;
            if (_lastMouseDownPositionForDragSelecting != null)
            {
                if (_dragSelectRectangle == null)
                {
                    return false;
                }

                //若矩形两对角点的横坐标或纵坐标相等,则无法组成矩形,不能拖放选择;
                if (_lastMouseDownPositionForDragSelecting.Value.X == mousePosition.X
                    || _lastMouseDownPositionForDragSelecting.Value.Y == mousePosition.Y)
                {
                    return false;
                }

                var rect = _dragSelectRectangle.Rectangle;
                if (rect == null)
                {
                    return false;
                }

                //遍历选中所有在框选范围中的可见绘制对象;
                var selectedObjects = this.GetVisibleLayers().SelectMany(p => p.DrawObjects).Where(p => p.IsVisible).
                        Where(p => p.ObjectInRectangle(rect.Value, CADScreenConverter, _anyPointSelectForDragSelect)).ToArray();


                foreach (var drawObject in selectedObjects)
                {
                    drawObject.IsSelected = true;
                }

                //将选中矩形的数据置空;
                _lastMouseDownPositionForDragSelecting = null;
                _dragSelectRectangle.Rectangle = null;

                return true;
            }
            else if (e.LeftButton == MouseButtonState.Pressed)
            {
                //框选矩形的起点不能命中任何绘制对象;
                if (this.GetAllVisibleDrawObjects().Any(p => p.PointInObject(mousePosition, CADScreenConverter))){
                    return false;
                }
                //记录本次鼠标点击的位置;
                _lastMouseDownPositionForDragSelecting = mousePosition;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Handle mousemove operation for dargged drawobjects while mouse is pressed;
        /// </summary>
        /// <param name="e"></param>
        private bool MouseMoveOnDragingSelectDrawObject(MouseEventArgs e)
        {
            if (!IsDragSelectEnabled)
            {
                return false;
            }
            var mousePosition = CADScreenConverter.ToCAD(e.GetPosition(this));

            //若鼠标上次按下的位置不为空,则绘制选择矩形;
            if (_lastMouseDownPositionForDragSelecting == null)
            {
                return false;
            }

            //将高亮矩形加入到视觉树中;
            AddDrawable(_dragSelectRectangle,_dragSelectionContainerVisual);

            //若矩形两对角点的横坐标或纵坐标相等,则无法组成矩形,无法绘制矩形;
            if (_lastMouseDownPositionForDragSelecting.Value.X == mousePosition.X
                || _lastMouseDownPositionForDragSelecting.Value.Y == mousePosition.Y)
            {
                _dragSelectRectangle.Rectangle = null;
                return false;
            }

            //计算拖放选中区域;
            var rect = new CADRect(_lastMouseDownPositionForDragSelecting.Value, mousePosition);
            _dragSelectRectangle.Rectangle = rect;

            var dragArgs = new DragSelectMouseMoveEventArgs(rect, mousePosition);

            HandleDragSelectOnMouseMove(dragArgs);

            if (dragArgs.Handled) {
                _dragSelectRectangle.Rectangle = null;
                return true;
            }
            
            //根据本次鼠标位置和上次鼠标的位置判断是否为任意选中操作;
            if (_anyPointSelectForDragSelect)
            {
                _dragSelectRectangle.Fill = Constants.AnySelectBrush;
                _dragSelectRectangle.Pen = Constants.AnySelectPen;
            }
            else
            {
                _dragSelectRectangle.Fill = Constants.AllSelectBrush;
                _dragSelectRectangle.Pen = Constants.AllSelectPen;
            }

            return false;
        }

        /// <summary>
        /// Handle keydown operation for dargged drawobjects while mouse is pressed;
        /// </summary>
        /// <param name="e"></param>
        private bool KeyDownOnDragingSelectDrawObject(KeyEventArgs e)
        {
            //若按下的键为Esc,则清除拖放选取的状态;
            if (e.Key == Key.Escape && e.IsDown)
            {
                //若处于正在拖放状态,则清除拖放矩形和上次鼠标按下位置;
                if (_lastMouseDownPositionForDragSelecting == null || _dragSelectRectangle == null)
                {
                    return false;
                }

                _dragSelectRectangle.Rectangle = null;
                _lastMouseDownPositionForDragSelecting = null;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Handle drag selection while mouse is moving;
        /// </summary>
        private void HandleDragSelectOnMouseMove(DragSelectMouseMoveEventArgs dragArgs)
        {
            if (_lastMouseDownPositionForDragSelecting == null)
            {
                return;
            }

            if (_dragSelectRectangle.Rectangle == null)
            {
                return;
            }

            DrawSelectMouseMove?.Invoke(this, dragArgs);
            
            _anyPointSelectForDragSelect = dragArgs.IsAnyPoint ?? (dragArgs.Position.X < _lastMouseDownPositionForDragSelecting.Value.X);
            
        }
    }

    /// <summary>
    /// 与被选择绘制对象的交互部分;
    /// </summary>
    public partial class CADControl
    {
        private bool InteractWithSelectedDrawObjects<TEventArgs>(
            Action<DrawObject,TEventArgs> handler,
            TEventArgs eventArgs)
            where TEventArgs:CADRoutedEventArgs {

            foreach (var drawObject in this.GetAllVisibleDrawObjects())
            {
                handler(drawObject, eventArgs);
                if (eventArgs.Handled)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 鼠标按下时与被选取对象的交互;
        /// </summary>
        /// <param name="e"></param>
        private bool MouseDownOnSelectedDrawObjects(MouseButtonEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
            {
                return false;
            }
            
            var mouseScreenPosition = e.GetPosition(this);
            var mousePosition = CADScreenConverter.ToCAD(mouseScreenPosition);
            var mouseDownEventArgs = new CADMouseButtonEventArgs(mousePosition) { MouseButtonEventArgs = e };
            return InteractWithSelectedDrawObjects((drawObject, eventArgs) => drawObject.OnMouseDown(eventArgs), mouseDownEventArgs);
        }

        /// <summary>
        /// 鼠标移动时与选取对象的交互;
        /// </summary>
        /// <param name="e"></param>
        private bool MouseMoveOnSelectedDrawObjects(MouseEventArgs e)
        {
            var mouseScreenPosition = e.GetPosition(this);
            var mousePosition = CADScreenConverter.ToCAD(mouseScreenPosition);
            var mouseMoveEventArgs = new CADMouseEventArgs(mousePosition) { MouseEventArgs = e };
            //与所有选中的绘制对象进行交互;
            return InteractWithSelectedDrawObjects((drawObject, eventArgs) => drawObject.OnMouseMove(mouseMoveEventArgs), mouseMoveEventArgs);
        }

        /// <summary>
        /// 键盘按下时与选取对象的交互;
        /// </summary>
        /// <param name="e"></param>
        private bool KeyDownOnSelectedDrawObjects(KeyEventArgs e)
        {
            var keyEventArgs = new CADKeyEventArgs { KeyEventArgs = e };
            //与所有选中的绘制对象进行交互;
            return InteractWithSelectedDrawObjects((drawObject,eventArgs) => drawObject.OnKeyDown(eventArgs), keyEventArgs);
        }

        /// <summary>
        /// 键盘弹起时与选取对象的交互;
        /// </summary>
        /// <param name="e"></param>
        private bool KeyUpOnSelectedDrawObjects(KeyEventArgs e)
        {
            var keyEventArgs = new CADKeyEventArgs { KeyEventArgs = e };
            //与所有选中的绘制对象进行交互;
            return InteractWithSelectedDrawObjects((drawObject, eventArgs) => drawObject.OnKeyUp(eventArgs), keyEventArgs);
        }

        
    }

    /// <summary>
    /// 鼠标实时位置;
    /// </summary>
    public partial class CADControl
    {
        /// <summary>
        /// 当前的鼠标所在的工程数学坐标;
        /// </summary>
        public Point CurrentMousePosition { get; private set; }

        /// <summary>
        /// 当前的鼠标所在的工程数学坐标发生变化事件;
        /// </summary>
        public event EventHandler<ValueChangedEventArgs<Point>> CurrentMousePositionChanged;

        /// <summary>
        /// 鼠标移动时,通知当前鼠标的工程数学坐标节点变化;
        /// </summary>
        /// <param name="e"></param>
        private bool MouseMoveOnCurrentMousePosition(MouseEventArgs e)
        {
            var point = CADScreenConverter.ToCAD(e.GetPosition(this));
            var oldValue = CurrentMousePosition;
            CurrentMousePosition = point;
            CurrentMousePositionChanged?.Invoke(this, new ValueChangedEventArgs<Point>(CurrentMousePosition, oldValue));

            return false;
        }


    }

 
    /// <summary>
    /// 原生对象部分;
    /// </summary>
    public partial class CADControl {
        public void AddUIElement(UIElement child) {
            if (child == null) {
                throw new ArgumentNullException(nameof(child));
            }

            if (this.Children.Contains(child)) {
                return;
            }

            this.Children.Add(child);
        }

        public void RemoveUIElement(UIElement child) {

            if (child == null) {
                throw new ArgumentNullException(nameof(child));
            }

            
            if (!this.Children.Contains(child)) {
                return;
            }

            this.Children.Remove(child);
        }
    }
    
    /// <summary>
    /// 输入事件;
    /// </summary>
    public partial class CADControl {

        /// <summary>
        /// 鼠标按下事件;
        /// </summary>
        public event EventHandler<MouseEventArgs> CanvasPreviewMouseDown;

        /// <summary>
        /// 鼠标移动事件;
        /// </summary>
        public event EventHandler<MouseEventArgs> CanvasPreviewMouseMove;


        /// <summary>
        /// 鼠标弹起事件;
        /// </summary>
        public event EventHandler<MouseEventArgs> CanvasPreviewMouseUp;

        /// <summary>
        /// 键盘按下事件;
        /// </summary>
        public event EventHandler<KeyEventArgs> CanvasPreviewKeyDown;

        /// <summary>
        /// 键盘弹起事件;
        /// </summary>
        public event EventHandler<KeyEventArgs> CanvasPreviewKeyUp;

        /// <summary>
        /// 键盘输入事件;
        /// </summary>
        public event EventHandler<TextCompositionEventArgs> CanvasPreviewTextInput;

        
        
        /// <summary>
        /// 鼠标按下时,通知外部;
        /// </summary>
        /// <param name="e"></param>
        private bool MouseDownOnPreview(MouseButtonEventArgs e) {
            CanvasPreviewMouseDown?.Invoke(this, e);
            return e.Handled;
        }
        
        private bool MouseMoveOnPreview(MouseEventArgs e) {
            CanvasPreviewMouseMove?.Invoke(this, e);
            return e.Handled;
        }

        private bool MouseUpOnPreview(MouseButtonEventArgs e) {
            CanvasPreviewMouseUp?.Invoke(this, e);
            return e.Handled;
        }

        private bool KeyDownOnPreview(KeyEventArgs e) {
            CanvasPreviewKeyDown?.Invoke(this, e);
            return e.Handled;
        }

        private bool KeyUpOnPreview(KeyEventArgs e) {
            CanvasPreviewKeyUp?.Invoke(this, e);
            return e.Handled;
        }
        

        private bool TextInputOnPreview(TextCompositionEventArgs e) {
            CanvasPreviewTextInput?.Invoke(this, e);
            return e.Handled;
        }


    }
}
