using Tida.Canvas.Contracts;
using System;
using Tida.Geometry.Primitives;
using static Tida.Canvas.Infrastructure.Constants;

namespace Tida.Canvas.Infrastructure.Snaping.Shapes {
    /// <summary>
    /// 辅助图形,射线;
    /// </summary>
    public class RaySnapLine : SnapShapeBase {
        public RaySnapLine(Vector2D startPosition,Vector2D position) {
            if (startPosition == null) {
                throw new ArgumentNullException(nameof(startPosition));
            }
            
            if (position == null) {
                throw new ArgumentNullException(nameof(position));
            }


            this.StartPosition = startPosition;
            this.Position = position;
        }
        
        /// <summary>
        /// 射线的起始位置;
        /// </summary>
        public Vector2D StartPosition { get; }
        
        public override void Draw(ICanvas canvas, ICanvasScreenConvertable canvasProxy) {
            //若两点重合,则不能构成射线,不能绘制;
            if(StartPosition.X == Position.X && StartPosition.Y == Position.Y) {
                return;
            }

            //射线线段;
            var rayLine = new Line2D(StartPosition, Position);

            var topLeftPoint = canvasProxy.GetTopLeftUnitPoint();
            var bottomRightPoint = canvasProxy.GetBottomRightUnitPoint();

            //若画布面积为零,无法得到交点,不能绘制;
            if(topLeftPoint.X == bottomRightPoint.X || topLeftPoint.Y == bottomRightPoint.Y) {
                return;
            }

            //求解射线与四边相交的交点;
            var lines = new Line2D[] {
                new Line2D(topLeftPoint,new Vector2D(topLeftPoint.X,bottomRightPoint.Y)),
                new Line2D(new Vector2D(topLeftPoint.X,bottomRightPoint.Y),bottomRightPoint),
                new Line2D(bottomRightPoint,new Vector2D(bottomRightPoint.X,topLeftPoint.Y)),
                new Line2D(new Vector2D(bottomRightPoint.X,topLeftPoint.Y),topLeftPoint)
            };

            foreach (var line in lines) {
                var intersectPosition = line.IntersectStraightLine(rayLine);
                //交点需满足相对起始位置,需与现在位置同向;
                if(intersectPosition == null) {
                    continue;
                }

                if((intersectPosition.X - StartPosition.X) * (Position.X - StartPosition.X) >= 0
                    && (intersectPosition.Y - StartPosition.Y) * (Position.Y - StartPosition.Y) >= 0) {
                    canvas.DrawLine(RayPen, new Line2D(StartPosition, intersectPosition));
                    break;
                }
            }
        }

        
        public override Rectangle2D2 GetNativeBoundingRect(ICanvasScreenConvertable canvasProxy) {
            return null;
        }
    }
    
}
