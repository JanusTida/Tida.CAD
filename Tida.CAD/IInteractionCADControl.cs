using System.Collections.Generic;

namespace Tida.CAD
{

    /// <summary>
    /// 可交互画布控件契约;
    /// 本单位表示可以通过外部实现对画布控件的访问,修改;
    /// </summary>
    public interface IInteractionCADControl {
        /// <summary>
        /// 所有画布控件的交互处理器;
        /// </summary>
        IEnumerable<CADInteractionHandler> InteractionHandlers { get; set; }
    }
}
