using System;
using System.Collections.Generic;
using System.Text;
using Tida.Canvas.Contracts;
using Tida.Canvas.Media;
using Tida.Geometry.Primitives;

namespace SimpleSample
{
    class CustomInteractionHandler:CanvasInteractionHandler
    {
        public override void Draw(ICanvas canvas, ICanvasScreenConvertable canvasProxy)
        {
            canvas.DrawEllipse(Brushes.AliceBlue, new Pen(), new Ellipse2D(new Vector2D(0, 0), 10, 18));
            base.Draw(canvas, canvasProxy);
        }
    }
}
