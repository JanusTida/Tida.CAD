using Tida.Canvas.Contracts;
using Prism.Mvvm;
using System;

namespace Tida.Canvas.Base.Dialogs.Models {
    class DrawObjectModel:BindableBase {
        public DrawObjectModel(DrawObject drawObject) {

            DrawObject = drawObject ?? throw new ArgumentNullException(nameof(drawObject));

        }

        public DrawObject DrawObject { get; }

        /// <summary>
        /// 类型名;
        /// </summary>
        public string TypeName { get; set; }
    }
}
