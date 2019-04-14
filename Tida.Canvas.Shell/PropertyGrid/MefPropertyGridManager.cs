using Tida.Application.Contracts.App;
using Tida.Canvas.Shell.Contracts.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using static Tida.Canvas.Shell.Contracts.ComponentModel.Constants;
using TelerikEditor = Telerik.Windows.Controls.Data.PropertyGrid.EditorAttribute;
using TelerikEditorStyle = Telerik.Windows.Controls.Data.PropertyGrid.EditorStyle;
using Tida.Canvas.Shell.ComponentModel;

namespace Tida.Canvas.Shell.PropertyGrid {
    /// <summary>
    /// Mef 属性表格管理器;
    /// </summary>
    [Export]
   public  class MefPropertyGridManager {

        static MefPropertyGridManager() {
            _tidaFactoryType = DynamicAsmManager.ModuleBuilder.DefineType(TidaFactoryTypeName, TypeAttributes.Public);
            DynamicAsmManager.Saving += DynamicAsmManager_Saving;
        }

        private static void DynamicAsmManager_Saving(object sender, EventArgs e) {
            _tidaFactoryType.CreateType();
        }

        private const string ModelPrefix = "TidaModel_";
        private const string TidaFactoryTypeName = "TidaFactory";

        /// <summary>
        /// 工场类类型;
        /// </summary>
        private static readonly TypeBuilder _tidaFactoryType;

        [ImportingConstructor]
        public MefPropertyGridManager(
            [ImportMany]IEnumerable<Lazy<IPropertyDescriptor, IPropertyDescriptorMetaData>> mefPropertyDescriptors,
            [ImportMany]IEnumerable<IPropertyDescriptorProvider> mefPropertyDescriptorProviders,

            [ImportMany]IEnumerable<Lazy<IIgnoredPropertyDescriptor, IIgnoredPropertyDescriptorMetaData>> mefIgnoredPropertyDescriptors,
            [ImportMany]IEnumerable<IIgnoredPropertyDescriptorProvider> mefIgnoredPropertyDescriptorProviders,

            [ImportMany]IEnumerable<Lazy<IEditorDescriptor,IEditorDescriptorMetaData>> mefEditorDescriptors,
            [ImportMany]IEnumerable<IEditorDescriptorProvider> mefEditorDescriptorProviders) {
            
            _mefInheritablePropertyDescriptors =
                mefPropertyDescriptors.Where(p => p.Metadata.Inheritable).Select(p => new CreatedPropertyDescriptor(p.Value, p.Metadata)).
                Union(mefPropertyDescriptorProviders.SelectMany(p => p.PropertyDescriptors)).ToArray();

            _mefNonInheritablePropertyDescriptors = mefPropertyDescriptors.
                Where(p => !p.Metadata.Inheritable).ToDictionary(p => p.Value.PropertyInfo, p => new CreatedPropertyDescriptor(p.Value, p.Metadata));

            _mefIgnoredPropertyDescriptors =
                mefIgnoredPropertyDescriptors.Where(p => p.Metadata.Inheritable).Select(p => new CreatedIgnoredPropertyDescriptor(p.Value, p.Metadata)).
                Union(mefIgnoredPropertyDescriptorProviders.SelectMany(p => p.IgnoredPropertyDescriptors)).ToArray();

            _mefNonInheritableIgnoredProperties = new HashSet<PropertyInfo>(
                mefIgnoredPropertyDescriptors.Where(p => !p.Metadata.Inheritable).SelectMany(p => p.Value.PropertyInfos));

            _mefEditorDescriptors = mefEditorDescriptors.Select(p => new CreatedEditorDescriptor(p.Value, p.Metadata)).
                Union(mefEditorDescriptorProviders.SelectMany(p => p.EditorDescriptors)).
                ToArray();
        }
        
        /// <summary>
        /// 可以应用于派生类型的属性描述器;
        /// </summary>
        private readonly CreatedPropertyDescriptor[] _mefInheritablePropertyDescriptors;
        /// <summary>
        /// 不可应用于派生类型的属性描述器;
        /// </summary>
        private readonly Dictionary<PropertyInfo, CreatedPropertyDescriptor> _mefNonInheritablePropertyDescriptors;


        private readonly CreatedIgnoredPropertyDescriptor[] _mefIgnoredPropertyDescriptors;
        private readonly HashSet<PropertyInfo> _mefNonInheritableIgnoredProperties;

        private readonly CreatedEditorDescriptor[] _mefEditorDescriptors;

        private readonly Dictionary<Type, Func<object, object>> _createWrapperObjectDelegates = new Dictionary<Type, Func<object, object>>();

        private const MethodAttributes _getsetAttr = MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName;

        public object MakeObject(object originObject) {
            if (!_createWrapperObjectDelegates.TryGetValue(originObject.GetType(), out var func)) {
                func = CreateFactoryDelegate(originObject.GetType());
                _createWrapperObjectDelegates.Add(originObject.GetType(), func);
            }


            return func(originObject);
        }

        private Type CreateWrapperType(Type objectType) {
            var typeBuilder = DynamicAsmManager.ModuleBuilder.DefineType($"{ModelPrefix}_{objectType.FullName.Replace('.','_')}", TypeAttributes.Public | TypeAttributes.BeforeFieldInit);
            var originObjectField = typeBuilder.DefineField("_originObject", objectType, FieldAttributes.Private | FieldAttributes.InitOnly);

            GenerateConstructor(typeBuilder, objectType, originObjectField);

            foreach (var originPropInfo in objectType.GetProperties(BindingFlags.Public | BindingFlags.Instance)) {
                AddProperty(typeBuilder, originPropInfo, originObjectField);
            }

            var type = typeBuilder.CreateType();
#if DEBUG
            var props = type.GetProperties();
            foreach (var prop in props) {
                //var attr =  prop.GetCustomAttribute(typeof(TelerikEditor),false);
            }
            var ssProps = TypeDescriptor.GetProperties(type).OfType<System.ComponentModel.PropertyDescriptor>();
#endif
            return type;
            
        }
        

        /// <summary>
        /// 生成构造器;
        /// </summary>
        /// <param name="typeBuilder"></param>
        /// <param name="objectType"></param>
        /// <param name="originObjectField"></param>
        private void GenerateConstructor(TypeBuilder typeBuilder, Type objectType,FieldInfo originObjectField) {
            var constructor = typeBuilder.DefineConstructor(MethodAttributes.Public | MethodAttributes.HideBySig, CallingConventions.Standard, new Type[] { objectType });

            var constructorILGenerator = constructor.GetILGenerator();
            constructorILGenerator.Emit(OpCodes.Ldarg_0);
            constructorILGenerator.Emit(OpCodes.Call, typeof(object).GetConstructor(Type.EmptyTypes));
            constructorILGenerator.Emit(OpCodes.Ldarg_0);
            constructorILGenerator.Emit(OpCodes.Ldarg_1);
            constructorILGenerator.Emit(OpCodes.Stfld, originObjectField);
            constructorILGenerator.Emit(OpCodes.Ret);
        }

        /// <summary>
        /// 添加属性;
        /// </summary>
        /// <param name="typeBuilder"></param>
        /// <param name="originPropInfo"></param>
        private void AddProperty(TypeBuilder typeBuilder, PropertyInfo originPropInfo, FieldInfo originObjectField) {
            //原属性必须能够读取,添加属性才能具备意义;
            if (!CheckPropertyCanRead(originPropInfo)) {
                return;
            }

            var propertyBuilder = CreatePropertyBuilder(typeBuilder, originPropInfo);
            if (propertyBuilder == null) {
                return;
            }

            AddGetMethodToProperty(typeBuilder, propertyBuilder, originPropInfo, originObjectField);

            if (!CheckPropertyCanWrite(originPropInfo)) {
                return;
            }

            AddSetMethodToProperty(typeBuilder, propertyBuilder, originPropInfo, originObjectField);
        }

        /// <summary>
        /// 根据指定的原属性信息,检查是否能创建读取方法;
        /// </summary>
        /// <param name="originPropertyInfo"></param>
        /// <returns></returns>
        private bool CheckPropertyCanRead(PropertyInfo originPropertyInfo) {
            //检查是否能读取;
            if (!originPropertyInfo.CanRead) {
                return false;
            }

#if DEBUG
            if (originPropertyInfo.Name == "IsEditing") {

            }
#endif

            if (_mefNonInheritableIgnoredProperties.Contains(originPropertyInfo)) {
                return false;
            }

            //检查该属性是否被忽略;
            foreach (var ignoredPropDescriptorTuple in _mefIgnoredPropertyDescriptors) {
                var igDescriptor = ignoredPropDescriptorTuple.IgnoredPropertyDescriptor;
                foreach (var propInfo in igDescriptor.PropertyInfos) {
                    if (originPropertyInfo.CheckPropertyInfoInheritedFrom(propInfo)) {
                        return false;
                    }
                }
            }

            return true;
        }


        /// <summary>
        /// 根据指定的原属性信息,检查是否能创建写入方法;
        /// </summary>
        /// <param name="originPropertyInfo"></param>
        /// <returns></returns>
        private bool CheckPropertyCanWrite(PropertyInfo originPropertyInfo) {
            if (!originPropertyInfo.CanWrite) {
                return false;
            }

            //若该属性所属类型为值类型,将不能为该属性创建写入方法,否则这可能会导致一个运行时致命错误;
            if (originPropertyInfo.DeclaringType.IsValueType) {
                return false;
            }

            if(_mefNonInheritablePropertyDescriptors.TryGetValue(originPropertyInfo,out var createdPropDescriptor0)) {
                return !createdPropDescriptor0.PropertyDescriptorMetaData.IsReadOnly;
            }

            
            foreach (var createdPropDescriptor in _mefInheritablePropertyDescriptors.Where(p => !p.PropertyDescriptorMetaData.IsReadOnly)) {

                var descriptor = createdPropDescriptor.PropertyDescriptor;
                var metaData = createdPropDescriptor.PropertyDescriptorMetaData;

                if (originPropertyInfo.CheckPropertyInfoInheritedFrom(descriptor.PropertyInfo)) {
                    return true;
                }

            }

            return false;
        }

        /// <summary>
        /// 为属性添加读取方法;
        /// </summary>
        /// <param name="typeBuilder"></param>
        /// <param name="propertyBuilder"></param>
        /// <param name="originPropInfo"></param>
        /// <param name="originObjectField"></param>
        private void AddGetMethodToProperty(TypeBuilder typeBuilder, PropertyBuilder propertyBuilder, PropertyInfo originPropInfo, FieldInfo originObjectField) {
            //为属性添加读取方法;
            var getMethod = typeBuilder.DefineMethod($"get_{propertyBuilder.Name}", _getsetAttr, originPropInfo.PropertyType, Type.EmptyTypes);
            var getValueIlGenerator = getMethod.GetILGenerator();
            if (originObjectField.FieldType.IsValueType) {
                getValueIlGenerator.Emit(OpCodes.Ldarg_0);
                getValueIlGenerator.Emit(OpCodes.Ldflda, originObjectField);
                getValueIlGenerator.Emit(OpCodes.Call, originPropInfo.GetMethod);
                getValueIlGenerator.Emit(OpCodes.Ret);
            }
            else {
                getValueIlGenerator.Emit(OpCodes.Ldarg_0);
                getValueIlGenerator.Emit(OpCodes.Ldfld, originObjectField);
                getValueIlGenerator.Emit(OpCodes.Callvirt, originPropInfo.GetMethod);
                getValueIlGenerator.Emit(OpCodes.Ret);
            }

            propertyBuilder.SetGetMethod(getMethod);
        }

        /// <summary>
        /// 为属性添加写入方法;
        /// </summary>
        /// <param name="typeBuilder"></param>
        /// <param name="propertyBuilder"></param>
        /// <param name="originPropInfo"></param>
        /// <param name="originObjectField"></param>
        private void AddSetMethodToProperty(TypeBuilder typeBuilder, PropertyBuilder propertyBuilder, PropertyInfo originPropInfo, FieldInfo originObjectField) {
            var setMethod = typeBuilder.DefineMethod($"set_{propertyBuilder.Name}", _getsetAttr, null, new Type[] { originPropInfo.PropertyType });
            var setValueIlGenerator = setMethod.GetILGenerator();
            setValueIlGenerator.Emit(OpCodes.Ldarg_0);
            setValueIlGenerator.Emit(OpCodes.Ldfld, originObjectField);
            setValueIlGenerator.Emit(OpCodes.Ldarg_1);
            setValueIlGenerator.Emit(OpCodes.Callvirt, originPropInfo.SetMethod);
            setValueIlGenerator.Emit(OpCodes.Ret);

            propertyBuilder.SetSetMethod(setMethod);
        }

        /// <summary>
        /// 根据原属性信息,创建属性;
        /// </summary>
        /// <param name="typeBuilder"></param>
        /// <param name="originPropInfo"></param>
        /// <returns></returns>
        private PropertyBuilder CreatePropertyBuilder(TypeBuilder typeBuilder, PropertyInfo originPropInfo) {
            var propName = $"Value_{originPropInfo.Name}";
            var propertyBuilder = typeBuilder.DefineProperty(propName, PropertyAttributes.HasDefault, originPropInfo.PropertyType, Type.EmptyTypes);

            var attributes = GetCustomAttributeBuilders(originPropInfo);

            foreach (var attr in attributes) {
                propertyBuilder.SetCustomAttribute(attr);
            }

            return propertyBuilder;
        }

        /// <summary>
        /// 获取所有重要的注解;
        /// </summary>
        /// <param name="originPropInfo"></param>
        /// <returns></returns>
        private IEnumerable<CustomAttributeBuilder> GetCustomAttributeBuilders(PropertyInfo originPropInfo) {
            var relatedPropertyDescriptor = GetPropertyDescriptor(originPropInfo);
            if (relatedPropertyDescriptor != null) {
                var metaData = relatedPropertyDescriptor.PropertyDescriptorMetaData;
                yield return new CustomAttributeBuilder(typeof(DisplayNameAttribute).GetConstructor(new Type[] { typeof(string) }), new object[] { LanguageService.FindResourceString(metaData.DisplayNameKey) });
                yield return new CustomAttributeBuilder(typeof(ReadOnlyAttribute).GetConstructor(new Type[] { typeof(bool) }), new object[] { metaData.IsReadOnly });
                yield return new CustomAttributeBuilder(typeof(CategoryAttribute).GetConstructor(new Type[] { typeof(string) }), new object[] { LanguageService.FindResourceString(metaData.CategoryNameKey) });
                yield return new CustomAttributeBuilder(typeof(DescriptionAttribute).GetConstructor(new Type[] { typeof(string) }), new object[] { LanguageService.FindResourceString(metaData.DescriptionNameKey) });

                if (string.IsNullOrEmpty(metaData.EditorTypeGUID)) {
                    yield break;
                }

                var editorDescriptor = _mefEditorDescriptors.FirstOrDefault(p => p.EditorDescriptorMetaData.TypeGUID == metaData.EditorTypeGUID);
                if(editorDescriptor == null) {
                    yield break;
                }

                var editorMetaData = editorDescriptor.EditorDescriptorMetaData;
                yield return new CustomAttributeBuilder(
                    typeof(TelerikEditor).GetConstructor(new Type[] { typeof(Type), typeof(string), typeof(TelerikEditorStyle) }),
                    new object[] { editorMetaData.EditorType, metaData.EditorTargetProperty,
                        //TelerikEditorStyle.None
                        ConvertEditorStyleToTEditorStyle(metaData.EditorStyle)
                    }
                );
            }
            else {
                yield return new CustomAttributeBuilder(typeof(DisplayNameAttribute).GetConstructor(new Type[] { typeof(string) }), new object[] { originPropInfo.Name });
                yield return new CustomAttributeBuilder(typeof(ReadOnlyAttribute).GetConstructor(new Type[] { typeof(bool) }), new object[] { !originPropInfo.CanWrite });
                yield return new CustomAttributeBuilder(typeof(CategoryAttribute).GetConstructor(new Type[] { typeof(string) }), new object[] { LanguageService.FindResourceString(CategoryName_Other) });
                yield return new CustomAttributeBuilder(typeof(DescriptionAttribute).GetConstructor(new Type[] { typeof(string) }), new object[] { originPropInfo.Name });
            }
        }

        /// <summary>
        /// 转换编辑器展示类型;
        /// </summary>
        /// <param name="editorStyle"></param>
        /// <returns></returns>
        private static TelerikEditorStyle ConvertEditorStyleToTEditorStyle(EditorStyle editorStyle) {
            switch (editorStyle) {
                case EditorStyle.Modal:
                    return TelerikEditorStyle.Modal;
                case EditorStyle.DropDown:
                    return TelerikEditorStyle.DropDown;
                case EditorStyle.None:
                    return TelerikEditorStyle.None;
                default:
                    return TelerikEditorStyle.None;
            }
        }

        /// <summary>
        /// 根据原属性信息,获取相关的属性描述器;
        /// </summary>
        /// <returns></returns>
        private CreatedPropertyDescriptor GetPropertyDescriptor(PropertyInfo originPropInfo) {
            if (_mefNonInheritablePropertyDescriptors.TryGetValue(originPropInfo,out var createdPropDescriptor0)) {
                return createdPropDescriptor0;
            }

            foreach (var createdPropDescriptor in _mefInheritablePropertyDescriptors) {
                var propDescriptor = createdPropDescriptor.PropertyDescriptor;
                var metaData = createdPropDescriptor.PropertyDescriptorMetaData;
                if (originPropInfo.CheckPropertyInfoInheritedFrom(propDescriptor.PropertyInfo)) {
                    return createdPropDescriptor;
                }
            }

            return null;
        }

        /// <summary>
        /// 创建工厂委托;
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        private Func<object, object> CreateFactoryDelegate(Type objectType) {
            var wrapperType = CreateWrapperType(objectType);
            var dm = new DynamicMethod($"CreateFuckyoumodelObject_{objectType.Name}", typeof(object), new Type[] { typeof(object) }, DynamicAsmManager.ModuleBuilder);
            var ilGenerator = dm.GetILGenerator();
            SetILGenerator(ilGenerator, objectType, wrapperType);
            
            var method = _tidaFactoryType.DefineMethod($"CreateFuckyoumodelObject_{objectType.Name}", MethodAttributes.Static | MethodAttributes.Public, typeof(object), new Type[] { typeof(object) });
            var mGenerator = method.GetILGenerator();
            SetILGenerator(mGenerator, objectType, wrapperType);
            
            return dm.CreateDelegate(typeof(Func<object, object>)) as Func<object, object>;
        }

        /// <summary>
        /// 向IL生成代码器中添加需要一个行为:
        /// 1.接受一个object原类型参数，
        /// 2.并返回封装类型的object实例. 
        /// </summary>
        /// <param name="ilGenerator"></param>
        /// <param name="objectType"></param>
        /// <param name="wrapperType"></param>
        private static void SetILGenerator(ILGenerator ilGenerator, Type objectType, Type wrapperType) {

            var localObject = ilGenerator.DeclareLocal(objectType);

            var retNullLabel = ilGenerator.DefineLabel();

            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Isinst, objectType);

            if (objectType.IsValueType) {

                ilGenerator.Emit(OpCodes.Brfalse_S, retNullLabel);

                ilGenerator.Emit(OpCodes.Ldarg_0);
                ilGenerator.Emit(OpCodes.Unbox_Any, objectType);
                ilGenerator.Emit(OpCodes.Stloc_0);
            }
            else {
                ilGenerator.Emit(OpCodes.Dup);
                ilGenerator.Emit(OpCodes.Stloc_0);
                ilGenerator.Emit(OpCodes.Brfalse_S, retNullLabel);
            }

            var wrapperConstructor = wrapperType.GetConstructor(new Type[] { objectType });
            ilGenerator.Emit(OpCodes.Ldloc_0);
            ilGenerator.Emit(OpCodes.Newobj, wrapperConstructor);

            ilGenerator.Emit(OpCodes.Ret);

            ilGenerator.MarkLabel(retNullLabel);
            ilGenerator.Emit(OpCodes.Ldnull);
            ilGenerator.Emit(OpCodes.Ret);
        }

    }

    /// <summary>
    /// 反射拓展;
    /// </summary>
    static class ReflectionExtension {

        /// <summary>
        /// 比较两个属性信息是否是在同一个类型中声明的同一个成员;
        /// 即第一个属性信息是否"继承"自第二个属性信息;
        /// </summary>
        /// <param name="propertyInfo0"></param>
        /// <param name="propertyInfo1"></param>
        /// <param name="inheritedOk">
        /// 是否考虑继承,当<paramref name="propertyInfo0"/>所属类型继承自<paramref name="propertyInfo1"/>所属类型时,
        /// 且该属性在父类型中声明时,将会判定为相等;
        /// </param>
        /// <returns></returns>
        public static bool CheckPropertyInfoInheritedFrom(this PropertyInfo propertyInfo0, PropertyInfo propertyInfo1) {

            if (propertyInfo1.PropertyType != propertyInfo0.PropertyType) {
                return false;
            }
#if DEBUG
            if (propertyInfo0.Name == "StartNodeGuid") {

            }
            if (propertyInfo0 == propertyInfo1) {

            }
#endif

            if (!propertyInfo1.DeclaringType.IsAssignableFrom(propertyInfo0.DeclaringType)) {
                return false;
            }

            if (propertyInfo1.Name != propertyInfo0.Name) {
                return false;
            }

            return true;
        }

        
    }
}
