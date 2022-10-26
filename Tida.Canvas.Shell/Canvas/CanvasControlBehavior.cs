using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Tida.Canvas.Contracts;
using Tida.Canvas.WPFCanvas;

namespace Tida.Canvas.Shell.Canvas
{
    public class CanvasControlBehavior : Behavior<CanvasControl>
    {
        public ICanvasScreenConvertable CanvasProxy
        {
            get { return (ICanvasScreenConvertable)GetValue(CanvasProxyProperty); }
            set { SetValue(CanvasProxyProperty, value); }
        }

        public static readonly DependencyProperty CanvasProxyProperty =
            DependencyProperty.Register(nameof(CanvasProxy), typeof(ICanvasScreenConvertable), typeof(CanvasControlBehavior), new FrameworkPropertyMetadata(null) { BindsTwoWayByDefault = true});


        protected override void OnAttached()
        {
            CanvasProxy = AssociatedObject.CanvasProxy;
            base.OnAttached();
        }

        protected override void OnDetaching()
        {
            CanvasProxy = null;
            base.OnDetaching();
        }
    }
}
