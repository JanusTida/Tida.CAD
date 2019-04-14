using Tida.Application.Contracts.Common;

namespace Tida.Canvas.Shell.Contracts.Ribbon {
    /// <summary>
    /// Ribbon项基本契约;
    /// </summary>
    public interface IRibbonItem {
        
    }

    /// <summary>
    /// Ribbon项元数据;
    /// </summary>
    public interface IRibbonItemMetaData:IHaveOrder {
        /// <summary>
        /// <see cref="IRibbonGroup"/>的标识;
        /// </summary>
        string GroupGUID { get; }


        /// <summary>
        /// 唯一标识;
        /// </summary>
        string GUID { get; }
    }
}
