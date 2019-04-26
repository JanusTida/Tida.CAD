using Tida.Canvas.Infrastructure.DrawObjects;
using Tida.Canvas.Infrastructure.MirrorTools;
using Tida.Canvas.Infrastructure.MoveTools;
using Tida.Geometry.Alternation;
using Tida.Geometry.Primitives;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.MoveTools
{   
    [Export(typeof(IDrawObjectMoveTool))]
    public class LineBaseMoveTool : DrawObjectMoveToolBase<LineBase>
    {
        protected override void OnMove(LineBase drawObject, Vector2D offset)
        {
            drawObject.Line2D += offset;
        }
    }
}
