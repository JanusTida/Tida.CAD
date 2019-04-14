using Tida.Canvas.Contracts;
using Tida.Canvas.Media;
using Tida.Geometry.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Tida.Canvas.Infrastructure.Constants;

namespace Tida.Canvas.Infrastructure.Snaping.Shapes {
    /// <summary>
    /// 以数学坐标为准的线性辅助图形;
    /// </summary>
    public class LineSnapShape : StandardSnapPoint {
        public LineSnapShape(Line2D screenLine2D, Vector2D position) : base(position) {

            this.Line2D = screenLine2D ?? throw new ArgumentNullException(nameof(screenLine2D));

        }

        private Pen _linePen = HighLightLinePen;
        public Pen LinePen {
            get => _linePen;
            set {
                _linePen = value;
                RaiseVisualChanged();
            }
        }

        private Line2D _line2D;
        public Line2D Line2D {
            get => _line2D;
            set {
                _line2D = value;
                RaiseVisualChanged();
            }
        }

        public override void Draw(ICanvas canvas, ICanvasScreenConvertable canvasProxy) {
            base.Draw(canvas, canvasProxy);

            canvas.DrawLine(LinePen, Line2D);
        }
    }
}
