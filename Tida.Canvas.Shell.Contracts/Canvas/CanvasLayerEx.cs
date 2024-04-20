using System;
using System.Collections.Generic;
using System.ComponentModel;
using Tida.Canvas.Contracts;
using Tida.Extending;

namespace Tida.Canvas.Shell.Contracts.Canvas {
    /// <summary>
    /// 可拓展图层,相对于<see cref="CanvasLayer"/>,本类别根据业务逻辑有所拓展;
    /// </summary>
    public partial class CanvasLayerEx:CanvasLayer,INotifyPropertyChanged,IExtensible {
        private readonly ExtensibleObject _extensibleObject = new ExtensibleObject();
        public CanvasLayerEx(string guid) {
            this.GUID = guid;
        }
        /// <summary>
        /// 图层名称;
        /// </summary>
        private string _layerName;
        public string LayerName {
            get {
                return _layerName;
            }
            set {
                _layerName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LayerName)));
            }
        }

        /// <summary>
        /// 类型唯一标识;
        /// </summary>
        public string GUID { get; }

        public event PropertyChangedEventHandler PropertyChanged;

    }

    /// <summary>
    /// 可拓展部分;
    /// </summary>
    public partial class CanvasLayerEx {

        public void SetInstance<TInstance>(TInstance instance, string extName) => _extensibleObject.SetInstance(instance, extName);

        public void RemoveInstance<TInstance>(string extName) => _extensibleObject.RemoveInstance<TInstance>(extName);

        public TInstance GetInstance<TInstance>(string extName) => _extensibleObject.GetInstance<TInstance>(extName);
    }

    /// <summary>
    /// 图层相关拓展;
    /// </summary>
    public static class CanvasLayerExtensions {
        /// <summary>
        /// 设定指定图层集合的可见状态;
        /// </summary>
        /// <param name="canvasDataContext"></param>
        private static void SetVisible(IEnumerable<CanvasLayer> layers, bool isVisible) {

            if (layers == null) {
                throw new ArgumentNullException(nameof(layers));
            }

            foreach (var layer in layers) {
                layer.IsVisible = isVisible;
            }
        }
    }
}
