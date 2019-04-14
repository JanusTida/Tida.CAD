using System;

namespace Tida.Canvas.Shell.Contracts.ComponentModel {
    /// <summary>
    /// 手动创建的忽略信息;
    /// </summary>
    public sealed class CreatedIgnoredPropertyDescriptor {
        public CreatedIgnoredPropertyDescriptor(IIgnoredPropertyDescriptor ignoredPropertyDescriptor,IIgnoredPropertyDescriptorMetaData metaData) {

            IgnoredPropertyDescriptor = ignoredPropertyDescriptor ?? throw new ArgumentNullException(nameof(ignoredPropertyDescriptor));

            IgnoredPropertyDescriptorMetaData = metaData ?? throw new ArgumentNullException(nameof(metaData));

        }
        public IIgnoredPropertyDescriptor IgnoredPropertyDescriptor { get; }


        public IIgnoredPropertyDescriptorMetaData IgnoredPropertyDescriptorMetaData { get; }

    }
}
