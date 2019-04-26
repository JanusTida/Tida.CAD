using Tida.Canvas.Infrastructure.Contracts;
using Tida.Canvas.Infrastructure.Utils;
using Tida.Canvas.Contracts;
using Tida.Canvas.Input;
using Tida.Geometry.External;
using Tida.Geometry.External.Util;
using Tida.Geometry.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using static Tida.Canvas.Infrastructure.Constants;
using static Tida.Canvas.Infrastructure.Utils.LineHitUtils;

namespace Tida.Canvas.Infrastructure.DrawObjects {
    /// <summary>
    /// 绘制对象,角;
    /// </summary>
    public class MeasureAngle : MousePositionTrackableDrawObject {
        /// <summary>
        /// 角的构造,本构造函数所有参数均不能为空;
        /// </summary>
        /// <param name="start">角的某一端端点</param>
        /// <param name="vertex">角的顶点</param>
        /// <param name="end">角的另一端端点</param>
        public MeasureAngle(Vector2D start,Vector2D vertex,Vector2D end) {
            Start = start ?? throw new ArgumentNullException(nameof(start));
            Vertex = vertex ?? throw new ArgumentNullException(nameof(vertex));
            End = end ?? throw new ArgumentNullException(nameof(end));
        }
        
        private Vector2D _start;
        /// <summary>
        /// 角的某一端端点
        /// </summary>
        public Vector2D Start {
            get => _start;
            set {
                if(value == null) {
                    throw new ArgumentNullException(nameof(value));
                }

                SetProperty(pos => _start = pos, () => _start, value, false);
            }
        }

        private Vector2D _end;
        /// <summary>
        /// 角的另一端端点;
        /// </summary>
        public Vector2D End {
            get => _end;
            set {
                if (value == null) {
                    throw new ArgumentNullException(nameof(value));
                }

                SetProperty(pos => _end = pos, () => _end, value, false);
            }
        }

        private Vector2D _vertex;
        /// <summary>
        /// 角的顶点;
        /// </summary>
        public Vector2D Vertex {
            get => _vertex;
            set {
                if (value == null) {
                    throw new ArgumentNullException(nameof(value));
                }

                SetProperty(pos => _vertex = pos, () => _vertex, value, false);
            }
        }

        

        public override DrawObject Clone() {
            return new MeasureAngle(Start, Vertex, End);
        }

        public override Rectangle2D2 GetBoundingRect() {
            var minX = GetImportantPositions().Min(p => p.X);
            var maxX = GetImportantPositions().Max(p => p.X);
            var minY = GetImportantPositions().Min(p => p.Y);
            var maxY = GetImportantPositions().Max(p => p.Y);

            var mediumY = (minX + maxY) / 2;

            return new Rectangle2D2(new Line2D(new Vector2D(minX,mediumY),new Vector2D(maxX,mediumY)), maxY - minY);
        }

        public override bool ObjectInRectangle(Rectangle2D2 rect, ICanvasScreenConvertable canvasProxy, bool anyPoint) {
            //若为任意选中,角任意一条边与矩形存在包含或相交关系即可;
            if (anyPoint) {
                return LineInRectangle(new Line2D(Vertex, Start), rect, true) ||
                    LineInRectangle(new Line2D(Vertex, End), rect, true);
            }
            else {
                return rect.Contains(Start) && rect.Contains(End) && rect.Contains(Vertex);
            }
        }

        public override bool PointInObject(Vector2D point, ICanvasScreenConvertable canvasProxy) {

            var pointInLines = PointInLine(new Line2D(Start, Vertex),point,canvasProxy) || PointInLine(new Line2D(End,Vertex),point,canvasProxy);
            
            return pointInLines;
        }

        public override void Draw(ICanvas canvas, ICanvasScreenConvertable canvasProxy) {
            base.Draw(canvas, canvasProxy);
            DrawLinesAndArc(canvas, canvasProxy);
            DrawSelectedState(canvas, canvasProxy);
        }

        private void DrawLinesAndArc(ICanvas canvas,ICanvasScreenConvertable canvasProxy) {
            var cell = GetPreviewCell();
            if (cell == null) {
                cell = new AngleThreePointsCell {
                    Start = Start,
                    Vertex = Vertex,
                    End = End
                };
            };

            var vertextToStartLine2D = new Line2D(cell.Vertex, cell.Start);
            var vertextToEndLine2D = new Line2D(cell.Vertex,cell.End);

            canvas.DrawLine(MeasureLengthPen, vertextToStartLine2D);
            canvas.DrawLine(MeasureLengthPen, vertextToEndLine2D);

            var vertexToStartDir = vertextToStartLine2D.Direction;
            var vertexToEndDir = vertextToEndLine2D.Direction;

            var angle0 = vertexToStartDir.AngleFrom(Vector2D.BasisX);
            var angleSub = vertexToStartDir.AngleTo(vertexToEndDir);

            var radius = Math.Min(vertextToStartLine2D.Length, vertextToEndLine2D.Length) / 2;
            
            canvas.DrawArc(MeasureArcPen, cell.Vertex, radius, angle0, vertexToEndDir.AngleFrom(vertexToStartDir), true);

            
            var stringPosition = cell.Vertex + (vertextToStartLine2D.Direction + vertextToEndLine2D.Direction) * radius;
            
            canvas.DrawText(
                //$"{LanguageService.FindResourceString(Constants.TipText_AngleMeasurement)}{Extension.RadToDeg(angleSub).ToString(AngleFormat)}°",
                $"{Extension.RadToDeg(angleSub).ToString(AngleFormat)}°",
                Constants.TipFontSize_AngleMeasurement,
                TextForeground_AngleMeasurement,
                stringPosition
            );
        }

        private void DrawSelectedState(ICanvas canvas, ICanvasScreenConvertable canvasProxy) {
            if (!IsSelected) {
                return;
            }

            var vertexToStartLine2D = new Line2D(Vertex, Start);
            var vertexToEndLine2D = new Line2D(Vertex, End);

            canvas.DrawLine(HighLightLinePen, vertexToStartLine2D);
            canvas.DrawLine(HighLightLinePen, vertexToEndLine2D);

            var screenPoint = new Vector2D();

            //使用矩(正方)形显示两端和顶点;
            foreach (var point in GetImportantPositions()) {
                canvasProxy.ToScreen(point, screenPoint);
                //得到以某点为中心的视图矩形;
                var rect = NativeGeometryExtensions.GetNativeSuroundingScreenRect(
                    screenPoint,
                    HighLightRectLength,
                    HighLightRectLength
                );
                
                canvas.NativeDrawRectangle(
                    rect,
                    HighLightRectColorBrush,
                    HighLightLinePen
                );
            }
        }
        
        /// <summary>
        /// 返回角的两个端点和一个顶点;
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Vector2D> GetImportantPositions() {
            yield return Start;
            yield return Vertex;
            yield return End;
        }
        
        protected override void OnMouseDown(MouseDownEventArgs e) {

            if (e == null) {
                throw new ArgumentNullException(nameof(e));
            }

            if(e.Button != MouseButton.Left) {
                return;
            }

            var clickedPosition = GetImportantPositions().FirstOrDefault(p => p.IsAlmostEqualTo(e.Position));
            
            if(MousePositionTracker.LastMouseDownPosition != null) {
                var cell = GetPreviewCell();
                if(cell != null) {
                    MousePositionTracker.Reset(true);

                    Start = cell.Start;
                    Vertex = cell.Vertex;
                    End = cell.End;
                }
                
                e.Handled = true;
                
            }
            else if (clickedPosition != null) {
                MousePositionTracker.LastMouseDownPosition = clickedPosition;
                e.Handled = true;
            }
        }

        /// <summary>
        /// 根据当前上下文状态,获取预览的<see cref="AngleThreePointsCell"/>;
        /// </summary>
        /// <returns></returns>
        private AngleThreePointsCell GetPreviewCell() {
            if (MousePositionTracker.LastMouseDownPosition == null) {
                return null;
            }

            if(MousePositionTracker.CurrentHoverPosition == null) {
                return null;
            }
            
            var cell = new AngleThreePointsCell {
                Start = Start,
                Vertex = Vertex,
                End = End
            };

            if (MousePositionTracker.LastMouseDownPosition.IsAlmostEqualTo(Start)) {
                cell.Start = MousePositionTracker.CurrentHoverPosition;
            }
            else if (MousePositionTracker.LastMouseDownPosition.IsAlmostEqualTo(Vertex)) {
                var moveVector = MousePositionTracker.CurrentHoverPosition - Vertex;
                cell.Vertex += moveVector;
                cell.Start += moveVector;
                cell.End += moveVector;
            }
            else if (MousePositionTracker.LastMouseDownPosition.IsAlmostEqualTo(End)){
                cell.End = MousePositionTracker.CurrentHoverPosition;
            }
            else {
                return null;
            }

            return cell;
        }

        /// <summary>
        /// 使用三个位置表示一个角的单元;内部使用;
        /// </summary>
        class AngleThreePointsCell {
            public Vector2D Start { get; set; }

            public Vector2D Vertex { get; set; }

            public Vector2D End { get; set; }


        }

        private const string AngleFormat = "F2";
    }
}
