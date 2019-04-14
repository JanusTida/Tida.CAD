
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tida.Canvas.Contracts;
using Tida.Geometry.Primitives;

namespace Tida.Canvas.Infrastructure.MirrorTools
{
    public interface IDrawObjectMirrorTool
    {
        /// <summary>
        /// 根据指定偏移,对绘制对象进行偏移调整;
        /// </summary>
        /// <param name="drawObject"></param>
        /// <param name="offset"></param>
        /// <param name="relativeTo"></param>
        /// <returns></returns>
        void Mirror(DrawObject drawObject, Line2D axis);

        /// <summary>
        /// 检查是否为可偏移的绘制对象类型;
        /// </summary>
        /// <param name="drawObject"></param>
        /// <returns></returns>
        bool CheckDrawObjectMirrorable(DrawObject drawObject);
    }
}
