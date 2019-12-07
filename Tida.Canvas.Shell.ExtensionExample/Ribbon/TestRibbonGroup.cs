using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tida.Canvas.Shell.Contracts.Ribbon;

namespace Tida.Canvas.Shell.ExtensionExample.Ribbon
{
    [ExportRibbonGroup(GUID = Constants.RibbonGroupGuid_Test,HeaderLanguageKey = "Test",Order = 4096,ParentGUID = Tida.Canvas.Shell.Contracts.Ribbon.Constants.RibbonTab_Tool)]
    class TestRibbonGroup : IRibbonGroup
    {
    }
}
