
using Tida.Canvas.Contracts;
using Tida.Canvas.Events;
using Prism.Events;
using Tida.Canvas.Shell.Contracts.Common;

namespace Tida.Canvas.Shell.Contracts.Canvas.Events {
    public class CanvasEditToolChangedEventArgs : CanvasEventArgs<ValueChangedEventArgs<EditTool>> {
        public CanvasEditToolChangedEventArgs(ICanvasDataContext canvasDataContext,ValueChangedEventArgs<EditTool> valueChangedEventArgs):base(canvasDataContext,valueChangedEventArgs) {

        }
                
    }
    
    /// <summary>
    /// 画布的编辑工具实例变更事件;
    /// </summary>
    public class CanvasEditToolChangedEvent:PubSubEvent<CanvasEditToolChangedEventArgs> {

    }

    public interface ICanvasEditToolChangedEventHandler : IEventHandler<CanvasEditToolChangedEventArgs> {

    }



    ///// <summary>
    ///// 导出事件处理器;
    ///// </summary>
    //public sealed class ExportEditToolChangedEventHandlerAttribute:ExportAttribute {
    //    public ExportEditToolLoadedEventHandlerAttribute():base(typeof(IEditToolChangedEventHandler)) {

    //    }
    //}
}
