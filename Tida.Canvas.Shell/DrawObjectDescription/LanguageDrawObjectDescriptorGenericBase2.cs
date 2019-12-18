using Tida.Canvas.Shell.Contracts.App;
using Tida.Canvas.Contracts;
using Tida.Canvas.Shell.Contracts.DrawObjectDescription;

namespace Tida.Canvas.Shell.DrawObjectDescription {
    /// <summary>
    /// 根据键值查找描述信息的绘制对象描述器基类;
    /// </summary>
    /// <typeparam name="TDrawObject"></typeparam>
    public abstract class LanguageDrawObjectDescriptorGenericBase2<TDrawObject> : DrawObjectDescriptorGenericBase2<TDrawObject> where TDrawObject : DrawObject {
        protected sealed override string GetTypeName(TDrawObject drawObject){
            if (_typeName == null && CheckIsValidDrawObject(drawObject)) {
                _typeName = LanguageService.FindResourceString(TypeLanguageKey);
            }

            return _typeName;
        }

        private string _typeName;
        /// <summary>
        /// 键值;
        /// </summary>
        public abstract string TypeLanguageKey { get; }
    }
}
