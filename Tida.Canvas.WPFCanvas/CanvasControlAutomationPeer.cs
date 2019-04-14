using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;

namespace CDO.Common.WPFCanvas {
    public class CanvasControlAutomationPeer:FrameworkElementAutomationPeer,IInvokeProvider {
        public CanvasControlAutomationPeer(CanvasControl canvasControl):base(canvasControl) {
            
        }

        protected override string GetClassNameCore() {
            return nameof(CanvasControl);
        }

        protected override AutomationControlType GetAutomationControlTypeCore() {
            return AutomationControlType.Custom;
        }

        public override object GetPattern(PatternInterface patternInterface) {
            if(patternInterface == PatternInterface.Invoke) {
                return this;
            }
            return base.GetPattern(patternInterface);
        }

        public void Invoke() {
            
        }



        

        
    }
}
