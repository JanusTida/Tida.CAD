using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Linq;
using System.Collections.Specialized;
using System.Windows.Input;

using Tida.Geometry.Primitives;
using Tida.Canvas.Contracts;
using Tida.Canvas.Events;
using Tida.Canvas.WPFCanvas.Geometry;
using Tida.Canvas.WPFCanvas.Input;

using SystemInput = System.Windows.Input;
using CanvasInput = Tida.Canvas.Input;
using System.Windows.Controls;
using Size = System.Windows.Size;

namespace Tida.Canvas.WPFCanvas {
    /// <summary>
    /// 画布控件;
    /// </summary>
    public partial class CanvasControl : Grid , ICanvasControl, IInteractionCanvasControl {
        static CanvasControl() {
            BackgroundProperty.OverrideMetadata(typeof(CanvasControl), new FrameworkPropertyMetadata(
                Constants.DefaultCanvasBackground,
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits
            ));
            
        }

        public CanvasControl() {
            this.Children.Add(_visualContainer);
            this.Focusable = true;
        }
        
        private readonly VisualContainer _visualContainer = new VisualContainer();

        /// <summary>
        /// ICanvas画布工具的内部实现,用于传递到<see cref="IDrawable.Draw(ICanvas)"/>中进行绘制;
        /// 通过更改<see cref="WindowsCanvas.DrawingContext"/>实现复用的目的;
        /// </summary>
        private WindowsCanvas InternalCanvas => _canvas ?? (_canvas = new WindowsCanvas(this.CanvasProxy));
        
        private WindowsCanvas _canvas;
        
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
        private Vector2D _lastPanOffsetBeforeDragging;

        /// <summary>
        /// 可绘制对象与WPF DrawingVisual缓存;
        /// </summary>
        private readonly Dictionary<IDrawable, DrawingVisual> _visualDict = new Dictionary<IDrawable, DrawingVisual>();

        /// <summary>
        /// 撤销编辑事务栈;
        /// </summary>
        private readonly Stack<Stack<IEditTransaction>> _undoTransactionBuffer = new Stack<Stack<IEditTransaction>>();

        /// <summary>
        /// 重做编辑事务栈;
        /// </summary>
        private readonly Stack<Stack<IEditTransaction>> _redoTransactionBuffer = new Stack<Stack<IEditTransaction>>();

        /// <summary>
        /// 是否正在处理路由事件;
        /// </summary>
        private bool _handlingRoutedEvent = false;

        /// <summary>
        /// 在一次路由事件的处理时间内中,指示是否已经创建了一个事务栈;
        /// </summary>
        private bool _transactionStackCreatedInOneRoutedEvent = false;

        /// <summary>
        /// 当前的活动的辅助图形;
        /// </summary>
        private ISnapShape _activeSnapShape;
        
        /// <summary>
        /// 当前被悬停的绘制对象集合;
        /// </summary>
        private readonly List<DrawObject> _hoveredDrawObjects = new List<DrawObject>();

        /// <summary>
        /// 内部存储维护的所有图层集合;
        /// </summary>
        private readonly List<CanvasLayer> _canvasLayers = new List<CanvasLayer>();

        /// <summary>
        /// 内部存储维护的所有位置预处理器集合;
        /// </summary>
        private readonly List<CanvasInteractionHandler> _positionHandlers = new List<CanvasInteractionHandler>();

        /// <summary>
        /// 记录上一次鼠标按下的位置,在拖放选择时使用;
        /// </summary>
        private Vector2D _lastMouseDownPositionForDragSelecting;

        /// <summary>
        /// 拖放选择时所呈现的矩形对象;
        /// </summary>
        private readonly SimpleRectangle _dragSelectRectangle = new SimpleRectangle();

        /// <summary>
        /// 拖放选取时是否为任意选中;
        /// </summary>
        private bool _anyPointSelectForDragSelect;


        /// <summary>
        /// 是否处于重做中;
        /// </summary>
        private bool _isRedoing = false;

        /// <summary>
        /// 是否处于撤销中;
        /// </summary>
        private bool _isUndoing = false;

        /// <summary>
        /// 画布内绘制对象选定状态发生了变化事件;
        /// </summary>
        public event EventHandler<DrawObjectSelectedChangedEventArgs> DrawObjectIsSelectedChanged;
        
        /// <summary>
        /// 编辑事务已撤销;
        /// </summary>
        public event EventHandler<EditTransactionUndoneEventArgs> EditTransactionUndone;

        /// <summary>
        /// 编辑事务已重做;
        /// </summary>
        public event EventHandler<EditTransactionRedoneEventArgs> EditTransactionRedone;

        /// <summary>
        /// 绘制对象被移除;
        /// </summary>
        public event EventHandler<DrawObjectsRemovedEventArgs> DrawObjectsRemoved;

        /// <summary>
        /// 绘制对象被添加;
        /// </summary>
        public event EventHandler<DrawObjectsAddedEventArgs> DrawObjectsAdded;

        /// <summary>
        /// 绘制对象是否正在被编辑变化;
        /// </summary>
        public event EventHandler<DrawObjectIsEditingChangedEventArgs> DrawObjectIsEditingChanged;

        /// <summary>
        /// 拖拽选择事件;
        /// </summary>
        public event EventHandler<DragSelectEventArgs> DragSelect;

        /// <summary>
        /// 拖拽选择鼠标移动事件;
        /// </summary>
        public event EventHandler<DragSelectMouseMoveEventArgs> DrawSelectMouseMove;

        /// <summary>
        /// 通知外部,将要针对指定的绘制对象集合,将要进行某种的类型输入交互的预处理事件;
        /// </summary>
        public event EventHandler<PreviewDrawObjectsInteractionEventArgs> PreviewInteractionWithDrawObjects;

        /// <summary>
        /// 点击选取事件;
        /// </summary>
        public event EventHandler<ClickSelectEventArgs> ClickSelect;

        /// <summary>
        /// 初始化原点所在坐标;
        /// </summary>
        private void InitializePanOffset() {
            if (PanScreenPosition == null) {
                PanScreenPosition = new Vector2D(this.ActualWidth / 2, this.ActualHeight / 2);
            }
        }

        /// <summary>
        /// 通过调整原点的视图偏移,使得某个某个视图坐标的某工程坐标点处于视图中心的位置;
        /// </summary>
        /// <param name="screenPoint"></param>
        protected void SetCenterScreen(Vector2D screenPoint) {
            PanScreenPosition = new Vector2D(
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

            InitializePanOffset();

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
            UpdateCanvasProxy(arrangeSize);
            return base.ArrangeOverride(arrangeSize);
        }
    }

    /// <summary>
    /// <see cref="ICanvasScreenConvertable"/>相关成员;
    /// </summary>
    public partial class CanvasControl {

        private static readonly DependencyPropertyKey CanvasProxyPropertyKey = DependencyProperty.RegisterReadOnly(nameof(CanvasProxy), typeof(ICanvasScreenConvertable), typeof(CanvasControl), new PropertyMetadata());
        public static readonly DependencyProperty CanvasProxyProperty = CanvasProxyPropertyKey.DependencyProperty;

     
        private readonly WindowsCanvasScreenConverter _canvasProxy = new WindowsCanvasScreenConverter();

        /// <summary>
        /// 画布坐标转化实例;
        /// </summary>
        public ICanvasScreenConvertable CanvasProxy {
            get => _canvasProxy;
            set => throw new NotSupportedException($"The {nameof(CanvasProxy)} property is readonly!");
        }

        /// <summary>
        /// 更新<see cref="_canvasProxy"/>中的关键参数;
        /// </summary>
        private void UpdateCanvasProxy(Size? actualSize = null) {
            if(actualSize != null)
            {
                _canvasProxy.ActualHeight = actualSize.Value.Height;
                _canvasProxy.ActualWidth = actualSize.Value.Width;
            }

            _canvasProxy.Zoom = this.Zoom;
            _canvasProxy.PanScreenPosition = this.PanScreenPosition;
        }

    }

    /// <summary>
    /// 只读部分;
    /// </summary>
    public partial class CanvasControl {
        /// <summary>
        /// 是否只读,指示是否可通过UI操作数据;
        /// </summary>
        public bool IsReadOnly {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        public static readonly DependencyProperty IsReadOnlyProperty =
            DependencyProperty.Register(nameof(IsReadOnly), typeof(bool), typeof(CanvasControl), new PropertyMetadata(false));
    }

    /// <summary>
    /// 交互预处理部分;
    /// </summary>
    public partial class CanvasControl {
        /// <summary>
        /// 预处理器集合;
        /// </summary>
        public IEnumerable<CanvasInteractionHandler> InteractionHandlers {
            get { return (IEnumerable<CanvasInteractionHandler>)GetValue(InteractionHandlersProperty); }
            set { SetValue(InteractionHandlersProperty, value); }
        }

        public static readonly DependencyProperty InteractionHandlersProperty =
            DependencyProperty.Register(nameof(InteractionHandlers),
                typeof(IEnumerable<CanvasInteractionHandler>), typeof(CanvasControl),
                new PropertyMetadata(null, InteractionHandlers_PropertyChanged));

        private static void InteractionHandlers_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            if (!(d is CanvasControl ctrl)) {
                return;
            }

            var oldHandlers = e.OldValue as IEnumerable<CanvasInteractionHandler>;
            var newHandlers = e.NewValue as IEnumerable<CanvasInteractionHandler>;

            //卸载/装载新/旧交互器;
            if (oldHandlers != null) {
                foreach (var handler in oldHandlers) {
                    ctrl.UnSetupInteractionHandler(handler);
                }
            }

            if (newHandlers != null) {
                foreach (var handler in newHandlers) {
                    ctrl.SetupInteractionHandler(handler);
                }
            }

            if (oldHandlers is INotifyCollectionChanged oldHandlerCollection) {
                oldHandlerCollection.CollectionChanged -= ctrl.InteractionHandlers_CollectionChanged;
            }

            if (newHandlers is INotifyCollectionChanged newHandlerCollection) {
                newHandlerCollection.CollectionChanged += ctrl.InteractionHandlers_CollectionChanged;
            }
        }

        /// <summary>
        /// 对位置进行预处理;
        /// </summary>
        /// <param name="oriPosition"></param>
        /// <returns></returns>
        private void HandlePosition(Vector2D oriPosition) {
            if (oriPosition == null) {
                return;
            }

            if (InteractionHandlers == null) {
                return;
            }

            foreach (var positionHandler in InteractionHandlers) {
                positionHandler.HandlePosition(this, oriPosition);
            }
        }

        /// <summary>
        /// 装载位置预处理器;
        /// </summary>
        /// <param name="positionHandler"></param>
        private void SetupInteractionHandler(CanvasInteractionHandler positionHandler) {
            if (positionHandler == null) {
                return;
            }

            AddDrawable(positionHandler);

            positionHandler.CanvasControl = this;

            _positionHandlers.Remove(positionHandler);
        }

        /// <summary>
        /// 卸载位置预处理器;
        /// </summary>
        /// <param name="positionHandler"></param>
        private void UnSetupInteractionHandler(CanvasInteractionHandler positionHandler) {
            if (positionHandler == null) {
                return;
            }

            RemoveDrawable(positionHandler);

            positionHandler.CanvasControl = null;

            _positionHandlers.Add(positionHandler);
        }

        /// <summary>
        /// 位置预处理器集合内容发生变化时的响应;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InteractionHandlers_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            if (!(sender is IEnumerable<CanvasInteractionHandler> handlers)) {
                return;
            }

            switch (e.Action) {
                case NotifyCollectionChangedAction.Add:
                    foreach (var item in e.NewItems) {
                        if (!(item is CanvasInteractionHandler handler)) {
                            continue;
                        }

                        SetupInteractionHandler(handler);
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    foreach (var item in e.OldItems) {
                        if (!(item is CanvasInteractionHandler handler)) {
                            continue;
                        }

                        UnSetupInteractionHandler(handler);
                    }
                    break;

                case NotifyCollectionChangedAction.Replace:
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;

                case NotifyCollectionChangedAction.Reset:
                    ///若是复位(可能是清除操作),则需将现有所有图层清除;
                    ///再逐次添加图层;
                    ///为避免遍历中移除元素,故使用<see cref="Enumerable.ToList{TSource}(IEnumerable{TSource})"/>
                    _positionHandlers.ToList().ForEach(handler => {
                        UnSetupInteractionHandler(handler);
                    });

                    if (handlers != null) {
                        foreach (var handler in handlers) {
                            UnSetupInteractionHandler(handler);
                        }
                    }
                    break;

                default:
                    break;
            }


        }
    }
    
    /// <summary>
    /// 鼠标,键盘动作的重写;
    /// </summary>
    public partial class CanvasControl
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
            //指示正在处理路由事件;
            _handlingRoutedEvent = true;

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

            //指示处理路由事件结束;
            _handlingRoutedEvent = false;
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
            yield return e => _transactionStackCreatedInOneRoutedEvent = false;

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
            //指示需添加新的事务集合;
            yield return e => _transactionStackCreatedInOneRoutedEvent = false;

            //通知外部;
            yield return MouseDownOnPreview;
            //辅助响应;
            yield return MouseDownOnSnaping;
            //拖拽响应;
            yield return MouseDownOnDrag;
            //编辑响应;
            yield return OnMouseDownOnEditTool;
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
            //指示需添加新的事务集合;
            yield return e => _transactionStackCreatedInOneRoutedEvent = false;

            //通知外界;
            yield return MouseMoveOnPreview;

            //通知鼠标当前的位置节点;
            yield return MouseMoveOnCurrentMousePosition;

            //拖拽响应;
            yield return MouseMoveOnDrag;

            //辅助响应;
            yield return MouseMoveOnSnaping;

            //编辑响应;
            yield return OnMouseMoveOnEditTool;

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
            //指示需添加新的事务集合;
            yield return e => _transactionStackCreatedInOneRoutedEvent = false;

            //通知外界;
            yield return MouseUpOnPreview;

            //拖拽响应;
            yield return MouseUpOnDrag;
            //编辑响应;
            yield return OnMouseUpOnEditTool;
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
            //指示需添加新的事务集合;
            yield return e => _transactionStackCreatedInOneRoutedEvent = false;

            //通知外界;
            yield return KeyDownOnPreview;

            //编辑响应;
            yield return OnKeyDownOnEditTool;

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
            //指示需添加新的事务集合;
            yield return e => _transactionStackCreatedInOneRoutedEvent = false;

            //通知外界;
            yield return KeyUpOnPreview;

            //与被选取对对象的交互操作;
            yield return KeyUpOnSelectedDrawObjects;

            yield return OnKeyUpOnEditTool;
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
        private IEnumerable<Predicate<TextCompositionEventArgs>> GetTextInputEventHandlers() {
            //指示需添加新的事务集合;
            yield return e => _transactionStackCreatedInOneRoutedEvent = false;

            //通知外界;
            yield return TextInputOnPreview;

            yield return OnTextInputOnEditTool;
        }
    }

    /// <summary>
    /// 网格部分;
    /// </summary>
    public partial class CanvasControl
    {

        /// <summary>
        /// 网格线颜色;
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
        DependencyProperty.Register(nameof(GridLineBrush), typeof(Brush), typeof(CanvasControl), new FrameworkPropertyMetadata(DefaultGridLineBrush, FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// 网格线宽度;
        /// </summary>
        public double GridLineThickness
        {
            get { return (double)GetValue(GirdLineThicknessProperty); }
            set { SetValue(GirdLineThicknessProperty, value); }
        }

        public static readonly DependencyProperty GirdLineThicknessProperty =
            DependencyProperty.Register(nameof(GridLineThickness), typeof(double), typeof(CanvasControl), new FrameworkPropertyMetadata(0.2D,FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// 绘制网格;
        /// </summary>
        private void DrawGridLines(DrawingContext drawingContext)
        {
            //获得单元格的边长视图大小;
            var unitLength = CanvasProxy.ToScreen(1);
            if (unitLength < 3)
            {
                return;
            }

            if(GridLineThickness <= 0)
            {
                return;
            }

            if(GridLineBrush == null)
            {
                return;
            }

            var pen = new Pen
            {
                Brush = GridLineBrush,
                Thickness = GridLineThickness
            };
            pen.Freeze();

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
                drawingContext.DrawLine(pen, point0, point1);
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
                drawingContext.DrawLine(pen, point0, point1);
                vertiPos += unitLength;
            }
            #endregion
        }
    }

    /// <summary>
    /// 原点部分;
    /// </summary>
    public partial class CanvasControl
    {
        /// <summary>
        /// 原点的十字边长;
        /// </summary>
        public double PanLength
        {
            get { return (double)GetValue(PanLengthProperty); }
            set { SetValue(PanLengthProperty, value); }
        }


        public static readonly DependencyProperty PanLengthProperty =
            DependencyProperty.Register(nameof(PanLength), typeof(double), typeof(CanvasControl), new PropertyMetadata(72.0D));


        /// <summary>
        /// 原点的十字画刷;
        /// </summary>
        public Brush PanBrush
        {
            get { return (Brush)GetValue(PanBrushProperty); }
            set { SetValue(PanBrushProperty, value); }
        }

        public static readonly DependencyProperty PanBrushProperty =
            DependencyProperty.Register(nameof(PanBrush), typeof(Brush), typeof(CanvasControl), new PropertyMetadata(Brushes.White));


        /// <summary>
        /// 原点的十字宽度;
        /// </summary>
        public double PanThickness
        {
            get { return (double)GetValue(PanThicknessProperty); }
            set { SetValue(PanThicknessProperty, value); }
        }

        public static readonly DependencyProperty PanThicknessProperty =
            DependencyProperty.Register(nameof(PanThickness), typeof(double), typeof(CanvasControl), new PropertyMetadata(2D));


        /// <summary>
        /// 原点所在的视图坐标;
        /// </summary>
        public Vector2D PanScreenPosition {
            get { return (Vector2D)GetValue(PanScreenPositionProperty); }
            set { SetValue(PanScreenPositionProperty, value); }
        }

        
        public static readonly DependencyProperty PanScreenPositionProperty =
            DependencyProperty.Register(nameof(PanScreenPosition), typeof(Vector2D), typeof(CanvasControl),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsRender,
                    PanScreenPosition_PropertyChanged));

        private static void PanScreenPosition_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            if(!(d is CanvasControl canvasControl)) {
                return;
            }

            canvasControl.UpdateCanvasProxy();
        }


        /// <summary>
        /// 绘制原点十字;
        /// </summary>
        /// <param name="drawingContext"></param>
        private void DrawPan(DrawingContext drawingContext)
        {
            var panPen = new Pen { Brush = PanBrush,Thickness = PanThickness };
            panPen.Freeze();
            
            drawingContext.DrawLine(
                panPen,
                new Point(PanScreenPosition.X - PanLength / 2, PanScreenPosition.Y),
                new Point(PanScreenPosition.X + PanLength / 2, PanScreenPosition.Y)
            );

            drawingContext.DrawLine(
                panPen,
                new Point(PanScreenPosition.X, PanScreenPosition.Y - PanLength / 2),
                new Point(PanScreenPosition.X, PanScreenPosition.Y + PanLength / 2)
            );

        }
    }

    /// <summary>
    /// 缩放部分;
    /// </summary>
    public partial class CanvasControl
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
            DependencyProperty.Register(nameof(MinZoom), typeof(double), typeof(CanvasControl), new PropertyMetadata(0.0005));


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
                typeof(CanvasControl),
                new FrameworkPropertyMetadata(1D, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Zoom_PropertyChanged)
            );

        private static void Zoom_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs args) {
            if(!(d is CanvasControl canvasControl)) {
                return;
            }

            canvasControl.UpdateCanvasProxy();
        }

        /// <summary>
        /// 鼠标滚动时的缩放响应;
        /// </summary>
        /// <param name="e"></param>
        private bool MouseWheelOnZoom(MouseWheelEventArgs e)
        {
            //根据鼠标的位置响应缩放;
            var mouseUnitPos = CanvasProxy.ToUnit(Vector2DAdapter.ConverterToVector2D(e.GetPosition(this)));

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
            else if (CanvasProxy.ToScreen(1) < 1200)
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
            Vector2D unitPos,
            Point screenPos)
        {
            var unitPosInScreen = CanvasProxy.ToScreen(unitPos);
            //通过调整原点的视图偏移实现;
            PanScreenPosition = new Vector2D(
                PanScreenPosition.X + screenPos.X - unitPosInScreen.X,
                PanScreenPosition.Y + screenPos.Y - unitPosInScreen.Y
            );
        }
    }

    /// <summary>
    /// 画布拖拽处理部分;
    /// </summary>
    public partial class CanvasControl
    {
        /// <summary>
        /// 鼠标按下时的拖拽响应;
        /// </summary>
        /// <param name="e"></param>
        private bool MouseDownOnDrag(MouseButtonEventArgs e)
        {
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
            if (SystemInput.Mouse.MiddleButton == MouseButtonState.Pressed)
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

                PanScreenPosition = new Vector2D(
                    _lastPanOffsetBeforeDragging.X + mousePos.X - _lastMouseDownPointForPan.X,
                    _lastPanOffsetBeforeDragging.Y + mousePos.Y - _lastMouseDownPointForPan.Y
                );
                
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
            DependencyProperty.Register(nameof(DragCursor), typeof(Cursor), typeof(CanvasControl), new PropertyMetadata(Cursors.Hand));


    }

    /// <summary>
    /// Cursor(鼠标形状)部分;
    /// </summary>
    public partial class CanvasControl
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
            //编辑判断;
            yield return GetCursorOnEditTool;
            //选取判断;
            yield return GetCursorOnSelect;
        }
    }

    /// <summary>
    /// 编辑工具以及撤销/重做部分;
    /// </summary>
    public partial class CanvasControl
    {
        public event EventHandler<ValueChangedEventArgs<EditTool>> CurrentEditToolChanged;

        /// <summary>
        /// 根据当前的状态,判断是否可以使用编辑工具;
        /// </summary>
        private bool CheckEditToolAvailable() {
            if(CurrentEditTool == null) {
                return false;
            }

            if (IsReadOnly) {
                return false;
            }

            if(ActiveLayer == null) {
                return false;
            }
            
            return true;
        }
            

        /// <summary>
        /// 当前的编辑控件;
        /// </summary>
        public EditTool CurrentEditTool
        {
            get { return (EditTool)GetValue(CurrentEditToolProperty); }
            set { SetValue(CurrentEditToolProperty, value); }
        }

        public static readonly DependencyProperty CurrentEditToolProperty =
            DependencyProperty.Register(
                nameof(CurrentEditTool),
                typeof(EditTool),
                typeof(CanvasControl),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault
                    , CurrentEditTool_PropertyChanged)
            );

        /// <summary>
        /// 当前编辑工具发生变化时;
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void CurrentEditTool_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is CanvasControl ctrl))
            {
                return;
            }

            EditTool oldEditTool = e.OldValue as EditTool;
            EditTool newEditTool = e.NewValue as EditTool;

            //处理原编辑工具;
            if (oldEditTool != null)
            {
                ctrl.UnSetupEditTool(oldEditTool);
            }

            //处理新编辑工具;
            if (newEditTool != null)
            {
                ctrl.SetupEditTool(newEditTool);
            }
            
            ctrl.OnEditToolChanged();

            //通知当前编辑工具发生了变化;
            ctrl.CurrentEditToolChanged?.Invoke(ctrl, new ValueChangedEventArgs<EditTool>(newEditTool, oldEditTool));
        }

        /// <summary>
        /// 当前编辑工具发生变化时;
        /// </summary>
        private void OnEditToolChanged()
        {
            //拖放选取状态处理;
            EditToolChangedOnDragingSelectDrawObject();
            //更新Cursor;
            UpdateCursor();
            
        }


        /// <summary>
        /// 装载编辑工具,添加视觉元素,订阅事件等;
        /// </summary>
        /// <param name="editTool"></param>
        private void SetupEditTool(EditTool editTool)
        {
            //添加视觉元素;
            AddDrawable(editTool);

            editTool.TransactionCommited += EditTool_TransactionCommited;
            editTool.CanUndoChanged += EditTool_CanUndoChanged;
            editTool.CanRedoChanged += EditTool_CanRedoChanged;


            RaiseCanUndoRedoChangedEvents();

            LastEditPosition = null;

            editTool.CanvasContext = this;
            editTool.BeginOperation();
        }

        /// <summary>
        /// 卸载编辑工具;
        /// </summary>
        /// <param name="editTool"></param>
        private void UnSetupEditTool(EditTool editTool)
        {
            if(editTool == null) {
                return;
            }

            //通知递交更改;
            editTool.Commit();
            
            RemoveDrawable(editTool);

            editTool.TransactionCommited -= EditTool_TransactionCommited;
            editTool.CanUndoChanged -= EditTool_CanUndoChanged;
            editTool.CanRedoChanged -= EditTool_CanRedoChanged;

            RaiseCanUndoRedoChangedEvents();
            
            LastEditPosition = null;

            editTool.EndOperation();
            editTool.CanvasContext = null;
        }
        
        /// <summary>
        /// 编辑工具的可重做状态变更时,触发事件;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditTool_CanRedoChanged(object sender, CanRedoChangedEventArgs e)
        {
            RaiseCanUndoRedoChangedEvents();
        }

        /// <summary>
        /// 编辑工具的可撤销状态变更时,触发事件;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditTool_CanUndoChanged(object sender, CanUndoChangedEventArgs e)
        {
            RaiseCanUndoRedoChangedEvents();
        }

        /// <summary>
        /// 编辑工具呈递事务时发生;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditTool_TransactionCommited(object sender, IEditTransaction e)
        {
            if (e == null)
            {
                return;
            }

            //保留到事务缓冲区中;便于撤销/重做操作;
            CommitTransaction(e);
        }

        /// <summary>
        /// 鼠标按下的编辑处理;
        /// </summary>
        /// <param name="e"></param>
        private bool OnMouseDownOnEditTool(MouseButtonEventArgs e) {
            if (!CheckEditToolAvailable()) {
                return false;
            }
            
            var viewPosition = Vector2DAdapter.ConverterToVector2D(e.GetPosition(this));
            var position = _activeSnapShape?.Position ?? CanvasProxy.ToUnit(viewPosition);
            
            //预处理位置;
            HandlePosition(position);

            //转换系统参数至协约中的参数;
            var arg = MouseEventAdapter.ConvertToMouseDownEventArgs(e, position);

            //通知编辑工具按下动作;
            CurrentEditTool.RaisePreviewMouseDown(arg);

            //变更上次编辑的位置;
            LastEditPosition = position;

            return arg.Handled;
        }
        
        /// <summary>
        /// 鼠标移动时的编辑处理;
        /// </summary>
        /// <param name="e"></param>
        private bool OnMouseMoveOnEditTool(MouseEventArgs e) {
            if (!CheckEditToolAvailable()) {
                return false;
            }

            var viewPosition = Vector2DAdapter.ConverterToVector2D(e.GetPosition(this));
            var position = _activeSnapShape?.Position ?? CanvasProxy.ToUnit(viewPosition);

            //预处理位置;
            HandlePosition(position);

            //转换系统参数至协约中的参数;
            var arg = MouseEventAdapter.ConvertToMouseMoveEventArgs(position);

            //通知编辑工具;
            CurrentEditTool.RaisePreviewMouseMove(arg);

            return arg.Handled;
        }

        /// <summary>
        /// 鼠标弹起时的编辑处理;
        /// </summary>
        /// <param name="e"></param>
        private bool OnMouseUpOnEditTool(MouseButtonEventArgs e) {
            if (!CheckEditToolAvailable()) {
                return false;
            }

            var viewPosition = Vector2DAdapter.ConverterToVector2D(e.GetPosition(this));
            var position = _activeSnapShape?.Position ?? CanvasProxy.ToUnit(viewPosition);

            //转换系统参数至协约中的参数;
            var arg = MouseEventAdapter.ConvertToMouseUpEventArgs(e, position);

            //通知编辑工具弹起动作;
            CurrentEditTool.RaisePreviewMouseUp(arg);

            return true;
        }
        
        /// <summary>
        /// 键盘按下时的编辑处理;
        /// </summary>
        /// <param name="e"></param>
        private bool OnKeyDownOnEditTool(KeyEventArgs e) {
            if (!CheckEditToolAvailable()) {
                return false;
            }

            var arg = KeyAdapter.ConvertToKeyDownEventArgs(e);

            //通知编辑工具按键按下;
            CurrentEditTool.RaisePreviewKeyDown(arg);

            return true;
        }

        /// <summary>
        /// 键盘弹起时的编辑处理;
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private bool OnKeyUpOnEditTool(KeyEventArgs e) {
            if (!CheckEditToolAvailable()) {
                return false;
            }

            var arg = KeyAdapter.ConvertToKeyUpEventArgs(e);

            //通知编辑工具按键弹起;
            CurrentEditTool.RaisePreviewKeyUp(arg);

            return arg.Handled;
        }
        
        /// <summary>
        /// 键盘键入时的编辑处理;
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private bool OnTextInputOnEditTool(TextCompositionEventArgs e) {
            if (!CheckEditToolAvailable()) {
                return false;
            }

            var args = TextInputAdapter.ConverterToTextInputEventArgs(e);
            CurrentEditTool.RaisePreviewTextInput(args);
            return args.Handled;
        }


        /// <summary>
        /// 根据当前是否处于编辑状态设定Cursor;
        /// </summary>
        /// <param name="e"></param>
        private Cursor GetCursorOnEditTool()
        {
            if (CurrentEditTool != null)
            {
                return Cursors.Cross;
            }
            return null;
        }


        /// <summary>
        /// 上次编辑的标识位置;
        /// </summary>
        public Vector2D LastEditPosition { get;private set; }

        /// <summary>
        /// 当当前编辑工具不为空,且指示<see cref="EditTool.IsEditing"/>为假时,不能显示辅助图形;
        /// </summary>
        /// <returns></returns>
        private CanSnapShowResult CanShowSnapOnEditTool()
        {
            if (CurrentEditTool != null)
            {
                return new CanSnapShowResult
                {
                    CanShow = CurrentEditTool.IsEditing,
                    Handled = true
                };
            }

            return new CanSnapShowResult (true, false);
        }

        
        
    }

    /// <summary>
    /// 撤销/重做部分;
    /// </summary>
    public partial class CanvasControl {

        /*撤销/重做部分*/

        /// <summary>
        /// 能否撤销;
        /// </summary>
        public bool CanUndo {
            get {
                //可撤销状态将根据是否处于编辑状态分为两种情况;
                if (CurrentEditTool != null) {
                    return CurrentEditTool.CanUndo;
                }
                return _undoTransactionBuffer.Count != 0;
            }
        }


        /// <summary>
        /// 能否重做;
        /// </summary>
        public bool CanRedo {
            get {
                //可重做状态将根据是否处于编辑状态分为两种情况;
                if (CurrentEditTool != null) {
                    return CurrentEditTool.CanRedo;
                }

                return _redoTransactionBuffer.Count != 0;
            }
        }

        /// <summary>
        /// 可撤销状态发生变化;
        /// </summary>
        public event EventHandler<CanUndoChangedEventArgs> CanUndoChanged;


        /// <summary>
        /// 重做;
        /// </summary>
        public void Redo() {
            //若处于重做或撤销状态则不能重做;
            if (_isRedoing || _isUndoing) {
                return;
            }

            _isRedoing = true;

            try {
                //重做操作将根据是否处于编辑状态分为两种情况;
                if (CurrentEditTool != null) {
                    CurrentEditTool.Redo();
                }
                else if (_redoTransactionBuffer.Count != 0) {
                    //拿取上次的重做事务集合,进行重做操作;
                    var lastRedoTransactions = _redoTransactionBuffer.Pop();

                    foreach (var transaction in lastRedoTransactions) {
                        if (transaction.CanRedo) {
                            transaction.Redo();
                        }
                    }

                    //触发事件;
                    EditTransactionRedone?.Invoke(this, new EditTransactionRedoneEventArgs(lastRedoTransactions));

                    //加入到撤销栈中;
                    var stack = new Stack<IEditTransaction>(lastRedoTransactions.Reverse());
                    _undoTransactionBuffer.Push(stack);
                }
            }
            finally {
                _isRedoing = false;
            }

            RaiseCanUndoRedoChangedEvents();
        }

        /// <summary>
        /// 撤销操作;
        /// </summary>
        public void Undo() {
            //若处于重做或撤销状态则不能撤销;
            if(_isRedoing || _isUndoing) {
                return;
            }

            _isUndoing = true;
            try {
                //撤销操作将根据是否处于编辑状态分为两种情况;
                if (CurrentEditTool != null) {
                    CurrentEditTool.Undo();
                }
                else if (_undoTransactionBuffer.Count != 0) {
                    var lastUndoTransactions = _undoTransactionBuffer.Pop();
                    foreach (var transaction in lastUndoTransactions) {
                        transaction.Undo();
                    }

                    //触发事件;
                    EditTransactionUndone?.Invoke(this, new EditTransactionUndoneEventArgs(lastUndoTransactions));

                    //加入到重做栈中;
                    var stack = new Stack<IEditTransaction>(lastUndoTransactions.Reverse());
                    _redoTransactionBuffer.Push(stack);
                }
            }
            finally {
                _isUndoing = false;
            }


            RaiseCanUndoRedoChangedEvents();
        }


        /// <summary>
        /// 可重做状态发生变化;
        /// </summary>
        public event EventHandler<CanRedoChangedEventArgs> CanRedoChanged;

        /// <summary>
        /// 触发可撤销/重做状态变更事件;
        /// </summary>
        private void RaiseCanUndoRedoChangedEvents() {
            CanRedoChanged?.Invoke(this, new CanRedoChangedEventArgs(CanRedo));
            CanUndoChanged?.Invoke(this, new CanUndoChangedEventArgs(CanUndo));
        }


        /// <summary>
        /// 添加事务;将事务添加至撤销栈内;
        /// </summary>
        /// <param name="editTransaction"></param>
        public void CommitTransaction(IEditTransaction editTransaction) {
            if (editTransaction == null) {
                throw new ArgumentNullException(nameof(editTransaction));
            }

            //若处于重做或撤销状态则不能呈递事务;
            if (_isRedoing || _isUndoing) {
                return;
            }

            //若正在处理路由事件,且已经建立了上一个事务栈,则直接附加到上一次的事务栈中;
            if (_handlingRoutedEvent && _undoTransactionBuffer.Count != 0 && _transactionStackCreatedInOneRoutedEvent) {
                var lastActions = _undoTransactionBuffer.Peek();
                lastActions.Push(editTransaction);

                _redoTransactionBuffer.Clear();
            }
            else {
                var stack = new Stack<IEditTransaction>();
                stack.Push(editTransaction);
                _undoTransactionBuffer.Push(stack);
                //当有新的事务入栈时,将清除重做栈;
                _redoTransactionBuffer.Clear();

                _transactionStackCreatedInOneRoutedEvent = true;

                //指示应附加到上次的事务集合中;
                //在无其他干预时，在下次添加事务时，新加入的事务将会附加到上一次被添加到撤销栈的事务集合中;
                //_handlingRoutedEvent = false;
            }

            RaiseCanUndoRedoChangedEvents();
        }

        /// <summary>
        /// 清除所有事务;
        /// </summary>
        public void ClearTransactions() {
            _undoTransactionBuffer.Clear();
            _redoTransactionBuffer.Clear();
            RaiseCanUndoRedoChangedEvents();
        }
    }

    /// <summary>
    /// 内容图层,绘制对象部分;
    /// </summary>
    public partial class CanvasControl
    {
        /// <summary>
        /// 当前活动图层;
        /// </summary>
        public CanvasLayer ActiveLayer
        {
            get { return (CanvasLayer)GetValue(ActiveLayerProperty); }
            set { SetValue(ActiveLayerProperty, value); }
        }

        public static readonly DependencyProperty ActiveLayerProperty =
            DependencyProperty.Register(
                nameof(ActiveLayer),
                typeof(CanvasLayer),
                typeof(CanvasControl),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    ActiveLayer_PropertyChanged
                )
            );

        private static void ActiveLayer_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            if(d is CanvasControl ctrl) {
                ctrl.ActiveLayerChanged?.Invoke(
                    ctrl,
                    new ValueChangedEventArgs<CanvasLayer>(e.OldValue as CanvasLayer,e.NewValue as CanvasLayer)
                );
            }
        }

        public event EventHandler<ValueChangedEventArgs<CanvasLayer>> ActiveLayerChanged;

        /// <summary>
        /// 所有内容图层;
        /// </summary>
        public IEnumerable<CanvasLayer> Layers
        {
            get { return (IEnumerable<CanvasLayer>)GetValue(LayersProperty); }
            set { SetValue(LayersProperty, value); }
        }

        public static readonly DependencyProperty LayersProperty =
            DependencyProperty.Register(nameof(Layers), typeof(IEnumerable<CanvasLayer>), typeof(CanvasControl), new PropertyMetadata(null, Layers_PropertyChanged));

        /// <summary>
        /// 图层集合发生变更时;
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void Layers_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is CanvasControl ctrl))
            {
                return;
            }

            var oldLayers = e.OldValue as IEnumerable<CanvasLayer>;
            var newLayers = e.NewValue as IEnumerable<CanvasLayer>;

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
            if(!(sender is IEnumerable<CanvasLayer> layers)) {
                return;
            }

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var item in e.NewItems)
                    {
                        if (!(item is CanvasLayer layer))
                        {
                            continue;
                        }

                        SetupLayer(layer);
                    }
                    break;


                case NotifyCollectionChangedAction.Remove:
                    foreach (var item in e.OldItems)
                    {
                        if (!(item is CanvasLayer layer))
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
                    _canvasLayers.ToList().ForEach(layer => {
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
        /// <param name="canvasLayer"></param>
        private void SetupLayer(CanvasLayer canvasLayer)
        {
            //添加画布内容;
            AddDrawable(canvasLayer);

            AddDrawObjects(canvasLayer.DrawObjects);
          
            //图层内绘制对象增减清除时,延长/缩减/清除绘制对象的缓冲池;
            canvasLayer.DrawObjectsAdded += CanvasLayer_DrawObjectsAdded;
            canvasLayer.DrawObjectsRemoved += CanvasLayer_DrawObjectsRemoved;
            canvasLayer.DrawObjectsClearing += CanvasLayer_DrawObjectClearing;

            //可见状态变化时进行响应;
            canvasLayer.IsVisibleChanged += CanvasLayer_IsVisibleChanged;

            _canvasLayers.Add(canvasLayer);
            
        }
        
        /// <summary>
        /// 卸载图层;
        /// </summary>
        /// <param name="canvasLayer"></param>
        private void UnSetupLayer(CanvasLayer canvasLayer)
        {
            RemoveDrawable(canvasLayer);
            //卸载该图层内所有绘制对象;
            RemoveAllVisualsOfLayer(canvasLayer);

            canvasLayer.DrawObjectsAdded -= CanvasLayer_DrawObjectsAdded;
            canvasLayer.DrawObjectsRemoved -= CanvasLayer_DrawObjectsRemoved;
            canvasLayer.DrawObjectsClearing -= CanvasLayer_DrawObjectClearing;

            canvasLayer.IsVisibleChanged -= CanvasLayer_IsVisibleChanged;

            _canvasLayers.Remove(canvasLayer);
        }
        
        /// <summary>
        /// 图层内可绘制元素被添加时的响应;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CanvasLayer_DrawObjectsAdded(object sender,IEnumerable<DrawObject> e)
        {
            if (e == null)
            {
                return;
            }

            AddDrawObjects(e);
        }
        
        /// <summary>
        /// 被清除响应;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CanvasLayer_DrawObjectClearing(object sender, EventArgs e) {
            if(!(sender is CanvasLayer layer)) {
                return;
            }

            RemoveAllVisualsOfLayer(layer);
        }


        /// <summary>
        /// 添加绘制对象;
        /// </summary>
        /// <param name="drawObject"></param>
        private void AddDrawObjects(IEnumerable<DrawObject> drawObjects) {
            if (drawObjects == null) {
                return;
            }

            foreach (var drawObject in drawObjects) {
                AddDrawable(drawObject);

                drawObject.IsVisibleChanged += DrawObject_IsVisibleChanged;
                drawObject.EditTransActionCommited += DrawObject_EditTransActionCommited;
                drawObject.IsSelectedChanged += DrawObject_IsSelectedChanged;
                drawObject.IsEditingChanged += DrawObject_IsEditingChanged;
            }

            DrawObjectsAdded?.Invoke(this, new DrawObjectsAddedEventArgs(drawObjects));
        }


        /// <summary>
        /// 移除绘制元素;
        /// </summary>
        /// <param name="drawObject"></param>
        private void RemoveDrawObjects(IEnumerable<DrawObject> drawObjects)
        {
            if(drawObjects == null) {
                return;
            }

            foreach (var drawObject in drawObjects) {
                RemoveDrawable(drawObject);

                drawObject.IsVisibleChanged -= DrawObject_IsVisibleChanged;
                drawObject.EditTransActionCommited -= DrawObject_EditTransActionCommited;
                drawObject.IsSelectedChanged -= DrawObject_IsSelectedChanged;
                drawObject.IsEditingChanged -= DrawObject_IsEditingChanged;
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
        /// 绘制对象呈递事务时加入到撤销栈中;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DrawObject_EditTransActionCommited(object sender, IEditTransaction e)
        {
            if (e == null)
            {
                return;
            }
            CommitTransaction(e);
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
        /// 绘制对象是否被编辑状态发生变化时,触发相关事件;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DrawObject_IsEditingChanged(object sender, ValueChangedEventArgs<bool> e) {
            if (!(sender is DrawObject drawObject)) {
                return;
            }
            
            DrawObjectIsEditingChanged?.Invoke(this, new DrawObjectIsEditingChangedEventArgs(drawObject, e.NewValue, e.OldValue));
        }

        /// <summary>
        /// 图层内可绘制元素被移除时的响应;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CanvasLayer_DrawObjectsRemoved(object sender, IEnumerable<DrawObject> e)
        {
            if (e == null)
            {
                return;
            }

            RemoveDrawObjects(e);
            
        }

        /// <summary>
        /// 添加Drawble对象,扩充缓冲区以及VisualTree等操作;
        /// </summary>
        /// <param name="drawable"></param>
        private void AddDrawable(IDrawable drawable)
        {
            //若缓存中包含当前该绘制对象,则返回;
            if (_visualDict.ContainsKey(drawable))
            {
                return;
            }

            //若不包含,则加入缓存队列中;
            var drawingVisual = new DrawingVisual
            {
                Clip = new RectangleGeometry(new Rect
                {
                    Width = this.ActualWidth,
                    Height = this.ActualHeight
                })
            };

            _visualDict.Add(drawable, drawingVisual);

            //加入Visual Tree;
            _visualContainer.AddVisual(drawingVisual);

            //订阅该对象的视觉变化事件;
            drawable.VisualChanged += Drawable_VisualChanged;

            //绘制该对象;
            DrawDrawable(drawable);
        }

        /// <summary>
        /// 移除Drawable对象,删减缓冲区以及VisualTree等操作;
        /// </summary>
        /// <param name="drawable"></param>
        private void RemoveDrawable(IDrawable drawable)
        {
            //若缓存中不包含当前该绘制对象,则返回;
            if (!_visualDict.ContainsKey(drawable))
            {
                return;
            }
            //从缓存队列中移除;
            var drawingVisual = _visualDict[drawable];
            _visualDict.Remove(drawable);

            //从Viusal Tree中移除;
            _visualContainer.RemoveVisual(drawingVisual);

            //退订视觉变化事件;
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

            if (!(drawable is CanvasElement canvasElement) || canvasElement.IsVisible)
            {
                drawable.Draw(InternalCanvas, CanvasProxy);
            }


            dc.Close();
            InternalCanvas.DrawingContext = null;
        }

        
        /// <summary>
        /// 移除来自对应图层内的所有绘制元素;
        /// </summary>
        /// <param name="canvasLayer"></param>
        private void RemoveAllVisualsOfLayer(CanvasLayer canvasLayer)
        {
            //遍历缓存中所有的Visual,移除该图层内的所有元素;
            var removeVisualPairs = _visualDict.Where(p =>
            {
                if (!(p.Key is DrawObject drawObject))
                {
                    return false;
                }
                return drawObject.Parent == canvasLayer;
            });

            RemoveDrawObjects(removeVisualPairs.Select(p => p.Key).OfType<DrawObject>().ToList());
        }

        /// <summary>
        /// 图层可见状态发生变化时的响应;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CanvasLayer_IsVisibleChanged(object sender, EventArgs e)
        {
            if (!(sender is CanvasLayer canvasLayer))
            {
                return;
            }

            //若可见,则加入来自该图层内的所有元素;
            if (canvasLayer.IsVisible)
            {
                AddDrawObjects(canvasLayer.DrawObjects);
            }
            //若不可见,则移除制来自该图层内的所有元素;
            else
            {
                RemoveDrawObjects(canvasLayer.DrawObjects);
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

        /// <summary>
        /// 根据当前是否存在绘制对象在自编辑的状态,指示是否可以显示辅助图形,
        /// </summary>
        /// <returns></returns>
        private CanSnapShowResult CanShowSnapOnLayersAndDrawObjects()
        {
            ///当存在绘制对象处于自编辑状态时 < see cref = "DrawObject.IsEditing" />,可以显示辅助图形,并指示已处理;
            if (this.GetInteractionableLayers()?.SelectMany(p => p.DrawObjects).Any(p => p.IsEditing) ?? false)
            {
                return new CanSnapShowResult(true, true);
            }

            return new CanSnapShowResult(false, false);
        }
    }

    /// <summary>
    /// 辅助规则部分;
    /// </summary>
    public partial class CanvasControl
    {
        /// <summary>
        /// 辅助是否可用;
        /// </summary>
        public bool IsSnapingEnabled
        {
            get { return (bool)GetValue(IsSnapingEnabledProperty); }
            set { SetValue(IsSnapingEnabledProperty, value); }
        }

        public static readonly DependencyProperty IsSnapingEnabledProperty =
            DependencyProperty.Register(nameof(IsSnapingEnabled), typeof(bool), typeof(CanvasControl), new PropertyMetadata(true));
        
        /// <summary>
        /// 辅助规则集合;
        /// </summary>
        public IEnumerable<ISnapShapeRule> SnapShapeRules
        {
            get { return (IEnumerable<ISnapShapeRule>)GetValue(SnapShapeRulesProperty); }
            set { SetValue(SnapShapeRulesProperty, value); }
        }

        public static readonly DependencyProperty SnapShapeRulesProperty =
            DependencyProperty.Register(nameof(SnapShapeRules), typeof(IEnumerable<ISnapShapeRule>), typeof(CanvasControl), new PropertyMetadata(null));


        /// <summary>
        /// 鼠标移动时,辅助的判断;
        /// </summary>
        /// <param name="e"></param>
        private bool MouseMoveOnSnaping(MouseEventArgs e)
        {
            SetSnapStateOnMouse(e);
            return false;
        }


        /// <summary>
        /// 鼠标按下时,辅助的判断;
        /// </summary>
        /// <param name="e"></param>
        private bool MouseDownOnSnaping(MouseEventArgs e)
        {

            SetSnapStateOnMouse(e);
            
            return false;
        }
        

        /// <summary>
        /// 根据鼠标的位置设定当前的辅助图形;
        /// </summary>
        /// <param name="e"></param>
        private void SetSnapStateOnMouse(MouseEventArgs e)
        {
            if (_activeSnapShape != null)
            {
                RemoveDrawable(_activeSnapShape);
            }

            var position = CanvasProxy.ToUnit(Vector2DAdapter.ConverterToVector2D(e.GetPosition(this)));

            //预处理位置;
            HandlePosition(position);

            var activeSnapShape = GetSnapShape(position);

            if (activeSnapShape != null)
            {
                var canShow = true;

                //遍历能否显示辅助图形的指示器队列,直到某一个指示器指示处理完成或完成遍历;
                foreach (var getter in GetCanShowSnapShapeGetters())
                {
                    var tuple = getter();
                    canShow = tuple.CanShow;

                    if (tuple.Handled)
                    {
                        break;
                    }
                }

                if (canShow)
                {
                    AddDrawable(activeSnapShape);
                }
            }
            
            //通知事件;
            MouseHoverSnapShapeChanged?.Invoke(this, new ValueChangedEventArgs<ISnapShape>(activeSnapShape, _activeSnapShape));

            _activeSnapShape = activeSnapShape;
        }

        /// <summary>
        /// 获取是否能够显示辅助图形的结果;
        /// </summary>
        struct CanSnapShowResult
        {
            public CanSnapShowResult(bool canShow,bool handled)
            {
                CanShow = canShow;
                Handled = handled;
            }
            
            /// <summary>
            /// 是否能够显示;
            /// </summary>
            public bool CanShow { get; set; }

            /// <summary>
            /// 是否已经处理;
            /// </summary>
            public bool Handled { get; set; }
        }

        /// <summary>
        /// 获取是否能够显示辅助图形的设定器集合;
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Func<CanSnapShowResult>> GetCanShowSnapShapeGetters()
        {
            yield return CanShowSnapOnEditTool;
            yield return CanShowSnapOnLayersAndDrawObjects;
        }

        

        /// <summary>
        /// 鼠标所处的辅助节点发生变化时;
        /// </summary>
        public event EventHandler<ValueChangedEventArgs<ISnapShape>> MouseHoverSnapShapeChanged;


        /// <summary>
        /// 根据关注点的位置,获得辅助图形;
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        private ISnapShape GetSnapShape(Vector2D point)
        {
#if DEBUG
            //return null;
#endif

            if (!IsSnapingEnabled)
            {
                return null;
            }

            if (Layers == null)
            {
                return null;
            }

#if DEBUG
            //var scp = new Vector(CanvasProxy.ToScreen(point.X), CanvasProxy.ToScreen(point.Y));
            //if (scp.X < -2 && scp.X > 2 && scp.Y < -2 && scp.Y > 2)
            //{

            //}
#endif
            //获取所有绘制对象,进行辅助判断;
            var drawObjects = this.Layers.
                Where(p => p.IsVisible).SelectMany(p => p.DrawObjects).
                Where(p => p.IsVisible).Where(p => p.PointInObject(point, CanvasProxy) || p.IsEditing).
                OrderByDescending(p => p.IsSelected);

            //触发辅助判断的事件;
            var snapingEventArgs = new SnapingEventArgs(drawObjects);
            Snaping?.Invoke(this, snapingEventArgs);

            var drawObjectsToSnaping = snapingEventArgs.DrawObjects.ToArray();

            if (SnapShapeRules != null)
            {
                foreach (var snapShapeRule in SnapShapeRules)
                {
                    var snapShape = snapShapeRule.MatchSnapShape(drawObjectsToSnaping, point, this);
                    if (snapShape != null)
                    {
                        return snapShape;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 正在判断辅助的事件;
        /// </summary>
        public event EventHandler<SnapingEventArgs> Snaping;
    }

    /// <summary>
    /// 绘制对象的选取(点击,按键)部分;
    /// </summary>
    public partial class CanvasControl
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
            var mousePosition = CanvasProxy.ToUnit(Vector2DAdapter.ConverterToVector2D(e.GetPosition(this)));

            _hoveredDrawObjects.Clear();

            _hoveredDrawObjects.AddRange(
                this.GetVisibleLayers().SelectMany(p => p.DrawObjects.Where(
                    q => q.PointInObject(mousePosition, CanvasProxy)
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
            var mousePosition = CanvasProxy.ToUnit(Vector2DAdapter.ConverterToVector2D(e.GetPosition(this)));

            var clickedDrawObjects = this.GetAllVisibleDrawObjects().
                Where(p => p.PointInObject(mousePosition, CanvasProxy)).ToArray();

            bool canApplySelect = true;

            //若当前编辑工具不为空,由其指示是否能够应用点击选中;
            if (CurrentEditTool != null) {
                var clickArgs = new ClickSelectEventArgs(mousePosition, clickedDrawObjects);
                ClickSelect?.Invoke(this, clickArgs);
                //若当前编辑工具指示取消,则不能应用拖放选中;
                canApplySelect = !clickArgs.Cancel;
            }

            if (canApplySelect) {
                foreach (var drawObject in clickedDrawObjects) {
                    drawObject.IsSelected = !drawObject.IsSelected;
                    return true;
                }
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
    /// 绘制对象的选取(拖放)部分;
    /// </summary>
    public partial class CanvasControl
    {
        /// <summary>
        /// 将拖放选中图形加入到<see cref="_visualDict"/>
        /// </summary>
        private void AddSelectRectangleToDict() {
            if (!_visualDict.ContainsKey(_dragSelectRectangle)) {
                //将拖放选择的高亮矩形加入到视觉树中;
                AddDrawable(_dragSelectRectangle);
            }
        }

        /// <summary>
        /// 鼠标按下时拖放选取绘制对象的处理;
        /// </summary>
        /// <param name="e"></param>
        private bool MouseDownOnDragingSelectDrawObject(MouseEventArgs e)
        {
            var mousePosition = CanvasProxy.ToUnit(Vector2DAdapter.ConverterToVector2D(e.GetPosition(this)));
            //若上次点击位置不为空,则进行拖放选中操作;
            if (_lastMouseDownPositionForDragSelecting != null)
            {
                if (_dragSelectRectangle == null)
                {
                    return false;
                }

                //若矩形两对角点的横坐标或纵坐标相等,则无法组成矩形,不能拖放选择;
                if (_lastMouseDownPositionForDragSelecting.X == mousePosition.X
                    || _lastMouseDownPositionForDragSelecting.Y == mousePosition.Y)
                {
                    return false;
                }

                var rect = _dragSelectRectangle.Rectangle2D;
                if (rect == null)
                {
                    return false;
                }

                //遍历选中所有在框选范围中的可见绘制对象;
                var selectedObjects = this.GetVisibleLayers().SelectMany(p => p.DrawObjects).Where(p => p.IsVisible).
                        Where(p => p.ObjectInRectangle(rect, CanvasProxy, _anyPointSelectForDragSelect)).ToArray();

                bool canApplySelect = true;

                //若当前编辑工具不为空,由其指示是否能够应用拖放选中;
                if (CurrentEditTool != null) {
                    var dragArgs = new DragSelectEventArgs(mousePosition, rect, selectedObjects);
                    DragSelect?.Invoke(this, dragArgs);
                    //若当前编辑工具指示取消,则不能应用拖放选中;
                    canApplySelect = !dragArgs.Cancel;
                }

                if (canApplySelect) {
                    foreach (var drawObject in selectedObjects) {
                        drawObject.IsSelected = true;
                    }
                }

                //将选中矩形的数据置空;
                _lastMouseDownPositionForDragSelecting = null;
                _dragSelectRectangle.Rectangle2D = null;

                return true;
            }
            else if (e.LeftButton == MouseButtonState.Pressed)
            {
                //框选矩形的起点不能命中任何绘制对象;
                if (this.GetAllVisibleDrawObjects().Any(p => p.PointInObject(mousePosition, CanvasProxy))){
                    return false;
                }
                //记录本次鼠标点击的位置;
                _lastMouseDownPositionForDragSelecting = mousePosition;
                return true;
            }

            return false;
        }

        /// <summary>
        /// 鼠标移动时拖放选取绘制对象的处理;
        /// </summary>
        /// <param name="e"></param>
        private bool MouseMoveOnDragingSelectDrawObject(MouseEventArgs e)
        {
            var mousePosition = CanvasProxy.ToUnit(Vector2DAdapter.ConverterToVector2D(e.GetPosition(this)));

            //若鼠标上次按下的位置不为空,则绘制选择矩形;
            if (_lastMouseDownPositionForDragSelecting == null)
            {
                return false;
            }

            //将高亮矩形加入到视觉树中;
            AddDrawable(_dragSelectRectangle);

            //若矩形两对角点的横坐标或纵坐标相等,则无法组成矩形,无法绘制矩形;
            if (_lastMouseDownPositionForDragSelecting.X == mousePosition.X
                || _lastMouseDownPositionForDragSelecting.Y == mousePosition.Y)
            {
                _dragSelectRectangle.Rectangle2D = null;
                return false;
            }

            //计算拖放选中区域;
            var middleLineY = (_lastMouseDownPositionForDragSelecting.Y + mousePosition.Y) / 2;
            var rect = new Rectangle2D2(
                new Line2D(
                    new Vector2D(_lastMouseDownPositionForDragSelecting.X, middleLineY),
                    new Vector2D(mousePosition.X, middleLineY)
                ),
                Math.Abs(_lastMouseDownPositionForDragSelecting.Y - mousePosition.Y)
            );

            _dragSelectRectangle.Rectangle2D = rect;

            var dragArgs = new DragSelectMouseMoveEventArgs(_dragSelectRectangle.Rectangle2D, mousePosition);

            HandleDragSelectOnMouseMove(dragArgs);

            if (dragArgs.Handled) {
                _dragSelectRectangle.Rectangle2D = null;
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
        /// 键盘弹起时拖放选取绘制对象的处理;
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

                _dragSelectRectangle.Rectangle2D = null;
                _lastMouseDownPositionForDragSelecting = null;

                return true;
            }

            return false;
        }

        /// <summary>
        /// 当前编辑编辑工具发生变化时;拖放选取绘制对象的处理;
        /// </summary>
        /// <param name="editTool"></param>
        private void EditToolChangedOnDragingSelectDrawObject()
        {
            //清除所有绘制对象的状态;
            _lastMouseDownPositionForDragSelecting = null;
            _dragSelectRectangle.Rectangle2D = null;
        }

        /// <summary>
        /// 设置拖放选取时鼠标移动参数;
        /// <paramref name="mousePosition">鼠标的当前位置</paramref>
        /// </summary>
        private void HandleDragSelectOnMouseMove(DragSelectMouseMoveEventArgs dragArgs)
        {
            if (_lastMouseDownPositionForDragSelecting == null)
            {
                return;
            }

            if (_dragSelectRectangle.Rectangle2D == null)
            {
                return;
            }
            
            //当前编辑工具若不为空,则由其指示是否为任意选中;
            if (CurrentEditTool != null)
            {
                DrawSelectMouseMove?.Invoke(this, dragArgs);
            }
            
            //若未指示,则根据鼠标的走向判断;
            _anyPointSelectForDragSelect = dragArgs.IsAnyPoint ?? (dragArgs.Position.X < _lastMouseDownPositionForDragSelecting.X);
            
        }
    }

    /// <summary>
    /// 与被选择绘制对象的交互部分;
    /// </summary>
    public partial class CanvasControl
    {
        /// <summary>
        /// 当前正在与之交互的绘制对象集合缓存;
        /// </summary>
        private readonly List<DrawObject> _selectedDrawObjectsToBeInteracted = new List<DrawObject>();
        /// <summary>
        /// 根据当前的状态,判断是否可以与被选择绘制对象交互;
        /// </summary>
        /// <returns></returns>
        private bool CheckInteractWithSelectedDrawObjectsEnabled() {
            if (Layers == null) {
                return false;
            }
            
            if (IsReadOnly) {
                return false;
            }

            return true;
        }
        
        private bool InteractWithSelectedDrawObjects<TEventArgs>(
            Action<DrawObject,TEventArgs> handler,
            TEventArgs eventArgs)
            where TEventArgs:CanvasInput.HandleableEventArgs {

            if (!CheckInteractWithSelectedDrawObjectsEnabled()) {
                return false;
            }
            
            //与所有被选中的绘制对象进行交互;
            var selectedDrawObjects = this.GetAllVisibleDrawObjects().Where(q => q.IsSelected);

            var previewArgs = new PreviewDrawObjectsInteractionEventArgs(selectedDrawObjects);
            //通知外部,将要和选定的绘制对象进行交互;
            PreviewInteractionWithDrawObjects?.Invoke(this, previewArgs);
            //若外部指示已处理,则不继续处理;
            if (previewArgs.Cancel) {
                return false;
            }

            var handled = false;
            var editingDrawObjectsHandled = false;

            void HandleWithDrawObject(DrawObject drawObject) {
                handler(drawObject, eventArgs);
                //若内部指示被处理,则将外部的状态置为被处理;
                if (eventArgs.Handled) {
                    handled = true;
                }

                eventArgs.Handled = false;
            }

            _selectedDrawObjectsToBeInteracted.Clear();
            ///为了防止在遍历过程中可能出现的迭代对象被更改或列表集合状态发生更改的情况,导致异常产生,
            ///先行将使用的对象保存到缓存集合<see cref="_selectedDrawObjectsToBeInteracted"/>中;下同;
            _selectedDrawObjectsToBeInteracted.AddRange(selectedDrawObjects.Where(p => p.IsEditing));

            //优先与正在被编辑的绘制对象交互;
            _selectedDrawObjectsToBeInteracted.ForEach(editingDrawObject => {
                HandleWithDrawObject(editingDrawObject);
                //若任意一个绘制对象的正在编辑状态变为了否,则指示已经处理了,后继其他非正在被编辑的对象不能继续被交互;
                if (!editingDrawObject.IsEditing) {
                    editingDrawObjectsHandled = true;
                }
            });

            ///同上;
            _selectedDrawObjectsToBeInteracted.Clear();
            _selectedDrawObjectsToBeInteracted.AddRange(selectedDrawObjects.Where(p => !p.IsEditing));
            
            //若未指示已处理,则仍能与未被编辑的绘制对象处理;
            if (!editingDrawObjectsHandled) {
                _selectedDrawObjectsToBeInteracted.ForEach(nonEditingDrawObject => {
                    HandleWithDrawObject(nonEditingDrawObject);
                });
            }
            
            return handled;
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
            
            var mouseScreenPosition = Vector2DAdapter.ConverterToVector2D(e.GetPosition(this));
            var mousePosition = CanvasProxy.ToUnit(mouseScreenPosition);
            HandlePosition(mousePosition);
            var mouseDownEventArgs = MouseEventAdapter.ConvertToMouseDownEventArgs(e, _activeSnapShape?.Position ?? mousePosition);

            return InteractWithSelectedDrawObjects((drawObject, eventArgs) => drawObject.RaisePreviewMouseDown(eventArgs), mouseDownEventArgs);
        }

        /// <summary>
        /// 鼠标移动时与选取对象的交互;
        /// </summary>
        /// <param name="e"></param>
        private bool MouseMoveOnSelectedDrawObjects(MouseEventArgs e)
        {
            var mouseScreenPosition = Vector2DAdapter.ConverterToVector2D(e.GetPosition(this));
            var mousePosition = CanvasProxy.ToUnit(mouseScreenPosition);

            HandlePosition(mousePosition);

            //与所有选中的绘制对象进行交互;
            var args = MouseEventAdapter.ConvertToMouseMoveEventArgs(_activeSnapShape?.Position??mousePosition);

            return InteractWithSelectedDrawObjects((drawObject, eventArgs) => drawObject.RaisePreviewMouseMove(eventArgs), args);
        }

        /// <summary>
        /// 键盘按下时与选取对象的交互;
        /// </summary>
        /// <param name="e"></param>
        private bool KeyDownOnSelectedDrawObjects(KeyEventArgs e)
        {
            //与所有选中的绘制对象进行交互;
            var args = KeyAdapter.ConvertToKeyDownEventArgs(e);
         
            return InteractWithSelectedDrawObjects((drawObject,eventArgs) => drawObject.RaisePreviewKeyDown(eventArgs),args);
        }

        /// <summary>
        /// 键盘弹起时与选取对象的交互;
        /// </summary>
        /// <param name="e"></param>
        private bool KeyUpOnSelectedDrawObjects(KeyEventArgs e)
        {
            //与所有选中的绘制对象进行交互;
            var args = KeyAdapter.ConvertToKeyUpEventArgs(e);
            return InteractWithSelectedDrawObjects((drawObject, eventArgs) => drawObject.RaisePreviewKeyUp(eventArgs), args);
        }

        
    }

    /// <summary>
    /// 鼠标实时位置;
    /// </summary>
    public partial class CanvasControl
    {
        /// <summary>
        /// 当前的鼠标所在的工程数学坐标;
        /// </summary>
        public Vector2D CurrentMousePosition { get; private set; }

        /// <summary>
        /// 当前的鼠标所在的工程数学坐标发生变化事件;
        /// </summary>
        public event EventHandler<ValueChangedEventArgs<Vector2D>> CurrentMousePositionChanged;

        /// <summary>
        /// 鼠标移动时,通知当前鼠标的工程数学坐标节点变化;
        /// </summary>
        /// <param name="e"></param>
        private bool MouseMoveOnCurrentMousePosition(MouseEventArgs e)
        {
            var point = CanvasProxy.ToUnit(Vector2DAdapter.ConverterToVector2D(e.GetPosition(this)));
            var oldValue = CurrentMousePosition;
            CurrentMousePosition = point;
            CurrentMousePositionChanged?.Invoke(this, new ValueChangedEventArgs<Vector2D>(CurrentMousePosition, oldValue));

            return false;
        }


    }

    /// <summary>
    /// 输入设备封装;
    /// </summary>
    public partial class CanvasControl
    {
        private CanvasInput.IInputDevice _inputDevice;
        public CanvasInput.IInputDevice InputDevice => _inputDevice??(_inputDevice = new InputDeviceWrapper(this));
    }

    /// <summary>
    /// 原生对象部分;
    /// </summary>
    public partial class CanvasControl {
        public void AddUIObject(object nativeVisual) {
            if (nativeVisual == null) {
                throw new ArgumentNullException(nameof(nativeVisual));
            }

            if(!(nativeVisual is UIElement uiElem)) {
                return;
            }

            if (this.Children.Contains(uiElem)) {
                return;
            }

            this.Children.Add(uiElem);
        }

        public void RemoveUIObject(object nativeVisual) {

            if (nativeVisual == null) {
                throw new ArgumentNullException(nameof(nativeVisual));
            }

            if (!(nativeVisual is UIElement uiElem)) {
                return;
            }

            if (!this.Children.Contains(uiElem)) {
                return;
            }

            this.Children.Remove(uiElem);
        }
    }
    
    /// <summary>
    /// 输入事件;
    /// </summary>
    public partial class CanvasControl {

        /// <summary>
        /// 鼠标按下事件;
        /// </summary>
        public event EventHandler<CanvasInput.MouseDownEventArgs> CanvasPreviewMouseDown;

        /// <summary>
        /// 鼠标移动事件;
        /// </summary>
        public event EventHandler<CanvasInput.MouseMoveEventArgs> CanvasPreviewMouseMove;


        /// <summary>
        /// 鼠标弹起事件;
        /// </summary>
        public event EventHandler<CanvasInput.MouseUpEventArgs> CanvasPreviewMouseUp;

        /// <summary>
        /// 键盘按下事件;
        /// </summary>
        public event EventHandler<CanvasInput.KeyDownEventArgs> CanvasPreviewKeyDown;

        /// <summary>
        /// 键盘弹起事件;
        /// </summary>
        public event EventHandler<CanvasInput.KeyUpEventArgs> CanvasPreviewKeyUp;

        /// <summary>
        /// 键盘输入事件;
        /// </summary>
        public event EventHandler<CanvasInput.TextInputEventArgs> CanvasPreviewTextInput;

        
        /// <summary>
        /// 获取鼠标事件的数学坐标位置;
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private Vector2D GetMouseUnitPosition(MouseEventArgs e) {
            return Vector2DAdapter.ConverterToVector2D(e.GetPosition(this));
        }
        
        /// <summary>
        /// 鼠标按下时,通知外部;
        /// </summary>
        /// <param name="e"></param>
        private bool MouseDownOnPreview(MouseButtonEventArgs e) {
            var viewPosition = GetMouseUnitPosition(e);
            var position = _activeSnapShape?.Position ?? CanvasProxy.ToUnit(viewPosition);
            var arg = MouseEventAdapter.ConvertToMouseDownEventArgs(e, position);

            CanvasPreviewMouseDown?.Invoke(this, arg);

            return arg.Handled;
        }
        
        private bool MouseMoveOnPreview(MouseEventArgs e) {
            var viewPosition = GetMouseUnitPosition(e);
            var position = _activeSnapShape?.Position ?? CanvasProxy.ToUnit(viewPosition);
            var arg = MouseEventAdapter.ConvertToMouseMoveEventArgs(position);

            RaisePreviewMouseMove(arg);
            
            return arg.Handled;
        }

        private bool MouseUpOnPreview(MouseButtonEventArgs e) {
            var viewPosition = GetMouseUnitPosition(e);
            var position = _activeSnapShape?.Position ?? CanvasProxy.ToUnit(viewPosition);
            var arg = MouseEventAdapter.ConvertToMouseUpEventArgs(e,position);

            RaisePreviewMouseUp(arg);

            return arg.Handled;
        }

        private bool KeyDownOnPreview(KeyEventArgs e) {
            var arg = KeyAdapter.ConvertToKeyDownEventArgs(e);
            RaisePreviewKeyDown(arg);
            CanvasPreviewKeyDown?.Invoke(this, arg);
            return arg.Handled;
        }

        private bool KeyUpOnPreview(KeyEventArgs e) {
            var arg = KeyAdapter.ConvertToKeyUpEventArgs(e);
            RaisePreviewKeyUp(arg);
            return arg.Handled;
        }
        

        private bool TextInputOnPreview(TextCompositionEventArgs e) {
            var arg = TextInputAdapter.ConverterToTextInputEventArgs(e);
            RaisePreviewTextInput(arg);
            return arg.Handled;
        }


        public void RaisePreviewMouseDown(CanvasInput.MouseDownEventArgs e) {
            CanvasPreviewMouseDown?.Invoke(this, e);
        }

        public void RaisePreviewMouseMove(CanvasInput.MouseMoveEventArgs e) {
            CanvasPreviewMouseMove?.Invoke(this, e);
        }

        public void RaisePreviewMouseUp(CanvasInput.MouseUpEventArgs e) {
            CanvasPreviewMouseUp?.Invoke(this, e);
        }

        public void RaisePreviewKeyDown(CanvasInput.KeyDownEventArgs e) {
            CanvasPreviewKeyDown?.Invoke(this, e);
        }

        public void RaisePreviewKeyUp(CanvasInput.KeyUpEventArgs e) {
            CanvasPreviewKeyUp?.Invoke(this, e);
        }

        public void RaisePreviewTextInput(CanvasInput.TextInputEventArgs e) {
            CanvasPreviewTextInput?.Invoke(this, e);
        }


    }
}
