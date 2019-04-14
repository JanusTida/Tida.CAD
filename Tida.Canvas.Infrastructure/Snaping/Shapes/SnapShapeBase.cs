using Tida.Canvas.Contracts;
using Tida.Geometry.Primitives;
using System;

namespace Tida.Canvas.Infrastructure.Snaping.Shapes {
    public abstract class SnapShapeBase : ISnapShape {
        public virtual Vector2D Position { get; set; }

        public event EventHandler VisualChanged;

        protected void RaiseVisualChanged() => VisualChanged?.Invoke(this, EventArgs.Empty);

        public abstract void Draw(ICanvas canvas, ICanvasScreenConvertable canvasProxy);

        public abstract Rectangle2D2 GetNativeBoundingRect(ICanvasScreenConvertable canvasProxy);
    }
}
