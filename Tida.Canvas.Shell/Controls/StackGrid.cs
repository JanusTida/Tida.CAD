using Tida.Canvas.Shell.Contracts.Common;
using Tida.Canvas.Shell.Contracts.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Tida.Canvas.Shell.Controls {
    class StackGrid<TStackItem> : IStackGrid<TStackItem> where TStackItem : class,IUIObjectProvider {
        public StackGrid(Grid grid = null) {
            this._grid = grid ?? new Grid();
        }
       
        /// <summary>
        /// StackGrid内用于存储<see cref="TStackItem"/>相关的状态的单元;
        /// </summary>
        private class StackItemCell {
            /// <summary>
            /// 本体;
            /// </summary>
            public TStackItem StackItem { get; set; }
            /// <summary>
            /// 网格长度;
            /// </summary>
            public GridChildLength GridChildLength { get; set; }
            /// <summary>
            /// 排序;
            /// </summary>
            public int Order { get; set; }
        }

        private readonly List<StackItemCell> _children = new List<StackItemCell>();
        public IEnumerable<TStackItem> Children => _children.Select(p => p.StackItem);
        
        private Orientation _orientation;
        public Orientation Orientation {
            get { return _orientation; }
            set {
                if (_orientation != value) {
                    _orientation = value;
                    UpdateGrid();
                }
            }
        }

            public object UIObject => _grid;
        private Grid _grid;

        public bool NeedSplitter { get; set; }

        public void AddChild(TStackItem child, GridChildLength gridChildLength, int index = -1) {
            if (index < 0) {
                index = 0;
            }

            _children.Add(new StackItemCell {
                StackItem = child,
                GridChildLength = gridChildLength,
                Order = index
            });

            UpdateGrid();
        }

        public void Remove(TStackItem child) {
            StackItemCell item = null;
            foreach (var ch in _children) {
                if (ch.StackItem == child) {
                    item = ch;
                    break;
                }
            }

            if (item != null) {
                _children.Remove(item);
                UpdateGrid();
            }

        }

        public double SplitterLength {
            get { return _splitterLength; }
            set {
                if (_splitterLength != value) {
                    _splitterLength = value;
                    UpdateGrid();
                }
            }
        }
        double _splitterLength;

        private void UpdateGrid() {
            _grid.Children.Clear();
            _grid.ColumnDefinitions.Clear();
            _grid.RowDefinitions.Clear();

            var needSplitter = false;
            double d = 0.05;
            int rowCol = 0;



            // Make sure the horizontal grid splitters can resize the content
            if (Orientation == Orientation.Vertical) {
                _grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                foreach (var child in _children.OrderBy(p => p.Order)) {
                    if (needSplitter && !child.GridChildLength.GridLength.IsAuto) {
                        var gridSplitter = GetSplitter();
                        Panel.SetZIndex(gridSplitter, 1);
                        _grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(_splitterLength, GridUnitType.Pixel) });
                        gridSplitter.SetValue(Grid.RowProperty, rowCol);
                        gridSplitter.Margin = new Thickness(0, -5, 0, -5);
                        gridSplitter.BorderThickness = new Thickness(0, 5, 0, 5);
                        gridSplitter.HorizontalAlignment = HorizontalAlignment.Stretch;
                        gridSplitter.VerticalAlignment = VerticalAlignment.Center;
                        rowCol++;
                    }

                    var rowDef = new RowDefinition() { Height = GetGridLength(child.GridChildLength.GridLength, -d) };
                    rowDef.MaxHeight = child.GridChildLength.MaxLength;
                    rowDef.MinHeight = child.GridChildLength.MinLength;

                    _grid.RowDefinitions.Add(rowDef);

                    var uiel = GetUIElement(child.StackItem);
                    uiel.SetValue(Grid.RowProperty, rowCol);
                    uiel.ClearValue(Grid.ColumnProperty);

                    rowCol++;
                    d = -d;
                    needSplitter = !child.GridChildLength.GridLength.IsAuto;
                }
            }
            else {
                _grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });

                foreach (var child in _children.OrderBy(p => p.Order)) {
                    if (needSplitter && !child.GridChildLength.GridLength.IsAuto) {
                        var gridSplitter = GetSplitter();
                        Panel.SetZIndex(gridSplitter, 1);
                        _grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(_splitterLength, GridUnitType.Pixel) });
                        gridSplitter.SetValue(Grid.ColumnProperty, rowCol);
                        gridSplitter.Margin = new Thickness(-5, 0, -5, 0);
                        gridSplitter.BorderThickness = new Thickness(5, 0, 5, 0);
                        gridSplitter.HorizontalAlignment = HorizontalAlignment.Center;
                        gridSplitter.VerticalAlignment = VerticalAlignment.Stretch;

                        rowCol++;
                    }

                    var colDef = new ColumnDefinition() { Width = GetGridLength(child.GridChildLength.GridLength, -d) };
                    colDef.MaxWidth = child.GridChildLength.MaxLength;
                    colDef.MinWidth = child.GridChildLength.MinLength;
                    _grid.ColumnDefinitions.Add(colDef);

                    var uiel = GetUIElement(child.StackItem);
                    uiel.ClearValue(Grid.RowProperty);
                    uiel.SetValue(Grid.ColumnProperty, rowCol);

                    rowCol++;
                    d = -d;
                    needSplitter = !child.GridChildLength.GridLength.IsAuto;
                }
            }
        }

        private GridLength GetGridLength(GridLength len, double d) {
            if (len.IsStar && len.Value == 1)
                return new GridLength(1 + d, GridUnitType.Star);
            return len;
        }

        private GridSplitter GetSplitter() {
            var gridSplitter = new GridSplitter();
            gridSplitter.BorderBrush = Brushes.Transparent;
            gridSplitter.Focusable = false;
            _grid.Children.Add(gridSplitter);
            return gridSplitter;
        }

        private UIElement GetUIElement(TStackItem child) {
            var obj = child.UIObject;
            var uiel = obj as UIElement;
            if (uiel == null)
                uiel = new ContentPresenter { Content = obj };

            _grid.Children.Add(uiel);
            return uiel;
        }

        public void Clear() {
            _children.Clear();
            UpdateGrid();
        }
    }

    
}
