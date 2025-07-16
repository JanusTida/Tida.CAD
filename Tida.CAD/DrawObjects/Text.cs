using System;
using Tida.CAD.Events;
using System.Linq;
using Tida.CAD.Input;
using System.Collections.Generic;
#if WPF
using System.Windows.Media;
using System.Windows;
#elif Avalonia
using Avalonia.Media;
using Avalonia;
#endif

namespace Tida.CAD.DrawObjects;

/// <summary>
/// DrawObject for Text;
/// </summary>
public partial class Text : DrawObject
{
    public Text()
    {
        _fmtdText = GetFormattedText(
            _content,
            _flowDirection,
            _typeface,
            _fontSize,
            _foreground,
            _textAlignment
        );

        _fmtedTextGeometry = _fmtdText.BuildGeometry(_position);
    }

    public static FormattedText GetFormattedText(
        string content,
        FlowDirection flowDirection,
        Typeface typeface,
        double fontSize,
        Brush foreground,
        TextAlignment textAlignment,
        double pixelsPerDip = 1
    )
    {
#if WPF
#if NET45 || NET46
        var formattedText = new FormattedText(
            content,
            System.Globalization.CultureInfo.CurrentCulture,
            flowDirection,
            typeface,
            fontSize,
            foreground
        )
        {
            TextAlignment = textAlignment,
        };
#else
        var formattedText = new FormattedText(
            content,
            System.Globalization.CultureInfo.CurrentCulture,
            flowDirection,
            typeface,
            fontSize,
            foreground,
            pixelsPerDip
        )
        {
            TextAlignment = textAlignment,
        };
#endif
#elif Avalonia
        var formattedText = new FormattedText(
            content,
            System.Globalization.CultureInfo.CurrentCulture,
            flowDirection,
            typeface,
            fontSize,
            foreground
        )
        {
            TextAlignment = textAlignment
        };
#endif
        return formattedText;
    }
}

/// <summary>
/// 属性;
/// </summary>
public partial class Text : DrawObject
{
    /// <summary>
    /// 以画布坐标原点的文本几何信息;
    /// 注意:边界框各点Y值需要取反为真实画布坐标系中的坐标;
    /// </summary>
    private Geometry? _fmtedTextGeometry;

    /// <summary>
    /// 文本内容;
    /// </summary>
    private FormattedText _fmtdText;

    public FormattedText FmtdText
    {
        get { return _fmtdText; }
        set
        {
            if (_fmtdText == value)
                return;
            _fmtdText = value;
            RaiseVisualChanged();
            var args = new ValueChangedEventArgs<FormattedText>(value, _fmtdText);
            _fmtdText = value;

            _fmtedTextGeometry = _fmtdText.BuildGeometry(Position);

            FormattedTextChanged?.Invoke(this, args);
            RaiseVisualChanged();
            
        }
    }

    public event EventHandler<ValueChangedEventArgs<FormattedText>>? FormattedTextChanged;

    /// <summary>
    /// 文本内容;
    /// </summary>
    private string _content = "";

    public string Content
    {
        get { return _content; }
        set
        {
            if (_content == value)
                return;

            var args = new ValueChangedEventArgs<string>(value, _content);
            _content = value;

            _fmtdText = GetFormattedText(
                value,
                FlowDirection,
                Typeface,
                FontSize,
                Foreground,
                TextAlignment
            );

            _fmtedTextGeometry = _fmtdText.BuildGeometry(Position);

            ContentChanged?.Invoke(this, args);
            RaiseVisualChanged();
        }
    }

    public event EventHandler<ValueChangedEventArgs<string>>? ContentChanged;

    /// <summary>
    /// 字体;
    /// </summary>
    private Typeface _typeface = new("Arial");

    public Typeface Typeface
    {
        get { return _typeface; }
        set
        {
            if (_typeface == value)
                return;

            var args = new ValueChangedEventArgs<Typeface>(value, _typeface);
            _typeface = value;

            _fmtdText.SetFontTypeface(value);

            _fmtedTextGeometry = _fmtdText.BuildGeometry(Position);

            TypefaceChanged?.Invoke(this, args);
            RaiseVisualChanged();
        }
    }

    public event EventHandler<ValueChangedEventArgs<Typeface>>? TypefaceChanged;

    /// <summary>
    /// 字体大小;
    /// </summary>
    private double _fontSize = 1D;

    public double FontSize
    {
        get { return _fontSize; }
        set
        {
            if (_fontSize == value)
                return;
            var args = new ValueChangedEventArgs<double>(value, _fontSize);
            _fontSize = value;

            _fmtdText.SetFontSize(value);

            _fmtedTextGeometry = _fmtdText.BuildGeometry(Position);

            FontSizeChanged?.Invoke(this, args);
            RaiseVisualChanged();
        }
    }

    public event EventHandler<ValueChangedEventArgs<double>>? FontSizeChanged;

    /// <summary>
    /// 字体颜色;
    /// </summary>
    private Brush _foreground = Brushes.White;

    public Brush Foreground
    {
        get { return _foreground; }
        set
        {
            if (_foreground == value)
                return;

            var args = new ValueChangedEventArgs<Brush>(value, _foreground);
            _foreground = value;

            _fmtdText.SetForegroundBrush(value);

            _fmtedTextGeometry = _fmtdText.BuildGeometry(Position);

            ForegroundChanged?.Invoke(this, args);
            RaiseVisualChanged();
        }
    }

    public event EventHandler<ValueChangedEventArgs<Brush>>? ForegroundChanged;

    /// <summary>
    /// 文本对齐方式;
    /// </summary>
    private TextAlignment _textAlignment = TextAlignment.Left;

    public TextAlignment TextAlignment
    {
        get { return _textAlignment; }
        set
        {
            if (_textAlignment == value)
                return;

            var args = new ValueChangedEventArgs<TextAlignment>(value, _textAlignment);
            _textAlignment = value;

            _fmtdText.TextAlignment = value;

            _fmtedTextGeometry = _fmtdText.BuildGeometry(Position);

            TextAlignmentChanged?.Invoke(this, args);
            RaiseVisualChanged();
        }
    }

    public event EventHandler<ValueChangedEventArgs<TextAlignment>>? TextAlignmentChanged;

    /// <summary>
    /// 位置;
    /// </summary>
    private Point _position = new Point(0,0);

    public Point Position
    {
        get { return _position; }
        set
        {
            if (_position == value)
                return;
            var args = new ValueChangedEventArgs<Point>(value, _position);
            _position = value;

            _fmtedTextGeometry = _fmtdText.BuildGeometry(value);

            PositionChanged?.Invoke(this, args);
            RaiseVisualChanged();
        }
    }

    public event EventHandler<ValueChangedEventArgs<Point>>? PositionChanged;

    /// <summary>
    /// Angle,In degrees, clockwise from the x-axis.
    /// </summary>
    private double _angle = 0D;

    public double Angle
    {
        get { return _angle; }
        set
        {
            if (_angle == value)
                return;
            var args = new ValueChangedEventArgs<double>(value, _angle);
            _angle = value;

            AngleChanged?.Invoke(this, args);
            RaiseVisualChanged();
        }
    }

    public event EventHandler<ValueChangedEventArgs<double>>? AngleChanged;

    /// <summary>
    /// 字体方向;
    /// </summary>
    private FlowDirection _flowDirection = FlowDirection.LeftToRight;

    public FlowDirection FlowDirection
    {
        get { return _flowDirection; }
        set
        {
            if (_flowDirection == value)
                return;
            var args = new ValueChangedEventArgs<FlowDirection>(value, _flowDirection);
            _flowDirection = value;

            _fmtdText.FlowDirection = value;

            FlowDirectionChanged?.Invoke(this, args);
            RaiseVisualChanged();
        }
    }

    public event EventHandler<ValueChangedEventArgs<FlowDirection>>? FlowDirectionChanged;
}

/// <summary>
/// 笔刷;
/// </summary>
public partial class Text : DrawObject
{
    /// <summary>
    /// 默认状态下线的笔刷;
    /// </summary>
    protected Brush _normalBrush = DefaultNormalBrush;
    public static readonly Brush DefaultNormalBrush = Brushes.White;
    public Brush NormalBrush
    {
        get => _normalBrush;
        set
        {
            _normalBrush = value;
        }
    }

    private static readonly Brush DefaultSelectedBrush = Brushes.Red;
    /// <summary>
    /// 默认状态下线的笔刷;
    /// </summary>
    protected Brush _selectedBrush = DefaultNormalBrush;

    public Brush SelectedBrush
    {
        get => _selectedBrush;
        set
        {
            _selectedBrush = value;
        }
    }
}

/// <summary>
/// 重写父类绘图的方法;
/// </summary>
public partial class Text : DrawObject
{
#if Avalonia
    private static double DegreeToRadian(double angle)
    {
        return angle * Math.PI / 180;
    }
#endif
    /// <summary>
    /// 绘制方法;
    /// </summary>
    /// <param name="canvas"></param>
    public override void Draw(ICanvas canvas)
    {
        if (FmtdText == null)
        {
            return;
        }
#if WPF
        canvas.DrawingContext.PushTransform
        (
            new RotateTransform
            (
                Angle,
                Position.X,
                Position.Y
            )
        );
#elif Avalonia
        canvas.DrawingContext.PushTransform
        (
            Matrix.CreateRotation(DegreeToRadian(Angle)) * Matrix.CreateTranslation(Position.X, Position.Y)
        );
#endif
        canvas.DrawText(FmtdText, Position);
#if WPF
        canvas.DrawingContext.Pop();
#elif Avalonia
        canvas.DrawingContext.PushTransform
        (
            Matrix.CreateTranslation(-Position.X, -Position.Y) * Matrix.CreateRotation(DegreeToRadian(-Angle))
        );
#endif
        // Draw bounding box;
        //var rect2d = GetBoundingRect();
        //if (rect2d != null)
        //    drawMethod.DrawRectangle(
        //        rect2d,
        //        Brushes.AliceBlue,
        //        PenExtensions.CreateFrozenPen(Brushes.Yellow, 1)
        //    );

        // 绘制选中状态;
        DrawSelectedState(canvas);
    }
    private bool _showSelectedState;

    public bool ShowSelectedState
    {
        get { return _showSelectedState; }
        set 
        { 
            if(_showSelectedState == value)
            {
                return;
            }
            _showSelectedState = value;
            RaiseVisualChanged();
        }
    }

    /// <summary>
    /// 绘制被选中状态;
    /// </summary>
    /// <param name="canvas"></param>
    protected virtual void DrawSelectedState(ICanvas canvas)
    {
        if (!IsSelected || !ShowSelectedState)
            return;

        if (Content == null)
            return;

        DrawSelectedTextState(
            this,
            canvas,
            GetFormattedText(
                Content,
                FlowDirection,
                Typeface,
                FontSize,
                SelectedBrush,
                TextAlignment
            ),
            FontSize,
            Position,
            Angle
        );
    }

    /// <summary>
    /// Draw selected text state;
    /// </summary>
    /// <param name="canvas"></param>
    /// <param name="copyFmtdText"></param>
    /// <param name="fontSize"></param>
    /// <param name="origion"></param>
    /// <param name="angle"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public static void DrawSelectedTextState(
        Text text,
        ICanvas canvas,
        FormattedText copyFmtdText,
        double fontSize,
        Point origion,
        double angle
    )
    {
        if (copyFmtdText == null)
        {
            throw new ArgumentNullException(nameof(copyFmtdText));
        }

#if WPF
        canvas.DrawingContext.PushTransform
        (
            new RotateTransform
            (
                angle,
                origion.X,
                origion.Y
            )                        
        );
#elif Avalonia
        canvas.DrawingContext.PushTransform
        (
            Matrix.CreateRotation(DegreeToRadian(angle)) * Matrix.CreateTranslation(origion.X, origion.Y)
        );
#endif

        // 显示选中对象;
        canvas.DrawText(copyFmtdText, origion);

#if WPF
        canvas.DrawingContext.Pop();
#elif Avalonia
        canvas.DrawingContext.PushTransform
        (
            Matrix.CreateTranslation(-origion.X, -origion.Y) * Matrix.CreateRotation(DegreeToRadian(-angle))
        );
#endif

    }
}

public partial class Text : DrawObject
{

    private static bool IsAlmostEqualTo(Point point1, Point point2, double tolerance)
    {
        return Math.Abs(point1.X - point2.X) < tolerance
            && Math.Abs(point1.Y - point2.Y) < tolerance;
    }

    /// <summary>
    /// 绕着某点进行旋转;
    /// </summary>
    /// <param name="point"></param>
    /// <param name="line"></param>
    /// <returns></returns>
    private static Point RotateByPoint(Point param, Point point, double radian)
    {
        if (IsAlmostEqualTo(param,point,SMALL_NUMBER))
            return param;

        double x1 =
            (param.X - point.X) * Math.Cos(radian)
            - (param.Y - point.Y) * Math.Sin(radian)
            + point.X;

        double y1 =
            (param.X - point.X) * Math.Sin(radian)
            + (param.Y - point.Y) * Math.Cos(radian)
            + point.Y;

        return new Point(x1, y1);
    }
    /// <summary>
    /// 获取对象的边界框(世界坐标系);
    /// </summary>
    /// <returns></returns>
    public override CADRect? GetBoundingRect(ICADScreenConverter screenConverter)
    {
        if (_fmtedTextGeometry == null)
        {
            return null;
        }

        var rectBBox = _fmtedTextGeometry.Bounds;
        if (rectBBox == new Rect(0,0,0,0))
            return null;

        // 由于屏幕坐标系与绘图坐标系不一致,
        // 需要先将点镜像(以文本位置Y值的水平轴);
        var bottomLeft = new Point(
            rectBBox.BottomLeft.X,
            Position.Y - Math.Abs(rectBBox.Bottom - Position.Y)
        );

        var topRight = new Point(
            rectBBox.TopRight.X,
            Position.Y - Math.Abs(rectBBox.Top - Position.Y)
        );

        var radian = Angle / 180 * Math.PI;
        if (Math.Abs(radian) > SMALL_NUMBER)
        {
            bottomLeft = RotateByPoint(bottomLeft,Position, -radian);
            topRight = RotateByPoint(topRight,Position, -radian);
        }
        bottomLeft = screenConverter.ToCAD(bottomLeft);
        topRight = screenConverter.ToCAD(topRight);
        var rect2d = new CADRect(bottomLeft, topRight);
        return rect2d;
    }

    /// <summary>
    /// 判断点是否在对象区域内;
    /// </summary>
    /// <param name="point">画布坐标系中的坐标</param>
    /// <param name="canvasZoomConverter"></param>
    /// <returns></returns>
    public override bool PointInObject(Point point, ICADScreenConverter cadScreenConverter)
    {
        var rect2d = GetBoundingRect(cadScreenConverter);
        if (rect2d == null)
            return false;
        
        bool isContain = rect2d.Value.Contains(point);

        return isContain;
    }

    private static double ClosestParameter(CADLine line, Point testPoint)
    {
        var v = line.End - line.Start;
        var ls = v.X * v.X + v.Y * v.Y;
        Vector v1 = testPoint - line.Start;
        Vector v2 = testPoint - line.End;
        var result = 0.0d;
        if (ls > 0)
        {
            if (v1.X * v1.X + v1.Y * v1.Y <= v2.X * v2.X + v2.Y * v2.Y)
            {
                result = v1 * v / ls;
            }
            else
            {
                result = 1 + v2 * v / ls;
            }
        }
        return result;
    }
    private static Point? Intersect(CADLine line1, CADLine line2, bool isSegement = true, double espilon = SMALL_NUMBER)
    {
        espilon = espilon < 0 ? SMALL_NUMBER : espilon;
        //double rxs = (line1.End - line1.Start).Cross(line2.End - line2.Start);
        var line1Vector = (line1.End - line1.Start);
        var line2Vector = (line2.End - line2.Start);
#if WPF
        double rxs = Vector.CrossProduct(line1Vector,line2Vector);
#elif Avalonia
        double rxs = Vector.Cross(line1Vector,line2Vector);
#endif
        if (Math.Abs(rxs) < espilon) return null;
#if WPF
        double r = Vector.CrossProduct(line2.Start - line1.Start,line2.End - line2.Start) / rxs;
#elif Avalonia
        double r = Vector.Cross(line2.Start - line1.Start,line2.End - line2.Start) / rxs;
#endif
        var point = line1.Start + line1Vector * r;
        if (!isSegement) return point;
        var t = ClosestParameter(line1,point);
        var u = ClosestParameter(line2,point);
        var isOnline = t >= -espilon && t <= 1 + espilon && u >= -espilon && u <= 1 + espilon;
        if (isOnline) return point;
        return null;
    }
    /// <summary>
    /// 判断两组线端是否有交点;
    /// 若任意两组线内的两个线存在交点,则返回True;
    /// </summary>
    /// <returns></returns>
    private static bool LinesHaveIntersectPoint(
        IEnumerable<CADLine> lines,
        IEnumerable<CADLine> otherLines
    )
    {
        foreach (var line in lines)
        {
            foreach (var otherLine in otherLines)
            {
                if (Intersect(line,otherLine) != null)
                {
                    return true;
                }
            }
        }
        return false;
    }
    /// <summary>
    /// 判断对象是否在矩形内;
    /// </summary>
    /// <param name="dragRect"></param>
    /// <param name="screenConverter"></param>
    /// <param name="anyPoint"></param>
    /// <returns></returns>
    public override bool ObjectInRectangle(
        CADRect dragRect,
        ICADScreenConverter screenConverter,
        bool anyPoint
    )
    {
        var rect2d = GetBoundingRect(screenConverter);
        if (rect2d == null)
            return false;

        bool isContain = rect2d.Value.GetVertexes().All(dragRect.Contains);

        if (anyPoint)
        {
            isContain =
                isContain
                || dragRect.GetVertexes().Any(rect2d.Value.Contains)
                || rect2d.Value.GetVertexes().Any(dragRect.Contains)
                || LinesHaveIntersectPoint(
                    rect2d.Value.GetBorders(),
                    dragRect.GetBorders()
                );
        }

        return isContain;
    }
}

/// <summary>
/// 鼠标事件监听;
/// </summary>
public partial class Text : DrawObject
{
    public const double SMALL_NUMBER = 1e-5;
    
    /// <summary>
    /// 鼠标点击;
    /// </summary>
    /// <param name="e"></param>
    protected override void OnMouseDownCore(CADMouseButtonEventArgs e)
    {
        if (e == null)
        {
            throw new ArgumentNullException(nameof(e));
        }

        if (_fmtedTextGeometry == null || _fmtedTextGeometry.Bounds == new Rect(0,0,0,0))
            return;

        var thisPosition = e.Position;

        var textBBox = _fmtedTextGeometry.Bounds;
        
        var btmLeft = new Point(textBBox.BottomLeft.X, -textBBox.BottomLeft.Y);
        
        //if (IsAlmostEqualTo(thisPosition,btmLeft, 0.1))
        //{
        //    Debug.WriteLine(111);
        //}
    }
    protected override void OnMouseMoveCore(CADMouseEventArgs e)
    {
        base.OnMouseMoveCore(e);
    }
}