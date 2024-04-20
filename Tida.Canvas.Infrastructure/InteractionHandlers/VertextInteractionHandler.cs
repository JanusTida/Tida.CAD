
using System;
using Tida.Geometry.Primitives;
using Tida.Canvas.Infrastructure.InteractionHandlers;
using Tida.Canvas.Infrastructure.Contracts;
using System.Linq;
using Tida.Canvas.Events;
using Tida.Canvas.Contracts;

namespace Tida.Canvas.Infrastructure.InteractionHandlers {
    /// <summary>
    /// 画布交互处理器——正交模式;
    /// </summary>
    public class VertextInteractionHandler : CanvasInteractionHandler {
        private static bool _isEnabled;
        /// <summary>
        /// 正交模式是否可用;
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

        /// <summary>
        /// 正交模式可用发生变化;
        /// </summary>
        public static event EventHandler<ValueChangedEventArgs<bool>> IsEnabledChanged;
    
        public override void  HandlePosition(ICanvasControl canvasContext, Vector2D oriPosition) {
            if (canvasContext == null) {
                throw new ArgumentNullException(nameof(canvasContext));
            }
            
            if (oriPosition == null) {
                throw new ArgumentNullException(nameof(oriPosition));
            }

            //若正交模式不可用,则不处理;
            if(!IsEnabled){
                return;
            }

            if(CanvasControl == null) {
                return;
            }

            if(CanvasControl.CurrentEditTool != null) {
                HandlePositionByEditTool(CanvasControl.CurrentEditTool, oriPosition);
            }
            else {
                var lastMouseDownPosition = GetLastMouseDownPositionFromDrawObjects(CanvasControl);
                if(lastMouseDownPosition == null) {
                    return;
                }

                AlignToPosition(oriPosition, lastMouseDownPosition);
            }
        }

        /// <summary>
        /// 根据编辑工具处理位置;
        /// </summary>
        /// <param name="editTool"></param>
        /// <returns></returns>
        private static void HandlePositionByEditTool(EditTool editTool,Vector2D oriPosition) {

            if (!(editTool is IHaveMousePositionTracker haveMousePositionTracker)) {
                return;
            }

            if (haveMousePositionTracker.MousePositionTracker?.LastMouseDownPosition == null) {
                return;
            }


            AlignToPosition(oriPosition, haveMousePositionTracker.MousePositionTracker.LastMouseDownPosition);
        }

        /// <summary>
        /// 将<paramref name="positionToBeAligned"/>的位置对齐到<paramref name="positionAligning"/>
        /// </summary>
        /// <param name="positionToBeAligned"></param>
        /// <param name="positionAligning"></param>
        private static void AlignToPosition(Vector2D positionToBeAligned,Vector2D positionAligning) {
            var abX = Math.Abs(positionToBeAligned.X - positionAligning.X);
            var abY = Math.Abs(positionToBeAligned.Y - positionAligning.Y);

            if (abX >= abY) {
                positionToBeAligned.Y = positionAligning.Y;
            }
            else {
                positionToBeAligned.X = positionAligning.X;
            }
        }

        /// <summary>
        /// 从可用的绘制对象集合中获取上次鼠标按下的位置;
        /// </summary>
        /// <param name="canvasControl"></param>
        /// <returns></returns>
        private static Vector2D GetLastMouseDownPositionFromDrawObjects(ICanvasContext canvasControl) {
            var lastMouseDownPositions = canvasControl.
                    GetAllVisibleDrawObjects().Where(p => p.IsEditing).
                    Select(p => p as IHaveMousePositionTracker).Where(p => p != null).
                    Select(p => p.MousePositionTracker.LastMouseDownPosition);

            var lastMouseDownPositionsDistinct = lastMouseDownPositions.Distinct(Vector2DEqualityComparer.StaticInstance);
            if (lastMouseDownPositionsDistinct.Count() != 1) {
                return null;
            }


            return lastMouseDownPositions.First();
        }
    }

    
}
