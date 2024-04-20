﻿using Tida.Canvas.Contracts;
using Tida.Geometry.Primitives;

namespace Tida.Canvas.Infrastructure.OffsetTools {
    /// <summary>
    /// 绘制对象偏移工具;
    /// </summary>
    public interface IDrawObjectOffsetTool {
        /// <summary>
        /// 根据指定偏移,对绘制对象进行偏移调整;
        /// </summary>
        /// <param name="drawObject"></param>
        /// <param name="offset"></param>
        /// <param name="relativeTo"></param>
        /// <returns></returns>
        void MoveOffset(DrawObject drawObject,double offset,Vector2D relativeTo);

        /// <summary>
        /// 检查是否为可偏移的绘制对象类型;
        /// </summary>
        /// <param name="drawObject"></param>
        /// <returns></returns>
        bool CheckDrawObjectMoveable(DrawObject drawObject);
    }
}
