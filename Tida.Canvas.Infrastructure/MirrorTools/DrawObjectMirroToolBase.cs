using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tida.Canvas.Contracts;
using Tida.Geometry.Primitives;

namespace Tida.Canvas.Infrastructure.MirrorTools
{
    public abstract class DrawObjectMirroToolBase<TDrawObject> : IDrawObjectMirrorTool where TDrawObject : DrawObject
    {
        public bool CheckDrawObjectMirrorable(DrawObject drawObject)
        {
            return drawObject is TDrawObject;
        }

        public void Mirror(DrawObject drawObject, Line2D axis)
        {
            if (drawObject == null)
            {
                throw new ArgumentNullException(nameof(drawObject));
            }

            if (axis == null)
            {
                throw new ArgumentNullException(nameof(axis));
            }

            if (!(drawObject is TDrawObject tDrawObject))
            {
                throw new InvalidOperationException();
            }

            OnMirror(tDrawObject, axis);
        }

        protected abstract void OnMirror(TDrawObject drawObject, Line2D axis);
    }
}
