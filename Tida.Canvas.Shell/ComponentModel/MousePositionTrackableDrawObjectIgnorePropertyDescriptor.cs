using Tida.Canvas.Infrastructure.DrawObjects;
using Tida.Canvas.Shell.Contracts.ComponentModel;

namespace Tida.Canvas.Shell.ComponentModel {
    /// <summary>
    /// 忽略<see cref="MousePositionTrackableDrawObject"/>相关内部属性的描述;
    /// </summary>
    [ExportIgnoredPropertyDescriptor(Inheritable = true)]
    class MousePositionTrackableDrawObjectIgnorePropertyDescriptor : IgnoredPropertyDescriptor {
        
        public MousePositionTrackableDrawObjectIgnorePropertyDescriptor() : 
            base(typeof(MousePositionTrackableDrawObject),nameof(MousePositionTrackableDrawObject.MousePositionTracker)) {

        }
    }
}
