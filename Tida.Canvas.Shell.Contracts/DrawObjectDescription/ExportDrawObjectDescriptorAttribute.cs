using System;
using System.ComponentModel.Composition;
using Tida.Canvas.Infrastructure.ComponentModel;

namespace Tida.Canvas.Shell.Contracts.DrawObjectDescription {
    [MetadataAttribute,AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class ExportDrawObjectDescriptorAttribute:ExportAttribute,IDrawObjectDescriptorMetaData {
        public ExportDrawObjectDescriptorAttribute() : base(typeof(IDrawObjectDescriptor)) {
            
        }

        public int Order { get; set; }
    }
}
