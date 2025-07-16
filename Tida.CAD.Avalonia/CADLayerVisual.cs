using Avalonia;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.CAD.Avalonia;

class CADLayerVisual : Visual
{
    public CADLayerVisual(CADLayer layer,AvaloniaCanvas canvas)
    {
        Layer = layer;
        Canvas = canvas;

        IsVisible = Layer.IsVisible;
    }

    public CADLayer Layer { get; }
    public AvaloniaCanvas Canvas { get; }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        Layer.DrawObjectsAdded -= CADLayer_DrawObjectsAdded;
        Layer.DrawObjectsAdded += CADLayer_DrawObjectsAdded;
        Layer.DrawObjectsRemoved -= CADLayer_DrawObjectsRemoved;
        Layer.DrawObjectsRemoved += CADLayer_DrawObjectsRemoved;
        Layer.DrawObjectsClearing -= CADLayer_DrawObjectClearing;
        Layer.DrawObjectsClearing += CADLayer_DrawObjectClearing;
        Layer.IsVisibleChanged -= CADLayer_IsVisibleChanged;
        Layer.IsVisibleChanged += CADLayer_IsVisibleChanged;

        base.OnAttachedToVisualTree(e);
    }
    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        Layer.DrawObjectsAdded -= CADLayer_DrawObjectsAdded;
        Layer.DrawObjectsRemoved -= CADLayer_DrawObjectsRemoved;
        Layer.DrawObjectsClearing -= CADLayer_DrawObjectClearing;
        Layer.IsVisibleChanged -= CADLayer_IsVisibleChanged;
        base.OnDetachedFromVisualTree(e);
    }

    private void CADLayer_IsVisibleChanged(object? sender, EventArgs e)
    {
        IsVisible = Layer.IsVisible;
    }

    private void CADLayer_DrawObjectClearing(object? sender, EventArgs e)
    {
        InvalidateVisual();
    }

    private void CADLayer_DrawObjectsRemoved(object? sender, IEnumerable<DrawObject> e)
    {
        InvalidateVisual();
    }

    private void CADLayer_DrawObjectsAdded(object? sender, IEnumerable<DrawObject> e)
    {
        InvalidateVisual();
    }

    public override void Render(DrawingContext context)
    {
        if (!Layer.IsVisible)
        {
            return;
        }
        
        try
        {
            Canvas.InernalDrawingContext = context;
            Layer.Draw(Canvas);
            foreach (var drawObject in Layer.DrawObjects)
            {
                drawObject.Draw(Canvas);
            }
        }
        catch(Exception ex)
        {
            Debugger.Break();
            Trace.TraceError(ex.Message);
        }
        finally
        {
            Canvas.InernalDrawingContext = null;
        }
        
    }
}
