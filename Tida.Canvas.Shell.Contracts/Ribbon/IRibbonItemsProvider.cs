
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tida.Canvas.Shell.Contracts.Common;

namespace Tida.Canvas.Shell.Contracts.Ribbon {
    /// <summary>
    /// Ribbon项动态提供者,本接口时候适用于需在运行时动态创建按钮项,而无法为不同类型的单元转化为注解属性时创建;
    /// </summary>
    /// <example>编辑工具对应到工具栏中编辑按钮时;< /example>
    public interface IRibbonItemsProvider {
        /// <summary>
        /// 得到所有Ribbon项;
        /// </summary>
        IEnumerable<CreatedRibbonItem> Items { get; }
    }

    public interface IRibbonItemsProviderMetaData:IHaveOrder {

    }

    //class EE {
    //    public event EventHandler Shock;
    //    public event EventHandler<int> ShockWho;
        
    //    public void Publish() {
    //        ShockWho?.Invoke(this, 1);
    //    }
    //}

    //class AA {
    //    public AA() {
    //        var ee = new EE();
    //        ee.ShockWho += Ee_ShockWho;
    //    }

    //    private void Ee_ShockWho(object sender, int e) {
            
    //    }
    //}
}
