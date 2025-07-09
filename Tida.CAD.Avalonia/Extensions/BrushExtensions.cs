using Avalonia.Media;
namespace Tida.CAD.Avalonia.Extensions;

internal static class BrushExtensions
{
    /// <summary>
    /// Create a frozen <see cref="SolidColorBrush"/>;
    /// </summary>
    /// <param name="argb"></param>
    /// <returns></returns>
    public static SolidColorBrush CreateColorBrush(uint argb)
    {
        var a = (byte)((argb & 0xFF000000) >> 24);
        var r = (byte)((argb & 0x00FF0000) >> 16);
        var g = (byte)((argb & 0x0000FF00) >> 8);
        var b = (byte)(argb & 0x000000FF);
        return new SolidColorBrush(Color.FromArgb(a,r,g,b));
    }

    /// <summary>
    /// Create a frozen <see cref="SolidColorBrush"/>;
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="r"></param>
    /// <param name="g"></param>
    /// <returns></returns>
    public static SolidColorBrush CreateColorBrush(byte a,byte r,byte g,byte b)
    {
        return new SolidColorBrush(Color.FromArgb(a, r, g, b));
    }


}
