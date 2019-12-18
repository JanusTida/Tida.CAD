using Tida.Canvas.Shell.Contracts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.Contracts.App {
    /// <summary>
    /// 线程调用器契约,此契约封装在逻辑中存在耗时操作的切换线程处理，
    /// 替换后可以方便地进行单元测试;
    /// </summary>
    public interface IThreadInvoker {
        /// <summary>
        /// 后台调用;
        /// </summary>
        /// <param name="act"></param>
        void BackInvoke(Action act);
        /// <summary>
        /// 从UI线程调用;
        /// </summary>
        /// <param name="act"></param>
        void UIInvoke(Action act);

    }

    public class ThreadInvoker : GenericServiceStaticInstance<IThreadInvoker> {
        //后台调用;
        public static void BackInvoke(Action act) {
            if(act == null) {
                throw new ArgumentNullException(nameof(act));
            }

            Current?.BackInvoke(act);
        }

        //UI调用;
        public static void UIInvoke(Action act) {
            if (act == null) {
                throw new ArgumentNullException(nameof(act));
            }

            Current?.UIInvoke(act);
        }
    }

    public static class ThreadInvokerExtensions {

        /// <summary>
        /// 添加某个集合的元素转化后到添加另一个元素集合(通常是与UI相关的元素);
        /// 本方法对于数据量比较大的集合添加时,对于改善UI卡顿是有用的;
        /// </summary>
        /// <typeparam name="TOriginalEntity">原元素种类</typeparam>
        /// <typeparam name="TEntity">需添加的元素种类</typeparam>
        /// <param name="entitySet">新集合</param>
        /// <param name="oriEntitySet">原集合</param>
        /// <param name="factory">转化工厂方法</param>
        /// <param name="bufferLength">缓冲区大小</param>
        /// <param name="sleepInterval">睡眠周期</param>
        /// <param name="callBack">完成后回调</param>
        public static void AddBufferItemsToCollection<TOriginalEntity,TEntity>(
            ICollection<TEntity> entitySet,
            IEnumerable<TOriginalEntity> oriEntitySet,
            Func<TOriginalEntity,TEntity> factory,
            Action callBack = null,
            int bufferLength = 10,
            int sleepInterval = 1) {

            if(oriEntitySet == null) {
                throw new ArgumentNullException(nameof(oriEntitySet));
            }
            if(entitySet == null) {
                throw new ArgumentNullException(nameof(entitySet));
            }
            if(factory == null) {
                throw new ArgumentNullException(nameof(factory));
            }
            if(bufferLength <= 0) {
                throw new ArgumentException($"{nameof(bufferLength)} should be larger than zero.");
            }

            var oriEntityBuffer = new TEntity[bufferLength];
            var index = 0;

            ThreadInvoker.BackInvoke(() => {
                foreach (var oriEntity in oriEntitySet) {
                    var entity = factory(oriEntity);
                    
                    oriEntityBuffer[index] = entity;
                    index++;
                    if(index == bufferLength) {
                        ThreadInvoker.UIInvoke(() => {
                            foreach (var row in oriEntityBuffer) {
                                entitySet.Add(row);
                            }
                        });
                        System.Threading.Thread.Sleep(sleepInterval);
                        index = 0;
                    }
                }

                for (int i = 0; i < index; i++) {
                    ThreadInvoker.UIInvoke(() => {
                        entitySet.Add(oriEntityBuffer[i]);
                    });
                }
                callBack?.Invoke();
            });
            
        }
    }
}
