using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Launcher {
    /// <summary>
    /// 持久化相关常量;
    /// </summary>
    static partial class Constants {

        /// <summary>
        /// 图层的标签名;
        /// </summary>
        public const string XElemName_Layer = "layer";

        /// <summary>
        /// 图层的标识属性名;
        /// </summary>
        public const string XPropName_LayerGUID = "GUID";

        /// <summary>
        /// 图层的名称属性名;
        /// </summary>
        public const string XPropName_LayerName = "name";

        /// <summary>
        /// 状态保存的根标签名;
        /// </summary>
        public const string XElemName_CanvasDataModel = "CanvasDataModel";

    }

    /// <summary>
    /// 菜单相关常量;
    /// </summary>
    static partial class Constants {


        public const string MenuItemGUID_OpenDoc = nameof(MenuItemGUID_OpenDoc);


        public const string MenuItemGUID_CreateDoc = nameof(MenuItemGUID_CreateDoc);


        public const string MenuItemGUID_SaveDoc = nameof(MenuItemGUID_SaveDoc);


        public const string MenuItemGUID_SaveAsDoc = nameof(MenuItemGUID_SaveAsDoc);


        public const string MenuItemGUID_CloseShell = nameof(MenuItemGUID_CloseShell);

    }

    /// <summary>
    /// 图标相关;
    /// </summary>
    static partial class Constants {
        private const string Prefix = "pack://application:,,,/Tida.Canvas.Launcher;component/Resources/";
        
        
        public const string MenuItemIcon_OpenDoc =    Prefix + "Open File.png";
        public const string MenuItemIcon_SaveDoc =     Prefix + "Save.png";
       
        public const string MenuItemIcon_CreateDoc =  Prefix + "New Scene.png";

        public const string MenuItemIcon_SaveAsDoc = MenuItemIcon_SaveDoc;
    }
    static partial class Constants {
        public const string MenuItemName_OpenDoc = nameof(MenuItemName_OpenDoc);

        public const string MenuItemName_Save = nameof(MenuItemName_Save);

        public const string MenuItemName_SaveAs = nameof(MenuItemName_SaveAs); 
        
        public const string MenuItemName_New = nameof(MenuItemName_New);


        public const string MenuItemName_CloseShell = nameof(MenuItemName_CloseShell);

    }
}
