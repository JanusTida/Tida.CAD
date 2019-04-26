using Tida.Canvas.Contracts;
using Tida.Geometry.Primitives;
using System;

namespace Tida.Canvas.Infrastructure.OffsetTools {
    /// <summary>
    /// 绘制对象偏移工具泛型基类;
    /// </summary>
    public abstract class DrawObjectOffsetToolGenericBase<TDrawObject> : IDrawObjectOffsetTool where TDrawObject : DrawObject {
        public bool CheckDrawObjectMoveable(DrawObject drawObject) {
            return drawObject is TDrawObject;
        }

        public void MoveOffset(DrawObject drawObject, double offset, Vector2D relativeTo) {
            if (!CheckDrawObjectMoveable(drawObject)) {
                throw new InvalidCastException(nameof(drawObject));
            }

            var tDrawObject = drawObject as TDrawObject;
            if (relativeTo == null) {
                throw new ArgumentNullException(nameof(relativeTo));
            }

            OnMoveOffset(tDrawObject, offset, relativeTo);
        }

        protected abstract void OnMoveOffset(TDrawObject drawObject, double offset, Vector2D relativeTo);
    }
}
