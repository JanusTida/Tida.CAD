using System;
using System.Collections.Generic;
using System.Text;
using Tida.Canvas.Contracts;
using Tida.Canvas.Media;
using Tida.Geometry.Primitives;

namespace SimpleSample
{
    class CustomPolygon : DrawObject
    {
        private static readonly SolidColorBrush FillBrush = Brushes.Blue;
        private static readonly Pen BorderPen = Pen.CreateFrozenPen(Brushes.Yellow, 3);
        public override void Draw(ICanvas canvas, ICanvasScreenConvertable canvasProxy)
        {
            var points = new Vector2D[]
            {
                 new Vector2D(-30,-30),
                 new Vector2D(0,60),
                 new Vector2D(30,-30)
            };
            var points2 = new Vector2D[]
            {
                new Vector2D(-30,30),
                new Vector2D(0,-60),
                new Vector2D(30,30)
            };
            canvas.DrawPolygon(points, FillBrush, BorderPen);
            canvas.DrawPolygon(points2, FillBrush, BorderPen);
            base.Draw(canvas, canvasProxy);
        }
    }
}
