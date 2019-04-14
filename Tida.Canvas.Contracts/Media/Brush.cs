using Tida.Geometry.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Media {
    /// <summary>
    /// 为屏蔽平台差异性所自定义的画刷基类,
    /// </summary>
    public abstract class Brush:Freezable {
        private double _opacity = 1;
        /// <summary>
        /// 透明度;
        /// </summary>
        public double Opacity {
            get => _opacity;
            set => SetFreezableProperty(ref _opacity, value);
        }
    }
}
