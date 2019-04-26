using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.Contracts.ComponentModel {
    /// <summary>
    /// 手动创建的类型描述器;
    /// </summary>
    public sealed class CreatedObjectTypeDescriptor {
        public CreatedObjectTypeDescriptor(IObjectTypeDescriptor objectTypeDescriptor, IObjectTypeDescriptorMetaData metaData) {

            ObjectTypeDescriptor = objectTypeDescriptor ?? throw new ArgumentNullException(nameof(objectTypeDescriptor));

            ObjectTypeDescriptorMetaData = metaData ?? throw new ArgumentNullException(nameof(metaData));

        }


        public IObjectTypeDescriptor ObjectTypeDescriptor { get; }


        public IObjectTypeDescriptorMetaData ObjectTypeDescriptorMetaData { get; }


    }
}
