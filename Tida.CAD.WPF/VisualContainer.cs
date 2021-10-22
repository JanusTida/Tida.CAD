using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Tida.CAD.WPF
{
    /// <summary>
    /// 可视化对象，实现了向界面上添加可视化对象的功能，并且能够获取可视化对象的数量
    /// </summary>
    public class VisualContainer : FrameworkElement {
        public VisualContainer() {
            this.Focusable = true;
        }
        
        /// <summary>
        /// 当前所有的可见对象
        /// </summary>
        private readonly List<Visual> _visuals = new List<Visual>();
        
        //获取Visual的个数
        protected override int VisualChildrenCount => _visuals.Count;

        /// <summary>
        /// 获取界面上所有的可视化对象
        /// </summary>
        public IEnumerable<Visual> Visuals => _visuals.Select(p => p);

        /// <summary>
        /// 获取Visual
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected override Visual GetVisualChild(int index) {
            if (index < 0 || index >= this._visuals.Count) {

                return null;
            }
            return _visuals[index];
        }
        
        /// <summary>
        /// 添加Visual
        /// </summary>
        /// <param name="visual"></param>
        public void AddVisual(Visual visual) {
            _visuals.Add(visual);
            base.AddVisualChild(visual);
            base.AddLogicalChild(visual);
        }
        /// <summary>
        /// 插入Visual;
        /// </summary>
        /// <param name="index"></param>
        /// <param name="visual"></param>
        public void InsertVisual(int index, Visual visual)
        {
            _visuals.Insert(index, visual);
            base.AddVisualChild(visual);
            base.AddLogicalChild(visual);
        }
        /// <summary>
        /// 删除Visual
        /// </summary>
        /// <param name="visual"></param>
        public void RemoveVisual(Visual visual) {
            if (!_visuals.Contains(visual)) {
                throw new InvalidOperationException($"The Visual Children doesn't contain the visual.");
            }

            _visuals.Remove(visual);
            base.RemoveVisualChild(visual);
            base.RemoveLogicalChild(visual);
        }


        /// <summary>
        /// 清除视图;
        /// </summary>
        protected void ClearVisuals() {
            foreach (var visual in _visuals) {
                base.RemoveVisualChild(visual);
                base.RemoveLogicalChild(visual);
            }
            _visuals.Clear();
        }

        /// <summary>
        /// 命中测试
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        protected Visual GetVisual(Point point) {
            HitTestResult hitResult = VisualTreeHelper.HitTest(this, point);
            return hitResult.VisualHit as Visual;
        }



    }
}
