using Tida.Canvas.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Infrastructure.ExtendTools {
    /// <summary>
    /// 绘制对象延伸工具;
    /// </summary>
    public interface IDrawObjectExtendTool {

        /// <summary>
        /// 检查是否是本单位可以延伸的绘制对象;
        /// </summary>
        /// <param name="drawObject"></param>
        /// <returns></returns>
        bool CheckIsValidDrawObject(DrawObject drawObject);

        /// <summary>
        /// 返回对绘制对象进行延伸后的绘制对象;
        /// </summary>
        /// <param name="trimingInfo">裁剪信息</param>
        /// <returns></returns>
        DrawObject ExtendDrawObject(DrawObjectExtendInfo extendInfo);
    }
}
