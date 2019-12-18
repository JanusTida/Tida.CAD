using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.Contracts.Interactivity {
    public class InteractionRequestTrigger : EventTrigger {
        /// <summary>
        /// Specifies the name of the Event this EventTriggerBase is listening for.
        /// </summary>
        /// <returns>This implementation always returns the Raised event name for ease of connection with <see cref="T:Prism.Interactivity.InteractionRequest.IInteractionRequest" />.</returns>
        // Token: 0x06000389 RID: 905 RVA: 0x0000923E File Offset: 0x0000743E
        protected override string GetEventName() {
            return "Raised";
        }
    }
}
