using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Infrastructure.EditTools {
    /// <summary>
    /// 用于测量的编辑工具接口;
    /// </summary>
    public interface IMeasureEditTool {
        /// <summary>
        /// 绘制操作完成,呈递事务时是否将保持的数据绘制对象添加到到指定图层中;
        /// (对应"标注"勾选);
        /// </summary>
        bool ShouldCommitMeasureData { get; set; }
    }
}
