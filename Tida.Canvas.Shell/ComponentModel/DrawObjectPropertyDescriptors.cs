using Tida.Canvas.Contracts;
using Tida.Canvas.Shell.Contracts.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Tida.Canvas.Shell.Contracts.ComponentModel.Constants;
using static Tida.Canvas.Shell.ComponentModel.Constants;

namespace Tida.Canvas.Shell.ComponentModel {
    /// <summary>
    /// 绘制对象属性——可见性;
    /// </summary>
    [ExportPropertyDescriptor(Inheritable = true, CategoryNameKey = CategoryName_Appearance, DescriptionNameKey = DescriptionName_IsVisible, DisplayNameKey = DisplayName_IsVisible)]
    class IsVisiblePropertyDescriptor : PropertyDescriptor, IPropertyDescriptor {
        public IsVisiblePropertyDescriptor() : base(typeof(DrawObject),nameof(DrawObject.IsVisible)) {
        }
    }

    /// <summary>
    /// 绘制对象属性——选中;
    /// </summary>
    //[ExportPropertyDescriptor(ApplyToInherited = true, CategoryNameKey = CategoryName_State, DescriptionNameKey = DescriptionName_IsSelected, DisplayNameKey = DisplayName_IsSelected)]
    //class IsSelectedPropertyDescriptor : PropertyDescriptor {
    //    public IsSelectedPropertyDescriptor() : base(typeof(DrawObject),nameof(DrawObject.IsSelected)) {
    //    }
    //}

    /// <summary>
    /// 绘制对象以下属性将被忽略:
    /// 1.正在被编辑;
    /// 2.父对象.
    /// 3.选中
    /// </summary>
    [ExportIgnoredPropertyDescriptor(Inheritable = true)]
    class IsEditingIgnoredPropertyDescriptor : IgnoredPropertyDescriptor {  
        public IsEditingIgnoredPropertyDescriptor() : base(typeof(DrawObject),
            nameof(DrawObject.IsEditing),
            nameof(DrawObject.Parent), 
            nameof(DrawObject.IsSelected)
            ) {

        }
    }

    
}
