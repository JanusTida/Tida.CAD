using System;
using System.Collections.Generic;
using System.Reflection;

namespace Tida.Canvas.Shell.Contracts.ComponentModel {
    /// <summary>
    /// 忽略属性描述器默认实现类型;
    /// </summary>
    public class IgnoredPropertyDescriptor : IIgnoredPropertyDescriptor {
        public IgnoredPropertyDescriptor(Type ownerType, params string[] propNames) : this(ownerType, BindingFlags.Public | BindingFlags.Instance, propNames) { }

        public IgnoredPropertyDescriptor(Type ownerType,BindingFlags bindingFlags,params string[] propNames) {
            if (ownerType == null) {
                throw new ArgumentNullException(nameof(ownerType));
            }

            if (propNames == null) {
                throw new ArgumentNullException(nameof(propNames));
            }

            _propertyInfos = new PropertyInfo[propNames.Length];
            for (int i = 0; i < propNames.Length; i++) {
                _propertyInfos[i] = ownerType.GetProperty(propNames[i], bindingFlags);
            }
        }
        
        private readonly PropertyInfo[] _propertyInfos;
        public IEnumerable<PropertyInfo> PropertyInfos => _propertyInfos;
    }
}

