using Tida.Canvas.Shell.Contracts.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.Contracts.Menu {
    /// <summary>
    /// 菜单项提供者;
    /// </summary>
    public interface IMenuItemProvider {
        IEnumerable<CreatedMenuItem> Items { get; }
    }

    /// <summary>
    /// 菜单项提供者元数据;
    /// </summary>
    public interface IMenuItemProviderMetaData : IHaveOrder {

    }

    [MetadataAttribute,AttributeUsage(AttributeTargets.Class,AllowMultiple = false)]
    public sealed class ExportMenuItemProvider : ExportAttribute,IMenuItemProviderMetaData {
        public ExportMenuItemProvider():base(typeof(IMenuItemProvider)) {

        }

        public int Order { get; set; }
    }
}
