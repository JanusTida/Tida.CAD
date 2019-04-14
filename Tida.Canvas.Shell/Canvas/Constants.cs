using System;

namespace Tida.Canvas.Shell.Canvas {
    public static partial class Constants {

        public const string RibbonGroup_Edit = nameof(RibbonGroup_Edit);


        public const string RibbonItem_Undo = nameof(RibbonItem_Undo);


        public const string RibbonItem_Redo = nameof(RibbonItem_Redo);


        public const string RibbonItem_Select = nameof(RibbonItem_Select);


        public const string Exception_ActiveLayerCannotBeNull = nameof(Exception_ActiveLayerCannotBeNull);


        public const string MenuItem_CanvasContextMenu_DeleteSelectedDrawObjects = nameof(MenuItem_CanvasContextMenu_DeleteSelectedDrawObjects);
        public const string MenuItem_CanvasContextMenu_Undo = nameof(MenuItem_CanvasContextMenu_Undo);
        public const string MenuItem_CanvasContextMenu_Redo = nameof(MenuItem_CanvasContextMenu_Redo);
        public const string MenuItem_CanvasContextMenu_CommitEdit = nameof(MenuItem_CanvasContextMenu_CommitEdit);
        public const string MenuItem_CanvasContextMenu_HideSelectedDrawObjects = nameof(MenuItem_CanvasContextMenu_HideSelectedDrawObjects);
        public const string MenuItemGUID_ShowAllDrawObjects = nameof(MenuItemGUID_ShowAllDrawObjects);
    }

    /// <summary>
    /// 图标相关;
    /// </summary>
    public static partial class Constants {
        private const string Prefix = "pack://application:,,,/Tida.Canvas.Shell;component/Resources/";

        public static readonly string MenuItemIcon_Select = Prefix + "选择.png";
        public static readonly string MenuItemIcon_Undo = Prefix + "撤销.png";
        public static readonly string MenuItemIcon_Redo = Prefix + "重做.png";
    }

    /// <summary>
    /// Canvas相关唯一标识;
    /// </summary>
    public static partial class Constants {


        public const string StatusBarItem_Zoom = nameof(StatusBarItem_Zoom);

        public const string StatusBarItem_CurrentEditTool = nameof(StatusBarItem_CurrentEditTool);

        public const string StatusBarItem_SnapPosition = nameof(StatusBarItem_SnapPosition);


        public const string StatusBarItem_CanvasLayers = nameof(StatusBarItem_CanvasLayers);

        public const string StatusBarItem_SnappingEnabled = nameof(StatusBarItem_SnappingEnabled);

        public const string StatusBarItem_VertexMode = nameof(StatusBarItem_VertexMode);

        public const string StatusBarItem_DynamicInput = nameof(StatusBarItem_DynamicInput);

        public const string StatusBarItem_AxisTrackingEnabled = nameof(StatusBarItem_AxisTrackingEnabled);

        public const string StatusBarItem_IsReadOnly = nameof(StatusBarItem_IsReadOnly);

        public const string CanvasLayersStatusBarItem = nameof(CanvasLayersStatusBarItem);


        public const string DocumentPaneHeader_Canvas = nameof(DocumentPaneHeader_Canvas);

    }

    /// <summary>
    /// Canvas语言相关;
    /// </summary>
    public static partial class Constants {
        //默认图层标识/名称;
        public const string CanvasLayer_Default = nameof(CanvasLayer_Default);
        public const string CanvasLayerName_Default = nameof(CanvasLayerName_Default);


        public const string MenuTabGroupName_Edit = nameof(MenuTabGroupName_Edit);

        public const string MenuTabGroupName_Layers = nameof(MenuTabGroupName_Layers);


        public const string MenuItemName_Select = nameof(MenuItemName_Select);

        public const string MenuItemName_Undo = nameof(MenuItemName_Undo);

        public const string MenuItemName_Redo = nameof(MenuItemName_Redo);
        
        public const string MenuItemName_File = nameof(MenuItemName_File);


        public const string MenuItemName_CanvasContextMenu_DeleteSelectedDrawObjects = nameof(MenuItemName_CanvasContextMenu_DeleteSelectedDrawObjects);


        public const string MenuItemName_CanvasContextMenu_HideSelectedDrawObjects = nameof(MenuItemName_CanvasContextMenu_HideSelectedDrawObjects);


        public const string MenuItemName_CanvasContextMenu_CommitEdit = nameof(MenuItemName_CanvasContextMenu_CommitEdit);



        public const string MenuItemName_ShowAllDrawObjects = nameof(MenuItemName_ShowAllDrawObjects);


        


        public const string LanguageFormat_CurrentMousePosition = nameof(LanguageFormat_CurrentMousePosition);

        public const string StatusBarText_CurrentSnapPosition = nameof(StatusBarText_CurrentSnapPosition);

        public const string StatusBarText_AxisTrackingEnabled = nameof(StatusBarText_AxisTrackingEnabled);

        public const string StatusBarText_IsReadOnly = nameof(StatusBarText_IsReadOnly);


        public const string StatusBarText_Zoom = nameof(StatusBarText_Zoom);

        public const string StatusBarText_CurrentEditTool = nameof(StatusBarText_CurrentEditTool);


        public const string StatusBarText_IsSnapingEnabled = nameof(StatusBarText_IsSnapingEnabled);

        public const string StatusBarText_VertextMode = nameof(StatusBarText_VertextMode);

        public const string StatusBarText_DynamicInput = nameof(StatusBarText_DynamicInput);

        public const int StatusBarOrder_Zoom = 256;

        public const int StatusBarOrder_CurrentEditTool = 460;

        public const int StatusBarOrder_CanvasLayers = 1024;

        public const int StatusBarOrder_SnapingEnabled = 384;

        public const int StatusBarOrder_SnapingPosition = 288;

        public const int StatusBarOrder_VertexMode = 456;

        public const int StatusBarOrder_DynamicInput = 422;

        public const int StatusBarOrder_AxisTrackingEnabled = 458;

        public const int StatusBarOrder_IsReadOnly = 470;


        public const string InputInstruction_MultiSelect = nameof(InputInstruction_MultiSelect);

        public const int MenuItemOrder_CanvasContextMenu_DeleteSelectedDrawObjects = 256;
        public const int MenuItemOrder_CanvasContextMenu_Undo = 128;
        public const int MenuItemOrder_CanvasContextMenu_Redo = 192;
        public const int MenuItemOrder_CanvasContextMenu_CommitEdit = 48;

    }

    public static partial class Constants {

        /// <summary>
        /// 当前选定图层不可见时不可交互;
        /// </summary>
        public const string Exception_UnVisibleLayerNotInteractable = nameof(Exception_UnVisibleLayerNotInteractable);

    }
}
