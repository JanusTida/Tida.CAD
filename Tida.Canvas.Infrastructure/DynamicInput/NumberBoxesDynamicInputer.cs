using Tida.Canvas.Infrastructure.NativePresentation;
using Tida.Canvas.Contracts;
using Tida.Geometry.External;
using Tida.Geometry.Primitives;
using Tida.Canvas.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using Tida.Canvas.Input;
using Tida.Canvas.Infrastructure.DynamicInput.Events;

namespace Tida.Canvas.Infrastructure.DynamicInput {
    /// <summary>
    /// 本类适用于设定多个NumberBox的动态输入情况;
    /// </summary>
    public partial class NumberBoxesDynamicInputer :
        CanvasControlDynamicInputerBase {

        public NumberBoxesDynamicInputer(
            NumberBoxInteractionHandlerContainer numberBoxInteractionHandler,
            ICanvasControl canvasControl,
            INumberBoxService numberBoxService
        ) : base(canvasControl) {
            _numberBoxService = numberBoxService ?? throw new ArgumentNullException(nameof(numberBoxService));
            _numberBoxContainer = _numberBoxService.CreateContainer();
            
            NumberBoxInteractionHandlerContainer = numberBoxInteractionHandler ?? throw new ArgumentNullException(nameof(numberBoxInteractionHandler));

            Initialize();
        }

        private readonly INumberBoxService _numberBoxService;
        /// <summary>
        /// NumberBox交互容器;
        /// </summary>
        public NumberBoxInteractionHandlerContainer NumberBoxInteractionHandlerContainer { get; }
        private readonly List<NumberBoxCell> _numberBoxCells = new List<NumberBoxCell>();
        
        /// <summary>
        /// 指示是否内部正在产生变化,
        /// 防止在<see cref="INumberBox.NumberChanged"/>的处理中,
        /// 实际数据和<see cref="INumberBox.Number"/>相互影响;
        /// </summary>
        private bool _internalChanging;

        /// <summary>
        /// 输入框容器;
        /// </summary>
        private readonly INumberBoxContainer _numberBoxContainer;

        /// <summary>
        /// 初始化;
        /// </summary>
        private void Initialize() {
            InitializeNumberBoxes();
            InitializeNumberInteractionHandlers();

            Reset();
        }


        /// <summary>
        /// 初始化数字交互容器;
        /// </summary>
        private void InitializeNumberInteractionHandlers() {
            foreach (var numberBoxInteractionHandler in
                NumberBoxInteractionHandlerContainer.NumberBoxInteractionHandlers.Distinct()) {

                numberBoxInteractionHandler.NumberChanged += NumberboxInteractionHandler_NumberChanged;
                numberBoxInteractionHandler.ScreenPositionChanged += NumberBoxInteractionHandler_ScreenPositionChanged;
            }

            NumberBoxInteractionHandlerContainer.VisualChanged += NumberBoxInteractionHandlerContainer_VisualChanged;
        }

        private void UnInitializeNumberInteractionHandlers() {
            foreach (var numberBoxInteractionHandler in
                NumberBoxInteractionHandlerContainer.NumberBoxInteractionHandlers.Distinct()) {

                numberBoxInteractionHandler.NumberChanged -= NumberboxInteractionHandler_NumberChanged;
                numberBoxInteractionHandler.ScreenPositionChanged -= NumberBoxInteractionHandler_ScreenPositionChanged;

            }

            NumberBoxInteractionHandlerContainer.VisualChanged -= NumberBoxInteractionHandlerContainer_VisualChanged;
        }

        private void NumberBoxInteractionHandlerContainer_VisualChanged(object sender, EventArgs e) {
            this.RaiseVisualChanged();
        }


        private void NumberBoxInteractionHandler_ScreenPositionChanged(object sender, ValueChangedEventArgs<Vector2D> e) {
            if (!(sender is NumberBoxInteractionHandler numberBoxInteractionHandler)) {
                return;
            }

            var numberBox = _numberBoxCells.FirstOrDefault(p => p.NumberBoxInteractionHandler == numberBoxInteractionHandler)?.NumberBox;
            if(numberBox == null) {
                return;
            }

            numberBox.Visible = e.NewValue != null;

            SetPropertyInternal(
                e.NewValue,
                pos => {
                    numberBox.Position = pos;
                }
            );
        }

        private void NumberboxInteractionHandler_NumberChanged(object sender, ValueChangedEventArgs<double?> e) {
            if (!(sender is NumberBoxInteractionHandler numberBoxInteractionHandler)) {
                return;
            }
            UpdateNumberBoxWithNumberboxInteractionHandler(numberBoxInteractionHandler);
        }

        /// <summary>
        /// 将<paramref name="numberBoxInteractionHandler"/>的数据同步到对应的<see cref="INumberBox"/>
        /// </summary>
        /// <param name="numberBoxInteractionHandler"></param>
        private void UpdateNumberBoxWithNumberboxInteractionHandler(NumberBoxInteractionHandler numberBoxInteractionHandler) {

            var numberBox = _numberBoxCells.FirstOrDefault(p => p.NumberBoxInteractionHandler == numberBoxInteractionHandler)?.NumberBox;
            if (numberBox == null) {
                return;
            }

            numberBox.Visible = numberBoxInteractionHandler.Number != null;

            SetPropertyInternal(
                numberBoxInteractionHandler.Number,
                number => {
                    numberBox.Number = number;
                    numberBox.Visible = true;
                }
            );
        }

        /// <summary>
        /// 设定值;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="setAct"></param>
        /// <param name="value"></param>
        private void SetPropertyInternal<T>(T value, Action<T> setAct) {
            if (_internalChanging) {
                return;
            }

            if (value == null) {
                return;
            }
            else {
                _internalChanging = true;

                setAct(value);

                _internalChanging = false;
            }

        }

        protected override void OnDispose() {
            UnInitializeNumberBoxes();
            UnInitializeNumberInteractionHandlers();

            _numberBoxContainer.Dispose();
            NumberBoxInteractionHandlerContainer.Dispose();

            base.OnDispose();
        }

        public override void Draw(ICanvas canvas, ICanvasScreenConvertable canvasProxy) {

            NumberBoxInteractionHandlerContainer.Draw(canvas, canvasProxy);
            
            
            base.Draw(canvas, canvasProxy);
        }

        /// <summary>
        /// 复位,清除所有输入状态;
        /// </summary>
        private void Reset() {
            _numberBoxContainer.Reset();

            _numberBoxCells.ForEach(pair => pair.NumberBox.Visible = false);
        }
    }

    /// <summary>
    /// 文本框交互部分;
    /// </summary>
    public partial class NumberBoxesDynamicInputer {

        /// <summary>
        /// 初始化文本框;
        /// </summary>
        private void InitializeNumberBoxes() {
            if (_numberBoxContainer == null) {
                return;
            }
            
            
            CanvasControl.AddUIObject(_numberBoxContainer);

            foreach (var numberBoxInteractionHandler in 
                NumberBoxInteractionHandlerContainer.NumberBoxInteractionHandlers.Distinct()) {

                var numberBox = GetNumberBoxByNumberBoxInteractionHandler(numberBoxInteractionHandler);

                _numberBoxCells.Add(
                    new NumberBoxCell( numberBoxInteractionHandler,  numberBox )
                );
                
            }

            _numberBoxCells.ForEach(cell => {
                _numberBoxContainer.AddNumberBox(cell.NumberBox);

                cell.NumberBox.TabConfirmed += NumberBox_TabConfirmed;
                cell.NumberBox.EnterConfirmed += NumberBox_EnterConfirmed;
            });

            _numberBoxContainer.IsInputingChanged += NumberBoxContainer_IsInputingChanged;

            _numberBoxContainer.Reset();

            CanvasControl.CanvasPreviewKeyDown += CanvasControl_CanvasPreviewKeyDown;
        }

        private void UnInitializeNumberBoxes() {
            if (_numberBoxContainer == null) {
                return;
            }

            CanvasControl.RemoveUIObject(_numberBoxContainer);

            _numberBoxCells.ForEach(cell => {
                _numberBoxContainer.RemoveNumberBox(cell.NumberBox);

                cell.NumberBox.TabConfirmed -= NumberBox_TabConfirmed;
                cell.NumberBox.EnterConfirmed -= NumberBox_EnterConfirmed;
            });

            _numberBoxCells.Clear();

            _numberBoxContainer.IsInputingChanged -= NumberBoxContainer_IsInputingChanged;

            _numberBoxContainer.Reset();

            CanvasControl.CanvasPreviewKeyDown -= CanvasControl_CanvasPreviewKeyDown;
        }

        private void NumberBox_TabConfirmed(object sender, EventArgs e) {
            if(!(sender is INumberBox numberBox)) {
                return;
            }

            UpdateNumberBoxInteractionHandlerWithNumberbox(numberBox);
        }

        private void NumberBox_EnterConfirmed(object sender, EventArgs e) {
            if (_numberBoxCells.Any(p => p.NumberBoxInteractionHandler.Number == null)) {
                return;
            }

            if(!(sender is INumberBox numberBox)) {
                return;
            }

            UpdateNumberBoxInteractionHandlerWithNumberbox(numberBox);

            NumberBoxInteractionHandlerContainer.Commit();

            _numberBoxContainer.Reset();
        }

        /// <summary>
        /// 将<paramref name="numberBox"/>的数据同步到对应的<see cref="NumberBoxInteractionHandler"/>
        /// </summary>
        private void UpdateNumberBoxInteractionHandlerWithNumberbox(INumberBox numberBox) {

            if (numberBox == null) {
                throw new ArgumentNullException(nameof(numberBox));
            }

            var numberBoxInteractionHandler = _numberBoxCells.FirstOrDefault(p => p.NumberBox == numberBox)?.NumberBoxInteractionHandler;
            if (numberBoxInteractionHandler == null) {
                return;
            }

            SetPropertyInternal(
                numberBox.Number,
                number => {
                    numberBoxInteractionHandler.Reset();
                    numberBoxInteractionHandler.CommitNumber(number.Value);
                }
            );
        }
        
        private void NumberBoxContainer_IsInputingChanged(object sender, ValueChangedEventArgs<bool> e) {
            NumberBoxInteractionHandlerContainer.IsInputing = e.NewValue;
        }

        /// <summary>
        /// 处于输入状态,按下Esc键时,清除关注的文本框内的内容;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CanvasControl_CanvasPreviewKeyDown(object sender, KeyDownEventArgs e) {
            //if (e.Key == Key.Escape && NumberBoxInteractionHandlerContainer.IsInputing) {
            //    var cell = _numberBoxCells.FirstOrDefault(p => p.NumberBox.IsReadOnly);
            //    if(cell == null) {
            //        return;
            //    }
            //    cell.NumberBoxInteractionHandler.Reset();

            //    NumberBoxInteractionHandlerContainer.IsInputing = false;
            //    e.Handled = true;
            //}
        }
        
    }

    /// <summary>
    /// 状态部分;
    /// </summary>
    public partial class NumberBoxesDynamicInputer {
        /// <summary>
        /// 存储数字输入呈现于对应交互对应状态的单元;
        /// </summary>
        class NumberBoxCell {
            public NumberBoxCell(NumberBoxInteractionHandler numberBoxInteractionHandler,INumberBox numberBox) {

                NumberBox = numberBox ?? throw new ArgumentNullException(nameof(numberBox));

                NumberBoxInteractionHandler = numberBoxInteractionHandler ?? throw new ArgumentNullException(nameof(numberBoxInteractionHandler));

            }
            public NumberBoxInteractionHandler NumberBoxInteractionHandler { get; }
            public INumberBox NumberBox { get; }
        }

        private INumberBox GetNumberBoxByNumberBoxInteractionHandler(NumberBoxInteractionHandler numberBoxInteractionHandler) {

            if (numberBoxInteractionHandler == null) {
                throw new ArgumentNullException(nameof(numberBoxInteractionHandler));
            }
            
            var numberBox = _numberBoxService.CreateNumberBox();
            numberBox.SavedBits = numberBoxInteractionHandler.SavedBits;
            return numberBox;
        }
    }

    /// <summary>
    /// 与<see cref="INumberBox"/>的交互器;
    /// </summary>
    public class NumberBoxInteractionHandler : CanvasElement, IDrawable, IDisposable {
        private double? _number;
        /// <summary>
        /// 当前数值;
        /// </summary>
        public double? Number {
            get => _number;
            set {
                if (IsNumberCommited) {
                    throw new InvalidOperationException($"The number can't be modified while {nameof(IsNumberCommited)} is set to true,please invoke {nameof(Reset)} first.");
                }

                SetNumberCore(value);
            }
        }
        
        private void SetNumberCore(double? number) {
            if (_number == number) {
                return;
            }

            var oldValue = _number;
            _number = number;
            
            var changedArgs = new ValueChangedEventArgs<double?>(_number, oldValue);
            OnNumberChanged(changedArgs);

            NumberChanged?.Invoke(this, changedArgs);
        }


        /// <summary>
        /// 应用数字的更改;
        /// </summary>
        /// <param name="number">数字值</param>
        public void CommitNumber(double number) {
            IsNumberCommited = true;
            SetNumberCore(number);
            
            NumberCommited?.Invoke(this, new NumberCommitedEventArgs(Number));
        }

        /// <summary>
        /// 指定当前值的更改;
        /// </summary>
        public void CommitNumber() {
            IsNumberCommited = true;
            NumberCommited?.Invoke(this, new NumberCommitedEventArgs(Number));
        }

        /// <summary>
        /// 当前数字更改是否被呈递;
        /// </summary>
        public bool IsNumberCommited { get; private set; }
        
        private const int DefaultSaveBits = 4;
        private int _saveBits = DefaultSaveBits;
        ///<summary>
        /// 保留小数点小数的位数;
        /// </summary>
        public int SavedBits {
            get => _saveBits;
            set {
                if (value < 0) {
                    throw new ArgumentOutOfRangeException($"{nameof(value)} can't be less than zero.");
                }

                _saveBits = value;
            }
        }

        /// <summary>
        /// 当数字发生变化时发生;
        /// </summary>
        /// <param name="args"></param>
        protected virtual void OnNumberChanged(ValueChangedEventArgs<double?> args) {

        }

        /// <summary>
        /// 复位;解除冻结状态,属性重置;
        /// </summary>
        public void Reset() {
            IsNumberCommited = false;
            SetNumberCore(null);
            OnReset();
        }
        
        protected virtual void OnReset() {

        }

        public void Dispose() {

        }

        protected virtual void OnDispose() {

        }

        
        /// <summary>
        /// 数字已经发生变化;
        /// </summary>
        public event EventHandler<ValueChangedEventArgs<double?>> NumberChanged;

        /// <summary>
        /// 数字被呈递事件;
        /// </summary>
        public event EventHandler<NumberCommitedEventArgs> NumberCommited;

        private Vector2D _screenPosition;
        /// <summary>
        /// 屏幕位置;
        /// </summary>
        public Vector2D ScreenPosition {
            get => _screenPosition;
            set {
                var oldValue = _screenPosition;
                _screenPosition = value;
                ScreenPositionChanged?.Invoke(this, new ValueChangedEventArgs<Vector2D>(_screenPosition, oldValue));
            }
        }

        /// <summary>
        /// 屏幕位置变化事件;
        /// </summary>
        public event EventHandler<ValueChangedEventArgs<Vector2D>> ScreenPositionChanged;
        
    }

    public static class NumberBoxInteractionHandlerExtension {
        /// <summary>
        /// 获取<see cref="NumberBoxInteractionHandler"/> 确定更改的数字;
        /// </summary>
        /// <param name="numberBoxInteractionHandler"></param>
        /// <returns></returns>
        public static double? GetCommitedNumber(this NumberBoxInteractionHandler numberBoxInteractionHandler) {

            if (numberBoxInteractionHandler == null) {
                throw new ArgumentNullException(nameof(numberBoxInteractionHandler));
            }

            if (!numberBoxInteractionHandler.IsNumberCommited) {
                return null;
            }

            return numberBoxInteractionHandler.Number;
        }
    }

    /// <summary>
    /// 长度与角度交互器;
    /// </summary>
    public abstract class NumberBoxInteractionHandlerContainer:CanvasElement,IDisposable {
        public NumberBoxInteractionHandlerContainer(IEnumerable<NumberBoxInteractionHandler> numberBoxInteractionHandlers) {
            this.NumberBoxInteractionHandlers = numberBoxInteractionHandlers ??
                throw new ArgumentNullException(nameof(numberBoxInteractionHandlers));

            Initialize();
        }
        
        private void Initialize() {
            foreach (var handler in NumberBoxInteractionHandlers) {
                handler.VisualChanged += Handler_VisualChanged;
            }
        }
        
        private void Uninitialize() {
            foreach (var handler in NumberBoxInteractionHandlers) {
                handler.VisualChanged -= Handler_VisualChanged;
            }
        }

        private void Handler_VisualChanged(object sender, EventArgs e) {
            RaiseVisualChanged();
        }

        public IEnumerable<NumberBoxInteractionHandler> NumberBoxInteractionHandlers { get; }

        /// <summary>
        /// 呈递交互;
        /// </summary>
        public abstract void Commit();

        public void Dispose() {
            Uninitialize();
            OnDispose();
        }

        protected virtual void OnDispose() { }


        /// <summary>
        /// 是否正在输入,将由使用者指示;
        /// </summary>
        public bool IsInputing { get; set; }

        public override void Draw(ICanvas canvas, ICanvasScreenConvertable canvasProxy) {
            foreach (var interactionHandler in NumberBoxInteractionHandlers) {
                interactionHandler.Draw(canvas, canvasProxy);
            }
            base.Draw(canvas, canvasProxy);
        }
    }
}
