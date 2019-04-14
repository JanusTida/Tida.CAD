using Tida.Canvas.Infrastructure.DrawObjects;
using Tida.Canvas.Shell.Contracts.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
