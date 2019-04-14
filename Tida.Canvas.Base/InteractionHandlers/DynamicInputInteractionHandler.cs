using Tida.Canvas.Infrastructure.DynamicInput;
using Tida.Canvas.Infrastructure.InteractionHandlers;
using Tida.Application.Contracts.Setting;
using Tida.Canvas.Contracts;
using Tida.Canvas.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using static Tida.Canvas.Base.Constants;
using System.ComponentModel.Composition;
using Tida.Canvas.Shell.Contracts.InteractionHandlers;

namespace Tida.Canvas.Base.InteractionHandlers {
    /// <summary>
    /// 画布交互处理器——动态输入;
    /// </summary>
    public class DynamicInputInteractionHandler : CanvasInteractionHandler{
        public DynamicInputInteractionHandler() {
            Initialize();
        }

        /// <summary>
        /// 初始化;
        /// </summary>
        private void Initialize() {
            //订阅可用变更事件;
            IsEnabledChanged += DynamicInputInteractionHandler_IsEnabledChanged;
        }

        private void UnInitialize() {
            //退订可用变更事件;
            IsEnabledChanged -= DynamicInputInteractionHandler_IsEnabledChanged;
        }

        private void DynamicInputInteractionHandler_IsEnabledChanged(object sender, ValueChangedEventArgs<bool> e) {
            RefreshCurrentEditTool(CanvasControl);
        }
        
        /// <summary>
        /// 所有编辑工具的交互处理器;
        /// </summary>
        public static readonly List<ICanvasControlDynamicInputerProvider> CanvasControlDynamicInputerProviders = new List<ICanvasControlDynamicInputerProvider>();
        /// <summary>
        /// 当前使用的动态输入处理器;
        /// </summary>
        private IDynamicInputer _currentCanvasControlDynamicInputer;

        /// <summary>
        /// 设定当前的动态输入处理器为<paramref name="dynamicInputer"/>
        /// </summary>
        /// <param name="dynamicInputer"></param>
        private void SetCurrentCanvasControlDynamicInputer(IDynamicInputer dynamicInputer) {
            UnSetupCurrentEditToolDynamicInputer();
            _currentCanvasControlDynamicInputer?.Dispose();

            _currentCanvasControlDynamicInputer = dynamicInputer;
            SetupCurrentEditToolDynamicInputer();

            RaiseVisualChanged();
        }
        
        /// <summary>
        /// 卸载当前的编辑工具动态输入处理器;
        /// </summary>
        private void UnSetupCurrentEditToolDynamicInputer() {
            if(_currentCanvasControlDynamicInputer == null) {
                return;
            }

            _currentCanvasControlDynamicInputer.VisualChanged -= CurrentEditToolDynamicInputer_VisualChanged;
        }
        
        /// <summary>
        /// 装载当前的编辑工具动态输入处理器;
        /// </summary>
        private void SetupCurrentEditToolDynamicInputer() {
            if (_currentCanvasControlDynamicInputer == null) {
                return;
            }

            _currentCanvasControlDynamicInputer.VisualChanged += CurrentEditToolDynamicInputer_VisualChanged;
        }

        /// <summary>
        /// 当当前的编辑工具动态输入处理器内容发生变化时,触发内容变化事件;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CurrentEditToolDynamicInputer_VisualChanged(object sender, EventArgs e) {
            RaiseVisualChanged();
        }


        private static bool _isEnabled = true;
        /// <summary>
        /// 动态输入是否可用;
        /// </summary>
        public static bool IsEnabled {
            get => _isEnabled;
            set {
                if(_isEnabled == value) {
                    return;
                }

                _isEnabled = value;
                IsEnabledChanged?.Invoke(null, new ValueChangedEventArgs<bool>(_isEnabled, !_isEnabled));
            }
        }

        public static event EventHandler<ValueChangedEventArgs<bool>> IsEnabledChanged;

        protected override void OnLoad(ICanvasControl canvasControl) {
            if(canvasControl == null) {
                return;
            }

            canvasControl.CurrentEditToolChanged += CanvasControl_CurrentEditToolChanged;
            canvasControl.DrawObjectIsEditingChanged += CanvasControl_DrawObjectIsEditingChanged;
            SetCurrentCanvasControlDynamicInputer(null);

            //base.OnLoad(canvasControl);
        }

        private void CanvasControl_DrawObjectIsEditingChanged(object sender, DrawObjectIsEditingChangedEventArgs e) {
            if (!(sender is ICanvasControl canvasControl)) {
                return;
            }

            SetCurrentCanvasControlDynamicInputer(null);

            RefreshCurrentEditTool(canvasControl);
        }

        /// <summary>
        /// 当画布控件的当前编辑工具发生变化时;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CanvasControl_CurrentEditToolChanged(object sender, ValueChangedEventArgs<EditTool> e) {
            if(!(sender is ICanvasControl canvasControl)) {
                return;
            }

            SetCurrentCanvasControlDynamicInputer(null);
            
            var editTool = canvasControl.CurrentEditTool;
            if (editTool == null) {
                return;
            }

            RefreshCurrentEditTool(canvasControl);
        }

        private void RefreshCurrentEditTool(ICanvasControl canvasControl) {
            SetCurrentCanvasControlDynamicInputer(null);
            
            if (!IsEnabled) {
                return;
            }

            foreach (var inputProvider in CanvasControlDynamicInputerProviders) {
                var inputer = inputProvider.CreateInputer(canvasControl);
                if (inputer != null) {
                    SetCurrentCanvasControlDynamicInputer(inputer);
                    break;
                }
            }
        }

        protected override void OnUnLoad(ICanvasControl canvasControl) {
            if(canvasControl == null) {
                return;
            }

            canvasControl.CurrentEditToolChanged -= CanvasControl_CurrentEditToolChanged;
            canvasControl.DrawObjectIsEditingChanged -= CanvasControl_DrawObjectIsEditingChanged;
            SetCurrentCanvasControlDynamicInputer(null);

            //base.OnUnLoad(canvasControl);
        }
        
        public override void Draw(ICanvas canvas, ICanvasScreenConvertable canvasProxy) {
            if (!IsEnabled) {
                return;
            }

            _currentCanvasControlDynamicInputer?.Draw(canvas, canvasProxy);

            base.Draw(canvas, canvasProxy);
        }

        protected override void OnDispose() {
            UnInitialize();
        }
    }

    /// <summary>
    /// 动态输入交互处理器提供器;
    /// </summary>
    [ExportCanvasInteractionHandlerProvider]
    class DynamicInputInteractionHandlerProvider : ICanvasInteractionHandlerProvider {
        [ImportingConstructor]
        public DynamicInputInteractionHandlerProvider([ImportMany]IEnumerable<Lazy<ICanvasControlDynamicInputerProvider,ICanvasInteractionHandlerProviderMetaData>> mefEditToolDynamicInputerProviders) {

            DynamicInputInteractionHandler.CanvasControlDynamicInputerProviders.
                AddRange(mefEditToolDynamicInputerProviders.OrderBy(p => p.Metadata.Order).Select(p => p.Value));

            var section = SettingsService.GetOrCreateSection(SettingSectionGUID_Canvas);

            //默认设为可用;
            section?.SetAttribute(SettingName_DynamicInput, true);
            DynamicInputInteractionHandler.IsEnabled = true;
            DynamicInputInteractionHandler.IsEnabledChanged += DynamicInputInteractionHandler_IsEnabledChanged;
        }

        /// <summary>
        /// 同步设定;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DynamicInputInteractionHandler_IsEnabledChanged(object sender, ValueChangedEventArgs<bool> e) {

            var section = SettingsService.GetOrCreateSection(SettingSectionGUID_Canvas);

            //默认设为可用;
            section?.SetAttribute(SettingName_DynamicInput, e.NewValue);
        }

        public CanvasInteractionHandler CreateHandler() => new DynamicInputInteractionHandler();
    }
}
