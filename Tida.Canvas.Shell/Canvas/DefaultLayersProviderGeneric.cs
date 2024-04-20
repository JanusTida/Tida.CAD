using Tida.Canvas.Shell.Contracts.App;
using Tida.Canvas.Shell.Contracts.Canvas;
using System.Collections.Generic;

namespace Tida.Canvas.Shell.Canvas {
    /// <summary>
    /// 真-默认图层提供器;(优先级别较低)
    /// </summary>
    [ExportCanvasLayersProvider(Priority = 0)]
    class DefaultLayersProviderGeneric : ICanvasLayersProvider {
        public IEnumerable<CanvasLayerEx> CreateLayers() => new CanvasLayerEx[]{
            new CanvasLayerEx(Constants.CanvasLayer_Default) {
                LayerName = LanguageService.FindResourceString(Constants.CanvasLayerName_Default)
            }
        };
    }
}
