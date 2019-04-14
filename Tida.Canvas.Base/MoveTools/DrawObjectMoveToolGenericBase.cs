using CDO.Common.Canvas.Contracts;
using CDO.Common.Canvas.Shell.Contracts.Canvas;
using CDO.Common.Geometry.Primitives;
using System;

namespace CDO.Common.Canvas.Shell.MoveTools {
    /// <summary>
    /// 绘制对象移动工具的一个泛型基类，为子类提供了默认的类型安全实现;
    /// </summary>
    /// <typeparam name="TDrawObject"></typeparam>
    public abstract class DrawObjectMoveToolGenericBase<TDrawObject> : IDrawObjectMoveTool where TDrawObject : DrawObject {
        public bool CheckIsValidDrawObject(DrawObject drawObject) {
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
