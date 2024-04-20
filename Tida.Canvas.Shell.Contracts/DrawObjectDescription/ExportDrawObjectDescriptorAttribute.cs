using System;
using System.ComponentModel.Composition;

namespace Tida.Canvas.Shell.Contracts.DrawObjectDescription {
    /// <summary>
    /// 导出绘制对象描述器;
    /// </summary>
    [MetadataAttribute, AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class ExportDrawObjectDescriptorAttribute : ExportAttribute, IDrawObjectDescriptorMetaData {
        public ExportDrawObjectDescriptorAttribute() : base(typeof(IDrawObjectDescriptor)) {

        }

        public int Order { get; set; }
    }
}
