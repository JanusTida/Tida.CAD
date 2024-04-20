using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Media {
    /// <summary>
    /// 为屏蔽平台差异性所自定义的颜色结构;
    /// </summary>
    public struct Color {
        public static Color FromArgb(byte a, byte r, byte g, byte b) {
            return new Color {
                A = a,
                R = r,
                G = g,
                B = b
            };
        }

        public static Color FromArgb(uint argb) {
            return new Color {
                A = (byte)((argb & 0xFF000000) >> 24),
                R = (byte)((argb & 0x00FF0000) >> 16),
                G = (byte)((argb & 0x0000FF00) >> 8),
                B = (byte)((argb & 0x000000FF))
            };
        }

        public static Color FromRgb(UInt32 rgb) {
            return FromArgb(rgb | 0xFF000000);
        }

        // Token: 0x06001BC1 RID: 7105 RVA: 0x00072B54 File Offset: 0x00071F54
        internal static Color FromUInt32(uint argb) {
            Color color = new Color {
                A = (byte)((argb & 4278190080u) >> 24),
                R = (byte)((argb & 16711680u) >> 16),
                G = (byte)((argb & 65280u) >> 8),
                B = (byte)(argb & 255u),
            };
            return color;
        }

        //
        // 摘要:
        //     获取或设置颜色的 sRGB alpha 通道值。
        //
        // 返回结果:
        //     颜色的 sRGB Alpha 通道值，介于 0 到 255 之间。
        public byte A { get; set; }
        //
        // 摘要:
        //     获取或设置颜色的 sRGB 蓝色通道值。
        //
        // 返回结果:
        //     System.Windows.Media.Color 结构的 sRGB 蓝色通道值，介于 0 到 255 之间。
        public byte B { get; set; }
        //
        // 摘要:
        //     获取颜色的 国际色彩联合会 (ICC) 或 图像颜色管理 (ICM) 颜色配置文件。
        //
        // 返回结果:
        //     颜色的 国际色彩联合会 (ICC) 或 图像颜色管理 (ICM) 颜色配置文件。
        public byte G { get; set; }
        //
        // 摘要:
        //     获取或设置颜色的 sRGB 红色通道值。
        //
        // 返回结果:
        //     System.Windows.Media.Color 结构的 sRGB 红色通道值，介于 0 到 255 之间。
        public byte R { get; set; }


    }
}
