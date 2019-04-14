namespace Tida.Canvas.Shell.EditTools {
   

    /// <summary>
    /// 图标;
    /// </summary>
    public static partial class Constants {
        const string Prefix = "pack://application:,,,/Tida.Canvas.Shell;component/Resources/";


        public const string EditToolIcon_Line = Prefix + "Line Shape.png";
        public const string EditToolIcon_SingleLine = Prefix + "Line Shape.png";
        public const string EditToolIcon_MultiLine = Prefix + "Line Shape.png";
        public const string EditToolIcon_Point = Prefix + "点.png";
        public const string EditToolIcon_Round = Prefix + "圆.png";
        public const string EditToolIcon_Rectangle = Prefix + "矩形.png";

        public const string EditToolIcon_Move = Prefix + "移动.png";
        public const string EditToolIcon_Copy = Prefix + "复制.png";
        public const string EditToolIcon_Mirror = Prefix + "镜像.png";
        public const string EditToolIcon_Trim = Prefix + "裁剪.png";
        public const string EditToolIcon_Offset = Prefix + "offset.png";
        public const string EditToolIcon_MeasureLength = Prefix + "Line Shape.png";

        public const string EditToolIcon_MeasureAngle = Prefix + "Angle.png";

    }

    /// <summary>
    /// 语言相关;
    /// </summary>
    static partial class Constants {
        public const string EditToolGroupName_BasicEditor = nameof(EditToolGroupName_BasicEditor);
        
        public const string EditToolGroupName_Round = nameof(EditToolGroupName_Round);


        public const string EditToolGroupName_Line = nameof(EditToolGroupName_Line);


        public const string EditToolGroupName_Rectangle = nameof(EditToolGroupName_Rectangle);


        public const string EditToolGroupName_Measure = nameof(EditToolGroupName_Measure);


        public const string EditToolName_SingleLine = nameof(EditToolName_SingleLine);

        public const string EditToolName_Point = nameof(EditToolName_Point);

        public const string EditToolName_MultiLine = nameof(EditToolName_MultiLine);

        public const string EditToolName_RoundDiameterTwoPoints = nameof(EditToolName_RoundDiameterTwoPoints);

        public const string EditToolName_RoundCenterRadiusPoints = nameof(EditToolName_RoundCenterRadiusPoints);

        public const string EditToolName_RectangleDiagLinePoints = nameof(EditToolName_RectangleDiagLinePoints);

        public const string EditToolName_MoveTool = nameof(EditToolName_MoveTool);


        public const string EditToolName_OffsetTool = nameof(EditToolName_OffsetTool);

        public const string EditToolName_CopyTool = nameof(EditToolName_CopyTool);
        public const string EditToolName_MirrorTool = nameof(EditToolName_MirrorTool);

        public const string EditToolName_TrimTool = nameof(EditToolName_TrimTool);


        public const string EditToolName_MeasureLength = nameof(EditToolName_MeasureLength);


        public const string EditToolName_MeasureAngle = nameof(EditToolName_MeasureAngle);

    }

    /// <summary>
    /// 裁剪工具提示;
    /// </summary>
    static partial class Constants {

        public const string EditToolTip_BeginText_Trim = nameof(EditToolTip_BeginText_Trim);
        public const string EditToolTip_EndText_Trim = nameof(EditToolTip_EndText_Trim);

        public const string EditToolTip_BeginText_Copy = nameof(EditToolTip_BeginText_Copy);
        public const string EditToolTip_EndText_Copy = nameof(EditToolTip_EndText_Copy);

        public const string EditToolTip_BeginText_Line = nameof(EditToolTip_BeginText_Line);
        public const string EditToolTip_EndText_Line = nameof(EditToolTip_EndText_Line);

        public const string EditToolTip_BeginText_Move = nameof(EditToolTip_BeginText_Move);
        public const string EditToolTip_EndText_Move = nameof(EditToolTip_EndText_Move);


        public const string EditToolTip_BeginText_Offset = nameof(EditToolTip_BeginText_Offset);
        public const string EditToolTip_EndText_Offset = nameof(EditToolTip_EndText_Offset);
        
        public const string EditToolTip_InputOffset = nameof(EditToolTip_InputOffset);
        public const string EditToolTip_SelectDrawObject = nameof(EditToolTip_SelectDrawObject);

        public const string EditToolTip_DrawObjectSelected = nameof(EditToolTip_DrawObjectSelected);

        public const string EditToolTip_BeginText_LengthMeasure = nameof(EditToolTip_BeginText_LengthMeasure);
        public const string EditToolTip_EndText_LengthMeasure = nameof(EditToolTip_EndText_LengthMeasure);

        public const string EditToolTip_BeginText_AngleMeasure = nameof(EditToolTip_BeginText_AngleMeasure);
        public const string EditToolTip_EndText_AngleMeasure = nameof(EditToolTip_EndText_AngleMeasure);


        public const string EditToolTip_AngleMeasure_ConfirmFirstMouseDownPosition = nameof(EditToolTip_AngleMeasure_ConfirmFirstMouseDownPosition);

        public const string EditToolTip_AngleMeasure_FirstMouseDownPositionConfirmed = nameof(EditToolTip_AngleMeasure_FirstMouseDownPositionConfirmed);

        public const string EditToolTip_AngleMeasure_SecondMouseDownPositionConfirmed = nameof(EditToolTip_AngleMeasure_SecondMouseDownPositionConfirmed);


        public const string EditToolTip_AngleMeasure_AngleCreated = nameof(EditToolTip_AngleMeasure_AngleCreated);

        public const string MenuItemName_MeasureShouldCommitMeasureData = nameof(MenuItemName_MeasureShouldCommitMeasureData);

    }
}
