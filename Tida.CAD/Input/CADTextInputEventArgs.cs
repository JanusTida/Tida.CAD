using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Tida.CAD.Input
{
    /// <summary>
    /// CADTextInputEventArgs;
    /// </summary>
    public class CADTextInputEventArgs:CADRoutedEventArgs
    {
#if WPF
        /// <summary>
        /// TextCompositionEventArgs in WPF;
        /// </summary>
        public TextCompositionEventArgs TextCompositionEventArgs { get; set; }
#endif
    }
}
