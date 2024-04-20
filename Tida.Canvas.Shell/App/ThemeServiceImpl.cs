using Tida.Canvas.Shell.Contracts.App;
using Tida.Canvas.Shell.Contracts.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Tida.Canvas.Shell.App {
    /// <summary>
    /// 资源服务实现;
    /// </summary>
    [Export(typeof(IThemeService))]
    class ThemeServiceImpl : IThemeService {
        public void AddDictionary(ResourceDictionary res) {
            if(ThemeDict == null) {
                LoggerService.WriteCallerLine($"{nameof(ThemeDict)} can't be null.");
                return;
            }

            if (res == null) {
                throw new ArgumentNullException(nameof(res));
            }

            if (!ThemeDict.MergedDictionaries.Contains(res)) {
                ThemeDict.MergedDictionaries.Add(res);
            }
        }

        public void RemoveDictionary(ResourceDictionary res) {
            if (ThemeDict == null) {
                LoggerService.WriteCallerLine($"{nameof(ThemeDict)} can't be null.");
                return;
            }

            if (res == null) {
                throw new ArgumentNullException(nameof(res));
            }

            if (ThemeDict.MergedDictionaries.Contains(res)) {
                ThemeDict.MergedDictionaries.Remove(res);
            }
        }


        private ResourceDictionaryEx _themeDict;
        private ResourceDictionaryEx ThemeDict {
            get {
                //查找Application中的现存字典项;
                if (_themeDict == null) {
                    _themeDict = System.Windows.Application.Current.Resources.MergedDictionaries.FirstOrDefault(p => {
                        if (p is ResourceDictionaryEx rsEx) {
                            return rsEx.Name == Constants.LanguageDict;
                        }
                        return false;
                    }) as ResourceDictionaryEx;
                }

                //若未发现,则创建一个新的; 
                if (_themeDict == null) {
                    _themeDict = new ResourceDictionaryEx { Name = Constants.LanguageDict };
                    System.Windows.Application.Current.Resources.MergedDictionaries.Add(_themeDict);
                }


                return _themeDict;
            }
        }

    }
}
