using Tida.Canvas.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Infrastructure.ComponentModel {
    /// <summary>
    /// 绘制对象描述器;
    /// </summary>
    public interface IDrawObjectDescriptor {
        /// <summary>
        /// 获取特定绘制对象的类别名;
        /// </summary>
        /// <param name="drawObject"></param>
        /// <returns></returns>
        DrawObjectDescription GetDescription(DrawObject drawObject);
    }

}
