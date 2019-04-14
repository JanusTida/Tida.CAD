using Tida.Application.Contracts.App;
using Tida.Application.Contracts.Common;
using Tida.Canvas.Contracts;
using Tida.Canvas.Shell.Contracts.Canvas.Events;
using Tida.Util.Extending;
using Tida.Geometry.Primitives;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Tida.Canvas.Shell.Contracts.Canvas {
    /// <summary>
    /// 业务层的画布的上下文数据;
    /// </summary>
    public interface ICanvasDataContext : IExtensible {
        /// <summary>
        /// 是否只读;本控件及其内容将无法通过输入设备被操作;
        /// </summary>
        bool IsReadOnly { get; set; }

        /// <summary>
        /// 图层集合;
        /// </summary>
        ObservableCollection<CanvasLayerEx> Layers { get; }

        /// <summary>
        /// 当前活跃图层;
        /// </summary>
        CanvasLayerEx ActiveLayer { get; set; }

        /// <summary>
        /// 坐标转换器;
        /// </summary>
        ICanvasScreenConvertable CanvasProxy { get; }

        /// <summary>
        /// 当前缩放比例;
        /// </summary>
        double Zoom { get; set; }

        /// <summary>
        /// 当前鼠标所在的工程数学坐标;
        /// </summary>
        Vector2D CurrentMousePosition { get; }

        /// <summary>
        /// 与鼠标位置相关的,画布当前的辅助图形;
        /// </summary>
        ISnapShape MouseHoverSnapShape { get; }

        /// <summary>
        /// 原点所在视图位置;
        /// </summary>
        Vector2D PanScreenPosition { get; set; }

        /// <summary>
        /// 辅助是否可用;
        /// </summary>
        bool IsSnapingEnabled { get; set; }
        

        /// <summary>
        /// 当前使用的编辑工具;
        /// </summary>
        EditTool CurrentEditTool { get; set; }

        /// <summary>
        /// 当前编辑工具发生了变化;
        /// </summary>
        //event EventHandler CurrentEditToolChanged;

        /// <summary>
        /// 清空事务栈;
        /// </summary>
        void ClearTransactions();

        /// <summary>
        /// 呈递事务;
        /// </summary>
        void CommitTransaction(IEditTransaction editTransaction);

        /// <summary>
        /// 重置图层状态;使得图层集合仅存在默认图层;
        /// </summary>
        void ResetLayers();

        /// <summary>
        /// 撤销;
        /// </summary>
        void Undo();

        /// <summary>
        /// 能否撤销;
        /// </summary>
        bool CanUndo { get; }

        /// <summary>
        /// 重做;
        /// </summary>
        void Redo();

        /// <summary>
        /// 能否重做;
        /// </summary>
        bool CanRedo { get; }

        
    }

    /// <summary>
    /// 业务层的画布的上下文数据拓展;
    /// </summary>
    public static class CanvasDataContextExtensions {
        /// <summary>
        /// 调整画布位置和缩放比例,以使得所有绘制对象在可见的范围内;
        /// </summary>
        public static void ViewAllDrawObjects(this ICanvasDataContext canvasDataContext) {
            if (canvasDataContext == null) {
                throw new ArgumentNullException(nameof(canvasDataContext));
            }

            if(canvasDataContext.CanvasProxy == null) {
                return;
            }

            if(canvasDataContext.Layers == null) {
                return;
            }
            
            //获取所有绘制对象所在的矩形;
            var rects = canvasDataContext.Layers.
                SelectMany(p => p.DrawObjects).
                Select(p => p.GetBoundingRect()).Where(p => p != null);

            var allVertexes = rects.SelectMany(p => p.GetVertexes()).ToArray();

            if(allVertexes.Length == 0) {
                return;
            }

            //取所有矩形的最小/大的横/纵坐标;
            //获得关注区域的信息,这将是一个矩形;(长度/宽度可能为零);
            var minX = allVertexes.Min(p => p.X);
            var maxX = allVertexes.Max(p => p.X);
            var minY = allVertexes.Min(p => p.Y);
            var maxY = allVertexes.Max(p => p.Y);

            var canvasProxy = canvasDataContext.CanvasProxy;
            var actualWidth = canvasProxy.ActualWidth;
            var actualHeight = canvasProxy.ActualHeight;

            //计算该矩形区域的中点位置;
            var middleX = (minX + maxX) / 2;
            var middleY = (minY + maxY) / 2;
            
            //计算该矩形区域的长宽;加上一个常数是为了防止该矩形区域中任意一边为零的情况,导致出现除以零的异常;
            var newWidth = canvasProxy.ToUnit(canvasProxy.ToScreen(maxX - minX) + SavedSpace);
            var newHeight = canvasProxy.ToUnit(canvasProxy.ToScreen(maxY - minY) + SavedSpace);

            //与当前放大比例下的矩形区域大小进行比例计算,以计算缩放应该乘以的倍数;
            var timeX = canvasProxy.ToUnit(actualWidth) / newWidth;
            var timeY = canvasProxy.ToUnit(actualHeight) / newHeight;

            canvasDataContext.Zoom *= Math.Min(timeX,timeY);
            
            //通过改变原点所在的视图坐标,将该矩形区域的中点移动至视图中心位置;
            //计算该中点与原点的视图偏移;
            var middlePointToPanOffset = 
                canvasProxy.ToScreen(new Vector2D(middleX, middleY)) -
                canvasProxy.ToScreen(Vector2D.Zero);

            //以下操作等效于将原点平移至视图中心位置后再将其向矩形区域中点相反的方向进行平移;
            canvasDataContext.PanScreenPosition = new Vector2D(actualWidth / 2, actualHeight / 2) - middlePointToPanOffset;
        }
        
        /// <summary>
        /// 在画布上下文中,获取所有特定类型的可见绘制对象;
        /// </summary>
        /// <typeparam name="TDrawObject"></typeparam>
        /// <param name="canvasDataContext"></param>
        /// <returns></returns>
        public static IEnumerable<TDrawObject> GetAllVisibleDrawObjects<TDrawObject>(this ICanvasDataContext canvasDataContext) where TDrawObject:DrawObject{

            if (canvasDataContext == null) {
                throw new ArgumentNullException(nameof(canvasDataContext));
            }

            return canvasDataContext.GetAllDrawObjects<TDrawObject>()?.Where(p => p.IsVisible);
        }

        /// <summary>
        /// 获取所有指定类型的绘制对象;
        /// </summary>
        /// <typeparam name="TDrawObject"></typeparam>
        /// <param name="canvasDataContext"></param>
        /// <returns></returns>
        public static IEnumerable<TDrawObject> GetAllDrawObjects<TDrawObject>(this ICanvasDataContext canvasDataContext) where TDrawObject : DrawObject {
            if (canvasDataContext == null) {
                throw new ArgumentNullException(nameof(canvasDataContext));
            }

            return canvasDataContext.GetAllDrawObjects()?.OfType<TDrawObject>();
        }

        /// <summary>
        /// 获取所有的绘制对象;
        /// </summary>
        /// <param name="canvasDataContext"></param>
        /// <returns></returns>
        public static IEnumerable<DrawObject> GetAllDrawObjects(this ICanvasDataContext canvasDataContext) {
            if (canvasDataContext == null) {
                throw new ArgumentNullException(nameof(canvasDataContext));
            }

            if(canvasDataContext.Layers == null) {
                throw new ArgumentNullException(nameof(canvasDataContext.Layers));
            }

            return canvasDataContext.Layers.SelectMany(p => p.DrawObjects);
        }

        public static IEnumerable<DrawObject> GetAllVisibleDrawObjects(this ICanvasDataContext canvasDataContext) {
            if (canvasDataContext == null) {
                throw new ArgumentNullException(nameof(canvasDataContext));
            }

            return canvasDataContext.GetAllDrawObjects()?.Where(p => p.IsVisible);
        }

        /// <summary>
        /// 将当前<paramref name="canvasDataContext"/>中的未完成编辑完成,并将<see cref="ICanvasDataContext.CurrentEditTool"/>置空;
        /// </summary>
        /// <param name="canvasDataContext"></param>
        public static void CommitEdit(this ICanvasDataContext canvasDataContext) {

            if (canvasDataContext == null) {
                throw new ArgumentNullException(nameof(canvasDataContext));
            }

            canvasDataContext.CurrentEditTool = null;
        }

        /// <summary>
        /// 清除画布中所有的画布中所有的绘制对象;
        /// </summary>
        public static void RemoveAllDrawObjects(this ICanvasDataContext canvasDataContext) {

            if (canvasDataContext == null) {
                throw new ArgumentNullException(nameof(canvasDataContext));
            }

            if (canvasDataContext.Layers == null) {
                return;
            }

            foreach (var layer in canvasDataContext.Layers) {
                layer.Clear();
            }
            
        }

        /// <summary>
        /// 当前上下文中的所有图层的可见状态;
        /// </summary>
        /// <param name="canvasDataContext"></param>
       public static void SetAllLayersVisible(this ICanvasDataContext canvasDataContext,bool isVisible) {

            if (canvasDataContext == null) {
                throw new ArgumentNullException(nameof(canvasDataContext));
            }

            foreach (var layer in canvasDataContext.Layers) {
                layer.IsVisible = isVisible;
            }
            
        }

        /// <summary>
        /// 移除所有选中绘制对象;将自动呈递事务;
        /// </summary>
        public static void RemoveSelectedDrawObjects(this ICanvasDataContext canvasDataContext) {

            if (canvasDataContext == null) {
                throw new ArgumentNullException(nameof(canvasDataContext));
            }

            //处于编辑状态时不能进行删除;
            if (canvasDataContext.CurrentEditTool != null) {
                return;
            }

            //选定画布中所有的选中的绘制对象为即将移除的对象;
            var allDrawObjectsToBeRemoved = CanvasService.CanvasDataContext?.GetAllVisibleDrawObjects().Where(p => p.IsSelected).ToList();
            if (allDrawObjectsToBeRemoved == null || allDrawObjectsToBeRemoved.Count == 0) {
                return;
            }

            canvasDataContext.RemoveDrawObjects(allDrawObjectsToBeRemoved);
        }

        
        public static  void RemoveDrawObjects(this ICanvasDataContext canvasDataContext,ICollection<DrawObject> drawObjects) {

            if (drawObjects == null) {
                throw new ArgumentNullException(nameof(drawObjects));
            }

            if (drawObjects.Count == 0) {
                return;
            }

            //激发即将移除事件;
            var removingArgs = new CanvasDrawObjectsRemovingEventArgs(drawObjects, canvasDataContext);
            CommonEventHelper.Publish<CanvasDrawObjectsRemovingEvent, CanvasDrawObjectsRemovingEventArgs>(removingArgs);
            CommonEventHelper.PublishEventToHandlers<ICanvasDrawObjectsRemovingEventHandler, CanvasDrawObjectsRemovingEventArgs>(removingArgs);
            //若指示取消或集合为空,则不继续执行;
            if (removingArgs.Cancel || removingArgs.RemovingDrawObjects.Count == 0) {
                return;
            }

            var removingGroups = removingArgs.RemovingDrawObjects.
                GroupBy(p => p.Parent as CanvasLayer).Where(p => p.Key != null).ToArray();

            void RemoveDrawObjects() {
                try {
                    foreach (var tuple in removingGroups) {
                        tuple.Key.RemoveDrawObjects(tuple);
                    }
                }
                catch (Exception ex) {
                    LoggerService.WriteException(ex);
                    MsgBoxService.ShowError(ex.Message);
                }
            }

            void AddDrawObjects() {
                try {
                    foreach (var tuple in removingGroups) {
                        tuple.Key.AddDrawObjects(tuple);
                    }
                }
                catch (Exception ex) {
                    LoggerService.WriteException(ex);
                    MsgBoxService.ShowError(ex.Message);
                }
            }

            RemoveDrawObjects();

            var action = new StandardEditTransaction(AddDrawObjects, RemoveDrawObjects);

            canvasDataContext.CommitTransaction(action);
        }

        public static void AddDrawObjects(this ICanvasDataContext canvasDataContext,ICollection<DrawObject> drawObjects) {
            if (drawObjects == null) {
                throw new ArgumentNullException(nameof(drawObjects));
            }

            if (drawObjects.Count == 0) {
                return;
            }

            var activeLayer = canvasDataContext.ActiveLayer;
            if (activeLayer == null) {
                return;
                //throw new InvalidOperationException(LanguageService.FindResourceString(Exception_ActiveLayerCannotBeNull));
            }

            void AddDrawObjects() {
                activeLayer.AddDrawObjects(drawObjects);
            };

            void RemoveDrawObjects() {
                activeLayer.RemoveDrawObjects(drawObjects);
            }

            AddDrawObjects();

            var transaction = new StandardEditTransaction(RemoveDrawObjects, AddDrawObjects);

            canvasDataContext.CommitTransaction(transaction);
        }
        /// <summary>
        /// 保留的空余间距;
        /// </summary>
        private const double SavedSpace = 200;
    }


}
