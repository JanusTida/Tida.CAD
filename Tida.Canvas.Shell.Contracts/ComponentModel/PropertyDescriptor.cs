using System;
using System.Reflection;

namespace Tida.Canvas.Shell.Contracts.ComponentModel {
    /// <summary>
    /// 属性描述器默认实现类型;
    /// </summary>
    public class PropertyDescriptor : IPropertyDescriptor {
        public PropertyDescriptor(Type ownerType, string propName):this(ownerType,propName, BindingFlags.Public | BindingFlags.Instance) {

        }
        public PropertyDescriptor(Type ownerType,string propName,BindingFlags bindingFlags) {
            if (ownerType == null) {
                throw new ArgumentNullException(nameof(ownerType));
            }
            
            if (propName == null) {
                throw new ArgumentNullException(nameof(propName));
            }


            PropertyInfo = ownerType.GetProperty(propName,bindingFlags);

            if (PropertyInfo == null) {
                throw new MissingMethodException($"We failed to get a property named {propName} from {ownerType}.");
            }
        }

        public PropertyInfo PropertyInfo { get; }
    }
}
