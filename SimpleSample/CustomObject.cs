using System;
using System.Collections.Generic;
using System.Text;
using Tida.Canvas.Contracts;
using Tida.Canvas.Media;
using Tida.Geometry.Primitives;

namespace SimpleSample
{
    class CustomObject : DrawObject
    {
        public override DrawObject Clone() => null;

        public override Rectangle2D2 GetBoundingRect() => null;

        public override bool ObjectInRectangle(Rectangle2D2 rect, ICanvasScreenConvertable canvasProxy, bool anyPoint)
        {
            return false;
        }

        public override bool PointInObject(Vector2D point, ICanvasScreenConvertable canvasProxy)
        {
            return false;
        }

        public override void Draw(ICanvas canvas, ICanvasScreenConvertable canvasProxy)
        {
            base.Draw(canvas, canvasProxy);

            
            canvas.DrawEllipse(null, new Pen { Brush = Brushes.Purple, Thickness = 2 }, Vector2D.Zero, 2, 4);
            canvas.DrawText("This is a custom drawObject", 12, Brushes.White, new Vector2D(-1, 0));
        }
    }
}
