using System;
using System.Collections.Generic;
using System.Text;
using Tida.Canvas.Contracts;
using Tida.Canvas.Infrastructure.DrawObjects;
using Tida.Canvas.Input;
using Tida.Geometry.Primitives;

namespace Tida.Canvas.Infrastructure.EditTools {

    /// <summary>
    /// 起点,圆心,中点的圆弧绘制工具;
    /// </summary>
    public class ArcStartAndCenterThenEndEditTool : UniqueTypeEditToolGenericBase<Arc> {
        private Vector2D _firstClickPoint;
        private Vector2D _secondClickPoint;
        private Vector2D _hoverPoint;

        private readonly Line _firstToSecondLine = new Line(Vector2D.Zero,Vector2D.Zero);
        private readonly Arc _editingArc = new Arc(new Arc2D(Vector2D.Zero));
        public override bool IsEditing => true;

        protected override void OnMouseDown(MouseDownEventArgs e)
        {
            base.OnMouseDown(e);
            e.Handled = true;
            if (e.Button != MouseButton.Left){
                return;
            }

            if(_firstClickPoint == null){
                _firstClickPoint = e.Position;
                return;
            }

            if(_secondClickPoint == null){
                _secondClickPoint = e.Position;
                return;
            }

            var arc2D = GetArc2DOnCurrentState(e.Position);
            if(arc2D == null){
                return;
            }

            AddDrawObjectToUndoStack(new Arc(GetArc2DOnCurrentState(e.Position)));
            _firstClickPoint = null;
            _secondClickPoint = null;
            _hoverPoint = null;
        }
        protected override void OnMouseMove(MouseMoveEventArgs e) {
            e.Handled = true;
            _hoverPoint = e.Position;
            RaiseVisualChanged();
        }
        
        /// <summary>
        /// 根据当前状态获取一个圆弧;
        /// </summary>
        /// <param name="thirdPoint"></param>
        /// <returns></returns>
        private Arc2D GetArc2DOnCurrentState(Vector2D thirdPoint)
        {
            if(_firstClickPoint == null || _secondClickPoint == null){
                return null;
            }

            if (_secondClickPoint.IsAlmostEqualTo(_firstClickPoint)){
                return null;
            }

            if (thirdPoint.IsAlmostEqualTo(_secondClickPoint)){
                return null;
            }

            var firstLineVector = _secondClickPoint - _firstClickPoint;
            var secondLineVector = thirdPoint - _secondClickPoint;

            var startAngle = firstLineVector.AngleFrom(Vector2D.BasisX);
            var angle = secondLineVector.AngleFrom(Vector2D.BasisX) - startAngle;

            return new Arc2D(_secondClickPoint) {
                StartAngle = startAngle,
                Angle = angle,
                Radius = firstLineVector.Modulus()
            };
        }

        public override void Draw(ICanvas canvas, ICanvasScreenConvertable canvasProxy)
        {
            base.Draw(canvas, canvasProxy);
            if (_firstClickPoint == null) {
                return;
            }

            if(_secondClickPoint == null && _hoverPoint != null) {
                _firstToSecondLine.Line2D.Start = _firstClickPoint;
                _firstToSecondLine.Line2D.End = _hoverPoint;
                _firstToSecondLine.Draw(canvas,canvasProxy);
                return;
            }


            var editingArc2D = GetArc2DOnCurrentState(_hoverPoint);
            if(editingArc2D != null) {
                _editingArc.Arc2D = editingArc2D;
                _editingArc.Draw(canvas, canvasProxy);
            }
        }

       
        
    }
}
