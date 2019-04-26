using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.Contracts.ComponentModel {
    /// <summary>
    /// 手动创建的属性描述器及其元数据类型;
    /// </summary>
    public sealed class CreatedPropertyDescriptor {
        public CreatedPropertyDescriptor(IPropertyDescriptor propertyDescriptor,IPropertyDescriptorMetaData metaData) {

            PropertyDescriptor = propertyDescriptor ?? throw new ArgumentNullException(nameof(propertyDescriptor));

            PropertyDescriptorMetaData = metaData ?? throw new ArgumentNullException(nameof(metaData));

        }

        public IPropertyDescriptor PropertyDescriptor { get; }

        public IPropertyDescriptorMetaData PropertyDescriptorMetaData { get; }

    }
}
