using System.Windows.Media;

namespace Tida.CAD.WPF.Extensions;

internal class PenExtensions
{

    /// <summary>
    /// 构建一个新的被冻结的笔;
    /// </summary>
    /// <returns></returns>
    public static Pen CreateFrozenPen(Brush brush, double thickness)
    {
        return CreateFrozenPen(brush, thickness, null);
    }

    /// <summary>
    /// 构建一个新的被冻结的笔;
    /// </summary>
    /// <param name="brush"></param>
    /// <param name="thickness"></param>
    /// <param name="dashStyle"></param>
    /// <returns></returns>
    public static Pen CreateFrozenPen(Brush brush, double thickness, DashStyle? dashStyle)
    {
        var pen = new Pen
        {
            Brush = brush,
            Thickness = thickness,
            DashStyle = dashStyle
        };
        pen.Freeze();
        return pen;
    }
}
