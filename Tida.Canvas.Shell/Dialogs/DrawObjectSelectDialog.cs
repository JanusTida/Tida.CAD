using Tida.Canvas.Shell.Dialogs.Models;
using Tida.Canvas.Shell.Dialogs.ViewModels;
using Tida.Canvas.Shell.Dialogs.Views;
using Tida.Canvas.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using Tida.Canvas.Shell.Contracts.DrawObjectDescription;
using SDrawObjectDescription = Tida.Canvas.Shell.Contracts.DrawObjectDescription.DrawObjectDescription;

namespace Tida.Canvas.Shell.Dialogs {
    /// <summary>
    /// 从多个绘制对象中选定一个的对话框;
    /// </summary>
    public static class DrawObjectSelectDialog {    
        /// <summary>
        /// 在指定的绘制对象集合中,选取一个唯一的绘制对象;
        /// </summary>
        /// <param name="drawObjects"></param>
        /// <returns></returns>
        public static DrawObject SelectOneDrawObject(IEnumerable<DrawObject> drawObjects) {

            if (drawObjects == null) {
                throw new ArgumentNullException(nameof(drawObjects));
            }
            
            var vm = new DrawObjectSelectWindowViewModel();
            var models = drawObjects.Select(p => GetDrawObjectModel(p));
            vm.DrawObjectModels.AddRange(models);
            
            var window = new DrawObjectSelectWindow {
                DataContext = vm
            };
            vm.CloseRequest += delegate { window.Close(); };
            window.ShowDialog();
            if (vm.DialogResult) {
                return vm.SelectedDrawObjectModel?.DrawObject;
            }

            return null;
        }

        private static DrawObjectModel GetDrawObjectModel(DrawObject drawObject) {
            var model = new DrawObjectModel(drawObject);
            SDrawObjectDescription description = null;
            
            foreach (var descriptor in DrawObjectDescriptionUtil.DrawObjectDescriptors) {
                description = descriptor.GetDescription(drawObject);
                if (description != null) {
                    break;
                }
            }

            model.TypeName = description?.TypeName??drawObject.ToString();
            return model;
        }
    }
}
