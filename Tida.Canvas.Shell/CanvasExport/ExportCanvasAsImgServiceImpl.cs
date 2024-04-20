using Tida.Canvas.Contracts;
using Tida.Canvas.Shell.Contracts.CanvasExport;
using Tida.Geometry.Primitives;
using Tida.Canvas.WPFCanvas;
using Tida.Canvas.WPFCanvas.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static Tida.Canvas.WPFCanvas.WindowsCanvasScreenConverter;

namespace Tida.Canvas.Shell.CanvasExport {
    /// <summary>
    /// 导出为图像服务实现;
    /// </summary>
    [Export(typeof(IExportCanvasAsImgService))]
    public class ExportCanvasAsImgServiceImpl : IExportCanvasAsImgService {
        public void ExportDrawObjectsAsImg(IEnumerable<DrawObject> drawObjects, ExportImgSetting exportImgSetting) {

            if (drawObjects == null) {
                throw new ArgumentNullException(nameof(drawObjects));
            }

            if (exportImgSetting == null) {
                throw new ArgumentNullException(nameof(exportImgSetting));
            }

            if (exportImgSetting.ExportStream == null) {
                throw new ArgumentNullException(nameof(exportImgSetting.ExportStream));
            }

            var stream = exportImgSetting.ExportStream;
            if (!stream.CanWrite) {
                throw new ArgumentException($"{nameof(stream)} can't be written.");
            }

            var width = exportImgSetting.Width;
            var height = exportImgSetting.Height;

            var renderTargetBitMap = new RenderTargetBitmap(
                width, height,
                ScreenResolution,
                ScreenResolution, 
                PixelFormats.Default
            );

            ///使用<see cref="DrawingVisual"/>作为<see cref="Visual"/>参数;
            var drawingVisual = new DrawingVisual {
                Clip = new RectangleGeometry(new Rect {
                    Width = width,
                    Height = height
                })
            };

            var drawingContext = drawingVisual.RenderOpen();
            
            var canvasProxy = new WindowsCanvasScreenConverter {
                ActualWidth = width,
                ActualHeight = height,
                PanScreenPosition = new Vector2D(width / 2, height / 2),
                Zoom = 1
            };

            var canvas = new WindowsCanvas(canvasProxy) {
                DrawingContext = drawingContext
            };

            var scope = GetScopeWithDrawObjects(drawObjects, width, height);

            canvasProxy.Zoom = scope.Zoom;
            canvasProxy.PanScreenPosition = scope.PanScreenPosition;

            //绘制背景;
            if(exportImgSetting.Background != null) {
                drawingContext.DrawRectangle(BrushAdapter.ConvertToSystemBrush(exportImgSetting.Background),null,new Rect(new System.Windows.Size(width,height)));
            }

            //绘制绘制对象;
            foreach (var drawObject in drawObjects.Where(p => exportImgSetting.ExportUnvisible || p.IsVisible)) {
                drawObject.Draw(canvas, canvasProxy);
            }

            drawingContext.Close();

            renderTargetBitMap.Render(drawingVisual);

            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(renderTargetBitMap));
            encoder.Save(stream);
            
#if DEBUG
            //if (stream.Length > 0)
            //{

            //    using (var fs = File.Create($"D://Random/{Guid.NewGuid().ToString("P")}.png"))
            //    {
            //        stream.Position = 0;
            //        stream.CopyTo(fs);
            //    }
            //}
#endif
        }   

        /// <summary>
        /// 根据<paramref name="drawObjects"/>集合获取一个理想的放大倍数,在该放大倍数下,可以确保所有的绘制对象均可见;
        /// </summary>
        /// <param name="drawObjects"></param>
        /// <returns></returns>
        private static CanvasViewScope GetScopeWithDrawObjects(IEnumerable<DrawObject> drawObjects,double actualWidth,double actualHeight) {
            var scope = new CanvasViewScope {
                PanScreenPosition = new Vector2D(actualWidth / 2, actualHeight / 2),
                Zoom = DefaultZoom
            };
            
            //获取所有绘制对象所在的矩形;
            var rects = drawObjects.Select(p => p.GetBoundingRect()).Where(p => p != null);

            var allVertexes = rects.SelectMany(p => p.GetVertexes()).ToArray();

            if (allVertexes.Length != 0) {
                var minX = allVertexes.Min(p => p.X);
                var maxX = allVertexes.Max(p => p.X);
                var minY = allVertexes.Min(p => p.Y);
                var maxY = allVertexes.Max(p => p.Y);

                var rectScreenWidth = GetScreenLength( maxX - minX,DefaultZoom);
                var rectScreenHeight = GetScreenLength( maxY - minY,DefaultZoom);

                scope.Zoom = Math.Min(actualHeight / rectScreenHeight, actualWidth / rectScreenWidth);

                var middleX = (minX + maxX) / 2;
                var middleY = (minY + maxY) / 2;

                scope.PanScreenPosition = new Vector2D(
                    actualWidth / 2 - GetScreenLength(middleX,scope.Zoom),
                    actualHeight / 2 + GetScreenLength(middleY,scope.Zoom)
                );
            }

            return scope;
        }


        /// <summary>
        /// 返回在默认指定比例下,<paramref name="unitValue"/>在视图上的长度;
        /// </summary>
        /// <param name="unitValue"/>
        /// <returns></returns>
        private static double GetScreenLength(double unitValue,double zoom) {
            return unitValue * zoom * ScreenResolution;
        }

        
        /// <summary>
        /// 描述画布的位置以及放大倍数的相关信息的实体;
        /// </summary>
        struct CanvasViewScope {

            /// <summary>
            /// 放大倍数;
            /// </summary>
            public double Zoom { get; set; } 

            /// <summary>
            /// 原点位置;
            /// </summary>
            public Vector2D PanScreenPosition { get; set; }
        }
    }
}
