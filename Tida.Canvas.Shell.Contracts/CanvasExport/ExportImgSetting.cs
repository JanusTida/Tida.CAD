using Tida.Canvas.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.Contracts.CanvasExport {
    /// <summary>
    /// 导出为图像设定;
    /// </summary>
    public sealed class ExportImgSetting {
        public ExportImgSetting(Stream exportStream) {
            this.ExportStream = exportStream ?? throw new ArgumentNullException(nameof(exportStream));
            if (!exportStream.CanWrite) {
                throw new ArgumentException($"{nameof(exportStream)} should be writable.");
            }
        }

        public const int DefaultWidth = 1024;
        public const int DefaultHeight = 768;

        private int _width = DefaultWidth;
        /// <summary>
        /// 宽度;
        /// </summary>
        public int Width {
            get => _width;
            set {
                if(value <= 0) {
                    throw new ArgumentException($"{nameof(value)} can't be less than zero.");
                }

                _width = value;
            }
        }

        private int _height = DefaultHeight;
        /// <summary>
        /// 高度;
        /// </summary>
        public int Height {
            get => _height;
            set {
                if(value <= 0) {
                    throw new ArgumentException($"{nameof(value)} can't be less than zero.");
                }

                _height = value;
            }
        } 
        
        /// <summary>
        /// 被写入的流;
        /// </summary>
        public Stream ExportStream { get; }

        public static readonly Brush DefaultBackground = new SolidColorBrush(Color.FromArgb(0xff, 0x1, 0x39, 0x39));

        /// <summary>
        /// 背景;
        /// </summary>
        public Brush Background { get; set; } = DefaultBackground;

        /// <summary>
        /// 是否导出不可见的对象;
        /// </summary>
        public bool ExportUnvisible { get; set; }
    }
}
