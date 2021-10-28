using System;

namespace Tida.CAD
{
    /// <summary>
    /// The base class of things like Layer and drawobjects;
    /// </summary>
    public abstract class CADElement : IDrawable {
        private bool _isVisible = true;
        /// <summary>
        /// IsVisible;
        /// </summary>
        public bool IsVisible {
            get {
                return _isVisible;
            }
            set {
                if(_isVisible == value) {
                    return;
                }
                _isVisible = value;
                IsVisibleChanged?.Invoke(this, EventArgs.Empty);
                RaiseVisualChanged();
            }
        }

        /// <summary>
        /// Occurs when isvisible changed;
        /// </summary>
        public event EventHandler IsVisibleChanged;

        /// <summary>
        /// Occurs when the visual content changed;
        /// </summary>
        public event EventHandler VisualChanged;

        /// <summary>
        /// Notify the components that registered the <see cref="IsVisibleChanged"/>;
        /// </summary>
        public void RaiseVisualChanged() {
            VisualChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        ///   When overridden in a derived class, participates in rendering operations that
        //     are directly used by UI framework.
        /// </summary>
        /// <param name="canvas"></param>
        public virtual void Draw(ICanvas canvas) {

        }
    }
}
