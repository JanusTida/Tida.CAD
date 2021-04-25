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

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CanvasProxyProperty =
            DependencyProperty.Register("CanvasProxy", typeof(ICanvasScreenConvertable), typeof(CanvasControlBehavior), new PropertyMetadata(null));


        protected override void OnAttached()
        {
            CanvasProxy = AssociatedObject.CanvasProxy;
            base.OnAttached();
        }
    }
}
