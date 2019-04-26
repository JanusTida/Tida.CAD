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
