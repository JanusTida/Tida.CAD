using Tida.Canvas.Contracts;
using Tida.Canvas.Media;
using Tida.Geometry.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.WPFCanvas {
    /// <summary>
    /// 矩形绘制对象的简单实现,将用于用作拖放选择等;
    /// </summary>
    class SimpleRectangle : IDrawable {
        /// <summary>
        /// 矩形数据;
        /// </summary>
        private Rectangle2D2 _rectangle2D;
        public Rectangle2D2 Rectangle2D {
            get {
                return _rectangle2D;
            }
            set {
                _rectangle2D = value;
                VisualChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// 填充画刷;
        /// </summary>
        private Brush _fill;
        public Brush Fill {
            get { return _fill; }
            set {
                _fill = value;
                VisualChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// 笔;
        /// </summary>
        private Pen _pen;

        public Pen Pen {
            get { return _pen; }
            set {
                _pen = value;
                VisualChanged?.Invoke(this, EventArgs.Empty);
            }
        }


        public event EventHandler VisualChanged;

        public void Draw(ICanvas canvas, ICanvasScreenConvertable canvasProxy) {
            if(Fill == null) {
                return;
            }

            if(Pen == null) {
                return;
            }

            if(Rectangle2D == null) {
                return;
            }

            canvas.DrawRectangle(Rectangle2D, Fill, Pen);
        }
    }
}
