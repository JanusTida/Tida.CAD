using CDO.Common.Application.Contracts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDO.Common.Canvas.Shell.Contracts.StatusBar {
    /// <summary>
    /// 状态栏自定义UI项;
    /// </summary>
    public interface IStatusBarObjectItem:IStatusBarItem,IUIObjectProvider {
        
    }

    public interface IStatusBarObjectItemMetaData:IStatusBarItemMetaData {

    }

    public class ExportStatusBarObjectItemAttribute:ExportStatusBarItemAttribute, IStatusBarObjectItemMetaData {
        public ExportStatusBarObjectItemAttribute():base(typeof(IStatusBarObjectItem)) {

        }
    }


}
