using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.Contracts.Common {
    /// <summary>
    /// 事件处理帮助者;本事件采用了Prism的Event多对多订阅-发布的机制,
    /// 当且仅当ServiceProvider被设定且指定了<see cref="IEventAggregator"/>实例后,本类才可以正常工作;
    /// 一般情况下,订阅对象应为在内存中常驻的对象,
    /// 除非使用者明确地知道对象应在何时取消订阅动作;
    /// </summary>
    public static class CommonEventHelper {
        /// <summary>
        /// Prism事件聚合器,本单位为Prism事件处理的核心单位;
        /// 当且仅当ServiceProvider被设定实现后,本类才可以正常工作;
        /// </summary>
        private static IEventAggregator _aggregator;
        public static IEventAggregator Aggregator => _aggregator ?? (_aggregator = ServiceProvider.Current.GetInstance<IEventAggregator>());

        //public static void Publish<TEvent, TPayload>(TPayload payload) where TEvent : PubSubEvent<TPayload>, new()
        //    => Aggregator?.GetEvent<TEvent>()?.Publish(payload);

        public static void Publish<TEvent>() where TEvent : PubSubEvent, new() => Aggregator?.GetEvent<TEvent>()?.Publish();
        public static void Publish<TEvent, TPayload>(TPayload payload) where TEvent : PubSubEvent<TPayload>, new() =>
            Aggregator?.GetEvent<TEvent>()?.Publish(payload);

        public static TEventType GetEvent<TEventType>() where TEventType : EventBase, new() => Aggregator?.GetEvent<TEventType>();

        public static void SubsToken<TEvent, TPayload>(ref SubscriptionToken token, Action<TPayload> subscriber)
            where TEvent : PubSubEvent<TPayload>, new() {
            var evt = Aggregator.GetEvent<TEvent>();
            if (token != null) {
                evt?.Unsubscribe(token);
            }

            token = evt?.Subscribe(subscriber);
        }

        public static void SubsToken<TEvent>(ref SubscriptionToken token, Action subscriber) where TEvent : PubSubEvent, new() {
            var evt = Aggregator.GetEvent<TEvent>();
            if (token != null) {
                evt.Unsubscribe(token);
            }
            token = evt.Subscribe(subscriber);
        }

        public static SubscriptionToken Subscribe<TEvent, TPayload>(Action<TPayload> subscriber) where TEvent : PubSubEvent<TPayload>, new() {
            return Aggregator?.GetEvent<TEvent>()?.Subscribe(subscriber);
        }

        public static void Subscribe<TEvent>(Action subscriber) where TEvent : PubSubEvent, new() {
            Aggregator?.GetEvent<TEvent>()?.Subscribe(subscriber);
        }

        /// <summary>
        /// 向事件处理器发布事件;
        /// </summary>
        /// <typeparam name="TEventHandler"></typeparam>
        /// <typeparam name="TEventArgs"></typeparam>
        /// <param name="args"></param>
        /// <param name="eventHandlers"></param>
        public static void PublishEventToHandlers<TEventHandler, TEventArgs>(TEventArgs args, IEnumerable<TEventHandler> eventHandlers)
            where TEventHandler : class, IEventHandler<TEventArgs> {
            if (eventHandlers == null) {
                return;
            }

            foreach (var handler in eventHandlers.OrderBy(p => p.Sort)) {
                if (handler == null) {
                    LoggerService.WriteCallerLine($"{nameof(handler)} coudn't be null.");
                    continue;
                }

                try {
                    if (!handler.IsEnabled) {
                        continue;
                    }
                    handler.Handle(args);
                }
                catch (Exception ex) {
                    LoggerService.WriteCallerLine($"{handler.GetType()}:{ex.Message}");
                    LoggerService.WriteException(ex);
                }
            }
        }
        /// <summary>
        /// 向事件处理契发布事件;将会自动寻找事件处理器队列;
        /// </summary>
        /// <typeparam name="TEventHandler"></typeparam>
        /// <typeparam name="TEventArgs"></typeparam>
        /// <param name="args"></param>
        public static void PublishEventToHandlers<TEventHandler, TEventArgs>(TEventArgs args)
            where TEventHandler : class, IEventHandler<TEventArgs> {

            if (args == null) {
                return;
            }

            var handlers = GenericServiceStaticInstances<TEventHandler>.Currents;
            PublishEventToHandlers(args, handlers);
        }
        

        /// <summary>
        /// 向事件处理器发布事件;
        /// </summary>
        /// <typeparam name="TEventHandler"></typeparam>
        /// <typeparam name="TEventArgs"></typeparam>
        /// <param name="args"></param>
        /// <param name="eventHandlers"></param>
        public static void PublishEventToHandlers<TEventHandler>(IEnumerable<TEventHandler> eventHandlers) where TEventHandler : class, IEventHandler {
            if (eventHandlers == null) {
                return;
            }

            foreach (var handler in eventHandlers.OrderBy(p => p.Sort)) {
                if (handler == null) {
                    LoggerService.WriteCallerLine($"{nameof(handler)} coudn't be null.");
                    continue;
                }

                try {
                    if (!handler.IsEnabled) {
                        continue;
                    }
                    handler.Handle();
                }
                catch (Exception ex) {
                    LoggerService.WriteCallerLine($"{handler.GetType()} ex.Message");
                    LoggerService.WriteException(ex);
                }
            }
        }
        /// <summary>
        /// 向事件处理契发布事件;将会自动寻找事件处理器队列;
        /// </summary>
        /// <typeparam name="TEventHandler"></typeparam>
        /// <typeparam name="TEventArgs"></typeparam>
        /// <param name="args"></param>
        public static void PublishEventToHandlers<TEventHandler>() where TEventHandler : class, IEventHandler {
            var handlers = GenericServiceStaticInstances<TEventHandler>.Currents;
            PublishEventToHandlers(handlers);
        }

        /// <summary>
        /// 订阅事件;若订阅者已经订阅,则将不会订阅;
        /// 此方法适合在内存中常驻的对象(比如各种稳定的服务)进行订阅操作;
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <typeparam name="TPayLoad"></typeparam>
        /// <param name="evt"></param>
        /// <param name="subscriber"></param>
        public static void SubscribeCheckingSubscribed<TEvent, TPayLoad>(this TEvent evt, Action<TPayLoad> subscriber) where TEvent : PubSubEvent<TPayLoad> {
            if (evt.Contains(subscriber)) {
                return;
            }
            evt.Subscribe(subscriber);
        }

        public static void SubscribeCheckingSubscribed<TEvent>(this TEvent evt, Action subscriber) where TEvent : PubSubEvent {
            if (evt.Contains(subscriber)) {
                return;
            }
            evt.Subscribe(subscriber);
        }
    }

    
}
