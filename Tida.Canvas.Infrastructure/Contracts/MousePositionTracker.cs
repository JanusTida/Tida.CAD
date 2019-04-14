using Tida.Canvas.Events;
using Tida.Geometry.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Infrastructure.Contracts {

    /// <summary>
    /// 记录部分鼠标状态的管理单元;
    /// </summary>
    public sealed class MousePositionTracker {
        public MousePositionTracker(object owner) {

            Owner = owner ?? throw new ArgumentNullException(nameof(owner));

        }

        /// <summary>
        /// 本实例所属的持有者;
        /// </summary>
        public object Owner { get; }

        private Vector2D _lastMouseDownPosition;
        /// <summary>
        /// 上一次鼠标按下的位置;
        /// </summary>
        public Vector2D LastMouseDownPosition {
            get => _lastMouseDownPosition;
            set {
                if (_lastMouseDownPosition == value) {
                    return;
                }

                var oldLastMouseDownPosition = _lastMouseDownPosition;
                _lastMouseDownPosition = value;

                var args = new ValueChangedEventArgs<Vector2D>(
                    _lastMouseDownPosition,
                    oldLastMouseDownPosition
                );

                PreviewLastMouseDownPositionChanged?.Invoke(this, args);
                //若通知被挂起,则不触发事件;
                if (NotificationSuspended) {
                    return;
                }

                LastMouseDownPositionChanged?.Invoke(this,args);
            }
        }

        private Vector2D _currentHoverPosition;
        /// <summary>
        /// 当前鼠标的位置;
        /// </summary>
        public Vector2D CurrentHoverPosition {
            get => _currentHoverPosition;
            set {
                if (_currentHoverPosition == value) {
                    return;
                }
                
                var oldCurrentHoverPosition = _currentHoverPosition;
                _currentHoverPosition = value;

                var args = new ValueChangedEventArgs<Vector2D>(
                    _currentHoverPosition,
                    oldCurrentHoverPosition
                );

                PreviewCurrentHoverPositionChanged?.Invoke(this, args);

                //若通知被挂起,则不触发事件;
                if (NotificationSuspended) {
                    return;
                }

                CurrentHoverPositionChanged?.Invoke(
                    this,
                    args
                );
            }
        }

        /// <summary>
        /// 当前鼠标位置发生变化(预览);
        /// </summary>
        public event EventHandler<ValueChangedEventArgs<Vector2D>> PreviewCurrentHoverPositionChanged;

        /// <summary>
        /// 上次鼠标位置发生变化(预览);
        /// </summary>
        public event EventHandler<ValueChangedEventArgs<Vector2D>> PreviewLastMouseDownPositionChanged;

        /// <summary>
        /// 当前鼠标位置发生变化;
        /// </summary>
        public event EventHandler<ValueChangedEventArgs<Vector2D>> CurrentHoverPositionChanged;

        /// <summary>
        /// 上次鼠标位置发生变化;
        /// </summary>
        public event EventHandler<ValueChangedEventArgs<Vector2D>> LastMouseDownPositionChanged;

        /// <summary>
        /// 状态变更的通知是否挂起;
        /// 当值为True时,将不会触发鼠标位置变更事件,但预览事件不受此影响;
        /// 外部可根据重要程度订阅不同级别的事件,以在可用性和性能间作一个平衡;
        /// </summary>
        public bool NotificationSuspended { get; set; }
    }

    /// <summary>
    /// <see cref="MousePositionTracker"/>拓展;
    /// </summary>
    public static class MousePositionTrackerExtention {
        /// <summary>
        /// 复位<see cref="MousePositionTracker"/>的状态;
        /// 将<see cref="MousePositionTracker.CurrentHoverPosition"/>和<see cref="MousePositionTracker.LastMouseDownPosition"/>置为空;
        /// </summary>
        /// <param name="notificationSuspended">是否挂起通知;</param>
        public static void Reset(this MousePositionTracker mousePositionTracker, bool notificationSuspended = false) {
            mousePositionTracker.SetBothMousePositions(null, notificationSuspended);
        }

        /// <summary>
        /// 将<paramref name="mousePositionTracker"/>的
        /// <see cref="MousePositionTracker.CurrentHoverPosition"/>和
        /// <see cref="MousePositionTracker.LastMouseDownPosition"/>均设为
        /// <paramref name="mousePosition"/>
        /// </summary>
        /// <param name="mousePositionTracker"></param>
        /// <param name="mousePosition"></param>
        public static void SetBothMousePositions(this MousePositionTracker mousePositionTracker, Vector2D mousePosition, bool notificationSuspended = false) {
            mousePositionTracker.SetMousePositions(mousePosition, mousePosition, notificationSuspended);
        }

        /// <summary>
        /// 将<paramref name="mousePositionTracker"/>的<see cref="MousePositionTracker.LastMouseDownPosition"/>和<see cref="MousePositionTracker.CurrentHoverPosition"/>
        /// 分别设置为<paramref name="lastMouseDownPosition"/>和<paramref name="currentHoverPosition"/>
        /// </summary>
        /// <param name="mousePositionTracker">管理单元</param>
        /// <param name="lastMouseDownPosition"></param>
        /// <param name="currentHoverPosition"></param>
        /// <param name="notificationSuspended">是否挂起通知;</param>
        public static void SetMousePositions(this MousePositionTracker mousePositionTracker,Vector2D lastMouseDownPosition,Vector2D currentHoverPosition, bool notificationSuspended = false) {
            if (mousePositionTracker == null) {
                throw new ArgumentNullException(nameof(mousePositionTracker));
            }

            var oldNotificationSuspended = mousePositionTracker.NotificationSuspended;

            mousePositionTracker.NotificationSuspended = notificationSuspended;

            mousePositionTracker.LastMouseDownPosition = lastMouseDownPosition;
            mousePositionTracker.CurrentHoverPosition = currentHoverPosition;

            mousePositionTracker.NotificationSuspended = oldNotificationSuspended;
        }
        
    }
}
