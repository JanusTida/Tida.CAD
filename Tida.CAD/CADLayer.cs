using System;
using System.Collections.Generic;

namespace Tida.CAD
{
    /// <summary>
    /// 画布图层;
    /// </summary>
    public class CADLayer : CADElement {
        /// <summary>
        /// 绘制本身;
        /// </summary>
        /// <param name="canvas">画布</param>
        public override void Draw(ICanvas canvas) {
            
        }
   
        /// <summary>
        /// 当前图层所呈现的绘制对象;
        /// </summary>
        private readonly List<DrawObject> _drawObjects = new List<DrawObject>();
        public IReadOnlyList<DrawObject> DrawObjects => _drawObjects;
        
        /// <summary>
        /// 添加绘制对象;
        /// </summary>
        /// <param name="drawObject"></param>
        public void AddDrawObject(DrawObject drawObject) {
            AddDrawObjectCore(drawObject);
            RaiseVisualChanged();
            DrawObjectsAdded?.Invoke(this,new DrawObject[] { drawObject });
        }
        
        /// <summary>
        /// 添加绘制对象集合;
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
            if (drawObject.Parent != null) {
                throw new InvalidOperationException("Please remove the drawObject from its parent first.");
            }
            
            _drawObjects.Add(drawObject);
            drawObject.InternalParent = this;
            
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
                if (_drawObjects.Contains(drawObject) && drawObject.Parent == this) {
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
            if (drawObject.Parent != this) {
                throw new InvalidOperationException("This instance doesn't own the drawObject.");
            }
            
            _drawObjects.Remove(drawObject);
            drawObject.InternalParent = null;

            
        }

        /// <summary>
        /// 清除绘制元素;
        /// </summary>
        public void Clear() {
            DrawObjectsClearing?.Invoke(this, EventArgs.Empty);

            foreach (var drawObject in DrawObjects) {
                drawObject.InternalParent = null;
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
