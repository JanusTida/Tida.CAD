using Tida.Canvas.Infrastructure.DrawObjects;
using Tida.Canvas.Infrastructure.OffsetTools;
using Tida.Geometry.Primitives;

namespace Tida.Canvas.Infrastructure.OffsetTools {
    /// <summary>
    /// 线段的偏移工具;
    /// </summary>
    public class LineOffsetTool : DrawObjectOffsetToolGenericBase<LineBase> {
        
        /// <summary>
        /// 线段将以垂直线段的方向进行偏移;
        /// 将偏向以<paramref name="relativeTo"/>那侧;
        /// </summary>
        /// <param name="line"></param>
        /// <param name="offset"></param>
        /// <param name="relativeTo"></param>
        /// <returns></returns>
        protected override void OnMoveOffset(LineBase line, double offset, Vector2D relativeTo) {
            var dir = (relativeTo - line.Line2D.Start).Normalize();
            var lineDir = line.Line2D.Direction;

            var offsetVector = new Vector2D(-lineDir.Y * offset, lineDir.X * offset);

            if (dir.Cross(lineDir) < 0) {
                line.Line2D = line.Line2D.CreateOffset(offsetVector);
            }
            else {
                line.Line2D = line.Line2D.CreateOffset(-offsetVector);
            }
        }
    }
}
