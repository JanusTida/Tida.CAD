using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Tida.Application.Contracts.Controls;

namespace Tida.Canvas.Shell.Contracts.StatusBar {
    /// <summary>
    /// 状态栏项默认基类;
    /// </summary>
    public abstract class StatusBarItemBase : IStatusBarItem {
        public StatusBarItemBase(string guid) {
            this.GUID = guid;
        }

                
        /// <summary>
        /// 默认长度为随内容变化;
        /// </summary>
        public virtual GridChildLength GridChildLength => GridChildLength.Auto;

        public string GUID { get; }
        
        public int Order { get; protected set; }

        public abstract object UIObject { get; }
    }
}
