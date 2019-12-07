using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Tida.Application.Contracts.App;
using Tida.Application.Contracts.Common;

namespace Tida.Canvas.Shell.ComponentModel {
    /// <summary>
    /// 用作动态生成代码的封装;
    /// </summary>
    public static class DynamicAsmManager {

        static DynamicAsmManager() {
            var asmName = new AssemblyName(TidaAsmName);
            AsmBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.RunAndSave);
            ModuleBuilder = AsmBuilder.DefineDynamicModule(asmName.Name, asmName.Name + ".dll");
        }

        private const string TidaAsmName = "TidaDynamicEditorAsm";
        private static readonly AssemblyBuilder AsmBuilder;
        /// <summary>
        /// 本应用程序域用于动态生成模块的模块生成器;
        /// </summary>
        public static readonly ModuleBuilder ModuleBuilder;
        /// <summary>
        /// 应用程序保存事件;
        /// </summary>
        public static event EventHandler Saving;
        /// <summary>
        /// 保存临时程序集,本方法只能被调用一次,用于在调试中检查生成后的代码;
        /// </summary>
        public static void SaveTempAssembly() {
            try {
                Saving?.Invoke(null, EventArgs.Empty);
                AsmBuilder.Save(TidaAsmName + ".dll");
            }
            catch (Exception ex) {
                LoggerService.WriteException(ex);
            }

        }
    }

    //    /// <summary>
    //    /// 使用了标准值集合的编辑器描述器的提供者;
    //    /// </summary>
    //    [Export(typeof(IEditorDescriptorProvider))]
    //    public class StandardValuesEditorDescriptorProvider : IEditorDescriptorProvider {
    //        [ImportingConstructor]
    //        public StandardValuesEditorDescriptorProvider([ImportMany] IEnumerable<Lazy<IStandardValuesEditorInfoDescriptor, IStandardValuesEditorInfoDescriptorMetaData>> standardValuesEditorInfoDescriptors) {
    //            _standardValuesEditorInfoDescriptor = standardValuesEditorInfoDescriptors.ToArray();
    //            _editorDescriptors = _standardValuesEditorInfoDescriptor.Select(p => CreateEditorDescriptor(p)).Where(p => p != null).ToArray();
    //        }

    //        private const string EditorTypePrefix = "TidaEditor_";

    //        /// <summary>
    //        /// 根据标准值信息描述信息,动态地生成一个编辑器描述器;
    //        /// </summary>
    //        /// <param name="editorInfoTuple"></param>
    //        /// <returns></returns>
    //        private static CreatedEditorDescriptor CreateEditorDescriptor(Lazy<IStandardValuesEditorInfoDescriptor, IStandardValuesEditorInfoDescriptorMetaData> editorInfoTuple) {
    //            var stdDescriptor = editorInfoTuple.Value;
    //            var stdMetaData = editorInfoTuple.Metadata;
    //            if (stdDescriptor == null || stdMetaData == null) {
    //                return null;
    //            }

    //            try {
    //                //生成Converter;
    //                var tupleList = CreateStandardValueLanguageTupleList(stdDescriptor.StandardValueInfos, stdMetaData.ValueType);
    //                var converterType = typeof(ValueToLanguageConveter<>).MakeGenericType(stdMetaData.ValueType);
    //                var converter = Activator.CreateInstance(converterType, tupleList);

    //                //生成标准值集合;
    //                var standardValueList = CreateStandardValueList(stdDescriptor.StandardValueInfos, stdMetaData.ValueType);

    //                //获取编辑器类型的基类,后继承其生成一个新的类型;
    //                var editorParentType = typeof(StandardValuesEditorEx<>).MakeGenericType(stdMetaData.ValueType);

    //                //通过反射,向其字典缓存中添加新的关键内容;
    //                var converterDictionaryProp = editorParentType.GetProperty(nameof(StandardValuesEditorEx<int>.ConverterDictionary));
    //                var stdValuesDictionaryProp = editorParentType.GetProperty(nameof(StandardValuesEditorEx<int>.StandardValuesDictionary));

    //                var converterDictionary = converterDictionaryProp.GetValue(null);
    //                var addConverterMethod = converterDictionary.GetType().GetMethod(nameof(IDictionary<int,int>.Add));
    //                addConverterMethod.Invoke(converterDictionary,new object[] { stdMetaData.EditorTypeGUID, converter });

    //                var stdValueDictionary = stdValuesDictionaryProp.GetValue(null);
    //                var addStdValuesMethod = stdValueDictionary.GetType().GetMethod(nameof(IDictionary<int, int>.Add));
    //                addStdValuesMethod.Invoke(stdValueDictionary, new object[] { stdMetaData.EditorTypeGUID,standardValueList});

    //                var editorTypeBuilder = DynamicAsmManager.ModuleBuilder.DefineType($"{EditorTypePrefix}{stdMetaData.EditorTypeGUID}", TypeAttributes.Class | TypeAttributes.Public | TypeAttributes.BeforeFieldInit, editorParentType);

    //                var ctor = editorTypeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, Type.EmptyTypes);
    //                var ilgenerator = ctor.GetILGenerator();
    //                ilgenerator.Emit(OpCodes.Ldarg_0);
    //                ilgenerator.Emit(OpCodes.Ldstr, stdMetaData.EditorTypeGUID);

    //                var baseCtor = editorParentType.GetConstructor(new Type[] { typeof(string) });
    //                ilgenerator.Emit(OpCodes.Call, baseCtor);
    //                ilgenerator.Emit(OpCodes.Ret);

    //                var editorType = editorTypeBuilder.CreateType();
    //#if DEBUG
    //                var editor = Activator.CreateInstance(editorType);
    //                var props = editorType.GetProperties();
    //#endif
    //                var editorDescriptorDummy = EditorDescriptorDummy.StaticInstance;
    //                var editorDescriptorMetaData = new ExportEditorDescriptorAttribute {
    //                    EditorType = editorType,
    //                    TypeGUID = stdMetaData.EditorTypeGUID
    //                };
    //                return new CreatedEditorDescriptor(editorDescriptorDummy, editorDescriptorMetaData);
    //            }
    //            catch(Exception ex) {
    //                LoggerService.WriteException(ex);
    //                return null;
    //            }
    //        }

    //        /// <summary>
    //        /// 根据标准值的集合和指定的标准值类型,通过反射生成一个标准值及其语言键值的元组列表;
    //        /// </summary>
    //        /// <param name="valueInfos"></param>
    //        /// <param name="valueType"></param>
    //        /// <returns></returns>
    //        public static object CreateStandardValueLanguageTupleList(IEnumerable<StandardValueLanguageInfo> valueInfos,Type valueType) {
    //            var tupleType = typeof(ValueTuple<,>).MakeGenericType(valueType,typeof(string));
    //            var tupleListType = typeof(List<>).MakeGenericType(tupleType);
    //            var tupleList = Activator.CreateInstance(tupleListType);
    //            var addMethod = tupleListType.GetMethod(nameof(List<int>.Add));

    //            foreach (var valueInfo in valueInfos) {
    //                if(valueInfo.StandardValue == null) {
    //                    continue;
    //                }

    //                if (!valueType.IsAssignableFrom(valueInfo.StandardValue.GetType())) {
    //                    continue;
    //                }

    //                var tuple = Activator.CreateInstance(tupleType, valueInfo.StandardValue, valueInfo.LanguageKey);

    //                addMethod.Invoke(tupleList,new object[] { tuple });
    //            }

    //            return tupleList;
    //        }

    //        /// <summary>
    //        /// 根据标准值的集合和指定的标准值类型,通过反射生成一个标准值的列表;
    //        /// </summary>
    //        /// <param name="valueInfos"></param>
    //        /// <param name="valueType"></param>
    //        /// <returns></returns>
    //        public static object CreateStandardValueList(IEnumerable<StandardValueLanguageInfo> valueInfos, Type valueType) {
    //            var listType = typeof(List<>).MakeGenericType(valueType);
    //            var list = Activator.CreateInstance(listType);
    //            var addMethod = listType.GetMethod(nameof(List<int>.Add));

    //            foreach (var valueInfo in valueInfos) {
    //                if (valueInfo.StandardValue == null) {
    //                    continue;
    //                }

    //                if (!valueType.IsAssignableFrom(valueInfo.StandardValue.GetType())) {
    //                    continue;
    //                }

    //                addMethod.Invoke(list, new object[] { valueInfo.StandardValue });
    //            }

    //            return list;
    //        }

    //        private readonly Lazy<IStandardValuesEditorInfoDescriptor, IStandardValuesEditorInfoDescriptorMetaData>[] _standardValuesEditorInfoDescriptor;
    //        private readonly CreatedEditorDescriptor[] _editorDescriptors;
    //        public IEnumerable<CreatedEditorDescriptor> EditorDescriptors => Enumerable.Empty<CreatedEditorDescriptor>();
    //    }

    //    class ValueToLanguageConveter<TValue>:IObjectEntityConverter<TValue,string> {
    //        public ValueToLanguageConveter(IEnumerable<(TValue value,string languageKey)> standardValueLanguageInfos) {
    //            this._standardValueLanguageInfos = standardValueLanguageInfos.ToArray();
    //        }

    //        private readonly (TValue value, string languageKey)[] _standardValueLanguageInfos;
    //        private readonly Dictionary<string, string> _languageCache = new Dictionary<string, string>();

    //        public Type ObjectType => typeof(TValue);

    //        public Type EntityType => typeof(string);

    //        public string ConvertObjectToEntity(TValue tObject) {
    //            (TValue value, string languageKey)? equalLanguageInfo = null;
    //            foreach (var stdValueLanguageInfo in _standardValueLanguageInfos) {
    //                if (Equals(stdValueLanguageInfo.value, tObject)) {
    //                    equalLanguageInfo = stdValueLanguageInfo;
    //                }
    //            }

    //            if(equalLanguageInfo != null) {

    //                if (_languageCache.TryGetValue(equalLanguageInfo.Value.languageKey, out var lanRes)) {
    //                    return lanRes;
    //                }
    //                else if (Equals(equalLanguageInfo.Value.value, tObject)) {
    //                    lanRes = LanguageService.FindResourceString(equalLanguageInfo.Value.languageKey);
    //                    _languageCache.Add(equalLanguageInfo.Value.languageKey, lanRes);
    //                    return lanRes;
    //                }
    //            }
    //            return tObject.ToString();
    //        }


    //        public TValue ConvertEntityToObject(string entity) {
    //            throw new NotSupportedException();
    //        }

    //        public object ConvertObjectToEntity(object tObject) {
    //            if(tObject is TValue value) {
    //                return ConvertObjectToEntity(value);
    //            }

    //            return null;
    //        }

    //        public object ConvertEntityToObject(object entity) {
    //            throw new NotSupportedException();
    //        }
    //    }


    //   /// <summary>
    //   /// 使用了静态缓存的泛型抽象编辑器,运行时将通过Emit生成继承自此类的类型;
    //   /// </summary>
    //   /// <typeparam name = "TValue" >对应的标准值类型</ typeparam >
    //    public abstract class StandardValuesEditorEx<TValue> : StandardValuesEditor<TValue> {
    //        /// <summary>
    //        /// 根据编辑器类型标识,构造一个编辑器类型;
    //        /// </summary>
    //        /// <param name="editorType"></param>
    //        public StandardValuesEditorEx(string editorType) : base(GetStandardValues(editorType),GetConverter(editorType)) {

    //        }

    //        /// <summary>
    //        /// 根据编辑器类型标识,从字典中获取对应的标准值集合;
    //        /// </summary>
    //        /// <param name="editorType"></param>
    //        /// <returns></returns>
    //        private static IEnumerable<TValue> GetStandardValues(string editorType) {
    //            if(StandardValuesDictionary.TryGetValue(editorType,out var values)){
    //                return values;
    //            }
    //            throw new ArgumentNullException($"We failed to find the standard values for {nameof(editorType)} {editorType}.");
    //        }

    //        /// <summary>
    //        /// 根据编辑器类型标识,从字典中获取对应的转换器;
    //        /// </summary>
    //        /// <param name="editorType"></param>
    //        /// <returns></returns>
    //        private static IObjectEntityConverter<TValue, string>  GetConverter(string editorType) {
    //            if(ConverterDictionary.TryGetValue(editorType,out var converter)) {
    //                return converter;
    //            }
    //            throw new ArgumentNullException($"We failed to find the standard values for {nameof(editorType)} {editorType}.");
    //        }

    //        /// <summary>
    //        ///存储标准值集合的静态存储字典;键为对应的编辑器类型标识;
    //        /// </summary>
    //        public static IDictionary<string, IEnumerable<TValue>> StandardValuesDictionary { get; } = new Dictionary<string, IEnumerable<TValue>>();

    //        /// <summary>
    //        ///存储标准值(语言)转换器的静态存储字典;键为对应的编辑器类型标识; 
    //        /// </summary>
    //        public static IDictionary<string, IObjectEntityConverter<TValue, string>> ConverterDictionary { get; } = new Dictionary<string, IObjectEntityConverter<TValue, string>>();
    //    }

    //    /// <summary>
    //    /// 编辑器描述器默认实现;
    //    /// </summary>
    //    class EditorDescriptorDummy : GenericStaticInstance<EditorDescriptorDummy>, IEditorDescriptor {

    //    }
}
