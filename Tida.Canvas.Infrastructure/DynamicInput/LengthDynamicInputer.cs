using Tida.Canvas.Infrastructure.Contracts;
using Tida.Canvas.Infrastructure.NativePresentation;
using Tida.Canvas.Contracts;
using Tida.Canvas.Input;

namespace Tida.Canvas.Infrastructure.DynamicInput {
    /// <summary>
    /// 长度的动态输入处理器;
    /// </summary>
    public class LengthDynamicInputer<THaveMousePositionTracker> : NumberBoxesDynamicInputer
        where THaveMousePositionTracker : class, IHaveMousePositionTracker, IInputElement {
        public LengthDynamicInputer(THaveMousePositionTracker haveMousePositionTracker,ICanvasControl canvasControl, INumberBoxService numberBoxService) :
            base(LengthNumContainerForMouseTrackable<THaveMousePositionTracker>.
                CreateFromHaveMousePositionTracker(haveMousePositionTracker,canvasControl.CanvasProxy),
                canvasControl,
                numberBoxService
            ){

        }
    }
}
