using System;
using System.Collections.Generic;
using System.Text;
using Tida.Canvas.Contracts;
using Tida.Canvas.Infrastructure.Utils;
using Tida.Canvas.Media;
using Tida.Geometry.Primitives;
using static Tida.Canvas.Infrastructure.Constants;

namespace Tida.Canvas.Infrastructure.DrawObjects {
    /// <summary>
    /// 绘制对象-圆弧;
    /// </summary>
    public class Arc : DrawObject {
        public Arc(Arc2D arc2D)
        {
            Arc2D = arc2D ?? throw new ArgumentNullException(nameof(arc2D));
            Pen = LinePen;
            SelectionPen = HilightLinePen;
        }

        private Arc2D _arc2D;
        /// <summary>
        /// 圆弧几何,该值不能为空;
        /// </summary>
        public Arc2D Arc2D {
            get => _arc2D;
            set {

                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                SetProperty(val => _arc2D = val, () => _arc2D, value);
            }
        }

        public override DrawObject Clone() => new Arc(Arc2D);

        public override Rectangle2D2 GetBoundingRect()
        {
            if(Arc2D.Center == null)
            {
                return null;
            }

            var radius = Arc2D.Radius;
            var center = Arc2D.Center;

            return new Rectangle2D2(
                new Line2D(center - Vector2D.BasisX * radius / 2, center + Vector2D.BasisX / 2),
                2 * radius
            );
        }

        public override bool ObjectInRectangle(Rectangle2D2 rect, ICanvasScreenConvertable canvasProxy, bool anyPoint)
        {
            return false;
        }

        public override bool PointInObject(Vector2D point, ICanvasScreenConvertable canvasProxy)
        {
            return ArcHitUtils.PointInArc(Arc2D, point, canvasProxy);
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

        private Pen _pen;
        /// <summary>
        /// 绘制圆弧的笔;
        /// </summary>
        public Pen Pen {
            get => _pen;
            set {
                _pen = value;
                RaiseVisualChanged();
            }
        }

        public override void Draw(ICanvas canvas, ICanvasScreenConvertable canvasProxy)
        {
            base.Draw(canvas, canvasProxy);

            if(Arc2D.Center == null)
            {
                return;
            }
            canvas.DrawArc(Pen,Arc2D.Center,Arc2D.Radius, Arc2D.StartAngle,Arc2D.Angle,Arc2D.Angle < Math.PI);

            DrawSelectedState(canvas, canvasProxy);
        }

        /// <summary>
        /// 绘制被选中状态;
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="canvasProxy"></param>
        private void DrawSelectedState(ICanvas canvas, ICanvasScreenConvertable canvasProxy) {
            if (!IsSelected) {
                return;
            }

            canvas.DrawArc(SelectionPen, Arc2D.Center, Arc2D.Radius, Arc2D.StartAngle, Arc2D.Angle, Arc2D.Angle < Math.PI); 
        }
    }
}
