using Tida.Canvas.Infrastructure.Contracts;
using Tida.Canvas.Infrastructure.Utils;
using Tida.Canvas.Contracts;
using Tida.Canvas.Events;
using Tida.Canvas.Input;
using Tida.Canvas.Media;
using Tida.Geometry.Primitives;
using System;
using static Tida.Canvas.Infrastructure.Constants;

namespace Tida.Canvas.Infrastructure.DrawObjects {
    /// <summary>
    /// 绘制对象-线段基类;
    /// </summary>
    public abstract class LineBase : MousePositionTrackableDrawObject {
        public LineBase(Vector2D start, Vector2D end) : this(new Line2D(start, end)) {
            
        }

        public LineBase(Line2D line2D) {
            Pen = GetDefaultPen();
            SelectionPen = GetDefaultSelectionPen();
            Line2D = line2D ?? throw new ArgumentNullException(nameof(line2D));
        }
        
        private Line2D _line2D;

        /// <summary>
        /// 线段几何;该值不能为空;
        /// </summary>
        public Line2D Line2D {
            get => _line2D;
            set {
                if (value == null) {
                    throw new ArgumentNullException(nameof(value));
                }

                SetProperty(SetLine2DCore, () => _line2D, value);
            }
        }

        /// <summary>
        /// Line2D发生了变化事件;
        /// </summary>
        public event EventHandler<ValueChangedEventArgs<Line2D>> Line2DChanged;

        
        /// <summary>
        /// 设定线段数据核心;
        /// </summary>
        /// <param name="line2D"></param>
        private void SetLine2DCore(Line2D line2D) {
            if (line2D == null) {
                throw new ArgumentNullException(nameof(line2D));
            }

            if (_line2D == line2D) {
                return;
            }

            var oldLine2D = _line2D;
            _line2D = line2D;
            Line2DChanged?.Invoke(this, new ValueChangedEventArgs<Line2D>(_line2D, oldLine2D));
            RaiseVisualChanged();
        }

        /// <summary>
        /// 当<see cref="Line2D"/>发生变化时是否触发事务;
        /// 默认为真;
        /// </summary>
        protected virtual bool RaiseLine2DChangedTransaction { get; } = true;

        public override Rectangle2D2 GetBoundingRect() {
            if (Line2D == null) {
                return null;
            }

            //获取起点和终点的Y坐标的中点位置;
            var middleY = (Line2D.Start.Y + Line2D.End.Y) / 2;

            return new Rectangle2D2(
                new Line2D(new Vector2D(Line2D.Start.X, middleY), new Vector2D(Line2D.End.X, middleY)),
                Math.Abs(Line2D.Start.Y - Line2D.End.Y)
            );
        }

        public override bool ObjectInRectangle(Rectangle2D2 rect, ICanvasScreenConvertable canvasProxy, bool anyPoint) {
            return LineHitUtils.LineInRectangle( Line2D , rect , anyPoint);
        }
        
        public override bool PointInObject(Vector2D point, ICanvasScreenConvertable canvasProxy) {
            return LineHitUtils.PointInLine(Line2D, point, canvasProxy);
        }
        
        public override void Draw(ICanvas canvas, ICanvasScreenConvertable canvasProxy) {
            if (Line2D == null) {
                return;
            }

            //绘制自身;
            canvas.DrawLine(Pen, Line2D);
            DrawSelectedState(canvas, canvasProxy);
            DrawPreviewState(canvas, canvasProxy);
        }

        private Pen _pen;
        /// <summary>
        /// 绘制线段的笔;
        /// </summary>
        public Pen Pen {
            get => _pen;
            set {
                _pen = value;
                RaiseVisualChanged();
            }
        }

        private Pen _selectionPen;
        /// <summary>
        /// 表示被选中的状态的笔;
        /// </summary>
        public Pen SelectionPen {
            get { return _selectionPen; }
            set {
                _selectionPen = value;
                if (IsSelected) {
                    RaiseVisualChanged();
                }
            }
        }

        /// <summary>
        /// 获得默认的选中状态的笔;
        /// </summary>
        /// <returns></returns>
        protected virtual Pen GetDefaultSelectionPen() => HighLightLinePen;

        /// <summary>
        /// 获得默认的笔;
        /// </summary>
        /// <returns></returns>
        protected virtual Pen GetDefaultPen() => LinePen;

        /// <summary>
        /// 绘制被选中的状态,即魂;
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="canvasProxy"></param>
        protected void DrawSelectedState(ICanvas canvas, ICanvasScreenConvertable canvasProxy) {
            if (!IsSelected) {
                return;
            }

            LineDrawExtensions.DrawSelectedLineState(Line2D, canvas, canvasProxy, SelectionPen);
        }
        
        /// <summary>
        /// 绘制更改后的预览状态;
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="canvasProxy"></param>
        protected void DrawPreviewState(ICanvas canvas, ICanvasScreenConvertable canvasProxy) {
            if (MousePositionTracker.LastMouseDownPosition == null || MousePositionTracker.CurrentHoverPosition == null) {
                return;
            }

            //绘制从上次鼠标按下位置到当前鼠标位置的辅助线;
            canvas.DrawLine(LastMouseDownToCurrentMouseLinePen, new Line2D(MousePositionTracker.LastMouseDownPosition, MousePositionTracker.CurrentHoverPosition));
            var line = GetPreviewLine2D(MousePositionTracker.CurrentHoverPosition);
            if(line != null) {
                canvas.DrawLine(HighLightLinePen, line);
                //LineEditExtensions.DrawEditingLine(canvas, canvasProxy, line);

                if (MousePositionTracker.LastMouseDownPosition.IsAlmostEqualTo(Line2D.Start) || MousePositionTracker.LastMouseDownPosition.IsAlmostEqualTo(Line2D.End)) {
                    LineDrawExtensions.DrawEditingLine(canvas, canvasProxy, line);
                }
            }            
        }

        /// <summary>
        /// 预览更改后的线段几何;
        /// </summary>
        protected Line2D GetPreviewLine2D(Vector2D thisMouseDownPosition) {
            if (MousePositionTracker.LastMouseDownPosition == null || thisMouseDownPosition == null) {
                return null;
            }

            //将应用为新线段几何的局部变量;
            Line2D previewine2D = null;

            //若上次鼠标按下的位置为两端之一,则变更对应端的位置;
            if (MousePositionTracker.LastMouseDownPosition.IsAlmostEqualTo(Line2D.Start)) {
                previewine2D = new Line2D(thisMouseDownPosition, Line2D.End);
            }
            else if (MousePositionTracker.LastMouseDownPosition.IsAlmostEqualTo(Line2D.End)) {
                previewine2D = new Line2D(Line2D.Start, thisMouseDownPosition);
            }
            //若上次鼠标按下的位置为中点,则平移;
            else if (MousePositionTracker.LastMouseDownPosition.IsAlmostEqualTo(Line2D.MiddlePoint)) {
                previewine2D = new Line2D(
                    Line2D.Start + (thisMouseDownPosition - MousePositionTracker.LastMouseDownPosition),
                    Line2D.End + (thisMouseDownPosition - MousePositionTracker.LastMouseDownPosition)
                );
            }

            return previewine2D;
        }

        public override bool IsEditing => MousePositionTracker.LastMouseDownPosition != null;
        
        protected override void OnMouseDown(MouseDownEventArgs e) {
            if (e == null) {
                throw new ArgumentNullException(nameof(e));
            }

            var thisPosition = e.Position;
            
            //若上次鼠标按下的位置不为空,则此次根据鼠标的位置应用更改;
            if (MousePositionTracker.LastMouseDownPosition != null) {
                var newLine2DToApply = GetPreviewLine2D(thisPosition);

                if (newLine2DToApply != null) {
                    MousePositionTracker.Reset(true);
                    Line2D = newLine2DToApply;
                }

                e.Handled = true;
            }
            //否则记录上次的位置;
            else {
                //只有两端和中点才可被认定为可编辑状态;
                if (thisPosition.IsAlmostEqualTo(Line2D.Start) ||
                   thisPosition.IsAlmostEqualTo(Line2D.End) ||
                   thisPosition.IsAlmostEqualTo(Line2D.MiddlePoint)
                ) {
                    MousePositionTracker.SetBothMousePositions(thisPosition, true);
                    RaiseVisualChanged();

                    e.Handled = true;
                }
            }
            
        }

        public void ApplyMouseDownPosition(Vector2D thisMouseDownPosition) {

        }

    }
}
