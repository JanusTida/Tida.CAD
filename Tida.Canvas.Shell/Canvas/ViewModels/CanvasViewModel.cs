
using Tida.Canvas.Contracts;
using Tida.Canvas.Events;
using Tida.Canvas.Shell.Contracts.Canvas;
using Tida.Canvas.Shell.Contracts.Canvas.Events;
using Tida.Geometry.Primitives;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Windows;
using Tida.Canvas.Infrastructure.Snaping;
using Tida.Canvas.Shell.Contracts.Menu;
using Tida.Canvas.Shell.Contracts.Snaping;
using Tida.Canvas.Shell.Contracts.InteractionHandlers;
using Tida.Canvas.Shell.Canvas.Models;
using static Tida.Canvas.Shell.Contracts.Canvas.Constants;
using Tida.Canvas.Shell.Contracts.Common;

namespace Tida.Canvas.Shell.Canvas.ViewModels {
    /// <summary>
    /// 主视图模型;
    /// </summary>
    [Export]
    public partial class CanvasViewModel : ExtensibleBindableBase, ICanvasDataContext
    {

        [ImportingConstructor]
        public CanvasViewModel(
            [ImportMany]IEnumerable<Lazy<ISnapShapeRule, ISnapShapeRuleMetaData>> snapShapeRules,
            [ImportMany]IEnumerable<Lazy<ISnapShapeRuleProvider, ISnapShapeRuleProviderMetaData>> snapShapeRuleProviders,
            [ImportMany]IEnumerable<Lazy<ICanvasInteractionHandlerProvider, ICanvasInteractionHandlerProviderMetaData>> interactionHandlers,
            [ImportMany]IEnumerable<Lazy<ICanvasLayersProvider, ICanvasLayersProviderMetadata>> canvasLayersProviders,

            [ImportMany]IEnumerable<Lazy<IMenuItem, IMenuItemMetaData>> menuItems
        )
        {
            this._mefSnapShapeRuleProviders = snapShapeRuleProviders;
            this._mefSnapShapeRules = snapShapeRules;

            //添加位置预处理器集合;
            InteractionHandlers.AddRange(interactionHandlers.OrderBy(p => p.Metadata.Order).Select(p => p.Value).Select(p => p.CreateHandler()));

            this._mefCanvasLayersProviders = canvasLayersProviders;
            this._mefMenuItems = menuItems.Where(p => p.Metadata.OwnerGUID == Menu_CanvasContextMenu).OrderBy(p => p.Metadata.Order).ToArray();

            Initialize();
        }

        private readonly IEnumerable<Lazy<ISnapShapeRule, ISnapShapeRuleMetaData>> _mefSnapShapeRules;
        private readonly IEnumerable<Lazy<ISnapShapeRuleProvider, ISnapShapeRuleProviderMetaData>> _mefSnapShapeRuleProviders;
        private readonly IEnumerable<Lazy<ICanvasLayersProvider, ICanvasLayersProviderMetadata>> _mefCanvasLayersProviders;

        private readonly Lazy<IMenuItem, IMenuItemMetaData>[] _mefMenuItems;

        private void Initialize()
        {
            InitializeSnapRules();
            IntializeContextMenu();
            ResetLayers();
            Zoom = 1;
        }

        /// <summary>
        /// 初始化辅助规则;
        /// </summary>
        private void InitializeSnapRules()
        {
            SnapShapeRules.Clear();

            var snapRuleItems = _mefSnapShapeRuleProviders.
                Union<object>(_mefSnapShapeRules).OrderBy(p =>
                {
                    if (p is Lazy<ISnapShapeRule, ISnapShapeRuleMetaData> snapShapeTuple)
                    {
                        return snapShapeTuple.Metadata.Order;
                    }
                    else if (p is Lazy<ISnapShapeRuleProvider, ISnapShapeRuleProviderMetaData> snapShapeProviderTuple)
                    {
                        return snapShapeProviderTuple.Metadata.Order;
                    }
                    else
                    {
                        return int.MaxValue;
                    }
                });

            foreach (var snapRuleItem in snapRuleItems)
            {
                if (snapRuleItem is Lazy<ISnapShapeRuleProvider, ISnapShapeRuleProviderMetaData> snapShapeRuleProvider)
                {
                    SnapShapeRules.AddRange(snapShapeRuleProvider.Value.Rules);
                }
                else if (snapRuleItem is Lazy<ISnapShapeRule, ISnapShapeRuleMetaData> snapShapeRulePair)
                {
                    SnapShapeRules.Add(snapShapeRulePair.Value);
                }
            }
        }

        ObservableCollection<CanvasLayerEx> ICanvasDataContext.Layers => Layers;

        /// <summary>
        /// 位置预处理器集合;
        /// </summary>
        public ObservableCollection<CanvasInteractionHandler> InteractionHandlers { get; } = new ObservableCollection<CanvasInteractionHandler>();

    }

    /// <summary>
    /// <see cref="ICanvasScreenConvertable"/>部分;
    /// </summary>
    public partial class CanvasViewModel
    {

        private DelegateCommand<SizeChangedEventArgs> _sizeChangedCommand;
        public DelegateCommand<SizeChangedEventArgs> SizeChangedCommand => _sizeChangedCommand ??
            (_sizeChangedCommand = new DelegateCommand<SizeChangedEventArgs>(
                e =>
                {
                    try
                    {
                        CommonEventHelper.GetEvent<CanvasSizeChangedEvent>().Publish(this);
                        CommonEventHelper.PublishEventToHandlers<ICanvasSizeChangedEventHandler, ICanvasDataContext>(this);
                    }
                    catch (Exception ex)
                    {
                        LoggerService.WriteException(ex);
                    }
                }
            ));

        private DelegateCommand<ValueChangedEventArgs<ICanvasScreenConvertable>> _canvasProxyChangedCommand;
        public DelegateCommand<ValueChangedEventArgs<ICanvasScreenConvertable>> CanvasProxyChangedCommand => _canvasProxyChangedCommand ??
            (_canvasProxyChangedCommand = new DelegateCommand<ValueChangedEventArgs<ICanvasScreenConvertable>>(
                e =>
                {
                    this.CanvasProxy = e.NewValue;
                }
            ));


        /// <summary>
        /// 画布坐标转化器,由于WPF无法绑定只读依赖属性,此值将由外部指定;
        /// </summary>
        public ICanvasScreenConvertable CanvasProxy { get; private set; }
    }

    /// <summary>
    /// 只读部分;
    /// </summary>
    public partial class CanvasViewModel
    {
        /// <summary>
        /// 是否只读;
        /// </summary>
        private bool _isReadOnly;
        public bool IsReadOnly
        {
            get { return _isReadOnly; }
            set { SetProperty(ref _isReadOnly, value); }
        }

    }

    /// <summary>
    /// 鼠标当前位置以及辅助信息;
    /// </summary>
    public partial class CanvasViewModel
    {

        public Vector2D CurrentMousePosition { get; private set; }

        //public event EventHandler CurrentMousePositionChanged;


        /// <summary>
        /// 鼠标当前位置发生变化时;
        /// </summary>
        private DelegateCommand<ValueChangedEventArgs<Vector2D>> _currentMousePositionChangedCommand;
        public DelegateCommand<ValueChangedEventArgs<Vector2D>> CurrentMousePositionChangedCommand => _currentMousePositionChangedCommand ??
            (_currentMousePositionChangedCommand = new DelegateCommand<ValueChangedEventArgs<Vector2D>>(
                e =>
                {
                    if (e == null)
                    {
                        return;
                    }


                    CurrentMousePosition = e.NewValue;
                    CommonEventHelper.GetEvent<CanvasCurrentMousePositionChangedEvent>().Publish(this);
                }
            ));

        /// <summary>
        /// 当前活跃辅助图形发生变化时;
        /// </summary>
        private DelegateCommand<ValueChangedEventArgs<ISnapShape>> _MouseHoverSnapShapeChangedCommand;
        public DelegateCommand<ValueChangedEventArgs<ISnapShape>> MouseHoverSnapShapeChangedCommand => _MouseHoverSnapShapeChangedCommand ??
            (_MouseHoverSnapShapeChangedCommand = new DelegateCommand<ValueChangedEventArgs<ISnapShape>>(
                e =>
                {
                    if (e == null)
                    {
                        return;
                    }

                    MouseHoverSnapShape = e.NewValue;
                    CommonEventHelper.GetEvent<CanvasMouseHoverSnapShapeChangedEvent>().Publish(this);
                }
            ));


        public ISnapShape MouseHoverSnapShape { get; private set; }

        //public event EventHandler MouseHoverSnapShapeChanged;

    }

    /// <summary>
    /// 图层,编辑工具,撤销/重做操作;
    /// </summary>
    public partial class CanvasViewModel
    {

        /// <summary>
        /// 编辑事务被呈递事件;
        /// </summary>
        //public event EventHandler<IEditTransaction> TransactionCommitted;


        //撤销/重做请求事件;
        public InteractionRequest<Notification> RedoRequest { get; } = new InteractionRequest<Notification>();
        public InteractionRequest<Notification> UndoRequest { get; } = new InteractionRequest<Notification>();

        //清除事务请求;
        public InteractionRequest<Notification> ClearTransactionsRequest { get; } = new InteractionRequest<Notification>();



        /// <summary>
        /// 当前的编辑工具;
        /// </summary>
        private EditTool _currentEditTool;
        public EditTool CurrentEditTool
        {
            get => _currentEditTool;
            set
            {
                if (_currentEditTool == value)
                {
                    return;
                }

                var oldEditTool = _currentEditTool;
                SetProperty(ref _currentEditTool, value);

                var args = new CanvasEditToolChangedEventArgs(
                    this, new ValueChangedEventArgs<EditTool>(_currentEditTool, oldEditTool)
                );

                CommonEventHelper.GetEvent<CanvasEditToolChangedEvent>().Publish(args);
                CommonEventHelper.PublishEventToHandlers<ICanvasEditToolChangedEventHandler, CanvasEditToolChangedEventArgs>(args);
            }
        }

        /// <summary>
        /// 当前编辑工具发生了变化;
        /// </summary>
        //public event EventHandler CurrentEditToolChanged;

        private DelegateCommand _selectCommand;
        public DelegateCommand SelectCommand => _selectCommand ??
            (_selectCommand = new DelegateCommand(
                () =>
                {
                    CurrentEditTool = null;
                }
            ));


        /// <summary>
        /// 当前活动图层;
        /// </summary>
        private CanvasLayerEx _activeLayer;

        public CanvasLayerEx ActiveLayer
        {
            get => _activeLayer;
            set
            {
                SetProperty(ref _activeLayer, value);
            }
        }

        /// <summary>
        /// 活跃图层发生变化时;
        /// </summary>
        //public event EventHandler ActiveLayerChanged;


        /// <summary>
        /// 内容图层集合;
        /// </summary>
        public ObservableCollection<CanvasLayerEx> Layers { get; } = new ObservableCollection<CanvasLayerEx>();


        private DelegateCommand<ValueChangedEventArgs<CanvasLayer>> _activeLayerChangedCommand;
        public DelegateCommand<ValueChangedEventArgs<CanvasLayer>> ActiveLayerChangedCommand => _activeLayerChangedCommand ??
            (_activeLayerChangedCommand = new DelegateCommand<ValueChangedEventArgs<CanvasLayer>>(
                e =>
                {
                    var args = new CanvasActiveLayerChangedEventArgs(this, e);
                    CommonEventHelper.GetEvent<CanvasActiveLayerChangedEvent>().Publish(args);
                    CommonEventHelper.PublishEventToHandlers<ICanvasActiveLayerChangedEventHandler, CanvasActiveLayerChangedEventArgs>(args);
                }
            ));

        /// <summary>
        /// 复位图层状态;使得图层集合仅存在默认图层;
        /// </summary>
        public void ResetLayers()
        {
            Layers.Clear();
            ActiveLayer = null;

            var defaultLayersProvider = _mefCanvasLayersProviders.OrderByDescending(p => p.Metadata.Priority).FirstOrDefault()?.Value;
            if (defaultLayersProvider == null)
            {
                return;
            }

            Layers.AddRange(defaultLayersProvider.CreateLayers());

            if (Layers.Count != 0)
            {
                ActiveLayer = Layers[0];
            }
        }
    }

    /// <summary>
    /// 当前缩放;
    /// </summary>
    public partial class CanvasViewModel
    {
        /// <summary>
        /// 当前缩放比例;(放大变大，缩小变小);
        /// </summary>

        private double _zoom = 1;
        public double Zoom
        {
            get { return _zoom; }
            set
            {
                SetProperty(ref _zoom, value);
                CommonEventHelper.GetEvent<CanvasZoomChangedEvent>().Publish(this);
            }
        }

        //public event EventHandler ZoomChanged;
    }

    /// <summary>
    /// 原点部分;
    /// </summary>
    public partial class CanvasViewModel
    {

        private Vector2D _panScreenPosition = null;
        /// <summary>
        /// 原点所在的视图坐标;
        /// </summary>
        public Vector2D PanScreenPosition
        {
            get { return _panScreenPosition; }
            set { SetProperty(ref _panScreenPosition, value); }
        }

    }

    /// <summary>
    /// 撤销/重做部分;
    /// </summary>
    public partial class CanvasViewModel
    {
        public bool CanUndo { get; private set; }
        public bool CanRedo { get; private set; }

        public void Undo()
        {
            if (!CanUndo)
            {
                return;
            }

            UndoRequest.Raise(new Notification());
        }

        public void Redo()
        {
            if (!CanRedo)
            {
                return;
            }

            RedoRequest.Raise(new Notification());
        }

        /// <summary>
        /// 撤销命令;
        /// </summary>
        private DelegateCommand _undoCommand;
        public DelegateCommand UndoCommand => _undoCommand ??
            (_undoCommand = new DelegateCommand(
                Undo,
                () => CanUndo
            ));

        /// <summary>
        /// 重做命令;
        /// </summary>
        private DelegateCommand _redoCommand;
        public DelegateCommand RedoCommand => _redoCommand ??
            (_redoCommand = new DelegateCommand(
                Redo,
                () => CanRedo
            ));

        /// <summary>
        /// 是否可重做发生了变化;
        /// </summary>
        private DelegateCommand<CanUndoChangedEventArgs> _canUndoChangedCommand;
        public DelegateCommand<CanUndoChangedEventArgs> CanUndoChangedCommand => _canUndoChangedCommand ??
            (_canUndoChangedCommand = new DelegateCommand<CanUndoChangedEventArgs>(
                e =>
                {
                    if (e == null)
                    {
                        return;
                    }
                    if (CanUndo == e.CanUndo)
                    {
                        return;
                    }

                    CanUndo = e.CanUndo;
                    var args = new CanvasCanUndoChangedEventArgs(this, e);
                    CommonEventHelper.GetEvent<CanvasCanUndoChangedEvent>().Publish(args);
                    CommonEventHelper.PublishEventToHandlers<ICanvasCanUndoChangedEventHandler, CanvasCanUndoChangedEventArgs>(args);
                }
            ));

        /// <summary>
        /// 是否可重做发生了变化;
        /// </summary>
        private DelegateCommand<CanRedoChangedEventArgs> _canRedoChangedCommand;
        public DelegateCommand<CanRedoChangedEventArgs> CanRedoChangedCommand => _canRedoChangedCommand ??
            (_canRedoChangedCommand = new DelegateCommand<CanRedoChangedEventArgs>(
                e =>
                {
                    if (e == null)
                    {
                        return;
                    }

                    if (CanRedo == e.CanRedo)
                    {
                        return;
                    }

                    CanRedo = e.CanRedo;
                    var args = new CanvasCanRedoChangedEventArgs(this, e);
                    CommonEventHelper.GetEvent<CanvasCanRedoChangedEvent>().Publish(args);
                    CommonEventHelper.PublishEventToHandlers<ICanvasCanRedoChangedEventHandler, CanvasCanRedoChangedEventArgs>(args);
                }
            ));

        /// <summary>
        /// 执行了撤销事务命令;
        /// </summary>
        private DelegateCommand<EditTransactionUndoneEventArgs> _editTransactionUndoneCommand;
        public DelegateCommand<EditTransactionUndoneEventArgs> EditTransactionUndoneCommand => _editTransactionUndoneCommand ??
            (_editTransactionUndoneCommand = new DelegateCommand<EditTransactionUndoneEventArgs>(
                e =>
                {
                    var args = new CanvasEditTransactionUndoneEventArgs(this, e);
                    CommonEventHelper.GetEvent<CanvasEditTransactionUndoneEvent>().Publish(args);
                    CommonEventHelper.PublishEventToHandlers<ICanvasEditTransactionUndoneEventHandler, CanvasEditTransactionUndoneEventArgs>(args);
                }
            ));


        /// <summary>
        /// 执行了重做事务命令;
        /// </summary>
        private DelegateCommand<EditTransactionRedoneEventArgs> _editTransactionRedoneCommand;
        public DelegateCommand<EditTransactionRedoneEventArgs> EditTransactionRedoneCommand => _editTransactionRedoneCommand ??
            (_editTransactionRedoneCommand = new DelegateCommand<EditTransactionRedoneEventArgs>(
                e =>
                {
                    var args = new CanvasEditTransactionRedoneEventArgs(this, e);
                    CommonEventHelper.GetEvent<CanvasEditTransactionRedoneEvent>().Publish(args);
                    CommonEventHelper.PublishEventToHandlers<ICanvasEditTransactionRedoneEventHandler, CanvasEditTransactionRedoneEventArgs>(args);
                }
            ));

    }

    /// <summary>
    /// 上下文菜单部分;
    /// </summary>
    public partial class CanvasViewModel
    {
        /// <summary>
        /// 初始化上下文菜单;
        /// </summary>
        private void IntializeContextMenu()
        {
            ContextMenuItems.Clear();

            foreach (var menuItem in _mefMenuItems)
            {
                ContextMenuItems.Add(new MenuItemModel(new CreatedMenuItem(menuItem.Value, menuItem.Metadata)));
            }

        }
        public ObservableCollection<MenuItemModel> ContextMenuItems { get; } = new ObservableCollection<MenuItemModel>();
    }

    /// <summary>
    /// 状态部分;
    /// </summary>
    public partial class CanvasViewModel
    {
        /// <summary>
        /// 清除包括事务状态;
        /// </summary>
        public void ClearTransactions()
        {
            //清除事务栈;
            ClearTransactionsRequest.Raise(new Notification());
        }

        /// <summary>
        /// 呈递事务;
        /// </summary>
        /// <param name="editTransaction"></param>
        public void CommitTransaction(IEditTransaction editTransaction)
        {

            if (editTransaction == null)
            {
                throw new ArgumentNullException(nameof(editTransaction));
            }

            EditTransactionCommited?.Invoke(this, editTransaction);
        }

        public event EventHandler<IEditTransaction> EditTransactionCommited;
    }

    /// <summary>
    /// 辅助规则部分;
    /// </summary>
    public partial class CanvasViewModel
    {
        /// <summary>
        /// 辅助规则集合;
        /// </summary>
        public ObservableCollection<ISnapShapeRule> SnapShapeRules { get; } = new ObservableCollection<ISnapShapeRule>();


        private bool _isSnapingEnabled = true;
        /// <summary>
        /// 辅助是否可用;
        /// </summary>
        public bool IsSnapingEnabled
        {
            get { return _isSnapingEnabled; }
            set
            {
                if (_isSnapingEnabled == value)
                {
                    return;
                }

                SetProperty(ref _isSnapingEnabled, value);

                CommonEventHelper.GetEvent<CanvasIsSnapingEnabledChangedEvent>().Publish(this);
                CommonEventHelper.PublishEventToHandlers<ICanvasIsSnapingEnabledChangedEventHandler, ICanvasDataContext>(this);
            }
        }

        //public event EventHandler IsSnapingEnabledChanged;
    }

    /// <summary>
    /// 画布内绘制对象选定状态;
    /// </summary>
    public partial class CanvasViewModel
    {
        /// <summary>
        /// 画布内绘制对象选定发生了变化命令;
        /// </summary>
        private DelegateCommand<DrawObjectSelectedChangedEventArgs> _drawObjectIsSelectedChangedCommand;
        public DelegateCommand<DrawObjectSelectedChangedEventArgs> DrawObjectIsSelectedChangedCommand => _drawObjectIsSelectedChangedCommand ??
            (_drawObjectIsSelectedChangedCommand = new DelegateCommand<DrawObjectSelectedChangedEventArgs>(
                e =>
                {
                    var args = new CanvasDrawObjectSelectedChangedEventArgs(this, e);
                    CommonEventHelper.GetEvent<CanvasDrawObjectIsSelectedChangedEvent>().Publish(args);
                    CommonEventHelper.PublishEventToHandlers<ICanvasDrawObjectIsSelectedChangedEventHandler, CanvasDrawObjectSelectedChangedEventArgs>(args);
                }
            ));

    }

    /// <summary>
    /// 绘制对象被移除部分;
    /// </summary>
    public partial class CanvasViewModel
    {

        private DelegateCommand<DrawObjectsRemovedEventArgs> _drawObjectRemovedCommand;
        public DelegateCommand<DrawObjectsRemovedEventArgs> DrawObjectRemovedCommand => _drawObjectRemovedCommand ??
            (_drawObjectRemovedCommand = new DelegateCommand<DrawObjectsRemovedEventArgs>(
                e =>
                {
                    var args = new CanvasDrawObjectsRemovedEventArgs(this, e);
                    CommonEventHelper.GetEvent<CanvasDrawObjectsRemovedEvent>().Publish(args);
                    CommonEventHelper.PublishEventToHandlers<ICanvasDrawObjectsRemovedEventHandler, CanvasDrawObjectsRemovedEventArgs>(args);
                }
            ));


        private DelegateCommand<DrawObjectsAddedEventArgs> _drawObjectAddedCommand;
        public DelegateCommand<DrawObjectsAddedEventArgs> DrawObjectAddedCommand => _drawObjectAddedCommand ??
            (_drawObjectAddedCommand = new DelegateCommand<DrawObjectsAddedEventArgs>(
                e =>
                {
                    var args = new CanvasDrawObjectsAddedEventArgs(this, e);
                    CommonEventHelper.GetEvent<CanvasDrawObjectsAddedEvent>().Publish(args);
                    CommonEventHelper.PublishEventToHandlers<ICanvasDrawObjectsAddedEventHandler, CanvasDrawObjectsAddedEventArgs>(args);
                }
            ));


    }
}
