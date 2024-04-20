using Tida.Canvas.Shell.Contracts.Canvas;
using Prism.Mvvm;
using System;

namespace Tida.Canvas.Shell.Canvas.Models {
    /// <summary>
    /// 画布图层Model;
    /// </summary>
    class CanvasLayerModel:BindableBase,IDisposable {
        public CanvasLayerModel(CanvasLayerEx canvasLayer) {
            CanvasLayer = canvasLayer ?? throw new ArgumentNullException(nameof(canvasLayer));
            CanvasLayer.IsVisibleChanged += CanvasLayer_IsVisibleChanged;
        }

        private void CanvasLayer_IsVisibleChanged(object sender, EventArgs e) {
            RaisePropertyChanged(nameof(IsVisible));
        }

        public void Dispose() {
            CanvasLayer.IsVisibleChanged -= CanvasLayer_IsVisibleChanged;
        }

        public CanvasLayerEx CanvasLayer { get; }
        
        public bool IsVisible {
            get => CanvasLayer.IsVisible;
            set {
                CanvasLayer.IsVisible = value;
                RaisePropertyChanged();
            }
        }
        
        public string LayerName => CanvasLayer.LayerName;
    }
}
