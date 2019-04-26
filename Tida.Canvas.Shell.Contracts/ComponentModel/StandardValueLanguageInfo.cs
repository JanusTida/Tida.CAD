using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tida.Util.Util;

namespace Tida.Canvas.Shell.Contracts.ComponentModel {


    /// <summary>
    /// 值及其对应的语言键名的映射单元;
    /// </summary>
    public struct StandardValueLanguageInfo<TValue> {
        public StandardValueLanguageInfo(TValue standardValue, string languageKey) {
            StandardValue = standardValue;
            LanguageKey = languageKey;
        }

        /// <summary>
        /// 原值;
        /// </summary>
        public TValue StandardValue { get; }

        /// <summary>
        /// 对应的语言键名;
        /// </summary>
        public string LanguageKey { get; }
    }

    /// <summary>
    /// 使用了标准值集合编辑器的描述信息,提供了构造标准值集合编辑器的关键构造信息;
    /// </summary>
    //public interface IStandardValuesEditorInfoDescriptor {

    //    /// <summary>
    //    /// 标准值集合,本集合中的实例中值的类型需与<see cref="IStandardValuesEditorInfoDescriptorMetaData.ValueType"/>相等,或继承后者;
    //    /// </summary>
    //    IEnumerable<StandardValueLanguageInfo> StandardValueInfos { get; }


    //}


    //public interface IStandardValuesEditorInfoDescriptorMetaData {
    //    /// <summary>
    //    /// 值的类型;
    //    /// </summary>
    //    Type ValueType { get; }

    //    /// <summary>
    //    /// 编辑器类型标识;
    //    /// </summary>
    //    string EditorTypeGUID { get; }
    //}

    ///// <summary>
    ///// 导出使用了标准值集合编辑器的描述器;
    ///// </summary>
    //[MetadataAttribute,AttributeUsage(AttributeTargets.Class,AllowMultiple = false)]
    //public sealed class ExportStandardValuesEditorInfoDescriptorAttribute : ExportAttribute, IStandardValuesEditorInfoDescriptorMetaData {
    //    public ExportStandardValuesEditorInfoDescriptorAttribute(Type valueType,string editorTypeGUID) :base(typeof(IStandardValuesEditorInfoDescriptor)) {

    //        ValueType = valueType ?? throw new ArgumentNullException(nameof(valueType));

    //        EditorTypeGUID = editorTypeGUID ?? throw new ArgumentNullException(nameof(editorTypeGUID));

    //    }

    //    /// <summary>
    //    /// 标准值类型;
    //    /// </summary>
    //    public Type ValueType { get; set; }

    //    /// <summary>
    //    /// 将要生成的编辑器类型的标识;
    //    /// </summary>
    //    public string EditorTypeGUID { get; set; }
    //}
}
