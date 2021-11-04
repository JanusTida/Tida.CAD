using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Tida.CAD;
using Tida.CAD.Extensions;

namespace Tida.CAD.DrawObjects {
    /// <summary>
    /// DrawObject——Rectangle;
    /// </summary>
    public class Rectangle : DrawObject {
        static Rectangle()
        {
            NormalRectPen.Freeze();
        }

        public const double TolerantedScreenLength = 8.0D;

        public static readonly Pen NormalRectPen = new Pen(Brushes.White, 1);

        public Rectangle(CADRect rect) 
        {
            this.Rectangle2D = rect;
        }

        
        private CADRect _rectangle2D;

        public CADRect Rectangle2D 
        {
            get => _rectangle2D; 
            set
            {
                _rectangle2D = value;
                RaiseVisualChanged();
            }
        }

        
        public override CADRect? GetBoundingRect()
        {
            return Rectangle2D;
        }

        public override bool ObjectInRectangle(CADRect rect, ICADScreenConverter cadScreenConverter, bool anyPoint) 
        {
            //根据四个顶点的位置判断与指定矩形的包含关系;
            if (anyPoint) {
                return Rectangle2D.GetVertexes()?.Any(p => rect.Contains(p)) ?? false;
            }
            else {
                return Rectangle2D.GetVertexes()?.All(p => rect.Contains(p)) ?? false;
            }
        }

        public override bool PointInObject(Point point, ICADScreenConverter cadScreenConverter) 
        {
            if(cadScreenConverter == null) {
                throw new ArgumentNullException(nameof(cadScreenConverter));
            }
            return false;
        }

        public override void Draw(ICanvas canvas)
        {
            if (canvas == null) {
                throw new ArgumentNullException(nameof(canvas));
            }
            canvas.DrawRectangle(Rectangle2D, null, NormalRectPen);


            base.Draw(canvas);
        }

       
    }
}
