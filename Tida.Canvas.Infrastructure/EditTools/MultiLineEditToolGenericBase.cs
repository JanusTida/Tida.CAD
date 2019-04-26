using Tida.Canvas.Infrastructure.DrawObjects;
using Tida.Geometry.Primitives;

namespace Tida.Canvas.Infrastructure.EditTools {
    /// <summary>
    /// 连续地绘制线性图形的编辑工具基类;
    /// </summary>
    /// <typeparam name="TLine"></typeparam>
    public abstract class MultiLineEditToolGenericBase<TLine>:MouseInteractableEditToolGenericBase<TLine> where TLine:LineBase {
        protected override void OnCommit() {
            MousePositionTracker.LastMouseDownPosition = null;
            MousePositionTracker.CurrentHoverPosition = null;
            base.OnCommit();
        }
        public override void Redo() {
            if (RedoDrawObjects.Count == 0) {
                return;
            }

            var lastRedoLine = RedoDrawObjects.Peek();
            MousePositionTracker.LastMouseDownPosition = lastRedoLine.Line2D.End;

            //继续撤销操作;
            base.Redo();
        }

        public override void Undo() {
            //若撤销栈不为空，则将上次鼠标位置置为最后压入的线段的起始位置;
            if (UndoDrawObjects.Count != 0) {
                var lastUndoLine = UndoDrawObjects.Peek();

                MousePositionTracker.LastMouseDownPosition = lastUndoLine.Line2D.Start;
            }
            //否则将上次鼠标位置置为空;
            else {
                MousePositionTracker.LastMouseDownPosition = null;
            }

            base.Undo();
        }


        protected override void OnApplyMouseDownPosition(Vector2D thisMouseDownPosition) {
            //若上一次鼠标按下的位置不为空,则不是第一次按下鼠标,需添加线段;
            if (MousePositionTracker.LastMouseDownPosition != null) {
                var drawObject = OnCreateDrawObject(MousePositionTracker.LastMouseDownPosition, thisMouseDownPosition);
                AddDrawObjectToUndoStack(drawObject);
            }

            MousePositionTracker.LastMouseDownPosition = thisMouseDownPosition;
            MousePositionTracker.CurrentHoverPosition = thisMouseDownPosition;
        }

        /// <summary>
        /// 根据关键信息,创建一个特定的绘制对象;
        /// </summary>
        /// <param name="lastDownPosition"></param>
        /// <param name="thisMouseDownPosition"></param>
        /// <returns></returns>
        protected abstract TLine OnCreateDrawObject(Vector2D lastDownPosition, Vector2D thisMouseDownPosition);
    }
}
