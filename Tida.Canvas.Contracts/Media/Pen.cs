using Tida.Geometry.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Media {
    /// <summary>
    /// 笔;
    /// </summary>
    public class Pen:Freezable {
        public Pen() {

        }

        private Brush _brush;
        /// <summary>
        /// 画刷;
        /// </summary>
        public Brush Brush {
            get => _brush;
            set => SetFreezableProperty(ref _brush, value);
        }

        private double _thickness;
        /// <summary>
        /// 笔宽度;
        /// </summary>
        public double Thickness {
            get => _thickness;
            set => SetFreezableProperty(ref _thickness, value);
        }

        private DashStyle _dashStyle;
        /// <summary>
        /// 笔的线条样式;
        /// </summary>
        public DashStyle DashStyle {
            get => _dashStyle;
            set => SetFreezableProperty(ref _dashStyle, value);
        }

        /// <summary>
        /// 构建一个新的被冻结的笔;
        /// </summary>
        /// <returns></returns>
        public static Pen CreateFrozenPen(Brush brush,double thickness) {
            return CreateFrozenPen(brush, thickness, null);
        }

        /// <summary>
        /// 构建一个新的被冻结的笔;
        /// </summary>
        /// <param name="brush"></param>
        /// <param name="thickness"></param>
        /// <param name="dashStyle"></param>
        /// <returns></returns>
        public static Pen CreateFrozenPen(Brush brush, double thickness,DashStyle dashStyle) {
            var pen = new Pen {
                Brush = brush,
                Thickness = thickness,
                DashStyle = dashStyle
            };
            pen.Freeze();
            return pen;
        }
    }

    
}
