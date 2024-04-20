using Tida.Canvas.Infrastructure.Contracts;
using Tida.Canvas.Infrastructure.MirrorTools;
using Tida.Canvas.Contracts;
using Tida.Canvas.Events;
using Tida.Canvas.Input;
using Tida.Geometry.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using static Tida.Canvas.Infrastructure.Constants;

namespace Tida.Canvas.Infrastructure.EditTools {
    /// <summary>
    /// 镜像工具
    /// </summary>
    public class MirrorEditTool : EditTool
    {
        /// <summary>
        /// 所有绘制对象操作工具
        /// </summary>
        public static readonly List<IDrawObjectMirrorTool> DrawObjectMirrorTools = new List<IDrawObjectMirrorTool>();

        /// <summary>
        /// 记录当前需要镜像的对象及其操作工具的状态集合
        /// </summary>
        private readonly List<MirrorEditDrawObjectCell> _mirrorEditDrawObjectCells = new List<MirrorEditDrawObjectCell>();

        /// <summary>
        /// 已经被镜像的,将被替换的绘制对象原件;
        /// 应用修改时,这些对象将会从其图层中移除,被新的对象替代;
        /// </summary>
        private readonly List<DrawObject> _originReplacedDrawObjects = new List<DrawObject>();

        /// <summary>
        /// 本次编辑的所有镜像操作所得到的新的绘制对象;
        /// </summary>
        private readonly List<DrawObject> _createdDrawObjects = new List<DrawObject>();

        /// <summary>
        /// 即将被添加的绘制对象的撤销/重做栈;
        /// </summary>
        private readonly Stack<DrawObject[]> _undoMirroredDrawObjectsStack = new Stack<DrawObject[]>();
        private readonly Stack<DrawObject[]> _redoMirroredDrawObjectsStack = new Stack<DrawObject[]>();

        /// <summary>
        /// 将<see cref="ICanvasContext"/>中选中的绘制对象与<see cref="_mirrorEditDrawObjectCells"/>同步;
        /// </summary>
        private void SyncSelectedDrawObjectsToMirroredCells()
        {
            //获取所有被选中的绘制对象;
            var selectedDrawObjects = CanvasContext.GetAllDrawObjects().Where(p => p.IsSelected).
                Union(_createdDrawObjects.Where(p => p.IsSelected));

            //清除集合后重新填充;
            _mirrorEditDrawObjectCells.Clear();

            foreach (var drawObject in selectedDrawObjects)
            {
                var copiedObject = drawObject.Clone();

                if (copiedObject == null)
                {
                    continue;
                }

                copiedObject.IsSelected = false;

                var mirrorCell = new MirrorEditDrawObjectCell
                {
                    MirroredDrawObject = copiedObject
                };

                mirrorCell.DrawObjectMirrorTool = DrawObjectMirrorTools.
                    FirstOrDefault(p => p.CheckDrawObjectMirrorable(drawObject));

                ///若找到了满足条件的工具项,则加入<see cref="_copyEditDrawObjectCells"/>
                if (mirrorCell.DrawObjectMirrorTool != null)
                {
                    _mirrorEditDrawObjectCells.Add(mirrorCell);
                }
            }
        }

        /// <summary>
        /// 将拷贝绘制对象(非原件)集合的位置回溯至上一次鼠标按下的位置;
        /// </summary>
        private void RollBackToLastMouseDownPosition()
        {
            if (MousePositionTracker.LastMouseDownPosition == null)
            {
                return;
            }

            if (MousePositionTracker.CurrentHoverPosition == null)
            {
                return;
            }

            _mirrorEditDrawObjectCells.ForEach(p =>
            {
                p.DrawObjectMirrorTool.Mirror(p.MirroredDrawObject, Line2D.Create(MousePositionTracker.LastMouseDownPosition, MousePositionTracker.CurrentHoverPosition));
            });
        }

        private MousePositionTracker _mousePositionTracker;
        public MousePositionTracker MousePositionTracker => _mousePositionTracker ?? (_mousePositionTracker = new MousePositionTracker(this));
        public override bool CanUndo => _undoMirroredDrawObjectsStack.Count != 0;

        public override bool CanRedo => _redoMirroredDrawObjectsStack.Count != 0;

        public override bool IsEditing => true;

        public override void Redo()
        {
            if (!CanRedo)
            {
                return;
            }

            var drawObjects = _redoMirroredDrawObjectsStack.Pop();
            _createdDrawObjects.AddRange(drawObjects);

            _undoMirroredDrawObjectsStack.Push(drawObjects);

            RaiseVisualChanged();
        }

        public override void Undo()
        {
            if (!CanUndo)
            {
                return;
            }

            var drawObjects = _undoMirroredDrawObjectsStack.Pop();
            foreach (var drawObject in drawObjects)
            {
                _createdDrawObjects.Remove(drawObject);
            }

            _redoMirroredDrawObjectsStack.Push(drawObjects);

            RaiseVisualChanged();
        }

        protected override void OnCommit()
        {
            if (CanvasContext.ActiveLayer == null)
            {
                throw new InvalidOperationException();
            }

            if (_createdDrawObjects.Count == 0)
            {
                return;
            }

            var commitedDrawObjects = _createdDrawObjects.ToArray();
            var layer = CanvasContext.ActiveLayer;
            layer.AddDrawObjects(commitedDrawObjects);

            //呈递事务;
            var transaction = new StandardEditTransaction(
                () => {
                    layer.RemoveDrawObjects(commitedDrawObjects);
                },
                () => {
                    layer.AddDrawObjects(commitedDrawObjects);
                }
            );

            CommitTransaction(transaction);
        }


        protected override void OnCanvasContextChanged(ValueChangedEventArgs<ICanvasContextEx> args)
        {
            if (args.NewValue is ICanvasContextEx newCanvasContext)
            {
                newCanvasContext.DragSelect += CanvasContext_DragSelect;
                newCanvasContext.DrawSelectMouseMove += CanvasContext_DrawSelectMouseMove;
                newCanvasContext.PreviewInteractionWithDrawObjects += CanvasContext_PreviewInteractionWithDrawObjects;
                newCanvasContext.ClickSelect += CanvasContext_ClickSelect;
            }

            if (args.OldValue is ICanvasContextEx oldCanvasContext)
            {
                oldCanvasContext.DragSelect -= CanvasContext_DragSelect;
                oldCanvasContext.DrawSelectMouseMove -= CanvasContext_DrawSelectMouseMove;
                oldCanvasContext.PreviewInteractionWithDrawObjects -= CanvasContext_PreviewInteractionWithDrawObjects;
                oldCanvasContext.ClickSelect -= CanvasContext_ClickSelect;
            }

            base.OnCanvasContextChanged(args);
        }

        protected override void OnMouseDown(MouseDownEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            //需指定为左键;
            if (e.Button != MouseButton.Left)
            {
                return;
            }

            var thisMouseDownPosition = e.Position;

            //若上次按下为空,则为首次按下鼠标;
            if (MousePositionTracker.LastMouseDownPosition == null)
            {
                ///若Control键被按下,则不处理;
                if (CheckIsContrlKeyDown())
                {
                    return;
                }

                SyncSelectedDrawObjectsToMirroredCells();
                //记录为上次鼠标按下的位置;
                MousePositionTracker.LastMouseDownPosition = thisMouseDownPosition;
            }
            //否则,在本次编辑栈内,应用复制对象;
            else
            {
                ApplyCurrentPositionState(thisMouseDownPosition);

                MousePositionTracker.LastMouseDownPosition = null;
                MousePositionTracker.CurrentHoverPosition = null;
                e.Handled = true;
            }

            RaiseVisualChanged();

            RaiseCanUndoRedoChanged();

            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            if (MousePositionTracker.LastMouseDownPosition == null)
            {
                return;
            }

            //回滚到最近一次鼠标按下的位置;
            RollBackToLastMouseDownPosition();

            var position = e.Position;

            //进行偏移变化;
            _mirrorEditDrawObjectCells.ForEach(p =>
            {
                p.DrawObjectMirrorTool.Mirror(p.MirroredDrawObject, Line2D.Create(position, MousePositionTracker.LastMouseDownPosition));
            });

            MousePositionTracker.CurrentHoverPosition = position;

            RaiseVisualChanged();

            e.Handled = true;

            base.OnMouseMove(e);
        }

        protected override void OnBeginOperation()
        {
            if (CanvasContext == null)
            {
                return;
            }

            CanvasContext.Snaping += CanvasContext_Snaping;

            base.OnBeginOperation();
        }

        protected override void OnEndOperation()
        {
            if (CanvasContext == null)
            {
                return;
            }

            CanvasContext.Snaping -= CanvasContext_Snaping;

            MousePositionTracker.LastMouseDownPosition = null;
            MousePositionTracker.CurrentHoverPosition = null;

            _createdDrawObjects.Clear();
            _mirrorEditDrawObjectCells.Clear();

            _undoMirroredDrawObjectsStack.Clear();
            _redoMirroredDrawObjectsStack.Clear();

            base.OnEndOperation();
        }

        /// <summary>
        /// 在进行辅助处理时,将缓存的拷贝的图形也一并加入辅助判断;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CanvasContext_Snaping(object sender, SnapingEventArgs e)
        {

            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            foreach (var drawObject in _createdDrawObjects)
            {
                e.DrawObjects.Add(drawObject);
            }

        }

        /// <summary>
        /// 在本次编辑栈内,应用拷贝状态的更改;
        /// </summary>
        /// <param name="thisMouseDownPosition">本次鼠标按下的位置</param>
        private void ApplyCurrentPositionState(Vector2D thisMouseDownPosition)
        {
            if (MousePositionTracker.LastMouseDownPosition == null)
            {
                return;
            }

            if (MousePositionTracker.CurrentHoverPosition == null)
            {
                return;
            }

            RollBackToLastMouseDownPosition();

            var axis = Line2D.Create(thisMouseDownPosition, MousePositionTracker.LastMouseDownPosition);

            _mirrorEditDrawObjectCells.ForEach(p =>
            {
                p.DrawObjectMirrorTool.Mirror(p.MirroredDrawObject, axis);
            });

            //本次拷贝的绘制对象入栈;
            var newMirroredDrawObjects = _mirrorEditDrawObjectCells.
                Select(p => p.MirroredDrawObject.Clone()).ToArray();

            _createdDrawObjects.AddRange(newMirroredDrawObjects);

            //将本次加入的绘制对象加入栈内;
            _undoMirroredDrawObjectsStack.Push(newMirroredDrawObjects);
            //发生了新的动作时,应清空重做栈;
            _redoMirroredDrawObjectsStack.Clear();
            //清空"抓起"的对象;
            _mirrorEditDrawObjectCells.Clear();
        }

        /// <summary>
        /// 根据鼠标按下的位置,更新拷贝的绘制对象(非原件)的选中状态;
        /// </summary>
        private void CanvasContext_ClickSelect(object sender, ClickSelectEventArgs e)
        {
            var hitPosition = e.HitPosition;
            if (hitPosition == null)
            {
                return;
            }

            //若Control键被按下,更新绘制对象(非原件)的选中状态后返回,使得外部继续执行队列;
            if (CheckIsContrlKeyDown())
            {
                _createdDrawObjects.ForEach(p =>
                {
                    if (p.PointInObject(hitPosition, CanvasContext.CanvasProxy))
                    {
                        p.IsSelected = !p.IsSelected;
                    }
                });
                this.RaiseVisualChanged();
            }
            else
            {
                e.Cancel = true;
            }

        }

        /// <summary>
        /// 禁用画布本身与绘制对象交互;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CanvasContext_PreviewInteractionWithDrawObjects(object sender, PreviewDrawObjectsInteractionEventArgs e)
        {
            e.Cancel = true;
        }


        /// <summary>
        /// 禁用拖拽选中;
        /// </summary>
        /// <param name="e"></param>
        private void CanvasContext_DragSelect(object sender, DragSelectEventArgs e)
        {
            e.Cancel = true;
        }

        /// <summary>
        /// 禁用拖拽选中;
        /// </summary>
        /// <param name="e"></param>
        private void CanvasContext_DrawSelectMouseMove(object sender, DragSelectMouseMoveEventArgs e)
        {
            e.Handled = true;
        }

        /// <summary>
        /// 检查Control键是否被按下;
        /// </summary>
        /// <returns></returns>
        private bool CheckIsContrlKeyDown() => ((CanvasContext?.InputDevice.KeyBoard.ModifierKeys ?? ModifierKeys.None & ModifierKeys.Control) == ModifierKeys.Control);


        public override void Draw(ICanvas canvas, ICanvasScreenConvertable canvasProxy)
        {
            //绘制拷贝的绘制对象(非原件);
            _createdDrawObjects.ForEach(p => p.Draw(canvas, canvasProxy));

            //绘制当前关注(抓起)的绘制对象;
            canvas.PushOpacity(0.5);
            _mirrorEditDrawObjectCells.ForEach(p => p.MirroredDrawObject.Draw(canvas, canvasProxy));
            canvas.Pop();

            if (MousePositionTracker.CurrentHoverPosition != null && MousePositionTracker.LastMouseDownPosition != null)
            {
                canvas.DrawLine(
                    LastMouseDownToCurrentMouseLinePen,
                    new Line2D(MousePositionTracker.LastMouseDownPosition, MousePositionTracker.CurrentHoverPosition)
                );
            }

            base.Draw(canvas, canvasProxy);
        }

        /// <summary>
        /// 本单位用于记录当前所关注的拷贝的绘制对象,及其操作工具的状态;
        /// </summary>
        class MirrorEditDrawObjectCell
        {
            /// <summary>
            /// 镜像的绘制对象;
            /// </summary>
            public DrawObject MirroredDrawObject { get; set; }

            /// <summary>
            /// 对应的操作工具;
            /// </summary>
            public IDrawObjectMirrorTool DrawObjectMirrorTool { get; set; }
        }
    }
}
