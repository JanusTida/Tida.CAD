using CDO.Common.Canvas.Contracts;
using CDO.Common.Canvas.Shell.Contracts.EditTools;
using System;
using System.Collections.Generic;

namespace CDO.Common.Canvas.Shell.EditTools {
    /// <summary>
    /// 适用于绘制单种绘制对象编辑工具实现类,本类实现了默认的撤销/重做;
    /// </summary>
    /// <typeparam name="TDrawObject">该工具所操作的绘制对象类别</typeparam>
    public abstract class UniqueTypeEditToolGenericBase<TDrawObject> : MouseInteractableEditToolBase, IEditTool where TDrawObject : DrawObject {
        /// <summary>
        /// 本次编辑时,创建的绘制对象的撤销栈;
        /// </summary>
        protected readonly Stack<TDrawObject> UndoDrawObjects = new Stack<TDrawObject>();

        /// <summary>
        /// 本次编辑时,创建的绘制对象的重做栈;
        /// </summary>
        protected readonly Stack<TDrawObject> RedoDrawObjects = new Stack<TDrawObject>();

        public override bool CanRedo => RedoDrawObjects.Count != 0;

        public override bool CanUndo => UndoDrawObjects.Count != 0;

        public bool SupportSelection => true;

        protected override void OnBeginOperation() {
            if (CanvasContext == null) {
                return;
            }

            RedoDrawObjects.Clear();
            UndoDrawObjects.Clear();
           
            CanvasContext.Snaping += CanvasContext_Snaping;

            base.OnBeginOperation();
        }

        private void CanvasContext_Snaping(object sender, Events.SnapingEventArgs e) {
            if(e == null) {
                return;
            }

            if(e.DrawObjects == null) {
                return;
            }

            //将本次缓存的对象加入辅助判断;
            foreach (var drawObject in UndoDrawObjects) {
                e.DrawObjects.Add(drawObject);
            }
        }

        protected override void OnEndOperation() {
            if (CanvasContext == null) {
                return;
            }

            RedoDrawObjects.Clear();
            UndoDrawObjects.Clear();

            CanvasContext.Snaping -= CanvasContext_Snaping;

            base.OnEndOperation();
        }

        /// <summary>
        /// 呈递修改的默认实现,本方法将呈递撤销栈中的修改操作,并清空撤销/重做栈;
        /// </summary>
        /// <param name="canvasContext"></param>
        public override void Commit() {
            if(CanvasContext == null) {
                return;
            }

            //若撤销栈为空,则不进行呈递修改操作;
            if (UndoDrawObjects.Count == 0) {
                return;
            }

            if(CanvasContext.ActiveLayer == null) {
                return;
            }

            //将撤销栈拷贝后呈递事务;
            var drawObjectBufferInTransaction = UndoDrawObjects.ToArray();
            var activeLayer = CanvasContext.ActiveLayer;
            activeLayer.AddDrawObjects(drawObjectBufferInTransaction);

            var transAction = new StandardEditTransaction(
                () => {
                    activeLayer.RemoveDrawObjects(drawObjectBufferInTransaction);
                },
                () => {
                    activeLayer.AddDrawObjects(drawObjectBufferInTransaction);
                }
            );

            CommitTransaction(transAction);

            //清除撤销/撤销栈;
            UndoDrawObjects.Clear();
            RedoDrawObjects.Clear();
        }
        
        /// <summary>
        /// 将绘制对象添加到指定的图层中,并添加到撤销栈中;
        /// </summary>
        /// <param name="drawObject"></param>
        /// <param name="layer"></param>
        protected void AddDrawObjectToUndoStack(TDrawObject drawObject) {
            if (drawObject == null) {
                throw new ArgumentNullException(nameof(drawObject));
            }

            //加入到撤销栈中;
            UndoDrawObjects.Push(drawObject);
            RaiseCanUndoRedoChanged();
            RaiseVisualChanged();
        }

        /// <summary>
        /// 重做默认实现,本实现将重做栈中的最后入栈的元素出栈,
        /// 添加到对应图层中,并压入撤销栈;
        /// </summary>
        /// <param name="canvasContext"></param>
        public override void Redo() {
            if(CanvasContext == null) {
                return;
            }

            if (RedoDrawObjects.Count == 0) {
                return;
            }

            var lastRedoDrawObject = RedoDrawObjects.Pop();
            
            UndoDrawObjects.Push(lastRedoDrawObject);

            RaiseVisualChanged();

            RaiseCanUndoRedoChanged();
        }

        /// <summary>
        /// 撤销默认实现,本实现将撤销栈中最后入栈的元素出栈,从对应图层中移除,并压入重做栈;
        /// </summary>
        /// <param name="canvasContext"></param>
        public override void Undo() {
            if (CanvasContext == null) {
                return;
            }

            if (UndoDrawObjects.Count == 0) {
                return;
            }

            var lastUndoDrawObject = UndoDrawObjects.Pop();
            
            RedoDrawObjects.Push(lastUndoDrawObject);

            RaiseVisualChanged();

            RaiseCanUndoRedoChanged();
        }

        /// <summary>
        /// 绘制缓存中的绘制对象;
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="canvasProxy"></param>
        public override void Draw(ICanvas canvas, ICanvasScreenConvertable canvasProxy) {
            foreach (var drawObject in UndoDrawObjects) {
                drawObject.Draw(canvas, canvasProxy);
            }
            base.Draw(canvas, canvasProxy);
        }
        
    }
}
