using System;
using System.ComponentModel.Composition;
using Tida.Canvas.Infrastructure.DynamicInput;

namespace Tida.Canvas.Shell.Contracts.DynamicInput {
    /// <summary>
    /// 导出动态输入提供器;
    /// </summary>
    [MetadataAttribute, AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ExportCanvasControlDynamicInputerProviderAttribute : ExportAttribute, ICanvasControlDynamicInputerProviderMetaData {
        public ExportCanvasControlDynamicInputerProviderAttribute() : base(typeof(ICanvasControlDynamicInputerProvider)) {

        }

        public int Order { get; set; }
    }
}
