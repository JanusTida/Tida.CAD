
using Tida.Canvas.Contracts;
using Tida.Application.Contracts.App;

namespace Tida.Canvas.Shell.Contracts.DrawObjectDescription {
    /// <summary>
    /// 根据键值查找描述信息的绘制对象描述器基类;
    /// </summary>
    /// <typeparam name="TDrawObject"></typeparam>
    public abstract class LanguageDrawObjectDescriptorGenericBase<TDrawObject> : DrawObjectDescriptorGenericBase<TDrawObject> where TDrawObject : DrawObject {
        protected sealed override string TypeName {
            get {
                if (_typeName == null) {
                    _typeName = LanguageService.FindResourceString(TypeLanguageKey);
                }

                return _typeName;
            }
        }

        private string _typeName;
        /// <summary>
        /// 键值;
        /// </summary>
        public abstract string TypeLanguageKey { get; }
    }
}
