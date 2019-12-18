using Tida.Canvas.Shell.Contracts.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Tida.Canvas.Shell.Contracts.Docking {
    /// <summary>
    /// 停靠区域契约;
    /// </summary>
    public interface IDockingPane : IDockingItem,IUIObjectProvider {
        /// <summary>
        /// 头文字发生变化事件;
        /// </summary>
        event EventHandler HeaderChanged;

        /// <summary>
        /// 是否隐藏发生变化;
        /// </summary>
        event EventHandler IsHiddenChanged;

        /// <summary>
        /// 头部文字是否可见发生了变化;
        /// </summary>
        event EventHandler PaneHeaderVisibilityChanged;

        /// <summary>
        /// 头部文字;
        /// </summary>
        string Header { get; set; }
        
        /// <summary>
        /// 头部栏可见状态;
        /// </summary>
        Visibility PaneHeaderVisibility { get; set; }

        /// <summary>
        /// 是否隐藏;
        /// </summary>
        bool IsHidden { get; set; }
    }

    /// <summary>
    /// 停靠区域元数据;
    /// </summary>
    public interface IDockingPaneMetaData:IDockingItemMetaData {
        /// <summary>
        /// 初始宽度;
        /// </summary>
        double InitialWidth { get; }

        /// <summary>
        /// 初始高度;
        /// </summary>
        double InitialHeight { get; }
        
        /// <summary>
        /// 初始停靠组唯一标识;
        /// </summary>
        string InitPaneGroupGUID { get; }

        /// <summary>
        /// 能否关闭;
        /// </summary>
        bool CanUserClose { get; }

        /// <summary>
        /// 能否浮动;
        /// </summary>
        bool CanFloat { get; }
    }

    /// <summary>
    /// 导出停靠区域注解;
    /// </summary>
    [MetadataAttribute, AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class ExportDockingPaneAttribute : ExportAttribute, IDockingPaneMetaData {
        public ExportDockingPaneAttribute():base(typeof(IDockingPane)) {

        }

        public double InitialWidth { get; set; } = double.NaN;

        public double InitialHeight { get; set; } = double.NaN;

        public string InitPaneGroupGUID { get; set; }

        public bool CanUserClose { get; set; } = true;

        public bool CanFloat { get; set; } = true;

        public string GUID { get; set; }
    }
}
