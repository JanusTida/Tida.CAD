using Tida.Canvas.Infrastructure.Contracts;
using Tida.Canvas.Contracts;
using Tida.Canvas.Input;

namespace Tida.Canvas.Base.DynamicInput {
    /// <summary>
    /// 长度的动态输入处理器;
    /// </summary>
    public class LengthDynamicInputer<THaveMousePositionTracker> : NumberBoxesDynamicInputer
        where THaveMousePositionTracker : class, IHaveMousePositionTracker, IInputElement {
        public LengthDynamicInputer(THaveMousePositionTracker haveMousePositionTracker,ICanvasControl canvasControl):
            base(LengthNumContainerForMouseTrackable<THaveMousePositionTracker>.
                CreateFromHaveMousePositionTracker(haveMousePositionTracker,canvasControl.CanvasProxy),
                canvasControl
            ){

        }
    }
}
