using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Tida.Canvas.Shell.Contracts.Controls {
    public static class GridExtensions {
        [AttachedPropertyBrowsableForType(typeof(Grid))]
        public static bool GetIsTable(DependencyObject obj) {
            return (bool)obj.GetValue(IsTableProperty);
        }

        public static void SetIsTable(DependencyObject obj, bool value) {
            obj.SetValue(IsTableProperty, value);
        }

        private static IEnumerable<Separator> GetGridLines(DependencyObject obj) {
            return (IEnumerable<Separator>)obj.GetValue(HorizontalGridLinesProperty);
        }

        private static void SetGridLines(DependencyObject obj, IEnumerable<Separator> value) {
            obj.SetValue(HorizontalGridLinesProperty, value);
        }

        private static readonly DependencyProperty HorizontalGridLinesProperty =
            DependencyProperty.RegisterAttached("GridLines", typeof(IEnumerable<Separator>), typeof(GridExtensions), new PropertyMetadata(null));



        public static readonly DependencyProperty IsTableProperty =
            DependencyProperty.RegisterAttached("IsTable", typeof(bool), typeof(GridExtensions), new PropertyMetadata(false, IsTable_PropertyChanged));

        private static void IsTable_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            if (!(d is Grid grid)) {
                return;
            }

            var isTable = (bool)e.NewValue;
            if (isTable) {
                grid.Loaded += Grid_Loaded;
                grid.Unloaded += Grid_UnLoaded;
            }
            else {
                grid.Loaded -= Grid_Loaded;
                grid.Unloaded -= Grid_UnLoaded;
            }


        }

        private static void Grid_UnLoaded(object sender, RoutedEventArgs e) {

        }

        private static void Grid_Loaded(object sender, RoutedEventArgs e) {
            if (!(sender is Grid grid)) {
                return;
            }

            if (grid.RowDefinitions.Count == 0) {
                return;
            }

            var oldGridLines = GetGridLines(grid);
            if (oldGridLines != null) {
                foreach (var hLine in oldGridLines) {
                    grid.Children.Remove(hLine);
                }
            }
            SetGridLines(grid, null);

            var isTable = GetIsTable(grid);
            var columnCount = grid.ColumnDefinitions.Count;
            var rowCount = grid.RowDefinitions.Count;

            if (isTable) {
                var newHorizontalLines = new Separator[rowCount];
                for (int i = 0; i < rowCount; i++) {
                    var seperator = new Separator {
                        Height = 2,
                        VerticalAlignment = VerticalAlignment.Bottom,
                        Background = Brushes.Black
                    };
                    Grid.SetRow(seperator, i);

                    if (columnCount > 0) {
                        Grid.SetColumnSpan(seperator, columnCount);
                    }

                    newHorizontalLines[i] = seperator;
                }

                foreach (var hLine in newHorizontalLines) {
                    grid.Children.Add(hLine);
                }

                SetGridLines(grid, newHorizontalLines);
            }
        }
    }
}
