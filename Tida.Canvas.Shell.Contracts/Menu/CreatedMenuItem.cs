
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.Contracts.Menu {
    /// <summary>
    /// 手动创建的菜单项;
    /// </summary>
    public sealed class CreatedMenuItem {
        public CreatedMenuItem(IMenuItem menuItem,IMenuItemMetaData metaData) {

            MenuItem = menuItem ?? throw new ArgumentNullException(nameof(menuItem));

            MenuItemMetaData = metaData ?? throw new ArgumentNullException(nameof(metaData));

        }
        public IMenuItem MenuItem { get; }

        public IMenuItemMetaData MenuItemMetaData { get; }
        
    }
}
