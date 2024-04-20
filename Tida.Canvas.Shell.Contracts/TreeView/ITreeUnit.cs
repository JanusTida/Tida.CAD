using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Security.Authentication;
using Prism.Mvvm;
using Tida.Canvas.Shell.Contracts.Common;
using Tida.Extending;

namespace Tida.Canvas.Shell.Contracts.TreeView {
    
    /// <summary>
    /// 树形节点契约;
    /// </summary>
    public interface ITreeUnit:IExtensible {
        /// <summary>
        /// 父节点;
        /// </summary>
        ITreeUnit Parent { get; }
        /// <summary>
        /// 类型标识;
        /// </summary>
        string TypeGuid { get; }
        /// <summary>
        /// 名称;
        /// </summary>
        string Label { get; set; }
        /// <summary>
        /// 等级;
        /// </summary>
        int Level { get; }
        /// <summary>
        /// 图标;
        /// </summary>
        Uri Icon { get; set; }
        /// <summary>
        /// 是否展开;
        /// </summary>
        bool IsExpanded { get; set; }
        /// <summary>
        /// 子集合;
        /// </summary>
        ICollection<ITreeUnit> Children { get; }       
    }

}
