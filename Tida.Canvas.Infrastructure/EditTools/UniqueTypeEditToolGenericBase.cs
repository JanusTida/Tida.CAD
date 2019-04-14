using Tida.Canvas.Contracts;
using Tida.Canvas.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tida.Canvas.Infrastructure.EditTools {
    /// <summary>
    /// 适用于绘制单种绘制对象编辑工具实现类,本类实现了默认的撤销/重做;
    /// </summary>
    /// <typeparam name="TDrawObject">该工具所操作的绘制对象类别</typeparam>
    public abstract class UniqueTypeEditToolGenericBase<TDrawObject> : EditTool where TDrawObject : DrawObject {
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

        private void CanvasContext_Snaping(object sender, SnapingEventArgs e) {
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
        protected override void OnCommit() {
            if(CanvasContext == null) {
                return;
            }

            //若撤销栈为空,则不进行呈递修改操作;
            if (UndoDrawObjects.Count == 0) {
                return;
            }

            if (CanvasContext.ActiveLayer == null) {
                return;
            }
            
            var commitParams = new UniqueTypeEditToolCommitParams(new List<DrawObject>(UndoDrawObjects.ToArray())) {
                AppliedLayer = CanvasContext.ActiveLayer
            };

            OnCommitParams(commitParams);

            if (commitParams.Cancel) {
                return;
            }

            if(commitParams.AppliedLayer == null) {
                return;
            }

            if(commitParams.AddedDrawObjects.Count == 0) {
                return;
            }

            //将撤销栈拷贝后呈递事务;
            var addedDrawObjects = commitParams.AddedDrawObjects.ToArray();
            var appliedLayer = commitParams.AppliedLayer;

            appliedLayer.AddDrawObjects(addedDrawObjects);

            var transAction = new StandardEditTransaction(
                () => {
                    appliedLayer.RemoveDrawObjects(addedDrawObjects);
                },
                () => {
                    appliedLayer.AddDrawObjects(addedDrawObjects);
                }
            );

            CommitTransaction(transAction);

            //清除撤销/撤销栈;
            UndoDrawObjects.Clear();
            RedoDrawObjects.Clear();
        }
        
        protected virtual void OnCommitParams(UniqueTypeEditToolCommitParams commitParams) {
            
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
            RedoDrawObjects.Clear();

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
