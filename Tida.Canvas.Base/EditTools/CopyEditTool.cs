using Tida.Canvas.Infrastructure.Contracts;
using Tida.Canvas.Infrastructure.MoveTools;
using Tida.Canvas.Contracts;
using Tida.Canvas.Events;
using Tida.Canvas.Input;
using Tida.Geometry.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using static Tida.Canvas.Infrastructure.Constants;

namespace Tida.Canvas.Base.EditTools {
    /// <summary>
    /// 绘制工具——复制;
    /// </summary>
    public partial class CopyEditTool : EditTool, IHaveMousePositionTracker {
        
        /// <summary>
        /// 所有绘制对象操作工具;
        /// </summary>
        public static readonly List<IDrawObjectMoveTool> DrawObjectMoveTools = new List<IDrawObjectMoveTool>();
        
        /// <summary>
        /// 记录当前“抓起”的对象,所关注(根据原件的选择状态而定)的拷贝的绘制对象及其操作工具的状态集合;
        /// </summary>
        private readonly List<CopyEditDrawObjectCell> _copyEditDrawObjectCells = new List<CopyEditDrawObjectCell>();

        /// <summary>
        /// 本次编辑中,已经从原件被拷贝的缓存绘制对象;
        /// </summary>
        private readonly List<DrawObject> _copiedDrawObjects = new List<DrawObject>();

        /// <summary>
        /// 即将被添加的绘制对象的撤销/重做栈;
        /// </summary>
        private readonly Stack<DrawObject[]> _undoCopiedDrawObjectsStack = new Stack<DrawObject[]>();
        private readonly Stack<DrawObject[]> _redoCopiedDrawObjectsStack = new Stack<DrawObject[]>();
        
        /// <summary>
        /// 将<see cref="ICanvasContext"/>中选中的绘制对象与<see cref="_copyEditDrawObjectCells"/>同步;
        /// </summary>
        /// <param name="canvasContext">对应的画布上下文</param>
        private void SyncSelectedDrawObjectsToCopiedCells() {
            //获取所有被选中的绘制对象;
            var selectedDrawObjects = CanvasContext.GetAllDrawObjects().Where(p => p.IsSelected).
                Union(_copiedDrawObjects.Where(p => p.IsSelected));

            //清除集合后重新填充;
            _copyEditDrawObjectCells.Clear();

            foreach (var drawObject in selectedDrawObjects) {
                var copiedObject = drawObject.Clone();

                if (copiedObject == null) {
                    continue;
                }

                copiedObject.IsSelected = false;

                var copyCell = new CopyEditDrawObjectCell {
                    CopiedDrawObject = copiedObject
                };

                copyCell.DrawObjectMoveTool = DrawObjectMoveTools.
                    FirstOrDefault(p => p.CheckDrawObjectMoveable(drawObject));

                ///若找到了满足条件的工具项,则加入<see cref="_copyEditDrawObjectCells"/>
                if (copyCell.DrawObjectMoveTool != null) {
                    _copyEditDrawObjectCells.Add(copyCell);
                }
            }
        }

        

        /// <summary>
        /// 将拷贝绘制对象(非原件)集合的位置回溯至上一次鼠标按下的位置;
        /// </summary>
        private void RollBackToLastMouseDownPosition() {
            if (MousePositionTracker.LastMouseDownPosition == null) {
                return;
            }

            if (MousePositionTracker.CurrentHoverPosition == null) {
                return;
            }

            _copyEditDrawObjectCells.ForEach(p => {
                p.DrawObjectMoveTool.Move(p.CopiedDrawObject, MousePositionTracker.LastMouseDownPosition - MousePositionTracker.CurrentHoverPosition);
            });
        }

        /// <summary>
        /// 在本次编辑栈内,应用拷贝状态的更改;
        /// </summary>
        /// <param name="thisMouseDownPosition">本次鼠标按下的位置</param>
        private void ApplyCurrentPositionState(Vector2D thisMouseDownPosition) {
            if (MousePositionTracker.LastMouseDownPosition == null) {
                return;
            }

            if (MousePositionTracker.CurrentHoverPosition == null) {
                return;
            }

            RollBackToLastMouseDownPosition();

            var offset = thisMouseDownPosition - MousePositionTracker.LastMouseDownPosition;

            _copyEditDrawObjectCells.ForEach(p => {
                p.DrawObjectMoveTool.Move(p.CopiedDrawObject, offset);
            });

            //本次拷贝的绘制对象入栈;
            var newCopiedDrawObjects = _copyEditDrawObjectCells.
                Select(p => p.CopiedDrawObject.Clone()).ToArray();

            _copiedDrawObjects.AddRange(newCopiedDrawObjects);
            
            //将本次加入的绘制对象加入栈内;
            _undoCopiedDrawObjectsStack.Push(newCopiedDrawObjects);
            //发生了新的动作时,应清空重做栈;
            _redoCopiedDrawObjectsStack.Clear();
            //清空"抓起"的对象;
            _copyEditDrawObjectCells.Clear();
        }

        public override bool CanUndo => _undoCopiedDrawObjectsStack.Count != 0;

        public override bool CanRedo => _redoCopiedDrawObjectsStack.Count != 0;

        public override bool IsEditing => true;

        private MousePositionTracker _mousePositionTracker;
        public MousePositionTracker MousePositionTracker => _mousePositionTracker ??(_mousePositionTracker = new MousePositionTracker(this));

        protected override void OnCommit() {
            if(CanvasContext.ActiveLayer == null) {
                throw new InvalidOperationException();
            }

            if(_copiedDrawObjects.Count == 0) {
                return;
            }
            
            var commitedDrawObjects = _copiedDrawObjects.ToArray();
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

        public override void Redo() {
            if (!CanRedo) {
                return;
            }

            var drawObjects = _redoCopiedDrawObjectsStack.Pop();
            _copiedDrawObjects.AddRange(drawObjects);

            _undoCopiedDrawObjectsStack.Push(drawObjects);

            RaiseVisualChanged();
        }

        public override void Undo() {
            if (!CanUndo) {
                return;
            }

            var drawObjects = _undoCopiedDrawObjectsStack.Pop();
            foreach (var drawObject in drawObjects) {
                _copiedDrawObjects.Remove(drawObject);
            }

            _redoCopiedDrawObjectsStack.Push(drawObjects);

            RaiseVisualChanged();
        }

        protected override void OnCanvasContextChanged(ValueChangedEventArgs<ICanvasContextEx> args) {
            if(args.NewValue is ICanvasContextEx newCanvasContext) {
                newCanvasContext.DragSelect += CanvasContext_DragSelect;
                newCanvasContext.DrawSelectMouseMove += CanvasContext_DrawSelectMouseMove;
                newCanvasContext.PreviewInteractionWithDrawObjects += CanvasContext_PreviewInteractionWithDrawObjects;
                newCanvasContext.ClickSelect += CanvasContext_ClickSelect;
            }

            if(args.OldValue is ICanvasContextEx oldCanvasContext) {
                oldCanvasContext.DragSelect -= CanvasContext_DragSelect;
                oldCanvasContext.DrawSelectMouseMove -= CanvasContext_DrawSelectMouseMove;
                oldCanvasContext.PreviewInteractionWithDrawObjects -= CanvasContext_PreviewInteractionWithDrawObjects;
                oldCanvasContext.ClickSelect -= CanvasContext_ClickSelect;
            }

            base.OnCanvasContextChanged(args);
        }



        /// <summary>
        /// 根据鼠标按下的位置,更新拷贝的绘制对象(非原件)的选中状态;
        /// </summary>
        private void CanvasContext_ClickSelect(object sender, ClickSelectEventArgs e) {
            var hitPosition = e.HitPosition;
            if (hitPosition == null) {
                return;
            }

            //若Control键被按下,更新绘制对象(非原件)的选中状态后返回,使得外部继续执行队列;
            if (CheckIsContrlKeyDown()) {
                _copiedDrawObjects.ForEach(p => {
                    if (p.PointInObject(hitPosition, CanvasContext.CanvasProxy)) {
                        p.IsSelected = !p.IsSelected;
                    }
                });
                this.RaiseVisualChanged();
            }
            else {
                e.Cancel = true;
            }

        }

        /// <summary>
        /// 禁用画布本身与绘制对象交互;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CanvasContext_PreviewInteractionWithDrawObjects(object sender, PreviewDrawObjectsInteractionEventArgs e) {
            e.Cancel = true;
        }


        /// <summary>
        /// 禁用拖拽选中;
        /// </summary>
        /// <param name="e"></param>
        private void CanvasContext_DragSelect(object sender, DragSelectEventArgs e) {
            e.Cancel = true;
        }

        /// <summary>
        /// 禁用拖拽选中;
        /// </summary>
        /// <param name="e"></param>
        private void CanvasContext_DrawSelectMouseMove(object sender, DragSelectMouseMoveEventArgs e) {
            e.Handled = true;
        }

       

        /// <summary>
        /// 检查Control键是否被按下;
        /// </summary>
        /// <returns></returns>
        private bool CheckIsContrlKeyDown() => ((CanvasContext?.InputDevice.KeyBoard.ModifierKeys??ModifierKeys.None & ModifierKeys.Control) == ModifierKeys.Control);

        
        protected override void OnMouseDown(MouseDownEventArgs e) {
            if (e == null) {
                throw new ArgumentNullException(nameof(e));
            }

            //需指定为左键;
            if(e.Button != MouseButton.Left) {
                return;
            }

            var thisMouseDownPosition = e.Position;
            
            //若上次按下为空,则为首次按下鼠标;
            if (MousePositionTracker.LastMouseDownPosition == null) {
                ///若Control键被按下,则不处理;
                if (CheckIsContrlKeyDown()) {
                    return;
                }

                SyncSelectedDrawObjectsToCopiedCells();
                //记录为上次鼠标按下的位置;
                MousePositionTracker.LastMouseDownPosition = thisMouseDownPosition;
            }
            //否则,在本次编辑栈内,应用复制对象;
            else {
                ApplyCurrentPositionState(thisMouseDownPosition);

                MousePositionTracker.LastMouseDownPosition = null;
                MousePositionTracker.CurrentHoverPosition = null;
                e.Handled = true;
            }

            RaiseVisualChanged();

            RaiseCanUndoRedoChanged();
            
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseMoveEventArgs e) {
            if (e == null) {
                throw new ArgumentNullException(nameof(e));
            }

            if(MousePositionTracker.LastMouseDownPosition == null) {
                return;
            }
            
            //回滚到最近一次鼠标按下的位置;
            RollBackToLastMouseDownPosition();

            var position = e.Position;

            //进行偏移变化;
            _copyEditDrawObjectCells.ForEach(p => {
                p.DrawObjectMoveTool.Move(p.CopiedDrawObject, position - MousePositionTracker.LastMouseDownPosition);
            });
            
            MousePositionTracker.CurrentHoverPosition = position;

            RaiseVisualChanged();

            e.Handled = true;

            base.OnMouseMove(e);
        }

        protected override void OnEndOperation() {
            if(CanvasContext == null) {
                return;
            }

            CanvasContext.Snaping -= CanvasContext_Snaping;

            MousePositionTracker.LastMouseDownPosition = null;
            MousePositionTracker.CurrentHoverPosition = null;

            _copiedDrawObjects.Clear();
            _copyEditDrawObjectCells.Clear();

            _undoCopiedDrawObjectsStack.Clear();
            _redoCopiedDrawObjectsStack.Clear();

            base.OnEndOperation();
        }

        protected override void OnBeginOperation() {
            if(CanvasContext == null) {
                return;
            }
            
            CanvasContext.Snaping += CanvasContext_Snaping;

            base.OnBeginOperation();
        }

        /// <summary>
        /// 在进行辅助处理时,将缓存的拷贝的图形也一并加入辅助判断;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CanvasContext_Snaping(object sender, SnapingEventArgs e) {

            if (e == null) {
                throw new ArgumentNullException(nameof(e));
            }
            
            

            foreach (var drawObject in _copiedDrawObjects) {
                e.DrawObjects.Add(drawObject);
            }
            
        }

        public override void Draw(ICanvas canvas, ICanvasScreenConvertable canvasProxy) {
            //绘制拷贝的绘制对象(非原件);
            _copiedDrawObjects.ForEach(p => p.Draw(canvas, canvasProxy));

            //绘制当前关注(抓起)的绘制对象;
            canvas.PushOpacity(0.5);
            _copyEditDrawObjectCells.ForEach(p => p.CopiedDrawObject.Draw(canvas, canvasProxy));
            canvas.Pop();

            if (MousePositionTracker.CurrentHoverPosition != null && MousePositionTracker.LastMouseDownPosition != null) {
                canvas.DrawLine(
                    LastMouseDownToCurrentMouseLinePen, 
                    new Line2D(MousePositionTracker.LastMouseDownPosition, MousePositionTracker.CurrentHoverPosition)
                );
            }

            base.Draw(canvas, canvasProxy);
        }
    }

    public partial class CopyEditTool {

        /// <summary>
        /// 本单位用于记录当前所关注的拷贝的绘制对象,及其操作工具的状态;
        /// </summary>
        class CopyEditDrawObjectCell {
            /// <summary>
            /// 拷贝的绘制对象;
            /// </summary>
            public DrawObject CopiedDrawObject { get; set; }

            /// <summary>
            /// 对应的操作工具;
            /// </summary>
            public IDrawObjectMoveTool DrawObjectMoveTool { get; set; }
        }
    }

    
   
}
