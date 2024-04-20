using Tida.Canvas.Contracts;
using Tida.Canvas.Media;
using Tida.Geometry.External;
using Tida.Geometry.Primitives;
using System;
using System.Collections.Generic;
using static Tida.Canvas.Infrastructure.Constants;

namespace Tida.Canvas.Infrastructure.Utils {
    /// <summary>
    /// 线段编辑的拓展;
    /// </summary>
    public static class LineDrawExtensions {

        /// <summary>
        /// 绘制未完成的编辑的线段及其提示;
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="canvasProxy"></param>
        /// <param name="editingLine"></param>
        public static void DrawEditingLine(ICanvas canvas, ICanvasScreenConvertable canvasProxy, Line2D editingLine) {
            //绘制未完成线段;
            canvas.DrawLine(LinePen, editingLine);
            DrawEditingLineOutLines(canvas, canvasProxy, editingLine);
            DrawEditingLineLengthString(canvas, canvasProxy, editingLine);
            DrawEditingLineArc(canvas, canvasProxy, editingLine);
            DrawEditingLineArcString(canvas, canvasProxy, editingLine);
        }


        /// <summary>
        /// 在未完成的线段的中点附近的位置绘制线段的长度;
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="canvasProxy"></param>
        /// <param name="editingLine"></param>
        public static void DrawEditingLineLengthString(ICanvas canvas, ICanvasScreenConvertable canvasProxy, Line2D editingLine) {
            var lengthString = editingLine.Length.ToString("F2");

            var direction = editingLine.Direction;
            if (direction == null) {
                return;
            }

            //通过法向量确定其它提示信息的位置;
            var verticalDir = new Vector2D(-direction.Y, direction.X);
            //将视图距离转化为工程数学距离;
            var unitDistance = canvasProxy.ToUnit(
                ScreenDistanceLineEditingWithLine
            );

            var paraLinesDistance = verticalDir * unitDistance;

            #region 绘制中点,调试用;
#if DEBUG
            //var middleScreenPoint = canvasProxy.ToScreen(editingLine.MiddlePoint);
            //var surroundRect = NativeGeometryUtil.GetNativeSuroundingScreenRect(middleScreenPoint, 8, 8);
            //canvas.NativeDrawRectangle(
            //    surroundRect,
            //    Tida.Canvas.Shell.Constants.HighLightRectColorBrush,
            //    Tida.Canvas.Shell.Constants.LinePen
            //);
#endif
            #endregion

            //绘制提示长度;
            var editingTextPosition = editingLine.MiddlePoint + paraLinesDistance / 1.2;

            canvas.DrawText(
                lengthString,
                LineEditingSnappingLengthFontSize,
                LineEditingTipBrush,
                editingTextPosition
            );

        }

        /// <summary>
        /// 为未完成的线段绘制外围辅助线,这包括了一条平行与原线段的线段,以及垂直于原线段的两条侧边;
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="canvasProxy"></param>
        /// <param name="editingLine"></param>
        public static void DrawEditingLineOutLines(ICanvas canvas, ICanvasScreenConvertable canvasProxy, Line2D editingLine) {
            var direction = editingLine.Direction;
            if (direction == null) {
                return;
            }

            //通过法向量确定其它提示线的位置;
            var verticalDir = new Vector2D(-direction.Y, direction.X);
            //将视图距离转化为工程数学距离;
            var unitDistance = canvasProxy.ToUnit(
                ScreenDistanceLineEditingWithLine
            );

            var paraLinesDistance = verticalDir * unitDistance;

            //绘制外围的平行线;
            var outLinePara = new Line2D(
                editingLine.Start + verticalDir * unitDistance,
                editingLine.End + verticalDir * unitDistance
            );
            canvas.DrawLine(LineEditingTipPen, outLinePara);

            var verticalLineTime = 1.2;
            //绘制两侧垂直线;
            var verticalLine0 = new Line2D(
                editingLine.Start,
                editingLine.Start + verticalDir * unitDistance * verticalLineTime
            );
            var verticalLine1 = new Line2D(
                editingLine.End,
                editingLine.End + verticalDir * unitDistance * verticalLineTime
            );

            canvas.DrawLine(LineEditingTipPen, verticalLine0);
            canvas.DrawLine(LineEditingTipPen, verticalLine1);
        }

        
        /// <summary>
        /// 为未完成的线段绘制角度弧形,该弧将X正半轴作为起始边;
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="canvasProxy"></param>
        /// <param name="editingLine"></param>
        public static void DrawEditingLineArc(ICanvas canvas, ICanvasScreenConvertable canvasProxy, Line2D editingLine) {

            var start = editingLine.Start;
            var end = editingLine.End;
            
            var angle = (end - start).AngleFrom(Vector2D.BasisX);

            //弧度化为角度;
            var degAngle = Extension.RadToDeg(angle);

            canvas.DrawArc(LineEditingTipPen, start, editingLine.Length ,0, angle,true);

            canvas.DrawLine(LineEditingTipPen, new Line2D(start, new Vector2D(start.X + editingLine.Length, start.Y)));
            //canvas.DrawArc(Constants.LineEditingTipPen, new Vector2D(0, 0), 31, 0, 3.14);

            //canvas.DrawText(angle.ToString(), 18, new Media.SolidColorBrush { Color = Media.Color.FromRgb(0xFFFFFF) }, new Vector2D(0, 0));
        }

        /// <summary>
        /// 为未完成的线段绘制角度信息,该角度将X正半轴作为起始边;
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="canvasProxy"></param>
        /// <param name="editingLine"></param>
        public static void DrawEditingLineArcString(ICanvas canvas, ICanvasScreenConvertable canvasProxy, Line2D editingLine) {
            if (canvas == null) {
                throw new ArgumentNullException(nameof(canvas));
            }
            
            if (canvasProxy == null) {
                throw new ArgumentNullException(nameof(canvasProxy));
            }
            
            if (editingLine == null) {
                throw new ArgumentNullException(nameof(editingLine));
            }

            var start = editingLine.Start;
            var end = editingLine.End;

            ///得到弧的两条边的向量(以<see cref="editingLine.Start"/>为起点),这两条边的长度相等;
            var lineVector = end - start;
            var xAxisLineVector = Vector2D.BasisX * lineVector.Modulus();

            //确定角度;
            var radAngle = Vector2D.BasisX.AngleTo(lineVector);
            var degAngle = (int)(Extension.RadToDeg(radAngle));
            //确定需要绘制角度的位置;
            var stringPosition = (lineVector + xAxisLineVector).Normalize() * lineVector.Modulus() + start;

            canvas.DrawText(degAngle.ToString(), 12, LineEditingTipBrush, stringPosition);
        }

        /// <summary>
        /// 绘制线段被选中的状态;
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="canvasProxy"></param>
        public static void DrawSelectedLineState(Line2D line2D,ICanvas canvas, ICanvasScreenConvertable canvasProxy,Pen selectionPen) {

            if (line2D == null) {
                throw new ArgumentNullException(nameof(line2D));
            }


            if (selectionPen == null) {
                throw new ArgumentNullException(nameof(selectionPen));
            }

            //显示被选择的线;
            canvas.DrawLine(selectionPen, line2D);

            if (canvasProxy == null) {
                return;
            }


            //使用矩(正方)形显示两端和中点;
            IEnumerable<Vector2D> GetPoints() {
                yield return line2D.Start;
                yield return line2D.MiddlePoint;
                yield return line2D.End;
            }

            foreach (var point in GetPoints()) {
                PointDrawExtensions.DrawSelectedPointState(point, canvas, canvasProxy);
            }
        }

        /// <summary>
        /// 根据线段绘制箭头,线段本身将不被绘制;
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="line2D"></param>
        /// <param name="pen"></param>
        /// <param name="horiSideLength">平行于原线段的翼边长度</param>
        /// <param name="vertiSideLength">垂直于原线段的翼边长度</param>
        public static void DrawArrow(ICanvas canvas,Line2D line2D, Pen pen,double horiSideLength,double vertiSideLength) {
            if (canvas == null) {
                throw new ArgumentNullException(nameof(canvas));
            }

            if (line2D == null) {
                throw new ArgumentNullException(nameof(line2D));
            }

            if (pen == null) {
                throw new ArgumentNullException(nameof(pen));
            }

            if (line2D.Length.AreEqual(0)) {
                return;
            }

            var lineDir = line2D.Direction;

            var vertiOffsetVector = new Vector2D(-lineDir.Y, lineDir.X) * vertiSideLength;
            var horiOffsetVector = - lineDir * horiSideLength;

            var sideLine = new Line2D(line2D.End,line2D.End + horiOffsetVector + vertiOffsetVector);

            canvas.DrawLine(pen, sideLine);

            sideLine.End = line2D.End + horiOffsetVector - vertiOffsetVector;

            canvas.DrawLine(pen, sideLine);
        }
    }
}
