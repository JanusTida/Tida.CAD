using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Infrastructure.Utils {
    /// <summary>
    /// 集合拓展方法;
    /// </summary>
    public static class CollectionExtensions {
        /// <summary>
        /// 从<paramref name="collection"/>中移除指定的集合,即<paramref name="items"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="items"></param>
        public static void RemoveItems<T>(this ICollection<T> collection,IEnumerable<T> items) {

            if (collection == null) {
                throw new ArgumentNullException(nameof(collection));
            }


            if (items == null) {
                throw new ArgumentNullException(nameof(items));
            }

            foreach (var item in items) {
                if (collection.Contains(item)) {
                    collection.Remove(item);
                }
                else {

                }
            }
        }

    }
}
