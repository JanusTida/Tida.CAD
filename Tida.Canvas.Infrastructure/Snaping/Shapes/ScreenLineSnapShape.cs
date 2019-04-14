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
    /// 以视图为标准的线性辅助图形;
    /// </summary>
    public class ScreenLineSnapShape : StandardSnapPoint {
        public ScreenLineSnapShape(Line2D screenLine2D,Vector2D position):base(position) {

            this.ScreenLine2D = screenLine2D ?? throw new ArgumentNullException(nameof(screenLine2D));

        }

        private Pen _linePen = HighLightLinePen;
        public Pen LinePen {
            get => _linePen;
            set {
                _linePen = value;
                RaiseVisualChanged();
            }
        }

        private Line2D _screenline2D;
        public Line2D ScreenLine2D {
            get => _screenline2D;
            set {
                _screenline2D = value;
                RaiseVisualChanged();
            }
        }
        

        public override void Draw(ICanvas canvas, ICanvasScreenConvertable canvasProxy) {
            base.Draw(canvas, canvasProxy);
            
            canvas.NativeDrawLine(LinePen, ScreenLine2D);
        }
    }
}
