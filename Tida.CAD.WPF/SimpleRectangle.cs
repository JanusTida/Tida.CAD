using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Tida.CAD;
using System.Windows.Media;

namespace Tida.CAD.WPF; 
/// <summary>
/// The simple implementation to draw a rect,used to drag and select;
/// </summary>
class SimpleRectangle : CADElement {
    /// <summary>
    /// 矩形数据;
    /// </summary>
    private CADRect? _rectangle;
    public CADRect? Rectangle {
        get {
            return _rectangle;
        }
        set {
            _rectangle = value;
            RaiseVisualChanged();
        }
    }

    /// <summary>
    /// 填充画刷;
    /// </summary>
    private Brush? _fill;
    public Brush? Fill {
        get { return _fill; }
        set {
            _fill = value;
            RaiseVisualChanged();
        }
    }

    /// <summary>
    /// 笔;
    /// </summary>
    private Pen? _pen;

    public Pen? Pen {
        get { return _pen; }
        set {
            _pen = value;
            RaiseVisualChanged();
        }
    }
   
    public override void Draw(ICanvas canvas) {
        if(Fill == null) {
            return;
        }

        if(Pen == null) {
            return;
        }
        if(Rectangle == null)
        {
            return;
        }
        canvas.DrawRectangle(Rectangle.Value, Fill, Pen);
    }
}
