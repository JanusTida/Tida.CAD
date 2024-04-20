using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Media {

    /// <summary>
    /// 文字字体颜色单元;
    /// </summary>
    public class ForegroundBlock {
        public ForegroundBlock(Brush brush, int start, int length) {
            if (length < 0) {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            if (start < 0) {
                throw new ArgumentOutOfRangeException(nameof(brush));
            }
            Start = start;
            Length = length;
            Brush = brush;
            End = start + length;
        }

        /// <summary>
        /// 字体颜色;
        /// </summary>
        public Brush Brush { get; }

        /// <summary>
        /// 起始位置;
        /// </summary>
        public int Start { get; }

        /// <summary>
        /// 大小;
        /// </summary>
        public int Length { get; }

        /// <summary>
        /// 终止位置;
        /// </summary>
        public int End { get; }
    }
}
