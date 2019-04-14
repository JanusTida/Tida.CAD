using Tida.Canvas.Infrastructure.Contracts;
using Tida.Canvas.Contracts;
using Tida.Canvas.Input;
using System;

namespace Tida.Canvas.Base.DynamicInput {
    /// <summary>
    /// 泛型动态输入处理器泛型基类;
    /// 本类适用于设定一个长度及一个角度的情况;
    /// 通过设定<see cref="MouseInteractableEditToolGenericBase.LastDownPosition"/>及
    /// <see cref="MouseInteractableEditToolGenericBase.CurrentHoverPosition"/>完成上述功能;
    /// </summary>
    public class LengthAndAngleDynamicInputer<THaveMousePositionTracker> :
        NumberBoxesDynamicInputer where THaveMousePositionTracker : class, IHaveMousePositionTracker, IInputElement {

        public LengthAndAngleDynamicInputer(THaveMousePositionTracker haveMousePositionTracker, ICanvasControl canvasControl):
            base(
                LengthAndAngleNumContainerForMouseTrackable<THaveMousePositionTracker>.
                    CreateFromHaveMousePositionTracker(haveMousePositionTracker,canvasControl.CanvasProxy),
                canvasControl
            )  {

            
        }
        
    }
    
    public static class LengthAndAngleDynamicInputerUtil {

        /// <summary>
        /// 将<paramref name="angle"/>处理,返回与X正半轴的夹角(值域为[0,π]);
        /// </summary>
        /// <returns></returns>
        public static double GetFixedAnglePositiveToXAxizs(double angle) {
            angle = angle % (2 * Math.PI);
            var angleAbs = Math.Abs(angle);
            
            if(angleAbs <= Math.PI) {
                return angleAbs;
            }
            else {
                return 2 * Math.PI - angleAbs;
            }
        }
    }


    
}
