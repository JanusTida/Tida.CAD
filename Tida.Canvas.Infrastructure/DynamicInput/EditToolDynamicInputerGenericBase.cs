using Tida.Canvas.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Infrastructure.DynamicInput {
    /// <summary>
    /// 针对特定编辑工具的动态输入处理器泛型基类;
    /// </summary>
    public abstract class EditToolDynamicInputerGenericBase<TEditTool> : CanvasControlDynamicInputerBase, IDynamicInputer where TEditTool : EditTool {
        public EditToolDynamicInputerGenericBase(ICanvasControl canvasControl) :base(canvasControl){
            this.EditTool = (canvasControl.CurrentEditTool as TEditTool) ?? throw new InvalidCastException();
        }

        ///// <summary>
        ///// 当前对应的编辑工具实例;
        ///// </summary>
        public TEditTool EditTool { get; }


    }

}
