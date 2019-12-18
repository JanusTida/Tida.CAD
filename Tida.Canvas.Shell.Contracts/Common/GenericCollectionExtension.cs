using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.Contracts.Common {
    /// <summary>
    /// 集合拓展方法;
    /// </summary>
    public static class GenericCollectionExtension {
        /// <summary>
        /// 按排序的方式加入;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="item"></param>
        public static void AddOrderBy<T>(this IList<T> list, T item, Func<T, decimal> sort, bool isDescending = false) {
            if (item == null) {
                throw new ArgumentNullException(nameof(item));
            }

            if (sort == null) {
                throw new ArgumentNullException(nameof(sort));
            }

            if (list == null) {
                throw new ArgumentNullException(nameof(list));
            }


            var index = 0;

            foreach (var listItem in list) {
                if (isDescending && sort(listItem) < sort(item)) {
                    break;
                }

                if (sort(listItem) > sort(item)) {
                    break;
                }

                index++;
            }

            list.Insert(index, item);

        }
    }
}
