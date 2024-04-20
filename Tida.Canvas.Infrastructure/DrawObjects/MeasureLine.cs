using Tida.Canvas.Infrastructure.Utils;
using Tida.Canvas.Contracts;
using Tida.Canvas.Media;
using Tida.Geometry.Primitives;
using System;
using static Tida.Canvas.Infrastructure.Constants;

namespace Tida.Canvas.Infrastructure.DrawObjects {
    /// <summary>
    /// 用于表示长度的线段;
    /// </summary>
    public class MeasureLine : LineBase
    {
        public MeasureLine(Line2D line2D) : base(line2D)
        {

        }

        public override DrawObject Clone()
        {
            return new MeasureLine(Line2D);
        }
        private Line2D MeasureLineP;
        private Line2D MeasureLineV0;
        private Line2D MeasureLineV1;
        public override void Draw(ICanvas canvas, ICanvasScreenConvertable canvasProxy)
        {

            var line2D = GetPreviewLine2D(MousePositionTracker.CurrentHoverPosition);
            line2D = line2D ?? Line2D;
            var direction = line2D.Direction;
            if (direction == null) return;
            var vDir = direction.X > 0 ? new Vector2D(-direction.Y, direction.X) : new Vector2D(direction.Y, -direction.X);
            var screenDis = 36d;
            var unitDis = canvasProxy.ToUnit(screenDis);
            var measureLineDis = vDir * unitDis;

            MeasureLineP = new Line2D(line2D.Start + vDir * unitDis, line2D.End + vDir * unitDis);
            canvas.DrawLine(MeasurePenP, MeasureLineP);



            var verticalLineTimes = 1.2;
            MeasureLineV0 = new Line2D(
                line2D.Start,
                line2D.Start + vDir * unitDis * verticalLineTimes
            );
            MeasureLineV1 = new Line2D(
                line2D.End,
                line2D.End + vDir * unitDis * verticalLineTimes
            );

            canvas.DrawLine(MeasurePenV, MeasureLineV0);
            canvas.DrawLine(MeasurePenV, MeasureLineV1);


            if (IsSelected)
            {
                LineDrawExtensions.DrawSelectedLineState(MeasureLineP, canvas, canvasProxy, SelectionPen);
                LineDrawExtensions.DrawSelectedLineState(MeasureLineV0, canvas, canvasProxy, SelectionPen);
                LineDrawExtensions.DrawSelectedLineState(MeasureLineV1, canvas, canvasProxy, SelectionPen);
            }

            //line2D = outlineParaLine;

            var middlePoint = MeasureLineP.MiddlePoint;
            if (middlePoint == null)
            {
                return;
            }
            var text = $"{line2D.Length.ToString(LengthFormat)}";
            double width = 0;
            var chars = text.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                width += canvasProxy.GetCharScreenSize(chars[i]).Width;
            }
            width = -canvasProxy.ToUnit(width * Constants.TipFontSize_LengthMeasurement);
            var height = canvasProxy.ToUnit(canvasProxy.GetCharScreenSize(chars[0]).Height * Constants.TipFontSize_LengthMeasurement * 1.15);
            var a = direction.AngleWith(Vector2D.BasisX);
            a = direction.X * direction.Y > 0 ? a : -a;
            var vec = new Vector2D(width / 2 * Math.Cos(-a) + height * Math.Sin(-a), -width / 2 * Math.Sin(-a) + height * Math.Cos(-a));
            middlePoint = middlePoint + vec;
            canvas.DrawText(
                $"{line2D.Length.ToString(LengthFormat)}",
                Constants.TipFontSize_LengthMeasurement,
                TextForeground_LengthMeasurement,
                middlePoint, Math.PI / 2 - vDir.AngleFrom(Vector2D.BasisX)
            );


        }
        public override bool ObjectInRectangle(Rectangle2D2 rect, ICanvasScreenConvertable canvasProxy, bool anyPoint)
        {
            return LineHitUtils.LineInRectangle(MeasureLineP, rect, anyPoint) ||
                LineHitUtils.LineInRectangle(MeasureLineV0, rect, anyPoint) ||
                LineHitUtils.LineInRectangle(MeasureLineV1, rect, anyPoint);
        }

        public override bool PointInObject(Vector2D point, ICanvasScreenConvertable canvasProxy)
        {
            return LineHitUtils.PointInLine(MeasureLineP, point, canvasProxy) ||
                LineHitUtils.PointInLine(MeasureLineV0, point, canvasProxy) ||
                LineHitUtils.PointInLine(MeasureLineV1, point, canvasProxy);
        }


        private const string LengthFormat = "F2";

        /// <summary>
        /// 端点线的长度;
        /// </summary>
        private const double EndLength = 15d;

        /// <summary>
        /// 平行的线
        /// </summary>
        private static readonly Pen MeasurePenP = Pen.CreateFrozenPen(Brushes.White, 1.2);

        /// <summary>
        /// 垂直的线
        /// </summary>
        private static readonly Pen MeasurePenV = Pen.CreateFrozenPen(Brushes.White, 1.2,new DashStyle {
            Dashes = new double[] { 5, 2.5 }
        });
    }
}
