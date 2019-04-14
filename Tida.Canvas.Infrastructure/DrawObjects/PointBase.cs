using Tida.Canvas.Contracts;
using Tida.Canvas.Events;
using Tida.Canvas.Input;
using Tida.Canvas.Media;
using Tida.Geometry.External.Util;
using Tida.Geometry.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Tida.Canvas.Infrastructure.Constants;

namespace Tida.Canvas.Infrastructure.DrawObjects {
    /// <summary>
    /// 绘制对象——点基类;
    /// </summary>
    public abstract class PointBase : MousePositionTrackableDrawObject {
        /// <summary>
        /// 构造方法;
        /// </summary>
        /// <param name="position">点的位置,不可为空;</param>
        public PointBase(Vector2D position) {
            this.Position = position;
        }

        private Vector2D _position;
        public Vector2D Position {
            get => _position;
            set {
                if (value == null) {
                    throw new ArgumentNullException(nameof(value));
                }

                SetProperty(SetPositionCore, () => _position, value);
            }
        }

        private void SetPositionCore(Vector2D position) {
            if(position == null) {
                throw new ArgumentNullException(nameof(position));
            }
            
            var args = new ValueChangedEventArgs<Vector2D>(position, _position);
            _position = position;

            PositionChanged?.Invoke(this, args);
            RaiseVisualChanged();
        }

        /// <summary>
        /// 位置发生变化事件;
        /// </summary>
        public event EventHandler<ValueChangedEventArgs<Vector2D>> PositionChanged;

        /// <summary>
        /// 正在通过鼠标改变本实例的位置,即<see cref="Position"/>;
        /// </summary>
        public event EventHandler<ValueChangingEventArgs<Vector2D>> MouseChangingPosition;

        public override Rectangle2D2 GetBoundingRect() {
            var radius = ScreenRadius;
            return new Rectangle2D2(
                new Line2D(
                    new Vector2D(Position.X - radius, Position.Y),
                    new Vector2D(Position.X + radius, Position.Y)
                ),
                radius * 2
            );
        }

        public override bool ObjectInRectangle(Rectangle2D2 rect, ICanvasScreenConvertable canvasProxy, bool anyPoint) {
            if (Position == null) {
                return false;
            }

            return rect.Contains(Position);
        }

        public override bool PointInObject(Vector2D point, ICanvasScreenConvertable canvasProxy) {
            if (canvasProxy == null) {
                return false;
            }
            if (Position == null) {
                return false;
            }

#if DEBUG
            if (point.X == 0) {

            }
#endif
            var screenEllipse = GetScreenEllipse(canvasProxy);
            var screenPoint = canvasProxy.ToScreen(point);

            return screenEllipse.Contains(screenPoint);
        }


        private double _screenRadius = PointEllipseScreenRadius;
        
        /// <summary>
        /// 点在视图上显示的半径;
        /// </summary>
        public double ScreenRadius {
            get => _screenRadius;
            set {
                _screenRadius = value;
                RaiseVisualChanged();
            }
        }

        private Brush _selectedBackground = HighLightEllipseColorBrush;
        /// <summary>
        /// 选定状态的背景;
        /// </summary>
        public Brush SelectedBackground {
            get => _selectedBackground;
            set {
                _selectedBackground = value;
                RaiseVisualChanged();
            }
        }

        private Brush _normalBackground = NormalEllipseColorBrush;
        /// <summary>
        /// 非选择状态下的背景色;
        /// </summary>
        public Brush NormalBackground {
            get => _normalBackground;
            set => SetProperty(val => _normalBackground = val, () => _normalBackground, value,false,true);
        }

        public override void Draw(ICanvas canvas, ICanvasScreenConvertable canvasProxy) {
            if (canvasProxy == null) {
                return;
            }
            if (canvas == null) {
                return;
            }

            //绘制自身;
            var screenEllipse = GetScreenEllipse(canvasProxy);
            canvas.NativeDrawEllipse(
                NormalBackground,
                NormalEllipsePen,
                screenEllipse
            );

            var previewScreenEllipse = GetPreviewScreenEllipse2D(canvasProxy);
            if (previewScreenEllipse != null) {
                canvas.NativeDrawEllipse(NormalBackground, NormalEllipsePen, previewScreenEllipse);
            }

            DrawSelectedStates(canvas, canvasProxy);
        }

        /// <summary>
        /// 绘制选择状态;
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="canvasProxy"></param>
        private void DrawSelectedStates(ICanvas canvas, ICanvasScreenConvertable canvasProxy) {
            if (!IsSelected) {
                return;
            }

            var screenEllipse = GetScreenEllipse(canvasProxy);
            canvas.NativeDrawEllipse(
                SelectedBackground,
                HighLightLinePen,
                screenEllipse.Center,
                screenEllipse.RadiusX,
                screenEllipse.RadiusY
            );

        }

        /// <summary>
        /// 获取以视图坐标为准的,表示预览状态下的位置的圆;
        /// </summary>
        /// <param name="canvasProxy"></param>
        /// <returns></returns>
        private Ellipse2D GetPreviewScreenEllipse2D(ICanvasScreenConvertable canvasProxy) {
            if (MousePositionTracker.CurrentHoverPosition == null) {
                return null;
            }

            var screenEllipse = GetSurroundingScreenEllipse2D(canvasProxy, MousePositionTracker.CurrentHoverPosition);
            return screenEllipse;
        }
        
        /// <summary>
        /// 获得本实例以<see cref="Position"/>为圆心,视图坐标为准的圆;
        /// </summary>
        /// <returns></returns>
        private Ellipse2D GetScreenEllipse(ICanvasScreenConvertable canvasProxy) {
            if (canvasProxy == null) {
                throw new ArgumentNullException(nameof(canvasProxy));
            }

            return GetSurroundingScreenEllipse2D(canvasProxy, Position);
        }


        /// <summary>
        /// 获取以<paramref name="center"/>圆心,<see cref="ScreenRadius"/>为视图半径的圆;
        /// </summary>
        /// <param name="canvasProxy"></param>
        /// <param name="center"></param>
        /// <returns></returns>
        private Ellipse2D GetSurroundingScreenEllipse2D(ICanvasScreenConvertable canvasProxy, Vector2D center) {

            var screenPosition = canvasProxy.ToScreen(center);
            var screenEllipse = new Ellipse2D(screenPosition, ScreenRadius, ScreenRadius);

            return screenEllipse;
        }
        
        protected override void OnMouseDown(MouseDownEventArgs e) {
            var thisPosition = e.Position;
            if (!IsSelected) {
                return;
            }

            ///若上次点击位置为空;
            if (MousePositionTracker.LastMouseDownPosition == null) {
                //检查是否命中圆心;若是,则记录;
                if (thisPosition.IsAlmostEqualTo(Position)) {
                    MousePositionTracker.LastMouseDownPosition = thisPosition;
                    MousePositionTracker.CurrentHoverPosition = thisPosition;
                    e.Handled = true;
                }
            }
            //若不为空,则转移位置;
            else if (thisPosition != null) {
                var args = new ValueChangingEventArgs<Vector2D>(thisPosition, Position);
                MouseChangingPosition?.Invoke(this, args);
                if (!args.Cancel) {
                    Position = thisPosition;
                }
                IsSelected = false;
                e.Handled = true;
            }

        }

        
    }
}
