using System;
using Tida.Canvas.Contracts;
using Tida.Geometry.Primitives;

namespace Tida.Canvas.Infrastructure.MirrorTools
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TDrawObject"></typeparam>
    public abstract class DrawObjectMirroToolBase<TDrawObject> : IDrawObjectMirrorTool where TDrawObject : DrawObject
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="drawObject"></param>
        /// <returns></returns>
        public bool CheckDrawObjectMirrorable(DrawObject drawObject)
        {
            return drawObject is TDrawObject;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="drawObject"></param>
        /// <param name="axis"></param>
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

        /// <summary>
        /// /
        /// </summary>
        /// <param name="drawObject"></param>
        /// <param name="axis"></param>
        protected abstract void OnMirror(TDrawObject drawObject, Line2D axis);
    }
}
