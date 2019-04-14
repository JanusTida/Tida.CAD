namespace Tida.Canvas.Shell.Contracts {
    /// <summary>
    /// 图层相关常量;
    /// </summary>
    public static partial class Constants {
        //默认图层;
        public const string CanvasLayer_Default = nameof(CanvasLayer_Default);

        /// <summary>
        /// 设定节标识——画布
        /// </summary>
        public const string SettingSection_Canvas = nameof(SettingSection_Canvas);

        /// <summary>
        /// 设定——正交模式可用状态;
        /// </summary>
        //public const string SettingName_VertexMode = nameof(SettingName_VertexMode);

        /// <summary>
        /// 设定——动态输入;
        /// </summary>
        public const string SettingName_DynamicInput = nameof(SettingName_DynamicInput);

        /// <summary>
        /// 设定——极轴追踪;
        /// </summary>
        public const string SettingName_AxisTrackingEnabled = nameof(SettingName_AxisTrackingEnabled);


        /// <summary>
        /// 设定——只读;
        /// </summary>
        public const string SettingName_IsReadOnly = nameof(SettingName_IsReadOnly);

        /// <summary>
        /// 标识将在绘制对象序列化时使用的转换器类型;
        /// </summary>
        public const string ConverterType_SerializingDrawObjects   = nameof(ConverterType_SerializingDrawObjects);
    }

}
