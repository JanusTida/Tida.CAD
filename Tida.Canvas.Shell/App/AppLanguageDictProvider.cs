using Tida.Canvas.Shell.Contracts.App;
using Tida.Canvas.Shell.Contracts.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;

namespace Tida.Canvas.Shell.App {
    [Export(typeof(ILanguageDictionary))]
    public class AppLanguageDictonary : ILanguageDictionary {
        public string this[string keyName] => LanguageDict[keyName] as string;

        private ResourceDictionaryEx _languageDict;
        private ResourceDictionaryEx LanguageDict {
            get {
                //查找Application中的现存字典项;
                if (_languageDict == null) {
                    _languageDict = System.Windows.Application.Current.Resources.MergedDictionaries.FirstOrDefault(p => {
                        var rsEx = p as ResourceDictionaryEx;
                        if (rsEx != null) {
                            return rsEx.Name == Constants.LanguageDict;
                        }
                        return false;
                    }) as ResourceDictionaryEx;
                }

                //若未发现,则创建一个新的;
                if (_languageDict == null) {
                    _languageDict = new ResourceDictionaryEx { Name = Constants.LanguageDict };
                    System.Windows.Application.Current.Resources.MergedDictionaries.Add(_languageDict);
                }


                return _languageDict;
            }
        }

        public void AddMergedDictionaryFromPath(string path) {
            using (var rs = File.OpenRead(path)) {
                try {
                    var res = XamlReader.Load(rs) as ResourceDictionary;
                    LanguageDict.MergedDictionaries.Add(res);
                }
                catch (Exception ex) {
                    LoggerService.WriteException(ex);
                }
            }
        }

        public void ClearMergedDictionaries() {
            LanguageDict.MergedDictionaries.Clear();
        }
    }
}
