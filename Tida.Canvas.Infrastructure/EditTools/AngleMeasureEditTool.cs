using Tida.Canvas.Infrastructure.DrawObjects;
using Tida.Canvas.Infrastructure.Utils;
using Tida.Canvas.Contracts;
using Tida.Canvas.Events;
using Tida.Canvas.Input;
using Tida.Geometry.Primitives;
using System;
using static Tida.Canvas.Infrastructure.Constants;

namespace Tida.Canvas.Infrastructure.EditTools {
    /// <summary>
    /// 测量工具-角度;
    /// </summary>
    public class AngleMeasureEditTool : UniqueTypeEditToolGenericBase<MeasureAngle>, IMeasureEditTool {
        /// <summary>
        /// 使用一个绘制对象选择器实例构建一个角度测量编辑工具;
        /// </summary>
        /// <param name="drawObjectSelector">绘制对象选择器,用于在操作多个绘制对象时,需确定一个唯一的绘制对象时使用</param>
        public AngleMeasureEditTool(IDrawObjectSelector drawObjectSelector) {

            _drawObjectSelector = drawObjectSelector ?? throw new ArgumentNullException(nameof(drawObjectSelector));

        }

        private readonly IDrawObjectSelector _drawObjectSelector;

        /// <summary>
        /// 呈递事务时是否将保持的数据绘制对象添加到到指定图层中;
        /// </summary>
        public bool ShouldCommitMeasureData { get; set; } = true;

        protected override void OnCommit() {
            if (ShouldCommitMeasureData) {
                base.OnCommit();
            }
        }

        /// <summary>
        /// 鼠标首次按下位置;
        /// </summary>
        private Vector2D _firstMouseDownPosition;
        /// <summary>
        /// 鼠标第二次按下位置;
        /// </summary>
        private Vector2D _secondMouseDownPosition;

        private Vector2D _currentHoverPosition;

        /// <summary>
        /// 创建了一个新的角;
        /// </summary>
        public event EventHandler AngleCreated;

        /// <summary>
        /// 鼠标首次按下确定;
        /// </summary>
        public event EventHandler<Vector2D> FirstMouseDownPositionConfirmed;
        /// <summary>
        /// 鼠标第二次按下确定;
        /// </summary>
        public event EventHandler<Vector2D> SecondMouseDownPositionConfirmed;
        public event EventHandler<LineBase> FirstAngleLineConfirmed;

        protected override void OnCanvasContextChanged(ValueChangedEventArgs<ICanvasContextEx> args) {
            if (args.NewValue is ICanvasContextEx newCanvasContext) {
                newCanvasContext.ClickSelect += CanvasContext_ClickSelect;
            }

            if (args.OldValue is ICanvasContextEx oldCanvasContext) {
                oldCanvasContext.ClickSelect -= CanvasContext_ClickSelect;
            }
            base.OnCanvasContextChanged(args);
        }

        private void CanvasContext_ClickSelect(object sender,ClickSelectEventArgs e) {
            if(e.HitedDrawObjects.Length == 1) {
                return;
            }
            
            if(e.HitedDrawObjects.Length > 1) {
                var slDrawObject = _drawObjectSelector.SelectOneDrawObject(e.HitedDrawObjects);
                if(slDrawObject != null) {
                    slDrawObject.IsSelected = true;
                }
                e.Cancel = true;
            }
            
        }

        protected override void OnBeginOperation() {
            base.OnBeginOperation();
            if (CanvasContext == null) {
                return;
            }
            
            var drawObjects = CanvasContext.GetAllDrawObjects();

            foreach (var drawObject in drawObjects) {
                drawObject.IsSelected = false;
            }
        }

        protected override void OnMouseDown(MouseDownEventArgs e) {
            
            if (e == null) {
                throw new ArgumentNullException(nameof(e));
            }
           
            if(CanvasContext == null) {
                return;
            }

            var thisMouseDownPosition = e.Position;

            //if (CanvasContext.GetAllVisibleDrawObjects().Any(p => p.PointInObject(thisMouseDownPosition, CanvasContext.CanvasProxy))) {
            //    return;
            //}

            if(_secondMouseDownPosition == null && _firstMouseDownPosition == null) {
                _firstMouseDownPosition = thisMouseDownPosition;
                FirstMouseDownPositionConfirmed?.Invoke(this, _firstMouseDownPosition);
            }
            else if(_secondMouseDownPosition == null) {
                _secondMouseDownPosition = thisMouseDownPosition;
                SecondMouseDownPositionConfirmed?.Invoke(this, _secondMouseDownPosition);
            }
            else if(_firstMouseDownPosition != null){
                var measureAngle = new MeasureAngle(_firstMouseDownPosition, _secondMouseDownPosition, thisMouseDownPosition);
                _firstMouseDownPosition = null;
                _secondMouseDownPosition = null;
                AddDrawObjectToUndoStack(measureAngle);
                AngleCreated?.Invoke(this, EventArgs.Empty);
            }

            e.Handled = true;
            base.OnMouseDown(e);
            RaiseVisualChanged();
        }

        protected override void OnMouseMove(MouseMoveEventArgs e) {

            if (e == null) {
                throw new ArgumentNullException(nameof(e));
            }

            _currentHoverPosition = e.Position;
            RaiseVisualChanged();
            base.OnMouseMove(e);
        }

        public override bool IsEditing => true;

        public override void Draw(ICanvas canvas, ICanvasScreenConvertable canvasProxy) {
            
            base.Draw(canvas, canvasProxy);

            Vector2D lastMouseDownPosition = null;

            if(_firstMouseDownPosition != null && _secondMouseDownPosition != null) {
                canvas.DrawLine(HighLightLinePen, new Line2D(_secondMouseDownPosition, _firstMouseDownPosition));
                lastMouseDownPosition = _secondMouseDownPosition;
            }
            else if(_firstMouseDownPosition != null) {
                lastMouseDownPosition = _firstMouseDownPosition;
            }
            
            if(_firstMouseDownPosition != null) {
                PointDrawExtensions.DrawSelectedPointState(_firstMouseDownPosition, canvas, canvasProxy);
            }

            if(_secondMouseDownPosition != null) {
                PointDrawExtensions.DrawSelectedPointState(_secondMouseDownPosition, canvas, canvasProxy);
            }

            if(_currentHoverPosition != null && lastMouseDownPosition != null) {
                canvas.DrawLine(HighLightLinePen, new Line2D(lastMouseDownPosition, _currentHoverPosition));
            }
        }
    }
}
