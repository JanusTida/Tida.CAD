using System.Windows;

namespace Tida.CAD
{
    /// <summary>
    /// 根据画布信息,画布的交互信息的预处理器;
    /// 本接口适用范围为在通知编辑工具或绘制对象前，需预处理关注位置的情况,
    /// 如"正交模式"下的关注位置需在编辑工具或绘制对象处理之前进行修改;
    /// 主代码将处理器返回的结果用于通知编辑工具或绘制对象;
    /// </summary>
    public abstract class CADInteractionHandler : CADElement, IDrawable {
        private ICADControl _cadControl;
        /// <summary>
        /// 对应的画布控件;
        /// 在更改本属性的值时,将会调用<see cref="OnUnLoad(ICADControl)"/>以及<see cref="OnLoad(ICADControl)"/>
        /// </summary>
        public ICADControl CADControl {
            get => _cadControl;
            set {
                if (_cadControl == value) {
                    return;
                }

                if (_cadControl != null) {
                    OnUnLoad(_cadControl);
                }

                _cadControl = value;

                if(_cadControl != null) {
                    OnLoad(_cadControl);
                }
            }
        }
        
        public virtual void HandlePosition(ICADControl cadContext, Point originPosition) { }

        protected virtual void OnLoad(ICADControl cadControl) { }

        protected virtual void OnUnLoad(ICADControl cadControl) { }
        
    }
}
