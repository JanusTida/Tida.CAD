using Tida.Canvas.Infrastructure.Contracts;
using Tida.Canvas.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Infrastructure.DynamicInput {
    /// <summary>
    /// 画布控件的动态输入处理器提供者;
    /// </summary>
    public interface ICanvasControlDynamicInputerProvider {

        /// <summary>
        /// 创建一个编辑工具的动态输入处理器;
        /// </summary>
        /// <param name="canvasControl"></param>
        /// <returns>若类型不匹配,则返回为空</returns>
        IDynamicInputer CreateInputer(ICanvasControl canvasControl);
        
    }

   
}
