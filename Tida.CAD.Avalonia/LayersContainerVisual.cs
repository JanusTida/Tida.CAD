using Avalonia;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.CAD.Avalonia;

class LayersContainerVisual : Visual
{
    public LayersContainerVisual(AvaloniaCanvas canvas)
    {
        _canvas = canvas;
    }
    private IEnumerable<CADLayer>? _layers;
    private readonly AvaloniaCanvas _canvas;

    public IEnumerable<CADLayer>? Layers
    {
        get => _layers;
        set
        {
            var oldLayers = _layers;
            _layers = value;
            var newLayers = _layers;

            if (oldLayers != null)
            {
                foreach (var layer in oldLayers)
                {
                    UnSetupLayer(layer);
                }

            }

            if (newLayers != null)
            {
                foreach (var layer in newLayers)
                {
                    SetupLayer(layer);
                }
            }

            if (oldLayers is INotifyCollectionChanged oldLayerCollection)
            {
                oldLayerCollection.CollectionChanged -= Layers_CollectionChanged;
            }

            if (newLayers is INotifyCollectionChanged newLayerCollection)
            {
                newLayerCollection.CollectionChanged -= Layers_CollectionChanged;
                newLayerCollection.CollectionChanged += Layers_CollectionChanged;
            }
        }
    }

    /// <summary>
    /// Setup the layer;
    /// </summary>
    /// <param name="layer"></param>
    private void SetupLayer(CADLayer layer)
    {
        if (layer == null)
        {
            return;
        }
        var cadLayerVisual = VisualChildren.OfType<CADLayerVisual>().FirstOrDefault(p => p.Layer == layer);
        if (cadLayerVisual != null)
        {
            return;
        }
        var layerContaienrVisual = new CADLayerVisual(layer, _canvas);
        VisualChildren.Add(layerContaienrVisual);
    }

    /// <summary>
    /// Unsetup the layer;
    /// </summary>
    /// <param name="layer"></param>
    private void UnSetupLayer(CADLayer layer)
    {
        if (layer == null)
        {
            return;
        }

        var cadLayerVisual = VisualChildren.OfType<CADLayerVisual>().FirstOrDefault(p => p.Layer == layer);
        if (cadLayerVisual == null)
        {
            return;
        }
        VisualChildren.Remove(cadLayerVisual);
    }

    private void Layers_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (sender is not IEnumerable<CADLayer> layers)
        {
            return;
        }

        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                if (e.NewItems == null)
                {
                    break;
                }
                foreach (var item in e.NewItems)
                {
                    if (item is not CADLayer layer)
                    {
                        continue;
                    }

                    SetupLayer(layer);
                }
                break;


            case NotifyCollectionChangedAction.Remove:
                if (e.OldItems == null)
                {
                    break;
                }
                foreach (var item in e.OldItems)
                {
                    if (item is not CADLayer layer)
                    {
                        continue;
                    }

                    UnSetupLayer(layer);
                }
                break;

            case NotifyCollectionChangedAction.Reset:
                foreach (var layer in e.OldItems?.OfType<CADLayer>() ?? Enumerable.Empty<CADLayer>())
                {
                    UnSetupLayer(layer);
                }

                foreach (var layer in layers ?? Enumerable.Empty<CADLayer>())
                {
                    SetupLayer(layer);
                }
                break;

            case NotifyCollectionChangedAction.Replace:
                break;
            case NotifyCollectionChangedAction.Move:
                break;

            default:
                break;
        }
    }

    public void InvalidateVisuals()
    {
        foreach(var visual in VisualChildren)
        {
            visual.InvalidateVisual();
        }
    }
}
