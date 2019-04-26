using Tida.Canvas.Infrastructure.Contracts;
using Tida.Canvas.Contracts;
using Tida.Canvas.Input;
using Tida.Geometry.External;
using Tida.Geometry.Primitives;
using System;
using static Tida.Canvas.Infrastructure.Constants;
using Tida.Canvas.Infrastructure.DynamicInput.Events;

namespace Tida.Canvas.Infrastructure.DynamicInput {
    /// <summary>
    /// 为<see cref="IHaveMousePositionTracker"/>以及<see cref="IInputElement"/>
    /// 而封装的<see cref="NumberBoxInteractionHandlerContainer"/>,
    /// 只可设定一个长度和宽度;
    /// </summary>
    /// <typeparam name="THaveMousePositionTracker"></typeparam>
    public class LengthNumContainerForMouseTrackable<THaveMousePositionTracker> :
        NumberBoxInteractionHandlerContainer
        where THaveMousePositionTracker : class, IHaveMousePositionTracker, IInputElement {

        public static LengthNumContainerForMouseTrackable<THaveMousePositionTracker>
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

            return new LengthNumContainerForMouseTrackable<THaveMousePositionTracker>(
                haveMousePositionTracker,
                lengthInteractionHandler,
                canvasProxy
            );
        }

        private LengthNumContainerForMouseTrackable(
            THaveMousePositionTracker haveMousePositionTracker,
            NumberBoxInteractionHandler lengthInteractionHandler,
            ICanvasScreenConvertable canvasProxy
        ) :
            base(
                new NumberBoxInteractionHandler[] {
                    lengthInteractionHandler
                }
            ) {

            HaveMousePositionTracker = haveMousePositionTracker ?? throw new ArgumentNullException(nameof(haveMousePositionTracker));
            CanvasProxy = canvasProxy ?? throw new ArgumentNullException(nameof(canvasProxy));

            _lengthInteractionHandler = lengthInteractionHandler ?? throw new ArgumentNullException(nameof(lengthInteractionHandler));

            Initialize();
        }


        /// <summary>
        /// 指示是否内部正在产生变化,
        /// 防止在各项事件响应中,发生无限循环;
        /// </summary>
        private bool _internalChanging;

        private readonly NumberBoxInteractionHandler _lengthInteractionHandler;

        public THaveMousePositionTracker HaveMousePositionTracker { get; }
        public ICanvasScreenConvertable CanvasProxy { get; }

        private void Initialize() {
            InitializeHaveMouseTracker();
            InitializeNumberBoxes();
        }

        private void UnInitialize() {
            UnInitializeHaveMouseTracker();
            UnInitializeNumberBoxes();
        }

        /// <summary>
        /// 初始化文本框;
        /// </summary>
        private void InitializeNumberBoxes() {
            _lengthInteractionHandler.SavedBits = 4;

            _lengthInteractionHandler.NumberCommited += LengthInteractionHandler_NumberCommited;

            _lengthInteractionHandler.Reset();
        }


        private void UnInitializeNumberBoxes() {
            _lengthInteractionHandler.NumberCommited -= LengthInteractionHandler_NumberCommited;

            _lengthInteractionHandler.Reset();
        }


        private void LengthInteractionHandler_NumberCommited(object sender, NumberCommitedEventArgs e) {
            if (!_lengthInteractionHandler.IsNumberCommited) {
                return;
            }

            UpdateCurrentHoverPosition();

            UpdateLineInfoForLength();
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


        /// <summary>
        /// 根据当前长度和角度的输入情况,得到计算后的"悬停"位置;
        /// </summary>
        /// <param name="position">返回是否已经处理,以指示编辑工具是否继续处理</param>
        /// <param name="currentHoverPosition">当前的悬停位置</param>
        private Vector2D GetCalculatedHoverPosition(Vector2D currentHoverPosition) {
            var commitedLength = _lengthInteractionHandler.GetCommitedNumber();

            //若长度未确定,则不能计算位置;
            if (commitedLength == null) {
                return null;
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
            
            var length = commitedLength.Value;

            var destination = lastDownPosition + distanceVector.Normalize() * commitedLength.Value;
            return destination;
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

            RaiseVisualChanged();
        }

        private void HaveMouseTracker_LastDownPositionChanged(object sender, EventArgs e) {
            _lengthInteractionHandler.Reset();
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

        public override void Commit() {
            if (HaveMousePositionTracker.MousePositionTracker.LastMouseDownPosition == null || HaveMousePositionTracker.MousePositionTracker.CurrentHoverPosition == null) {
                return;
            }

            if (_lengthInteractionHandler.Number == null) {
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
    }
}
