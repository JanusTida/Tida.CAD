using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Tida.CAD
{
    /// <summary>
    /// The layer of cad;
    /// </summary>
    public class CADLayer : CADElement {
        /// <summary>
        /// Draw;
        /// </summary>
        /// <param name="canvas">画布</param>
        public override void Draw(ICanvas canvas) {
            
        }
   
        /// <summary>
        /// The drawobjects of the layer;
        /// </summary>
        private readonly List<DrawObject> _drawObjects = new List<DrawObject>();
        public IReadOnlyList<DrawObject> DrawObjects => new ReadOnlyCollection<DrawObject>(_drawObjects);
        
        /// <summary>
        /// Add draw object instance;
        /// </summary>
        /// <param name="drawObject"></param>
        public void AddDrawObject(DrawObject drawObject) {
            AddDrawObjectCore(drawObject);
            RaiseVisualChanged();
            DrawObjectsAdded?.Invoke(this,new DrawObject[] { drawObject });
        }
        
        /// <summary>
        /// Add a series of draw objects to the layer;
        /// </summary>
        /// <param name="drawObjects"></param>
        public void AddDrawObjects(IEnumerable<DrawObject> drawObjects) {
            if(drawObjects == null) {
                throw new ArgumentNullException(nameof(drawObjects));
            }

            foreach (var drawObject in drawObjects) {
                AddDrawObjectCore(drawObject);
            }

            RaiseVisualChanged();
            DrawObjectsAdded?.Invoke(this, drawObjects);
        }

        /// <summary>
        /// 添加绘制对象核心;
        /// </summary>
        /// <param name="drawObject"></param>
        private void AddDrawObjectCore(DrawObject drawObject) {
            if (drawObject == null) {
                throw new ArgumentNullException(nameof(drawObject));
            }

            //检查是否是独立的绘制元素;
            if (drawObject.Layer != null) {
                throw new InvalidOperationException("Please remove the drawObject from its parent first.");
            }
            
            _drawObjects.Add(drawObject);
            drawObject.InternalLayer = this;
            
        }

        /// <summary>
        /// 移除绘制元素;
        /// </summary>
        /// <param name="drawObject"></param>
        /// <param name="raiseRemoveEvent">是否触发移除事件</param>
        public void RemoveDrawObject(DrawObject drawObject) {
            RemoveDrawObjectCore(drawObject);

            RaiseVisualChanged();
            DrawObjectsRemoved?.Invoke(this,new DrawObject[] { drawObject });
        }
      
        /// <summary>
        /// 移除绘制元素集合;
        /// </summary>
        /// <param name="drawObjects"></param>
        public void RemoveDrawObjects(IEnumerable<DrawObject> drawObjects) {
            if(drawObjects == null) {
                throw new ArgumentNullException(nameof(drawObjects));
            }

            foreach (var drawObject in drawObjects) {
                if (_drawObjects.Contains(drawObject) && drawObject.Layer == this) {
                    RemoveDrawObjectCore(drawObject);
                }
            }

            RaiseVisualChanged();
            DrawObjectsRemoved?.Invoke(this, drawObjects);
        }

        /// <summary>
        /// 移除绘制元素核心;
        /// </summary>
        /// <param name="drawObject"></param>
        private void RemoveDrawObjectCore(DrawObject drawObject) {
            if (drawObject == null) {
                throw new ArgumentNullException(nameof(drawObject));
            }

            //检查绘制元素是否属于本实例;
            if (drawObject.Layer != this) {
                throw new InvalidOperationException("This instance doesn't own the drawObject.");
            }
            
            _drawObjects.Remove(drawObject);
            drawObject.InternalLayer = null;

            
        }

        /// <summary>
        /// 清除绘制元素;
        /// </summary>
        public void Clear() {
            DrawObjectsClearing?.Invoke(this, EventArgs.Empty);

            foreach (var drawObject in DrawObjects) {
                drawObject.InternalLayer = null;
            }
            
            _drawObjects.Clear();
            DrawObjectsCleared?.Invoke(this, EventArgs.Empty);
            RaiseVisualChanged();
        }

        /// <summary>
        /// 对象被移除事件;
        /// </summary>
        public event EventHandler<IEnumerable<DrawObject>> DrawObjectsRemoved;
        
        /// <summary>
        /// 对象被增加事件;
        /// </summary>
        public event EventHandler<IEnumerable<DrawObject>> DrawObjectsAdded;

         
        /// <summary>
        /// 绘制对象集合已经被清除事件;
        /// </summary>
        public event EventHandler DrawObjectsCleared;

        /// <summary>
        /// 绘制对象集合即将被清除事件;
        /// </summary>
        public event EventHandler DrawObjectsClearing;
    }


}
