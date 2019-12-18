using Tida.Canvas.Shell.Contracts.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Tida.Canvas.Shell.Contracts.Menu {
    /// <summary>
    /// 菜单项;
    /// </summary>
    public interface IMenuItem {

        /// <summary>
        /// 命令项;
        /// </summary>
        ICommand Command { get; }
    }


    /// <summary>
    /// 菜单项元数据;
    /// </summary>
    public interface IMenuItemMetaData : IHaveOrder {

        /// <summary>
        /// 菜单项;
        /// </summary>
        string GUID { get; }

        /// <summary>
        /// 父菜单项GUID;
        /// </summary>
        string OwnerGUID { get; }

        /// <summary>
        /// 显示名;
        /// </summary>
        string HeaderLanguageKey { get; }

        /// <summary>
        /// Icon;
        /// </summary>
        string Icon { get; }

        /// <summary>
        /// 快捷输入提示显示;
        /// </summary>
        string InputGestureText { get; }


        /// <summary>
        /// 快捷键;
        /// </summary>
        Key Key { get; }

        /// <summary>
        /// 快捷键修饰键;
        /// </summary>
        ModifierKeys ModifierKeys { get; }
    }

    /// <summary>
    /// 导出菜单项;
    /// </summary>
    [MetadataAttribute, AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ExportMenuItemAttribute : ExportAttribute, IMenuItemMetaData {
        public ExportMenuItemAttribute() : base(typeof(IMenuItem)) {

        }

        public string GUID { get; set; }

        public string OwnerGUID { get; set; }

        public string HeaderLanguageKey { get; set; }

        public string Icon { get; set; }

        public string InputGestureText { get; set; }

        public Key Key { get; set; }

        public ModifierKeys ModifierKeys { get; set; }

        public int Order { get; set; }
    }
}
