using System;
using Tida.Canvas.Base.DynamicInput.Events;
using Tida.Canvas.Infrastructure.Contracts;
using Tida.Canvas.Infrastructure.Utils;
using Tida.Canvas.Contracts;
using Tida.Canvas.Input;
using Tida.Geometry.External;
using Tida.Geometry.Primitives;
using static Tida.Canvas.Infrastructure.Constants;

namespace Tida.Canvas.Base.DynamicInput {

    /// <summary>
    /// 为<see cref="IHaveMousePositionTracker"/>以及<see cref="IInputElement"/>
    /// 而封装的<see cref="NumberBoxInteractionHandlerContainer"/>,
    /// 可设定一个长度和宽度;
    /// </summary>
    /// <typeparam name="THaveMousePositionTracker"></typeparam>
    public sealed partial class LengthAndAngleNumContainerForMouseTrackable<THaveMousePositionTracker> : 
        NumberBoxInteractionHandlerContainer
        where THaveMousePositionTracker : class, IHaveMousePositionTracker, IInputElement {

        public static LengthAndAngleNumContainerForMouseTrackable<THaveMousePositionTracker>
            CreateFromHaveMousePositionTracker(
                THaveMousePositionTracker haveMousePositionTracker,
                ICanvasScreenConvertable canvasProxy
            ) {


            if (haveMousePositionTracker == null) {
                throw new ArgumentNullException(nameof(haveMousePositionTracker));
            }


            if (canvasProxy == null) {
                throw new ArgumentNullException(nameof(canvasProxy));
            }


            var lengthInteractionHandler = new NumberBoxInteractionHandler();
            var angleInteractionHandler = new NumberBoxInteractionHandler();

            return new LengthAndAngleNumContainerForMouseTrackable<THaveMousePositionTracker>(
                haveMousePositionTracker,
                lengthInteractionHandler,
                angleInteractionHandler,
                canvasProxy
            );
        }

        private LengthAndAngleNumContainerForMouseTrackable(
            THaveMousePositionTracker haveMousePositionTracker,
            NumberBoxInteractionHandler lengthInteractionHandler,
            NumberBoxInteractionHandler angleInteractionHandler,
            ICanvasScreenConvertable canvasProxy
        ) :
            base(
                new NumberBoxInteractionHandler[] {
                    lengthInteractionHandler,
                    angleInteractionHandler
                }
            ) {

            HaveMousePositionTracker = haveMousePositionTracker ?? throw new ArgumentNullException(nameof(haveMousePositionTracker));
            CanvasProxy = canvasProxy ?? throw new ArgumentNullException(nameof(canvasProxy));

            _lengthInteractionHandler = lengthInteractionHandler ?? throw new ArgumentNullException(nameof(lengthInteractionHandler));
            _angleInteractionHandler = angleInteractionHandler ?? throw new ArgumentNullException(nameof(angleInteractionHandler));
            
            Initialize();
        }

        /// <summary>
        /// 指示是否内部正在产生变化,
        /// 防止在各项事件响应中,发生无限循环;
        /// </summary>
        private bool _internalChanging;

        private readonly NumberBoxInteractionHandler _lengthInteractionHandler;
        private readonly NumberBoxInteractionHandler _angleInteractionHandler;
        /// <summary>
        /// 角度是否为顺时针;
        /// </summary>
        private bool _isClockWise = false;
        public THaveMousePositionTracker HaveMousePositionTracker { get; }

        public ICanvasScreenConvertable CanvasProxy { get; }

        private void HaveMousePositionTracker_CanvasPreviewMouseMove(object sender, MouseMoveEventArgs e) {
            if (IsInputing) {
                e.Handled = true;
            }

            //根据当前上下文的情况,变更鼠标的当前位置;
            var destination = GetCalculatedHoverPosition(e.Position);
            if (destination == null) {
                return;
            }

            e.Position.X = destination.X;
            e.Position.Y = destination.Y;
        }

        private void HaveMousePositionTracker_CanvasPreviewMouseDown(object sender, MouseDownEventArgs e) {
            //若非内部激发,则不处理;
            if (!_internalChanging) {
                return;
            }

            var destination = GetCalculatedHoverPosition(e.Position);
            if (destination == null) {
                return;
            }
            else {
                e.Position.X = destination.X;
                e.Position.Y = destination.Y;
            }
        }

        /// <summary>
        /// 根据当前长度和角度的输入情况,得到计算后的"悬停"位置;
        /// </summary>
        /// <param name="position">返回是否已经处理,以指示编辑工具是否继续处理</param>
        /// <param name="currentHoverPosition">当前的悬停位置</param>
        private Vector2D GetCalculatedHoverPosition(Vector2D currentHoverPosition) {
            var commitedLength = _lengthInteractionHandler.GetCommitedNumber();
            var commitedAngle = _angleInteractionHandler.GetCommitedNumber();

            //若长度和角度均未确定,则不能计算位置;
            if (commitedLength == null && commitedAngle == null) {
                return null;
            }


            //若角方向为顺时针方向,则反转角度值;
            if (commitedAngle != null) {
                commitedAngle = _isClockWise ? -commitedAngle.Value : commitedAngle.Value;
                commitedAngle = Extension.DegToRad(commitedAngle.Value);
            }
            
            if (HaveMousePositionTracker.MousePositionTracker.LastMouseDownPosition == null) {
                return null;
            }

            var lastDownPosition = HaveMousePositionTracker.MousePositionTracker.LastMouseDownPosition;
            var distanceVector = currentHoverPosition - lastDownPosition;
            var distance = distanceVector.Modulus();
            if (distance < Extension.SMALL_NUMBER) {
                return null;
            }

            //若长度未限定,则根据角度设定悬停点;
            if (commitedLength == null) {
                var x = Math.Cos(commitedAngle.Value) * distance;
                var y = Math.Sin(commitedAngle.Value) * distance;

                return new Vector2D(x + lastDownPosition.X, y + lastDownPosition.Y);
            }
            //若角度未限定,则根据长度设定悬停点;
            else if (commitedAngle == null) {
                var destination = lastDownPosition + distanceVector.Normalize() * commitedLength.Value;
                return destination;
            }
            ///若均已经限定,则<param name="currentHoverPosition"/>将被忽略;
            ///悬停点将全由本类字段决定;
            else {
                var length = commitedLength.Value;
                var angle = commitedAngle.Value;

                var x = Math.Cos(commitedAngle.Value) * length;
                var y = Math.Sin(commitedAngle.Value) * length;

                return new Vector2D(x + lastDownPosition.X, y + lastDownPosition.Y);
            }


        }
        
        public override void Commit() {
            if (HaveMousePositionTracker.MousePositionTracker.LastMouseDownPosition == null || HaveMousePositionTracker.MousePositionTracker.CurrentHoverPosition == null) {
                return;
            }

            if (_lengthInteractionHandler.Number == null || _angleInteractionHandler.Number == null) {
                return;
            }
            var position = GetCalculatedHoverPosition(HaveMousePositionTracker.MousePositionTracker.CurrentHoverPosition);
            if (position == null) {
                return;
            }

            _internalChanging = true;
            HaveMousePositionTracker.RaisePreviewMouseDown(new MouseDownEventArgs(MouseButton.Left, position));
            _internalChanging = false;
        }

        private void Initialize() {
            InitializeHaveMouseTracker();
            InitializeNumberBoxes();
        }
        
        private void UnInitialize() {
            UnInitializeHaveMouseTracker();
            UnInitializeNumberBoxes();
        }

        protected override void OnDispose() {
            base.OnDispose();

            UnInitialize();
            _lengthInteractionHandler.Dispose();
            _angleInteractionHandler.Dispose();
        }
    }

    public sealed partial class LengthAndAngleNumContainerForMouseTrackable<THaveMousePositionTracker> {

        /// <summary>
        /// 初始化文本框;
        /// </summary>
        private void InitializeNumberBoxes() {
            _lengthInteractionHandler.SavedBits = 4;
            _angleInteractionHandler.SavedBits = 1;

            _lengthInteractionHandler.NumberCommited += LengthInteractionHandler_NumberCommited;
            _angleInteractionHandler.NumberCommited += AngleInteractionHandler_NumberCommited;

            _lengthInteractionHandler.Reset();
            _angleInteractionHandler.Reset();
        }
        
        private void UnInitializeNumberBoxes() {
            _lengthInteractionHandler.NumberCommited -= LengthInteractionHandler_NumberCommited;
            _angleInteractionHandler.NumberCommited -= AngleInteractionHandler_NumberCommited;

            _lengthInteractionHandler.Reset();
            _angleInteractionHandler.Reset();
        }

        /// <summary>
        /// 角度框确认时,更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AngleInteractionHandler_NumberCommited(object sender, NumberCommitedEventArgs e) {
            if (!_angleInteractionHandler.IsNumberCommited) {
                return;
            }

            if (_angleInteractionHandler.Number == null) {
                return;
            }

            var lastMoseDownPosition = HaveMousePositionTracker.MousePositionTracker.LastMouseDownPosition;
            var currentHoverPosition = HaveMousePositionTracker.MousePositionTracker.CurrentHoverPosition;
            //根据当前鼠标位置与上次鼠标按下的纵坐标大小关系，判定角度是否为顺时针;
            _isClockWise = currentHoverPosition.Y < lastMoseDownPosition.Y;

            UpdateCurrentHoverPosition();

            UpdateLineInfoForLength();

            UpdateLineInfoForAngle();
        }

        private void LengthInteractionHandler_NumberCommited(object sender, NumberCommitedEventArgs e) {
            if (!_lengthInteractionHandler.IsNumberCommited) {
                return;
            }

            UpdateCurrentHoverPosition();

            UpdateLineInfoForLength();

            UpdateLineInfoForAngle();
        }

        /// <summary>
        /// 更新<typeparamref name="THaveMousePositionTracker"/>的 鼠标当前位置;
        /// </summary>
        private void UpdateCurrentHoverPosition() {
            var destination = GetCalculatedHoverPosition(
                HaveMousePositionTracker.MousePositionTracker.CurrentHoverPosition
            );

            if (destination == null) {
                return;
            }
            
            HaveMousePositionTracker.RaisePreviewMouseMove(new MouseMoveEventArgs(destination));
        }

        private void InitializeHaveMouseTracker() {
            //编辑工具->动态输入的显示;

            HaveMousePositionTracker.MousePositionTracker.CurrentHoverPositionChanged += HaveMouseTracker_CurrentHoverPositionChanged;
            HaveMousePositionTracker.MousePositionTracker.LastMouseDownPositionChanged += HaveMouseTracker_LastDownPositionChanged;

            //动态输入->编辑工具的交互控制;
            HaveMousePositionTracker.CanvasPreviewMouseMove += HaveMouseTracker_PreviewMouseMove;
            HaveMousePositionTracker.CanvasPreviewMouseDown += HaveMouseTracker_PreviewMouseDown;
        }

        private void UnInitializeHaveMouseTracker() {
            //编辑工具->动态输入的显示;
            HaveMousePositionTracker.MousePositionTracker.CurrentHoverPositionChanged -= HaveMouseTracker_CurrentHoverPositionChanged;
            HaveMousePositionTracker.MousePositionTracker.LastMouseDownPositionChanged -= HaveMouseTracker_LastDownPositionChanged;

            //动态输入->编辑工具的交互控制;
            HaveMousePositionTracker.CanvasPreviewMouseMove -= HaveMouseTracker_PreviewMouseMove;
            HaveMousePositionTracker.CanvasPreviewMouseDown -= HaveMouseTracker_PreviewMouseDown;
        }

        private void HaveMouseTracker_CurrentHoverPositionChanged(object sender, EventArgs e) {
            if (HaveMousePositionTracker.MousePositionTracker.CurrentHoverPosition == null) {
                return;
            }

            UpdateLineInfoForLength();

            UpdateLineInfoForAngle();
            
            RaiseVisualChanged();
        }

        private void HaveMouseTracker_LastDownPositionChanged(object sender, EventArgs e) {
            _lengthInteractionHandler.Reset();
            _angleInteractionHandler.Reset();
            RaiseVisualChanged();
        }

        private void HaveMouseTracker_PreviewMouseDown(object sender, MouseDownEventArgs e) {
            if (!_internalChanging) {
                return;
            }

            var calcPosition = GetCalculatedHoverPosition(e.Position);
            if (calcPosition == null) {
                return;
            }
            else {
                e.Position.X = calcPosition.X;
                e.Position.Y = calcPosition.Y;
            }
        }

        private void HaveMouseTracker_PreviewMouseMove(object sender, MouseMoveEventArgs e) {
            ////若正在输入状态,则指示不能继续动作;
            if (IsInputing) {
                e.Handled = true;
            }

            //根据当前上下文的情况,变更鼠标的当前位置;
            var destination = GetCalculatedHoverPosition(e.Position);
            if (destination == null) {
                return;
            }

            e.Position.X = destination.X;
            e.Position.Y = destination.Y;
        }

        /// <summary>
        /// 更新角度输入框;
        /// </summary>
        /// <param name="currentPosition"></param>
        private void UpdateLineInfoForAngle() {
            var currentPosition = HaveMousePositionTracker.MousePositionTracker.CurrentHoverPosition;
            var lastMouseDownPosition = HaveMousePositionTracker.MousePositionTracker.LastMouseDownPosition;

            if (lastMouseDownPosition == null || currentPosition == null) {
                return;
            }

            var editingLine = new Line2D(lastMouseDownPosition, currentPosition);
            var direction = editingLine.Direction;

            var start = editingLine.Start;
            var end = editingLine.End;

            //得到弧的两条边的向量(以<see cref="editingLine.Start"/>为起点),这两条边的长度相等;
            var lineVector = end - start;
            var xAxisLineVector = Vector2D.BasisX * lineVector.Modulus();

            //确定角度;
            var radAngle = lineVector.AngleFrom(Vector2D.BasisX);
            //确定需要绘制角度的位置;
            var stringPosition = (lineVector + xAxisLineVector).Normalize() * lineVector.Modulus() + start;

            _angleInteractionHandler.ScreenPosition = CanvasProxy.ToScreen(stringPosition);

            ///假如<see cref="_fixedAngle"/>存在,其值事实理论上等于<see cref="radAngle"/>,
            ///但浮点运算存在误差,所以此时需指定为<see cref="_fixedAngle"/>
            if (!_angleInteractionHandler.IsNumberCommited) {
                _angleInteractionHandler.Number = Extension.RadToDeg(LengthAndAngleDynamicInputerUtil.GetFixedAnglePositiveToXAxizs(radAngle));
            }

        }

        public override void Draw(ICanvas canvas, ICanvasScreenConvertable canvasProxy) {
            if (HaveMousePositionTracker.MousePositionTracker.CurrentHoverPosition != null && HaveMousePositionTracker.MousePositionTracker.LastMouseDownPosition != null) {
                var editingLine = new Line2D(HaveMousePositionTracker.MousePositionTracker.LastMouseDownPosition, HaveMousePositionTracker.MousePositionTracker.CurrentHoverPosition);
                LineDrawExtensions.DrawEditingLine(canvas, canvasProxy, editingLine);
            }
            base.Draw(canvas, canvasProxy);
        }

        /// <summary>
        /// 更新长度输入框;
        /// </summary>
        private void UpdateLineInfoForLength() {
            var currentPosition = HaveMousePositionTracker.MousePositionTracker.CurrentHoverPosition;
            var lastMouseDownPosition = HaveMousePositionTracker.MousePositionTracker.LastMouseDownPosition;

            if (lastMouseDownPosition == null || currentPosition == null) {
                return;
            }

            var editingLine = new Line2D(lastMouseDownPosition, currentPosition);

            var direction = editingLine.Direction;
            if (direction == null) {
                return;
            }

            //通过法向量确定其它提示信息的位置;
            var verticalDir = new Vector2D(-direction.Y, direction.X);
            //将视图距离转化为工程数学距离;
            var unitDistance = CanvasProxy.ToUnit(
                ScreenDistanceLineEditingWithLine
            );

            var paraLinesDistance = verticalDir * unitDistance;

            var editingTextPosition = editingLine.MiddlePoint + paraLinesDistance / 1.2;

            _lengthInteractionHandler.ScreenPosition = CanvasProxy.ToScreen(editingTextPosition);

            if (!_lengthInteractionHandler.IsNumberCommited) {
                _lengthInteractionHandler.Number = editingLine.Length;
            }

        }
    }
}
