using System;
using Tida.Canvas.Input;
using Tida.Canvas.Infrastructure.EditTools;
using Tida.Canvas.Base.DrawObjects;


namespace Tida.Canvas.Base.EditTools {

    /// <summary>
    /// 点的编辑工具;
    /// </summary>
    public class PointEditTool : UniqueTypeEditToolGenericBase<Point> {
        public override bool IsEditing => true;
        protected override void OnMouseDown(MouseDownEventArgs e) {

            if(CanvasContext.ActiveLayer == null) {
                throw new InvalidOperationException();
            }

            if (e == null) {
                throw new ArgumentNullException(nameof(e));
            }
            
            e.Handled = true;

            var pointPosition = e.Position;
            var point = new Point(pointPosition);

            AddDrawObjectToUndoStack(point);
        }
    }

   
}
