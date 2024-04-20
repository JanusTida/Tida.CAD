using Tida.Canvas.Contracts;
using Tida.Geometry.Primitives;
using System;

namespace Tida.Canvas.Infrastructure.MoveTools {
    /// <summary>
    /// 绘制对象移动工具的一个泛型基类，为子类提供了默认的类型安全实现;
    /// </summary>
    /// <typeparam name="TDrawObject"></typeparam>
    public abstract class DrawObjectMoveToolBase<TDrawObject> : IDrawObjectMoveTool where TDrawObject : DrawObject {
        public bool CheckDrawObjectMoveable(DrawObject drawObject) {
            return drawObject is TDrawObject;
        }
        
        public void Move(DrawObject drawObject, Vector2D offset) {

            if (drawObject == null) {
                throw new ArgumentNullException(nameof(drawObject));
            }

            if (offset == null) {
                throw new ArgumentNullException(nameof(offset));
            }

            if (!(drawObject is TDrawObject tDrawObject)) {
                throw new InvalidOperationException();
            }

            OnMove(tDrawObject, offset);
        }

        protected abstract void OnMove(TDrawObject drawObject, Vector2D offset);

    }
}
