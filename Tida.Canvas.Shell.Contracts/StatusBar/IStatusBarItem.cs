
using Tida.Canvas.Shell.Contracts.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Tida.Canvas.Shell.Contracts.Common;

namespace Tida.Canvas.Shell.Contracts.StatusBar {
    /// <summary>
    /// 状态栏项;
    /// </summary>
    public interface IStatusBarItem : IUIObjectProvider, IHaveOrder {
        /// <summary>
        /// 唯一标识;
        /// </summary>
        string GUID { get; }

        /// <summary>
        /// Grid长度;
        /// </summary>
        GridChildLength GridChildLength { get; }
        
    }

    /// <summary>
    /// 状态栏项相关附加元数据;
    /// </summary>
    //public interface IStatusBarItemMetaData {
    //    /// <summary>
    //    /// Grid长度;
    //    /// </summary>
    //    GridChildLength GridChildLength { get; }
    //}

    /// <summary>
    /// 状态栏项导出注解,所有状态栏项通过此注解导出;
    /// </summary>
    //public class ExportStatusBarItemAttribute : ExportAttribute,IStatusBarItemMetaData {
    //    public ExportStatusBarItemAttribute():base(typeof(IStatusBarItem)) {

    //    }

    //    /// <summary>
    //    /// Grid长度;
    //    /// </summary>
    //    public GridChildLength GridChildLength { get; set; }
    //}

    
}
