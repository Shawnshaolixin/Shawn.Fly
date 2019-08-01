using System;
using System.Collections.Generic;
using System.Text;

namespace Shawn.Fly.EventBus
{
    public interface IEventBus
    {
        /// <summary>
        /// 给事件添加事件处理
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <typeparam name="TEventHandler"></typeparam>
        /// <returns></returns>
        bool Subscribe<TEvent, TEventHandler>()
            where TEvent : IEventBase
            where TEventHandler : IEventHandler<TEvent>;

        /// <summary>
        /// 移除事件处理
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <typeparam name="TEventHandler"></typeparam>
        /// <returns></returns>
        bool Unsubscribe<TEvent, TEventHandler>()
               where TEvent : IEventBase
            where TEventHandler : IEventHandler<TEvent>;

        /// <summary>
        /// 发布事件
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="event"></param>
        /// <returns></returns>
        bool Publish<TEvent>(TEvent @event) where TEvent : IEventBase;
    }
}
