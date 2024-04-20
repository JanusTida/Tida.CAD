using System;
using System.Collections.Generic;
using System.Text;

namespace Tida.Canvas.Infrastructure {
#if NETSTANDARD1_0

    /// <summary>
    /// 此类用于补充.NetStandard1.0中缺失的<see cref="List{T}"/>部分;
    /// </summary>
    public static class CollectionExtensions {
        /// <summary>
        /// 补充<see cref="List{T}"/>的遍历方法;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="action"></param>
        public static void ForEach<T>(this List<T> list, Action<T> action) {

            if (list == null) {
                throw new ArgumentNullException(nameof(list));
            }


            if (action == null) {
                throw new ArgumentNullException(nameof(action));
            }

            for (int i = 0; i < list.Count; i++) {
                action(list[i]);
            }
        }
    }

#endif
}
