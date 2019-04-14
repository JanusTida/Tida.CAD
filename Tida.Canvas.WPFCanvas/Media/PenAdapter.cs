using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemMedia = System.Windows.Media;
using Tida.Canvas.Media;

namespace Tida.Canvas.WPFCanvas.Media {
    /// <summary>
    /// <see cref="Media.Pen"/>与<see cref="SystemMedia.Pen"/>的适配器;
    /// </summary>
    public static class PenAdapter {
        private static readonly Dictionary<Pen, SystemMedia.Pen> _frozenPenDict = new Dictionary<Pen, SystemMedia.Pen>();
        /// <summary>
        /// 从笔转化为系统笔;
        /// </summary>
        /// <param name="pen"></param>
        /// <returns></returns>
        public static SystemMedia.Pen ConverterToSystemPen(Pen pen) {
            if(pen == null) {
                throw new ArgumentNullException(nameof(pen));
            }

            if(_frozenPenDict.TryGetValue(pen,out var sysPen)) {
                return sysPen;
            }

            sysPen = new SystemMedia.Pen(BrushAdapter.ConvertToSystemBrush(pen.Brush), pen.Thickness);
            if(pen.DashStyle != null) {
                sysPen.DashStyle = DashStyleAdapter.ConvertToSystemDashStyle(pen.DashStyle);
            }

            if (pen.IsFrozen) {
                sysPen.Freeze();
                _frozenPenDict.Add(pen, sysPen);
            }
            
            return sysPen;
        }
    }
}
