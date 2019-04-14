using Tida.Canvas.Contracts;
using Tida.Geometry.External;
using Tida.Geometry.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Infrastructure.Snaping.Shapes {
    /// <summary>
    /// 中点的辅助图形;
    /// </summary>
    public class MiddlePointSnapShape : StandardSnapPoint {
        public static MiddlePointSnapShape CreateByLine2D(Line2D line2D) {

            if (line2D == null) {
                throw new ArgumentNullException(nameof(line2D));
            }

            if(line2D.Length == 0) {
                return null;
            }

            var dir = line2D.Direction;
            var middlePoint = line2D.MiddlePoint;
            return new MiddlePointSnapShape(middlePoint, dir);
        }

        /// <summary>
        /// 中点的辅助图形构造;
        /// </summary>
        /// <param name="position">中点位置</param>
        /// <param name="dir">方向</param>
        public MiddlePointSnapShape(Vector2D position,Vector2D dir):base(position) {
            
            Dir = dir ?? throw new ArgumentNullException(nameof(dir));
            if(!dir.Modulus().AreEqual(1)) {
                throw new ArgumentException($"The length of {nameof(dir)} should be equal to than 1");
            }
        }

        /// <summary>
        /// 中点所在的单位向量;
        /// </summary>
        public Vector2D Dir { get; }

        public override void Draw(ICanvas canvas, ICanvasScreenConvertable canvasProxy) {
            base.Draw(canvas, canvasProxy);

            var vertDir = new Vector2D(-Dir.Y, Dir.X) / 2;
            
            
        }
    }
}
