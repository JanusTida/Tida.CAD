using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Xml.Linq;
using System.Reflection;
using Tida.Canvas.Shell.Contracts.App;
using Tida.Canvas.Shell.Contracts.Common;
using Tida.Xml;

namespace Tida.Canvas.Shell.App {
    /// <summary>
    /// 语言服务实现者;
    /// </summary>
    [Export(typeof(ILanguageService))]
    public class LanguageServiceImpl : ILanguageService {
        private LanguageProvider _currentProvider;
        public LanguageProvider CurrentProvider {
            get { return _currentProvider; }
            set {
                if (value == _currentProvider) {
                    return;
                }

                _currentProvider = value;
                InitializeLanguageDict();
            }
        }

        private List<LanguageProvider> _allProviders = new List<LanguageProvider>();
        public IEnumerable<LanguageProvider> AllProviders => _allProviders.Select(p => p);

        private ILanguageDictionary _languageDict;
        public string FindResourceString(string keyName) {

            if (string.IsNullOrEmpty(keyName)) {
                return keyName;
            }

            if (_languageDict != null) {
                return (_languageDict[keyName] as string) ?? keyName;
            }

            return keyName;
        }

        public void Initialize() {
            InitializeDocument();
            InitilizeProviders();
            InitilizeCurrentProvider();
        }

        private string ConfigFileName => $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}/{Constants.LanguageConfigName}";
        /// <summary>
        /// 初始化/读取配置文档;
        /// </summary>
        private void InitializeDocument() {
            if (!File.Exists(ConfigFileName)) {
                return;
            }

            try {
                _xDoc = XDocument.Load(ConfigFileName);
            }
            catch (Exception ex) {
                LoggerService.WriteCallerLine(ex.Message);
            }
        }

        private XDocument _xDoc;

        /// <summary>
        /// 初始化所有语言提供者;
        /// </summary>
        private void InitilizeProviders() {
            if (_xDoc == null) {
                return;
            }
            _allProviders.Clear();

            if (_xDoc == null) {
                return;
            }

            var elem = _xDoc.Root.Element(Constants.LanguageProviders);
            if (elem == null) {
                return;
            }

            var providerElems = elem.Elements(Constants.Provider);

            foreach (var pElem in providerElems) {
                var lanName = pElem.GetXElemValue(Constants.ProviderName);
                var lanType = pElem.GetXElemValue(Constants.ProviderType);
                var provider = new LanguageProvider(lanName, lanType);
                _allProviders.Add(provider);
            }
        }

        /// <summary>
        /// 初始化当前语言;
        /// </summary>
        private void InitilizeCurrentProvider() {
            if (_xDoc == null) {
                return;
            }

            var curLan = _xDoc.Root.GetXElemValue(Constants.CurrentLanguage);
            if (string.IsNullOrEmpty(curLan)) {
                return;
            }

            var provider = AllProviders.FirstOrDefault(p => p.Type == curLan);
            if (provider == null) {
                return;
            }

            CurrentProvider = provider;
        }

        /// <summary>
        /// 初始化语言,操作字典等;
        /// </summary>
        private void InitializeLanguageDict() {

            if (CurrentProvider == null) {
                return;
            }


            _languageDict = ServiceProvider.GetInstance<ILanguageDictionary>();

            if (_languageDict == null) {
                return;
            }

            _languageDict.ClearMergedDictionaries();
            var dicts = new List<ResourceDictionary>();
            var providerDirect = $"{AppDomainService.ExecutingAssemblyDirectory}\\{Constants.LanguageDirect}\\{CurrentProvider.Type}";
            if (!Directory.Exists(providerDirect)) {
                return;
            }

            var di = new DirectoryInfo(providerDirect);
            //遍历添加语言文件;
            foreach (var file in di.GetFiles()) {
                try {
                    _languageDict.AddMergedDictionaryFromPath($"{providerDirect}/{file.Name}");
                }
                catch (Exception ex) {
                    LoggerService.WriteCallerLine(ex.Message);
                }
            }
        }

        public string TryGetStringWithFormat(string languageFormatKey, params object[] args) {
            try {
                var format = FindResourceString(languageFormatKey);
                return string.Format(format, args);
            }
            catch (Exception ex) {
                LoggerService.WriteException(ex);
                return FindResourceString(languageFormatKey);
            }
        }
    }


   
}
