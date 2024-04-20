namespace Tida.Canvas.Shell.Contracts.EditTools {
    /// <summary>
    /// 编辑工具所需常量;
    /// </summary>
    public static partial class Constants {
        /// <summary>
        /// 基本图形;
        /// </summary>
        public const string EditToolGroup_BasicEditor = nameof(EditToolGroup_BasicEditor);
        
        /// <summary>
        /// 圆;
        /// </summary>
        public const string EditToolGroup_Round = nameof(EditToolGroup_Round);

        /// <summary>
        /// 圆弧;
        /// </summary>
        public const string EditToolGroup_Arc = nameof(EditToolGroup_Arc);


        /// <summary>
        /// 直线;
        /// </summary>
        public const string EditToolGroup_Line = nameof(EditToolGroup_Line);

        /// <summary>
        /// 测量;
        /// </summary>
        public const string EditToolGroup_Measure = nameof(EditToolGroup_Measure);

        /// <summary>
        /// 矩形;
        /// </summary>
        public const string EditToolGroup_Rectangle = nameof(EditToolGroup_Rectangle);

        /// <summary>
        /// 编辑组-基本处理(复制、移动等);
        /// </summary>
        public const string EditToolGroup_EditBase = nameof(EditToolGroup_EditBase);

        public const string EditTool_SingleLine = nameof(EditTool_SingleLine);


        public const string EditTool_MultiLine = nameof(EditTool_MultiLine);


        public const string EditTool_ArcStartAndCenterThenEnd = nameof(EditTool_ArcStartAndCenterThenEnd);


        public const string EditTool_Point = nameof(EditTool_Point);

        public const string EditTool_RoundDiameterTwoPoints = nameof(EditTool_RoundDiameterTwoPoints);


        public const string EditTool_RoundCenterRadiusPoints = nameof(EditTool_RoundCenterRadiusPoints);

        public const string EditTool_RectangleDiagLinePoints = nameof(EditTool_RectangleDiagLinePoints);



        public const string EditTool_MoveTool = nameof(EditTool_MoveTool);


        public const string EditTool_OffsetTool = nameof(EditTool_OffsetTool);


        public const string EditTool_CopyTool = nameof(EditTool_CopyTool);

        public const string EditTool_MirrorTool = nameof(EditTool_MirrorTool);

        public const string EditTool_TrimTool = nameof(EditTool_TrimTool);


        public const string EditTool_MeasureLength = nameof(EditTool_MeasureLength);


        public const string EditTool_MeasureAngle = nameof(EditTool_MeasureAngle);

        public const int EditToolGroupOrder_Measure = 450;
        
        public const int EditToolGroupOrder_Basic = 256;

        public const int EditToolGroupOrder_Line = 4;

        public const int EditToolGroupOrder_Round = 12;

        public const int EditToolGroupOrder_Arc = 16;

        //public const int EditToolGroupOrder_Measure = 1024;
        //public const int EditToolGroupOrder_Measure = 1024;
    }

}
