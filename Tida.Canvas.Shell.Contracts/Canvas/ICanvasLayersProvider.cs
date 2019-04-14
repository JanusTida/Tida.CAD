using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace Tida.Canvas.Shell.Contracts.Canvas {
    /// <summary>
    /// 图层提供者;
    /// </summary>
    public interface ICanvasLayersProvider {
        /// <summary>
        /// 创建图层集合;
        /// </summary>
        IEnumerable<CanvasLayerEx> CreateLayers();
    }

    [MetadataAttribute, AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class ExportCanvasLayersProviderAttribute : ExportAttribute , ICanvasLayersProviderMetadata {
        public ExportCanvasLayersProviderAttribute():base(typeof(ICanvasLayersProvider)) {

        }

        public int Priority { get; set; }
    }

    /// <summary>
    /// <see cref="ICanvasLayersProvider"/>的相关元数据;
    /// </summary>
    public interface ICanvasLayersProviderMetadata {
        /// <summary>
        /// 优先级;
        /// </summary>
        int Priority { get; }
    }
}
