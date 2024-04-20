using Tida.Canvas.Infrastructure.NativePresentation;
using Tida.Canvas.Infrastructure.OffsetTools;
using Tida.Canvas.Contracts;
using Tida.Canvas.Input;
using Tida.Geometry.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tida.Canvas.Infrastructure.EditTools {
    /// <summary>
    /// 编辑工具-偏移;
    /// </summary>
    public partial class OffsetEditTool :  EditTool {
        public OffsetEditTool(INumberBoxService numberBoxService,IDrawObjectSelector drawObjectSelector) {

            if (numberBoxService == null) {
                throw new ArgumentNullException(nameof(numberBoxService));
            }


            _drawObjectSelector = drawObjectSelector ?? throw new ArgumentNullException(nameof(drawObjectSelector));

            _numberBoxContainer = numberBoxService.CreateContainer();
            _offsetNumberBox = numberBoxService.CreateNumberBox();

            _numberBoxContainer.AddNumberBox(_offsetNumberBox);

            _offsetNumberBox.EnterConfirmed += OffsetNumberBox_EnterConfirmed;
        }

        public static readonly List<IDrawObjectOffsetTool> DrawObjectOffsetTools = new List<IDrawObjectOffsetTool>();
        private readonly IDrawObjectSelector _drawObjectSelector;

        /// <summary>
        /// 偏移大小已确认;
        /// </summary>
        public event EventHandler FixedOffsetChanged;

        /// <summary>
        /// 选中了绘制对象的事件;
        /// </summary>
        public event EventHandler DrawObjectSelected;

        /// <summary>
        /// 偏移已经应用事件;
        /// </summary>
        public event EventHandler OffsetApplied;

        /// <summary>
        /// 已经确定的偏移大小;
        /// </summary>
        public double? FixedOffset {
            get => _fixedOffset;
            private set {
                if(_fixedOffset == value) {
                    return;
                }

                _fixedOffset = value;
                FixedOffsetChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private double? _fixedOffset;
      
        private readonly INumberBoxContainer _numberBoxContainer;
        private readonly INumberBox _offsetNumberBox;

        /// <summary>
        /// 被选中,将要被偏移的记录;
        /// </summary>
        private OffsetEditCell ClickedOffsetEditCell {
            get => _clickedOffsetEditCell;
            set {
                if(_clickedOffsetEditCell == value) {
                    return;
                }

                _clickedOffsetEditCell = value;
                if(_clickedOffsetEditCell == null) {
                    return;
                }

                DrawObjectSelected?.Invoke(this, EventArgs.Empty);
            }
        }
        private OffsetEditCell _clickedOffsetEditCell;
        

        /// <summary>
        /// 本次编辑过程中,偏移过程记录的撤销栈;
        /// </summary>
        private readonly Stack<OffsetEditCell> _undoOffsetCells = new Stack<OffsetEditCell>();

        /// <summary>
        /// 本次编辑过程中,偏移过程记录的重做栈;
        /// </summary>
        private readonly Stack<OffsetEditCell> _redoOffsetCells = new Stack<OffsetEditCell>();
        
        private void OffsetNumberBox_EnterConfirmed(object sender, EventArgs e) {
            if (_offsetNumberBox.Number == null) {
                return;
            }

            if (FixedOffset != null) {
                return;
            }

            FixedOffset = _offsetNumberBox.Number.Value;
            _offsetNumberBox.IsReadOnly = true;
        }

        public override bool CanUndo => _undoOffsetCells.Count != 0;

        public override bool CanRedo => _redoOffsetCells.Count != 0;

        protected override void OnCommit() {
            if(_undoOffsetCells.Count == 0) {
                return;
            }

            var undoOffsetCellsArr = _undoOffsetCells.ToArray();
            _undoOffsetCells.Clear();

            void Undo() {
                foreach (var cell in undoOffsetCellsArr) {
                    if(!(cell.CopiedDrawObject.Parent is CanvasLayer layer)) {
                        continue;
                    }

                    layer.RemoveDrawObject(cell.CopiedDrawObject);
                }
            }

            void Redo() {
                foreach (var cell in undoOffsetCellsArr) {
                    if (!(cell.OriginDrawObject.Parent is CanvasLayer layer)) {
                        continue;
                    }

                    layer.AddDrawObject(cell.CopiedDrawObject);
                }
            }

            var transaction = new StandardEditTransaction(Undo, Redo);
            CommitTransaction(transaction);
        }

        public override void Redo() {
            if (!CanRedo) {
                return;
            }

            var cell = _redoOffsetCells.Pop();
            if (!(cell.OriginDrawObject.Parent is CanvasLayer layer)) {
                return;
            }
            
            layer.AddDrawObject(cell.CopiedDrawObject);

            _undoOffsetCells.Push(cell);

            RaiseCanUndoRedoChanged();
        }

        public override void Undo() {
            if (!CanUndo) {
                return;
            }

            var cell = _undoOffsetCells.Pop();
            if (!(cell.CopiedDrawObject.Parent is CanvasLayer layer)) {
                return;
            }

            layer.RemoveDrawObject(cell.CopiedDrawObject);

            _redoOffsetCells.Push(cell);

            RaiseCanUndoRedoChanged();
        }

        protected override void OnBeginOperation() {
            base.OnBeginOperation();
            if(CanvasContext == null) {
                return;
            }

            CanvasContext.AddUIObject(_numberBoxContainer.UIObject);
            _numberBoxContainer.Reset();
            _offsetNumberBox.Number = 0;

            var drawObjects = CanvasContext.GetAllDrawObjects();

            foreach (var drawObject in drawObjects) {
                drawObject.IsSelected = false;
            }

            var nativePosition = CanvasContext.InputDevice.Mouse.GetNativePosition();
            _offsetNumberBox.Position = nativePosition;
        }

        protected override void OnEndOperation() {
            base.OnEndOperation();

            CanvasContext?.RemoveUIObject(_numberBoxContainer.UIObject);

            FixedOffset = null;

            _redoOffsetCells.Clear();
            _undoOffsetCells.Clear();

            ///将未最终应用的<see cref="ClickedOffsetEditCell"/>的副本从父图层中移除;
            if(ClickedOffsetEditCell != null && ClickedOffsetEditCell.CopiedDrawObject.Parent is CanvasLayer layer) {
                layer.RemoveDrawObject(ClickedOffsetEditCell.CopiedDrawObject);
            }
            ClickedOffsetEditCell = null;
        }

        protected override void OnMouseDown(MouseDownEventArgs e) {
            base.OnMouseDown(e);

            ///指示已处理,防止外部进行选中操作;
            e.Handled = true;

            if (CanvasContext == null) {
                return;
            }

            ///若<see cref="FixedOffset"/>为空,则尚未确定偏移量;
            if(FixedOffset == null) {
                return;
            }
            
            ///须为左键;
            if(e.Button != MouseButton.Left) {
                return;
            }

            ///<see cref="ClickedOffsetEditCell"/>为空,则为第一次按下鼠标;
            if (ClickedOffsetEditCell == null) {
                BuildClickedCellOffsetWithMousePosition(e.Position);
            }
            ///否则为第二次按下鼠标,确认更改;
            else {
                ApplyClickedCell();
            }

            RaiseCanUndoRedoChanged();
        }

        /// <summary>
        /// 在本次编辑栈内,生成当前的即将偏移的记录(即<see cref="ClickedOffsetEditCell"/>);
        /// </summary>
        protected void BuildClickedCellOffsetWithMousePosition(Vector2D clickedPosition) {
            if(ClickedOffsetEditCell != null) {
                return;
            }

            if(FixedOffset == null) {
                return;
            }

            ///找到点击后的唯一一个绘制对象;
            var drawObjects = CanvasContext.GetAllVisibleDrawObjects();
            var clickedObjects = drawObjects.
                //收集所有击中的绘制对象;
                Where(p => p.PointInObject(clickedPosition, CanvasContext.CanvasProxy)).
                //过滤不能支持偏移的绘制对象;
                Where(p => DrawObjectOffsetTools.Any(q => q.CheckDrawObjectMoveable(p)));

            DrawObject originDrawObject = null;

            var clickedObjectCount = clickedObjects.Count();
            
            //若只存在一个击中对象,预计对该对象进行偏移;
            if (clickedObjectCount == 1) {
                originDrawObject = clickedObjects.First();
            }
            else if(clickedObjectCount > 1){
                originDrawObject = _drawObjectSelector.SelectOneDrawObject(clickedObjects);
            }

            if(originDrawObject == null) {
                return;
            }
            
            var copiedDrawObject = originDrawObject.Clone();
            if (copiedDrawObject == null) {
                return;
            }

            var offsetTool = DrawObjectOffsetTools.FirstOrDefault(p => p.CheckDrawObjectMoveable(copiedDrawObject));

            if (offsetTool == null) {
                return;
            }
            
            offsetTool.MoveOffset(copiedDrawObject, FixedOffset.Value, clickedPosition);

            ClickedOffsetEditCell = new OffsetEditCell {
                OriginDrawObject = originDrawObject,
                CopiedDrawObject = copiedDrawObject,
                DrawObjectOffsetTool = offsetTool,
                Offset = FixedOffset.Value
            };

            if (originDrawObject.Parent is CanvasLayer layer) {
                layer.AddDrawObject(copiedDrawObject);
                originDrawObject.IsSelected = true;
            }
        }

        /// <summary>
        /// 在本次编辑栈内,应用当前的选中的记录;
        /// </summary>
        private void ApplyClickedCell() {
            if(ClickedOffsetEditCell == null) {
                return;
            }
            
            _undoOffsetCells.Push(ClickedOffsetEditCell);
            _redoOffsetCells.Clear();

            ClickedOffsetEditCell.OriginDrawObject.IsSelected = false;

            OffsetApplied?.Invoke(this, EventArgs.Empty);

            ClickedOffsetEditCell = null;
            FixedOffset = null;
            _offsetNumberBox.IsReadOnly = false;          
        }

        protected override void OnMouseMove(MouseMoveEventArgs e) {
            base.OnMouseMove(e);

            if (CanvasContext == null) {
                return;
            }

            _offsetNumberBox.Position = CanvasContext.CanvasProxy.ToScreen(e.Position);

            if (ClickedOffsetEditCell == null) {
                return;
            }

            if(FixedOffset == null) {
                return;
            }

            if (ClickedOffsetEditCell.CopiedDrawObject.Parent is CanvasLayer layer) {
                layer.RemoveDrawObject(ClickedOffsetEditCell.CopiedDrawObject);

                var newCopiedObject = ClickedOffsetEditCell.OriginDrawObject.Clone();
                ClickedOffsetEditCell.DrawObjectOffsetTool.MoveOffset(newCopiedObject, FixedOffset.Value, e.Position);
                ClickedOffsetEditCell.CopiedDrawObject = newCopiedObject;

                layer.AddDrawObject(newCopiedObject);
            }

            RaiseVisualChanged();
        }
    }

    public partial class OffsetEditTool {
        /// <summary>
        /// 偏移过程中的步骤记录;
        /// </summary>
        class OffsetEditCell {
            /// <summary>
            /// 原件;
            /// </summary>
            public DrawObject OriginDrawObject { get; set; }

            /// <summary>
            /// 偏移后的绘制对象;
            /// </summary>
            public DrawObject CopiedDrawObject { get; set; }

            /// <summary>
            /// 对应的偏移工具;
            /// </summary>
            public IDrawObjectOffsetTool DrawObjectOffsetTool { get; set; }

            /// <summary>
            /// 偏移量;
            /// </summary>
            public double Offset { get; set; }
        }
    }

    /// <summary>
    /// 编辑工具-偏移;
    /// </summary>
    public partial class OffsetEditTool2 : EditTool {
        public OffsetEditTool2(INumberBoxService numberBoxService,IDrawObjectSelector drawObjectSelector) {
            if (numberBoxService == null) {
                throw new ArgumentNullException(nameof(numberBoxService));
            }
            
            _drawObjectSelector = drawObjectSelector ?? throw new ArgumentNullException(nameof(drawObjectSelector));
            
            _numberBoxContainer = numberBoxService.CreateContainer();
            _offsetNumberBox = numberBoxService.CreateNumberBox();
            _numberBoxContainer.AddNumberBox(_offsetNumberBox);
            _offsetNumberBox.EnterConfirmed += OffsetNumberBox_EnterConfirmed;
        }

        public static readonly List<IDrawObjectOffsetTool> DrawObjectOffsetTools = new List<IDrawObjectOffsetTool>();
        private readonly IDrawObjectSelector _drawObjectSelector;

        /// <summary>
        /// 偏移大小已确认;
        /// </summary>
        public event EventHandler FixedOffsetChanged;

        /// <summary>
        /// 选中了绘制对象的事件;
        /// </summary>
        public event EventHandler DrawObjectSelected;

        /// <summary>
        /// 偏移已经应用事件;
        /// </summary>
        public event EventHandler OffsetApplied;

        /// <summary>
        /// 已经确定的偏移大小;
        /// </summary>
        public double? FixedOffset {
            get => _fixedOffset;
            private set {
                if (_fixedOffset == value) {
                    return;
                }

                _fixedOffset = value;
                FixedOffsetChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private double? _fixedOffset;

        private readonly INumberBoxContainer _numberBoxContainer;
        private readonly INumberBox _offsetNumberBox;

        /// <summary>
        /// 被选中,将要被偏移的记录;
        /// </summary>
        private OffsetEditCell ClickedOffsetEditCell {
            get => _clickedOffsetEditCell;
            set {
                if (_clickedOffsetEditCell == value) {
                    return;
                }

                _clickedOffsetEditCell = value;
                if (_clickedOffsetEditCell == null) {
                    return;
                }

                DrawObjectSelected?.Invoke(this, EventArgs.Empty);
            }
        }
        private OffsetEditCell _clickedOffsetEditCell;


        /// <summary>
        /// 本次编辑过程中,偏移过程记录的撤销栈;
        /// </summary>
        private readonly Stack<OffsetEditCell> _undoOffsetCells = new Stack<OffsetEditCell>();

        /// <summary>
        /// 本次编辑过程中,偏移过程记录的重做栈;
        /// </summary>
        private readonly Stack<OffsetEditCell> _redoOffsetCells = new Stack<OffsetEditCell>();

        private void OffsetNumberBox_EnterConfirmed(object sender, EventArgs e) {
            if (_offsetNumberBox.Number == null) {
                return;
            }

            if (FixedOffset != null) {
                return;
            }

            FixedOffset = _offsetNumberBox.Number.Value;
            _offsetNumberBox.IsReadOnly = true;
        }

        public override bool CanUndo => _undoOffsetCells.Count != 0;

        public override bool CanRedo => _redoOffsetCells.Count != 0;

        protected override void OnCommit() {
            if (_undoOffsetCells.Count == 0) {
                return;
            }

            var undoOffsetCellsArr = _undoOffsetCells.ToArray();
            _undoOffsetCells.Clear();

            void Undo() {
                foreach (var cell in undoOffsetCellsArr) {
                    if (!(cell.CopiedDrawObject.Parent is CanvasLayer layer)) {
                        continue;
                    }

                    layer.RemoveDrawObject(cell.CopiedDrawObject);
                }
            }

            void Redo() {
                foreach (var cell in undoOffsetCellsArr) {
                    if (!(cell.OriginDrawObject.Parent is CanvasLayer layer)) {
                        continue;
                    }

                    layer.AddDrawObject(cell.CopiedDrawObject);
                }
            }

            var transaction = new StandardEditTransaction(Undo, Redo);
            CommitTransaction(transaction);
        }

        public override void Redo() {
            if (!CanRedo) {
                return;
            }

            var cell = _redoOffsetCells.Pop();
            if (!(cell.OriginDrawObject.Parent is CanvasLayer layer)) {
                return;
            }

            layer.AddDrawObject(cell.CopiedDrawObject);

            _undoOffsetCells.Push(cell);

            RaiseCanUndoRedoChanged();
        }

        public override void Undo() {
            if (!CanUndo) {
                return;
            }

            var cell = _undoOffsetCells.Pop();
            if (!(cell.CopiedDrawObject.Parent is CanvasLayer layer)) {
                return;
            }

            layer.RemoveDrawObject(cell.CopiedDrawObject);

            _redoOffsetCells.Push(cell);

            RaiseCanUndoRedoChanged();
        }

        protected override void OnBeginOperation() {
            base.OnBeginOperation();
            if (CanvasContext == null) {
                return;
            }

            CanvasContext.AddUIObject(_numberBoxContainer.UIObject);
            _numberBoxContainer.Reset();
            _offsetNumberBox.Number = 0;

            var drawObjects = CanvasContext.GetAllDrawObjects();

            foreach (var drawObject in drawObjects) {
                drawObject.IsSelected = false;
            }

            var nativePosition = CanvasContext.InputDevice.Mouse.GetNativePosition();
            _offsetNumberBox.Position = nativePosition;
        }

        protected override void OnEndOperation() {
            base.OnEndOperation();

            CanvasContext?.RemoveUIObject(_numberBoxContainer.UIObject);

            FixedOffset = null;

            _redoOffsetCells.Clear();
            _undoOffsetCells.Clear();

            ///将未最终应用的<see cref="ClickedOffsetEditCell"/>的副本从父图层中移除;
            if (ClickedOffsetEditCell != null && ClickedOffsetEditCell.CopiedDrawObject.Parent is CanvasLayer layer) {
                layer.RemoveDrawObject(ClickedOffsetEditCell.CopiedDrawObject);
            }
            ClickedOffsetEditCell = null;
        }

        protected override void OnMouseDown(MouseDownEventArgs e) {
            base.OnMouseDown(e);

            ///指示已处理,防止外部进行选中操作;
            e.Handled = true;

            if (CanvasContext == null) {
                return;
            }

            ///若<see cref="FixedOffset"/>为空,则尚未确定偏移量;
            if (FixedOffset == null) {
                return;
            }

            ///须为左键;
            if (e.Button != MouseButton.Left) {
                return;
            }

            ///<see cref="ClickedOffsetEditCell"/>为空,则为第一次按下鼠标;
            if (ClickedOffsetEditCell == null) {
                BuildClickedCellOffsetWithMousePosition(e.Position);
            }
            ///否则为第二次按下鼠标,确认更改;
            else {
                ApplyClickedCell();
            }

            RaiseCanUndoRedoChanged();
        }

        /// <summary>
        /// 在本次编辑栈内,生成当前的即将偏移的记录(即<see cref="ClickedOffsetEditCell"/>);
        /// </summary>
        protected void BuildClickedCellOffsetWithMousePosition(Vector2D clickedPosition) {
            if (ClickedOffsetEditCell != null) {
                return;
            }

            if (FixedOffset == null) {
                return;
            }

            ///找到点击后的唯一一个绘制对象;
            var drawObjects = CanvasContext.GetAllVisibleDrawObjects();
            var clickedObjects = drawObjects.
                //收集所有击中的绘制对象;
                Where(p => p.PointInObject(clickedPosition, CanvasContext.CanvasProxy)).
                //过滤不能支持偏移的绘制对象;
                Where(p => DrawObjectOffsetTools.Any(q => q.CheckDrawObjectMoveable(p)));

            DrawObject originDrawObject = null;

            var clickedObjectCount = clickedObjects.Count();

            //若只存在一个击中对象,预计对该对象进行偏移;
            if (clickedObjectCount == 1) {
                originDrawObject = clickedObjects.First();
            }
            else if (clickedObjectCount > 1) {
                originDrawObject = _drawObjectSelector.SelectOneDrawObject(clickedObjects);
            }

            if (originDrawObject == null) {
                return;
            }

            var copiedDrawObject = originDrawObject.Clone();
            if (copiedDrawObject == null) {
                return;
            }

            var offsetTool = DrawObjectOffsetTools.FirstOrDefault(p => p.CheckDrawObjectMoveable(copiedDrawObject));

            if (offsetTool == null) {
                return;
            }

            offsetTool.MoveOffset(copiedDrawObject, FixedOffset.Value, clickedPosition);

            ClickedOffsetEditCell = new OffsetEditCell {
                OriginDrawObject = originDrawObject,
                CopiedDrawObject = copiedDrawObject,
                DrawObjectOffsetTool = offsetTool,
                Offset = FixedOffset.Value
            };

            if (originDrawObject.Parent is CanvasLayer layer) {
                layer.AddDrawObject(copiedDrawObject);
                originDrawObject.IsSelected = true;
            }
        }

        /// <summary>
        /// 在本次编辑栈内,应用当前的选中的记录;
        /// </summary>
        private void ApplyClickedCell() {
            if (ClickedOffsetEditCell == null) {
                return;
            }

            _undoOffsetCells.Push(ClickedOffsetEditCell);
            _redoOffsetCells.Clear();

            ClickedOffsetEditCell.OriginDrawObject.IsSelected = false;

            OffsetApplied?.Invoke(this, EventArgs.Empty);

            ClickedOffsetEditCell = null;

            //FixedOffset = null;
            //_offsetNumberBox.IsReadOnly = false;
        }

        protected override void OnMouseMove(MouseMoveEventArgs e) {
            base.OnMouseMove(e);

            if (CanvasContext == null) {
                return;
            }

            _offsetNumberBox.Position = CanvasContext.CanvasProxy.ToScreen(e.Position);

            if (ClickedOffsetEditCell == null) {
                return;
            }

            if (FixedOffset == null) {
                return;
            }

            if (ClickedOffsetEditCell.CopiedDrawObject.Parent is CanvasLayer layer) {
                layer.RemoveDrawObject(ClickedOffsetEditCell.CopiedDrawObject);

                var newCopiedObject = ClickedOffsetEditCell.OriginDrawObject.Clone();
                ClickedOffsetEditCell.DrawObjectOffsetTool.MoveOffset(newCopiedObject, FixedOffset.Value, e.Position);
                ClickedOffsetEditCell.CopiedDrawObject = newCopiedObject;

                layer.AddDrawObject(newCopiedObject);
            }

            RaiseVisualChanged();
        }
    }

    public partial class OffsetEditTool2 {
        /// <summary>
        /// 偏移过程中的步骤记录;
        /// </summary>
        class OffsetEditCell {
            /// <summary>
            /// 原件;
            /// </summary>
            public DrawObject OriginDrawObject { get; set; }

            /// <summary>
            /// 偏移后的绘制对象;
            /// </summary>
            public DrawObject CopiedDrawObject { get; set; }

            /// <summary>
            /// 对应的偏移工具;
            /// </summary>
            public IDrawObjectOffsetTool DrawObjectOffsetTool { get; set; }

            /// <summary>
            /// 偏移量;
            /// </summary>
            public double Offset { get; set; }
        }
    }
}
