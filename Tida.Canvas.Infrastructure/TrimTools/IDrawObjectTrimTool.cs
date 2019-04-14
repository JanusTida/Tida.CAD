using Tida.Canvas.Contracts;
using Tida.Geometry.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Infrastructure.TrimTools {
    /// <summary>
    /// 绘制对象裁剪工具;
    /// </summary>
    public interface IDrawObjectTrimTool {

        /// <summary>
        /// 检查是否是本单位可以裁剪的绘制对象;
        /// </summary>
        /// <param name="drawObject"></param>
        /// <returns></returns>
        bool CheckIsValidDrawObject(DrawObject drawObject);

        /// <summary>
        /// 返回对绘制对象进行裁剪后的绘制对象;
        /// </summary>
        /// <param name="trimingInfo">裁剪信息</param>
        /// <returns></returns>
        DrawObject[] TrimDrawObject(DrawObjectTrimingInfo trimingInfo);
    }
}
