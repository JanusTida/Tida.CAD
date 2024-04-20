using System;
using System.Collections.Generic;
using System.Text;
using Tida.Canvas.Contracts;

namespace Tida.Canvas.Shell.Canvas.IViews
{
    /// <summary>
    /// 画布呈现视图抽象;
    /// </summary>
    public interface ICanvasPresenter
    {
        ICanvasControl CanvasControl { get; }
    }
}
