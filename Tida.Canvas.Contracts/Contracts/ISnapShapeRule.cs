using Tida.Geometry.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Contracts {
    /// <summary>
    /// 绘制对象辅助感应坐标点规则契约;
    /// </summary>
    public interface ISnapShapeRule {
        /// <summary>
        /// 匹配辅助感应节点;
        /// </summary>
        /// <param name="drawObjects">所有相关的绘制对象</param>
        /// <param name="position">关注的工程数学坐标</param>
        /// <param name="canvasContext">画布上下文</param>
        /// <returns></returns>
        ISnapShape MatchSnapShape(DrawObject[] drawObjects, Vector2D position, ICanvasContext canvasContext);
    }
}
