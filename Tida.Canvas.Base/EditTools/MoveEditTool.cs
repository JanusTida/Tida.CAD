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
    /// 编辑工具——移动;
    /// </summary>
    public partial class MoveEditTool : EditTool,IHaveMousePositionTracker {

        /// <summary>
        /// 所有绘制对象移动工具;
        /// </summary>
        public static readonly List<IDrawObjectMoveTool> DrawObjectMoveTools = new List<IDrawObjectMoveTool>();
        
        
        /// <summary>
        /// 记录被"抓起"的绘制对象及其拷贝集合;
        /// </summary>
        private readonly List<MoveEditCell> _catchedDrawObjectCells = new List<MoveEditCell>();

        /// <summary>
        /// 本次编辑过程中,移动过程记录的撤销栈;
        /// </summary>
        private readonly Stack<(MoveEditCell[] drawObjectCells, Vector2D offset)> _undoOffsetCells = new Stack<(MoveEditCell[] drawObjectCells, Vector2D offset)>();

        /// <summary>
        /// 本次编辑过程中,移动过程记录的重做栈;
        /// </summary>
        private readonly Stack<(MoveEditCell[] drawObjectCells, Vector2D offset)> _redoOffsetCells = new Stack<(MoveEditCell[] drawObjectCells, Vector2D offset)>();


        public override bool CanUndo => _undoOffsetCells.Count != 0;

        public override bool CanRedo => _redoOffsetCells.Count != 0;
        
        public override bool IsEditing => true;

        private MousePositionTracker _mousePositionTracker;
        public MousePositionTracker MousePositionTracker => _mousePositionTracker ?? (_mousePositionTracker = new MousePositionTracker(this));

        protected override void OnCommit() {
            //若鼠标当前位置不同于上次按下的位置,则回溯同步绘制对象(非原件)至上一次按下的位置;
            if(MousePositionTracker.LastMouseDownPosition != null && MousePositionTracker.CurrentHoverPosition != null) {
                if (!MousePositionTracker.LastMouseDownPosition.IsAlmostEqualTo(MousePositionTracker.CurrentHoverPosition)) {
                    _catchedDrawObjectCells.ForEach(p => {
                        p.DrawObjectMoveTool.Move(p.CopiedDrawObject, MousePositionTracker.LastMouseDownPosition - MousePositionTracker.CurrentHoverPosition);
                    });
                }
            }

            //还原可见;
            _catchedDrawObjectCells.ForEach(p => p.OriginDrawObject.IsVisible = true);

            ///因为<see cref="DrawObject.Parent"/>可能会在之后的操作产生变化,
            ///故此处使用临时匿名类,记录下当前的父级图层;
            var _catchedCellArr = _catchedDrawObjectCells.Select(p => 
                new {
                    Cell = p,
                    Layer = p.OriginDrawObject.Parent as CanvasLayer
                }).
            ToArray();
            
            //清除状态;
            _catchedDrawObjectCells.Clear();
            
            void Do() {
                foreach (var p in _catchedCellArr) {
                    if(p.Layer == null) {
                        continue;
                    }

                    p.Layer.RemoveDrawObject(p.Cell.OriginDrawObject);
                    p.Layer.AddDrawObject(p.Cell.CopiedDrawObject);
                    p.Cell.CopiedDrawObject.IsSelected = p.Cell.OriginDrawObject.IsSelected;
                }
            }

            void UnDo() {
                foreach (var p in _catchedCellArr) {
                    if(p.Layer == null) {
                        continue;
                    }

                    p.Layer.RemoveDrawObject(p.Cell.CopiedDrawObject);
                    p.Layer.AddDrawObject(p.Cell.OriginDrawObject);
                    p.Cell.OriginDrawObject.IsSelected = p.Cell.CopiedDrawObject.IsSelected;
                }
            }

            Do();

            CommitTransaction(new StandardEditTransaction(UnDo, Do));
        }
        
        protected override void OnBeginOperation() {
            if (CanvasContext == null) {
                return;
            }

            CanvasContext.Snaping += CanvasContext_Snaping;
            base.OnBeginOperation();
        }

        /// <summary>
        /// <see cref="ICanvasContextEx "/>辅助判断时,加入被抓起的绘制对象(非原件),使本移动工具内的被抓起的绘制对象(非原件)也能参与辅助判断;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CanvasContext_Snaping(object sender, SnapingEventArgs e) {
            foreach (var cell in _catchedDrawObjectCells) {
                //若正处于拖动状态,则正在被拖动的绘制对象(非原件)即正在被选中的实例,不能参与辅助判断;
                if(MousePositionTracker.CurrentHoverPosition != null && MousePositionTracker.LastMouseDownPosition != null) {
                    if (cell.CopiedDrawObject.IsSelected) {
                        continue;
                    }
                }
                e.DrawObjects.Add(cell.CopiedDrawObject);
            }
        }

        /// <summary>
        /// 清除本次编辑的状态;
        /// </summary>
        protected override void OnEndOperation() {
            if (CanvasContext == null) {
                return;
            }

            CanvasContext.Snaping -= CanvasContext_Snaping;

            _catchedDrawObjectCells.ForEach(p => {
                p.OriginDrawObject.IsVisible = true;
            });

            _catchedDrawObjectCells.Clear();

            _undoOffsetCells.Clear();
            _redoOffsetCells.Clear();

            MousePositionTracker.CurrentHoverPosition = null;
            MousePositionTracker.LastMouseDownPosition = null;

            base.OnEndOperation();
        }
            
        public override void Redo() {
            if (!CanRedo) {
                return;
            }

            RollBackToLastMouseDownPosition();

            var lastRedoOffsetTuple = _redoOffsetCells.Pop();

            foreach (var cell in lastRedoOffsetTuple.drawObjectCells) {
                cell.DrawObjectMoveTool.Move(cell.CopiedDrawObject, lastRedoOffsetTuple.offset);
            }

            _undoOffsetCells.Push(lastRedoOffsetTuple);

            RaiseVisualChanged();
        }

        public override void Undo() {
            if (!CanUndo) {
                return;
            }

            RollBackToLastMouseDownPosition();

            var lastUndoOffsetTuple = _undoOffsetCells.Pop();
            
            foreach (var cell in lastUndoOffsetTuple.drawObjectCells) {
                cell.DrawObjectMoveTool.Move(cell.CopiedDrawObject, -lastUndoOffsetTuple.offset);
            }

            _redoOffsetCells.Push(lastUndoOffsetTuple);

            RaiseVisualChanged();
        }

        protected override void OnMouseDown( MouseDownEventArgs e) {
            if (e == null) {
                throw new ArgumentNullException(nameof(e));
            }

            if (CanvasContext.Layers == null) {
                return;
            }
            
            //需指定为左键;
            if (e.Button != MouseButton.Left) {
                return;
            }

            var thisMouseDownPosition = e.Position;
            
            //若上次按下位置为空,则为首次按下鼠标;
            if (MousePositionTracker.LastMouseDownPosition == null) {
                //若Control键被按下,更新绘制对象(非原件)的选中状态后返回,使得外部继续执行队列;
                if ((CanvasContext.InputDevice.KeyBoard?.ModifierKeys & ModifierKeys.Control) == ModifierKeys.Control) {
                    if(UpdateDrawObjectSelectedState(thisMouseDownPosition, CanvasContext)) {
                        e.Handled = true;
                    }
                    RaiseVisualChanged();
                    return;
                }

                //同步选中的绘制对象到缓冲区中;
                SyncSelectedDrawObjectsToCachedCells();

                //记录上次鼠标按下的位置,和鼠标当前的位置;
                MousePositionTracker.LastMouseDownPosition = thisMouseDownPosition;
                MousePositionTracker.CurrentHoverPosition = thisMouseDownPosition;   
            }
            //若上次按下位置不为空,则在本次编辑栈内,应用移动状态;
            else if (MousePositionTracker.LastMouseDownPosition != null && MousePositionTracker.CurrentHoverPosition != null){
                ApplyCurrentPositionState(thisMouseDownPosition);

                MousePositionTracker.CurrentHoverPosition = null;
                MousePositionTracker.LastMouseDownPosition = null;
            }
            
            RaiseVisualChanged();
            
            RaiseCanUndoRedoChanged();

            e.Handled = true;

            base.OnMouseDown(e);
        }

        /// <summary>
        /// 鼠标移动时的绘制对象变化;
        /// </summary>
        /// <param name="canvasContext"></param>
        /// <param name="snapShape"></param>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseMoveEventArgs e) {

            //若上次按下的位置为空,则认为尚未按下鼠标;
            if (MousePositionTracker.LastMouseDownPosition == null) {
                return;
            }

            //回滚到最近一次鼠标按下的位置;
            RollBackToLastMouseDownPosition();

            var position = e.Position;

            //进行偏移变化;
            _catchedDrawObjectCells.ForEach(p => {
                if (!p.CopiedDrawObject.IsSelected) {
                    return;
                }

                p.DrawObjectMoveTool.Move(p.CopiedDrawObject, position - MousePositionTracker.LastMouseDownPosition);
            });

            //记录当前位置;
            MousePositionTracker.CurrentHoverPosition = position;

            RaiseVisualChanged();

            e.Handled = true;

            base.OnMouseMove(e);
        }

        /// <summary>
        /// 根据鼠标按下的位置,更新"抓起"的绘制对象(非原件)的选中状态;
        /// </summary>
        /// <returns>是否命中绘制对象</returns>
        private bool UpdateDrawObjectSelectedState(Vector2D mouseDownPosition,ICanvasContextEx  canvasContext) {
            if(mouseDownPosition == null) {
                return false;
            }

            var handled = false;
            _catchedDrawObjectCells.ForEach(p => {
                if (p.CopiedDrawObject.PointInObject(mouseDownPosition,  canvasContext.CanvasProxy)) {
                    p.CopiedDrawObject.IsSelected = !p.CopiedDrawObject.IsSelected;
                    handled = true;
                }
            });

            return handled;
        }

        /// <summary>
        /// 在本次编辑栈内,应用偏移状态的更改;
        /// </summary>
        /// <remarks>并非将当前的偏移状态应用到原绘制对象;</remarks>
        /// <param name="thisMouseDownPosition">本次鼠标按下的位置</param>
        private void ApplyCurrentPositionState(Vector2D thisMouseDownPosition) {
            if(MousePositionTracker.LastMouseDownPosition == null) {
                return;
            }

            if(MousePositionTracker.CurrentHoverPosition == null) {
                return;
            }

            RollBackToLastMouseDownPosition();

            var offset = thisMouseDownPosition - MousePositionTracker.LastMouseDownPosition;

            _catchedDrawObjectCells.ForEach(p => {
                if (!p.CopiedDrawObject.IsSelected) {
                    return;
                }

                p.DrawObjectMoveTool.Move(p.CopiedDrawObject, offset);
            });

            _undoOffsetCells.Push(
                (_catchedDrawObjectCells.Where(p => p.CopiedDrawObject.IsSelected).ToArray(),offset)
            );

            //发生了新的动作时,应清空重做栈;
            _redoOffsetCells.Clear();
        }

        /// <summary>
        /// 将绘制对象(非原件)集合的位置回溯至上一次鼠标按下的位置;
        /// </summary>
        private void RollBackToLastMouseDownPosition() {
            if (MousePositionTracker.LastMouseDownPosition == null) {
                return;
            }

            if (MousePositionTracker.CurrentHoverPosition == null) {
                return;
            }
            
            _catchedDrawObjectCells.ForEach(p => {
                if (!p.CopiedDrawObject.IsSelected) {
                    return;
                }

                p.DrawObjectMoveTool.Move(p.CopiedDrawObject, MousePositionTracker.LastMouseDownPosition - MousePositionTracker.CurrentHoverPosition);
            });
        }

        /// <summary>
        /// 将<see cref="ICanvasContextEx "/>中选中的绘制对象与<see cref="_catchedDrawObjectCells"/>同步;
        /// </summary>
        /// <param name="canvasContext">对应的画布上下文</param>
        private void SyncSelectedDrawObjectsToCachedCells() {
            //获取所有被选中的绘制对象;
            var selectedDrawObjects = CanvasContext.GetAllDrawObjects().Where(p => p.IsSelected);
            
            foreach (var drawObject in selectedDrawObjects) {
                //去重;
                if(_catchedDrawObjectCells.Any(p => p.OriginDrawObject == drawObject)) {
                    continue;
                }

                //复制原件,并将原件隐藏;
                var copiedObject = drawObject.Clone();

                if(copiedObject == null) {
                    continue;
                }

                ///从<see cref="DrawObjectMoveTools"/>和<see cref="DrawObjectSyncTools"/>寻找满足条件的工具项;
                var drawObjectMoveTool = DrawObjectMoveTools.
                    FirstOrDefault(p => p.CheckDrawObjectMoveable(drawObject));

                if(drawObjectMoveTool == null) {
                    continue;
                }

                var moveCell = new MoveEditCell {
                    OriginDrawObject = drawObject,
                    CopiedDrawObject = copiedObject
                };
                
                copiedObject.IsSelected = true;
                drawObject.IsVisible = false;

                
                moveCell.DrawObjectMoveTool = drawObjectMoveTool;

                

                ///若未找到满足条件的移动工具,则不能移动该对象;
                if(moveCell.DrawObjectMoveTool != null) {
                    _catchedDrawObjectCells.Add(moveCell);
                }
            }
        }
        
        public override void Draw(ICanvas canvas, ICanvasScreenConvertable canvasProxy) {
            //绘制"抓起"的绘制对象(非原件);
            canvas.PushOpacity(0.5);
            _catchedDrawObjectCells.ForEach(p => p.CopiedDrawObject.Draw(canvas, canvasProxy));
            canvas.Pop();

            if(MousePositionTracker.CurrentHoverPosition !=  null && MousePositionTracker.LastMouseDownPosition != null) {
                canvas.DrawLine(
                    LastMouseDownToCurrentMouseLinePen,
                    new Line2D(MousePositionTracker.LastMouseDownPosition, MousePositionTracker.CurrentHoverPosition)
                );
            }
            
            base.Draw(canvas, canvasProxy);
        }
        
    }

    public partial class MoveEditTool {
        /// <summary>
        /// 本单位用于记录所"抓起"的绘制对象,其拷贝对象,及其操作工具的状态;
        /// </summary>
        class MoveEditCell {
            /// <summary>
            /// 原件;
            /// </summary>
            public DrawObject OriginDrawObject { get; set; }

            /// <summary>
            /// 拷贝;
            /// </summary>
            public DrawObject CopiedDrawObject { get; set; }

            /// <summary>
            /// 对应的移动工具;
            /// </summary>
            public IDrawObjectMoveTool DrawObjectMoveTool { get; set; }
        }
    }

    
}
