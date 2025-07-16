#if WPF
global using Point = System.Windows.Point;
global using Vector = System.Windows.Vector;
global using Brush = System.Windows.Media.Brush;
global using IInputElement = System.Windows.IInputElement;
global using RoutedEventArgs = System.Windows.RoutedEventArgs;
global using DrawingContext = System.Windows.Media.DrawingContext;
global using Pen = System.Windows.Media.Pen;
global using FormattedText = System.Windows.Media.FormattedText;
global using Brushes = System.Windows.Media.Brushes;
global using Rect = System.Windows.Rect;
global using Size = System.Windows.Size;
global using TextInputEventArgs = System.Windows.Input.TextCompositionEventArgs;
#elif Avalonia
global using Point = Avalonia.Point;
global using Vector = Avalonia.Vector;
global using Brush = Avalonia.Media.IBrush;
global using IInputElement = Avalonia.Input.IInputElement;
global using RoutedEventArgs = Avalonia.Interactivity.RoutedEventArgs;
global using DrawingContext = Avalonia.Media.DrawingContext;
global using Pen = Avalonia.Media.Pen;
global using FormattedText = Avalonia.Media.FormattedText;
global using Brushes = Avalonia.Media.Brushes;
global using Rect = Avalonia.Rect;
global using Size = Avalonia.Size;
global using TextInputEventArgs = Avalonia.Input.TextInputEventArgs;
#endif
