using System;
using System.Collections.Generic;
using System.Text;

namespace Shawn.Fly.EventBus
{
    public interface IEventStore
    {
        bool AddSubscription<TEvent, TEventHandler>()
            where TEvent : IEventBase
            where TEventHandler : IEventHandler<TEvent>;

        bool RemoveSubscription<TEvent, TEventHandler>()
              where TEvent : IEventBase
            where TEventHandler : IEventHandler<TEvent>;

        bool HasSubscriptionForEvent<TEvent>() where TEvent : IEventBase;
        ICollection<Type> GetEventHandlerTypes<TEvent>() where TEvent : IEventBase;

        bool Clear();
        string GetEventKey<TEvent>();
    }
}
