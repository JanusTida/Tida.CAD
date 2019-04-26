using System;
using System.Collections.Generic;
using System.Linq;
using Tida.Canvas.Infrastructure.ExtendTools;
using Tida.Canvas.Infrastructure.Snaping;
using Tida.Canvas.Infrastructure.TrimTools;
using Tida.Canvas.Infrastructure.Utils;
using Tida.Canvas.Contracts;
using Tida.Canvas.Events;
using Tida.Canvas.Input;
using Tida.Geometry.Primitives;

namespace Tida.Canvas.Infrastructure.EditTools {

    /// <summary>
    /// 编辑工具——裁剪;
    /// </summary>

    public partial class TrimEditTool : EditTool {
        /// <summary>
        /// 所有绘制对象的相交规则;
        /// </summary>
        public static readonly List<IDrawObjectIntersectRule> DrawObjectIntersectRules = new List<IDrawObjectIntersectRule>();
        /// <summary>
        /// 所有绘制对象的裁剪工具;
        /// </summary>
        public static readonly List<IDrawObjectTrimTool> DrawObjectTrimTools = new List<IDrawObjectTrimTool>();
        /// <summary>
        /// 所有绘制对象的延伸工具;
        /// </summary>
        public static readonly List<IDrawObjectExtendTool> DrawObjectExtendTools = new List<IDrawObjectExtendTool>();

        /// <summary>
        /// 已经被裁剪的,将被替换的绘制对象原件;
        /// 应用修改时,这些对象将会从其图层中移除,被新的对象替代;
        /// </summary>
        private readonly List<DrawObject> _originReplacedDrawObjects = new List<DrawObject>();

        /// <summary>
        /// 本次编辑的所有裁剪操作所得到的新的绘制对象;
        /// </summary>
        private readonly List<DrawObject> _createdDrawObjects = new List<DrawObject>();

        /// <summary>
        /// 本次编辑的撤销栈;
        /// </summary>
        private readonly Stack<TrimEditDrawObjectCell> _undoTrimEditDrawObjectCells = new Stack<TrimEditDrawObjectCell>();

        /// <summary>
        /// 本次编辑的重做栈;
        /// </summary>
        private readonly Stack<TrimEditDrawObjectCell> _redoTrimEditDrawObjectCells = new Stack<TrimEditDrawObjectCell>();

        /// <summary>
        /// 是否是延伸模式;
        /// </summary>
        public bool IsExtendMode {
            get {
                if(CanvasContext == null) {
                    return false;
                }

                return (CanvasContext.InputDevice.KeyBoard.ModifierKeys & ModifierKeys.Shift) != 0;
            }
        }

        public override bool CanUndo => _undoTrimEditDrawObjectCells.Count != 0;

        public override bool CanRedo => _redoTrimEditDrawObjectCells.Count != 0;

        protected override void OnCommit() {
            if(_createdDrawObjects.Count == 0 && _originReplacedDrawObjects.Count == 0) {
                return;
            }

            var activeLayer = CanvasContext.ActiveLayer;

            if(activeLayer == null) {
                return;
            }

            //还原并移除被裁剪的绘制对象原件;
            _originReplacedDrawObjects.ForEach(p => p.IsVisible = true);
            activeLayer.RemoveDrawObjects(_originReplacedDrawObjects);

            ///应用裁剪后的绘制对象,即<see cref="_createdDrawObjects"/>,将其加入活跃图层中;
            var copiedCreatedDrawObjects = _createdDrawObjects.ToArray();
            var copiedOriginReplacedDrawObjects = _originReplacedDrawObjects.ToArray();

           _createdDrawObjects.Clear();
           _originReplacedDrawObjects.Clear();

            activeLayer.AddDrawObjects(copiedCreatedDrawObjects);
            
            var transaction = new StandardEditTransaction(
                () => {
                    try {
                        activeLayer.RemoveDrawObjects(copiedCreatedDrawObjects);
                        activeLayer.AddDrawObjects(copiedOriginReplacedDrawObjects);
                    }
                    catch(Exception ex) {
                        RaiseErrorOccurred(ex);
                        //CommandOutputService.Current.WriteLine(ex.Message);
#if DEBUG
                        throw;
#endif
                    }
                },
                () => {
                    try {
                        activeLayer.AddDrawObjects(copiedCreatedDrawObjects);
                        activeLayer.RemoveDrawObjects(copiedOriginReplacedDrawObjects);
                    }
                    catch(Exception ex) {
                        RaiseErrorOccurred(ex);
                        //CommandOutputService.Current.WriteLine(ex.Message);
#if DEBUG
                        throw;
#endif
                    }
                }
            );

            CommitTransaction(transaction);
        }
        
        public override void Redo() {
            if (!CanRedo) {
                return;
            }

            var trimCell = _redoTrimEditDrawObjectCells.Pop();

            ApplyTrimCell(trimCell);

            _undoTrimEditDrawObjectCells.Push(trimCell);

            RaiseCanUndoRedoChanged();
        }

        public override void Undo() {
            if (!CanUndo) {
                return;
            }

            var trimCell = _undoTrimEditDrawObjectCells.Pop();

            UnApplyTrimCell(trimCell);

            _redoTrimEditDrawObjectCells.Push(trimCell);
            
            RaiseCanUndoRedoChanged();
        }

        protected override void OnCanvasContextChanged(ValueChangedEventArgs<ICanvasContextEx> args) {
            if (args.NewValue is ICanvasContextEx newCanvasContext) {
                newCanvasContext.DragSelect += OnDragSelect;
                newCanvasContext.DrawSelectMouseMove += OnDragSelectMouseMove;
                newCanvasContext.ClickSelect += CanvasContext_ClickSelect;
            }

            if (args.OldValue is ICanvasContextEx oldCanvasContext) {
                oldCanvasContext.DragSelect -= OnDragSelect;
                oldCanvasContext.DrawSelectMouseMove -= OnDragSelectMouseMove;
                oldCanvasContext.ClickSelect -= CanvasContext_ClickSelect;
            }

            base.OnCanvasContextChanged(args);
        }

        /// <summary>
        /// 指示为任意选中;
        /// </summary>
        /// <param name="canvasContext"></param>
        /// <param name="e"></param>
        private void OnDragSelectMouseMove(object sender,DragSelectMouseMoveEventArgs e) {
            if (e == null) {
                throw new ArgumentNullException(nameof(e));
            }

            e.IsAnyPoint = true;
        }

        /// <summary>
        /// 在拖放操作按下时,尝试进行裁剪操作;
        /// </summary>
        /// <param name="canvasContext"></param>
        /// <param name="e"></param>
        private void OnDragSelect(object sender, DragSelectEventArgs e) {
            if (e == null) {
                throw new ArgumentNullException(nameof(e));
            }
            
            //若Control键被按下,中止处理,指示外部可以进行拖放选中;
            if((CanvasContext.InputDevice.KeyBoard.ModifierKeys & ModifierKeys.Control) == ModifierKeys.Control) {
                return;
            }

            ///<see cref="ICanvasContextEx"/> 中被选中的绘制对象和
            ///<see cref="_createdDrawObjects"/>中被选中的绘制对象为裁剪标准;
            var originTrimingDrawObjects = CanvasContext.GetAllVisibleDrawObjects().Where(p => p.IsSelected);
            var createdTrimingDrawObjects = _createdDrawObjects.Where(p => p.IsSelected);

            var trimingDrawObjects = originTrimingDrawObjects.Union(createdTrimingDrawObjects).ToArray();


            ///下为原件以及<see cref="_createdDrawObjects"/>中将被裁剪的绘制对象;
            ///作为裁剪标准的绘制对象不能被裁剪;
            var drawObjectsToBeTrimedFromOrigin = e.HitedDrawObjects.
                Where(p => p.IsVisible).
                Where(p => !trimingDrawObjects.Contains(p));

            var drawObjectsToBeTrimedFromCreated = _createdDrawObjects.
                Where(p => p.ObjectInRectangle(e.Rectangle2D, CanvasContext.CanvasProxy,true)).
                Where(p => !trimingDrawObjects.Contains(p));


            var newCreatedDrawObjects = new List<DrawObject>();
            var replacedOriginDrawObjects = new List<DrawObject>();
            var replacedCreatedDrawObjects = new List<DrawObject>();

            ///对原件中被框选的绘制对象进行裁剪处理;
            foreach (var drawObjectToBeTrimedFromOrigin in drawObjectsToBeTrimedFromOrigin) {
                var trimedDrawObjects = GetTrimedOrExtendedDrawObjects(drawObjectToBeTrimedFromOrigin, trimingDrawObjects, e.Rectangle2D);
                //若trimedDrawObjects为空,则裁剪未能完成;
                if (trimedDrawObjects == null) {
                    continue;
                }

                newCreatedDrawObjects.AddRange(trimedDrawObjects);
                replacedOriginDrawObjects.Add(drawObjectToBeTrimedFromOrigin);
            }

            ///对<see cref="_createdDrawObjects"/>中框选的绘制对象进行裁剪处理;
            foreach (var drawObjectToBeTrimedFromCreated in drawObjectsToBeTrimedFromCreated) {
                var trimedDrawObjects = GetTrimedOrExtendedDrawObjects(drawObjectToBeTrimedFromCreated, trimingDrawObjects, e.Rectangle2D);
                //若trimedDrawObjects为空,则裁剪未能完成;
                if (trimedDrawObjects == null) {
                    continue;
                }

                newCreatedDrawObjects.AddRange(trimedDrawObjects);
                replacedCreatedDrawObjects.Add(drawObjectToBeTrimedFromCreated);
            }
            
            if(newCreatedDrawObjects.Count != 0) {
                var trimCell = new TrimEditDrawObjectCell {
                    NewCreatedDrawObjects = newCreatedDrawObjects.ToArray(),
                    ReplacedCreatedDrawObjects = replacedCreatedDrawObjects.ToArray(),
                    ReplacedOriginDrawObjects = replacedOriginDrawObjects.ToArray()
                };

                ApplyTrimCell(trimCell);

                _undoTrimEditDrawObjectCells.Push(trimCell);

                RaiseCanUndoRedoChanged();
            }
            
            //指示已处理;
            e.Cancel = true;
        }
        

        private void CanvasContext_ClickSelect(object sender, ClickSelectEventArgs e) {
            if(CanvasContext == null) {
                return;
            }

            if((CanvasContext.InputDevice.KeyBoard.ModifierKeys & ModifierKeys.Control) != ModifierKeys.Control) {
                e.Cancel = true;
            }
            
        }

        /// <summary>
        /// 在本次编辑栈内,应用<paramref name="trimEditDrawObjectCell"/>的更改内容;
        /// </summary>
        /// <param name="trimEditDrawObjectCell"></param>
        private void ApplyTrimCell(TrimEditDrawObjectCell trimEditDrawObjectCell) {

            if (trimEditDrawObjectCell == null) {
                throw new ArgumentNullException(nameof(trimEditDrawObjectCell));
            }

            foreach (var replacedOriginDrawObject in trimEditDrawObjectCell.ReplacedOriginDrawObjects) {
                replacedOriginDrawObject.IsVisible = false;
            }

            _createdDrawObjects.RemoveItems(trimEditDrawObjectCell.ReplacedCreatedDrawObjects);

            _createdDrawObjects.AddRange(trimEditDrawObjectCell.NewCreatedDrawObjects);
            _originReplacedDrawObjects.AddRange(trimEditDrawObjectCell.ReplacedOriginDrawObjects);

            RaiseVisualChanged();
        }

        /// <summary>
        /// 在本次编辑栈内,取消<paramref name="trimEditDrawObjectCell"/>的更改内容;
        /// </summary>
        /// <param name="trimEditDrawObjectCell"></param>
        private void UnApplyTrimCell(TrimEditDrawObjectCell trimEditDrawObjectCell) {
            if (trimEditDrawObjectCell == null) {
                throw new ArgumentNullException(nameof(trimEditDrawObjectCell));
            }

            foreach (var replacedOriginDrawObject in trimEditDrawObjectCell.ReplacedOriginDrawObjects) {
                replacedOriginDrawObject.IsVisible = true;
            }

            _createdDrawObjects.AddRange(trimEditDrawObjectCell.ReplacedCreatedDrawObjects);

            _createdDrawObjects.RemoveItems(trimEditDrawObjectCell.NewCreatedDrawObjects);
            _originReplacedDrawObjects.RemoveItems(trimEditDrawObjectCell.ReplacedOriginDrawObjects);

            RaiseVisualChanged();
        }
        
        protected override void OnEndOperation() {
            _originReplacedDrawObjects.Clear();
            _createdDrawObjects.Clear();

            _undoTrimEditDrawObjectCells.Clear();
            _redoTrimEditDrawObjectCells.Clear();
            
            base.OnEndOperation();
        }
        
        protected override void OnMouseDown(MouseDownEventArgs e) {
            
            if (e == null) {
                throw new ArgumentNullException(nameof(e));
            }

            //需指定为左键;
            if (e.Button != MouseButton.Left) {
                return;
            }

            ///若左键和Control键被按下,对<see cref="_createdDrawObjects"/>进行选择交换操作;
            if ((CanvasContext.InputDevice.KeyBoard?.ModifierKeys & ModifierKeys.Control) == ModifierKeys.Control) {
                UpdateDrawObjectSelectedState(e.Position, CanvasContext);
                //使得外部继续执行队列;
                return;
            }
            
            RaiseVisualChanged();

            base.OnMouseDown(e);
        }
        
        /// <summary>
        /// 根据裁剪对象<paramref name="trimingDrawObjects"/>和裁剪区域<paramref name="trimArea"/>,
        /// 对被裁剪对象<paramref name="toBeTrimedDrawObject"/>进行裁剪,
        /// 返回裁剪后的对象;
        /// </summary>
        /// <param name="canvasContext"></param>
        /// <param name="trimArea"></param>
        private DrawObject[] GetTrimedOrExtendedDrawObjects(
            DrawObject toBeTrimedDrawObject,
            IEnumerable<DrawObject> trimingDrawObjects,
            Rectangle2D2 trimArea
        ) {
            
            if (toBeTrimedDrawObject == null) {
                throw new ArgumentNullException(nameof(toBeTrimedDrawObject));
            }
            
            if (trimingDrawObjects == null) {
                throw new ArgumentNullException(nameof(trimingDrawObjects));
            }

            ///得到所有被裁剪绘制对象与裁剪对象的交点;
            var thisIntersectPositions = trimingDrawObjects.SelectMany(
                p => DrawObjectIntersectRules.SelectMany(
                    q => q.GetIntersectPositions(toBeTrimedDrawObject, p, IsExtendMode) ?? Enumerable.Empty<Vector2D>()
                )
            ).ToArray();

            if (thisIntersectPositions.Length == 0) {
                return null;
            }

            DrawObject[] thisCreatedDrawObjects = null;

            //若非延伸模式,则进行裁剪计算;
            if (!IsExtendMode) {
                //找到对应的裁剪工具;
                var trimTool = DrawObjectTrimTools.FirstOrDefault(p => p.CheckIsValidDrawObject(toBeTrimedDrawObject));

                if (trimTool == null) {
                    return null;
                }

                //进行裁剪;
                thisCreatedDrawObjects = trimTool.TrimDrawObject(
                    new DrawObjectTrimingInfo {
                        TrimedDrawObject = toBeTrimedDrawObject,
                        TrimingDrawObjects = trimingDrawObjects.ToArray(),
                        IntersectPositions = thisIntersectPositions,
                        TrimArea = trimArea
                    }
                );
            }
            //否则延伸计算;
            else {
                var extendTool = DrawObjectExtendTools.FirstOrDefault(p => p.CheckIsValidDrawObject(toBeTrimedDrawObject));

                if(extendTool == null) {
                    return null;
                }

                //进行延伸;
                var createdDrawObject = extendTool.ExtendDrawObject(
                    new DrawObjectExtendInfo {
                        ExtendedDrawObject = toBeTrimedDrawObject,
                        ExtendingDrawObjects = trimingDrawObjects.ToArray(),
                        ExtendArea = trimArea,
                        IntersectPositions = thisIntersectPositions
                    }
                );

                if(createdDrawObject == null) {
                    return null;
                }

                thisCreatedDrawObjects = new DrawObject[] { createdDrawObject };
            }
            
            //返回裁剪后的绘制对象;
            return thisCreatedDrawObjects;
        }

        

        /// <summary>
        /// 根据鼠标按下的位置,更新"裁剪后"的绘制对象(非原件)的选中状态;
        /// </summary>
        private void UpdateDrawObjectSelectedState(Vector2D mouseDownPosition, ICanvasContextEx canvasContext) {
            if (mouseDownPosition == null) {
                return;
            }

            _createdDrawObjects.ForEach(p => {
                if (p.PointInObject(mouseDownPosition, canvasContext.CanvasProxy)) {
                    p.IsSelected = !p.IsSelected;
                }
            });


        }

        public override void Draw(ICanvas canvas, ICanvasScreenConvertable canvasProxy) {
            _createdDrawObjects.ForEach(p => p.Draw(canvas, canvasProxy));
            base.Draw(canvas, canvasProxy);
        }

        
    }

    public partial class TrimEditTool {
        /// <summary>
        /// 本单位用于记录单次裁剪时的所使用的关键信息;
        /// </summary>
        class TrimEditDrawObjectCell {
            /// <summary>
            /// 单次裁剪所得到的新的绘制对象;
            /// </summary>
            public DrawObject[] NewCreatedDrawObjects { get; set; }

            /// <summary>
            /// 单次裁剪将被替换的绘制对象原件;
            /// 来自与原图层中现存的;
            /// </summary>
            public DrawObject[] ReplacedOriginDrawObjects { get; set; }

            /// <summary>
            /// 单次裁剪所被替换的新得到的绘制对象;
            /// 这些绘制对象将来自于单次裁剪之前的裁剪得到的绘制对象<see cref="_createdDrawObjects"/>
            /// </summary>
            public DrawObject[] ReplacedCreatedDrawObjects { get; set; }
        }
    }

    
}
