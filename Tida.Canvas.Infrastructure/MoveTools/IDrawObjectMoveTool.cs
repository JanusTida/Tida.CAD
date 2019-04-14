using Tida.Canvas.Contracts;
using Tida.Geometry.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Infrastructure.MoveTools {
    /// <summary>
    /// 绘制对象移动工具,对指定类型的绘制对象移动同步等操作;
    /// </summary>
    public interface IDrawObjectMoveTool {
        /// <summary>
        /// 检查是否为可移动的绘制对象类型;
        /// </summary>
        /// <param name="drawObject"></param>
        /// <returns></returns>
        bool CheckDrawObjectMoveable(DrawObject drawObject);

        /// <summary>
        /// 根据某个偏移量,移动某个绘制对象;
        /// </summary>
        /// <param name="drawObject"></param>
        /// <param name="offset"></param>
        void Move(DrawObject drawObject, Vector2D offset);

    }
}
