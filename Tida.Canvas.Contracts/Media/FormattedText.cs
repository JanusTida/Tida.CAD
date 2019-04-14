using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Media {
    /// <summary>
    /// 格式化文字,提供了，更低级的文字描述信息;
    /// </summary>
    public class FormattedText {

        public FormattedText(string textToFormat,Brush foreground,double fontSize) : 
            this(textToFormat,new ForegroundBlock[] { GetEntireForegroundBlock(textToFormat, foreground) },fontSize) {

        }

        private static ForegroundBlock GetEntireForegroundBlock(string textToFormat,Brush foreground) {
            return new ForegroundBlock(foreground, 0, textToFormat.Length);
        }

        public FormattedText(string textToFormat,IEnumerable<ForegroundBlock> foregroundBlocks,double fontSize) {
            Text = textToFormat ?? throw new ArgumentNullException(nameof(textToFormat));

            if (foregroundBlocks == null) {
                throw new ArgumentNullException(nameof(foregroundBlocks));
            }

            foreach (var block in _foregroundBlocks) {
                ValidateRange(block.Start, block.Length);
            }

            this._foregroundBlocks.AddRange(foregroundBlocks);

            this.FontSize = fontSize;
        }


        private readonly List<ForegroundBlock> _foregroundBlocks = new List<ForegroundBlock>();

        /// <summary>
        /// 需被格式化的文字;
        /// </summary>
        public string Text { get; }
        
        public double FontSize { get; }

        public IEnumerable<ForegroundBlock> ForegroundBlocks => _foregroundBlocks;

        // Token: 0x06001F02 RID: 7938 RVA: 0x0007F3EC File Offset: 0x0007E7EC
        private void ValidateRange(int startIndex, int count) {
            if (startIndex < 0 || startIndex > this.Text.Length) {
                throw new ArgumentOutOfRangeException("startIndex");
            }
            int num = startIndex + count;
            if (count < 0 || num < startIndex || num > this.Text.Length) {
                throw new ArgumentOutOfRangeException("count");
            }
        }
        
    }
}
