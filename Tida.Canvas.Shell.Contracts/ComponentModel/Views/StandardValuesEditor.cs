using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Tida.Application.Contracts.App;

namespace Tida.Canvas.Shell.Contracts.ComponentModel.Views {
    /// <summary>
    /// 使用了下拉的标准值集合编辑器即<see cref="StandardValueLanguageInfo{TValue}"/>的管理器,本类旨在在具备不同参数类型的编辑器类中共用重要的静态成员;
    /// </summary>
    public static class StandardValuesEditorManager {
        /// <summary>
        /// 静态的创建选择器UI的工厂.在使用<see cref="StandardValuesEditor{TValue}"/>前,必须设定此值,否则将会抛出一个运行时异常;
        /// </summary>
        public static Func<Selector> SelectorFactory { get; set; }
    }

    /// <summary>
    /// 泛型,使用了下拉的标准值集合编辑器;
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public class StandardValuesEditor<TValue> : ContentControl {
        

        public StandardValuesEditor(IEnumerable<StandardValueLanguageInfo<TValue>> values) {
            if (StandardValuesEditorManager.SelectorFactory == null) {
                throw new ArgumentNullException($"{nameof(StandardValuesEditorManager.SelectorFactory)} should be set to use {nameof(StandardValuesEditor<object>)}");
            }

            _selector = StandardValuesEditorManager.SelectorFactory();
            this.Content = _selector;
            _valueModels = values.Select(p => new ValueModel(p.StandardValue, LanguageService.FindResourceString(p.LanguageKey))).ToArray();

            _selector.ItemsSource = _valueModels;
            _selector.DisplayMemberPath = nameof(ValueModel.DisplayName);
            _selector.SelectionChanged += ComboBox_SelectionChanged;
            if (_valueModels.Length != 0) {
                RefreshSelectedItem(_valueModels[0].Value);
            }
        }


        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            RefreshSelectedValue();
        }

        /// <summary>
        /// 根据下拉的选中值,刷新依赖属性的值;
        /// </summary>
        private void RefreshSelectedValue() {
            if (!(_selector.SelectedItem is ValueModel valueModel)) {
                return;
            }

            SelectedValue = valueModel.Value;
        }

        /// <summary>
        /// 根据依赖属性的值,刷新下拉的选中值;
        /// </summary>
        private void RefreshSelectedItem(TValue selectedValue) {
            if (_selector.SelectedItem is ValueModel valueModel && object.Equals(selectedValue, valueModel.Value)) {
                return;
            }

            var selectedValueModel = _valueModels.FirstOrDefault(p => object.Equals(p.Value, selectedValue));
            _selector.SelectedItem = selectedValueModel;
        }

        private readonly Selector _selector;

        private readonly ValueModel[] _valueModels;

        public TValue SelectedValue {
            get { return (TValue)GetValue(SelectedValueProperty); }
            set { SetValue(SelectedValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedValueProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedValueProperty =
            DependencyProperty.Register(nameof(SelectedValue), typeof(TValue), typeof(StandardValuesEditor<TValue>), new FrameworkPropertyMetadata(default(TValue), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, SelectedValue_PropertyChanged));

        private static void SelectedValue_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            if (!(d is StandardValuesEditor<TValue> editor)) {
                return;
            }

            if (!(e.NewValue is TValue selectedValue)) {
                return;
            }

            editor.RefreshSelectedItem(selectedValue);
        }

        class ValueModel {
            public ValueModel(TValue value, string displayName) {
                this.Value = value;
                this.DisplayName = displayName;
            }

            public TValue Value { get; }

            public string DisplayName { get; }
        }
    }
}
