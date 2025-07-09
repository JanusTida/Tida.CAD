using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Tida.CAD.Input;

/// <summary>
/// CADKeyEventArgs;
/// </summary>
public class CADKeyEventArgs:CADRoutedEventArgs
{
#if WPF
    /// <summary>
    /// KeyEventArgs in WPF;
    /// </summary>
    public KeyEventArgs? KeyEventArgs { get; set; }
#endif
}
