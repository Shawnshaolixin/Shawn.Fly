using Autofac;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shawn.Fly.EventBus
{
    public class EventBus : IEventBus
    {
        private readonly IEventStore _eventStore;

        public EventBus(IEventStore eventStore)
        {
            _eventStore = eventStore;

        }
        public bool Publish<TEvent>(TEvent @event) where TEvent : IEventBase
        {
            if (!_eventStore.HasSubscriptionForEvent<TEvent>())
            {
                return false;
            }
            var handlers = _eventStore.GetEventHandlerTypes<TEvent>();
            if (handlers.Count > 0)
            {
                var handlerTasks = new List<Task>();
                foreach (Type handlerType in handlers)
                {

                    IEventHandler<TEvent> handler = Activator.CreateInstance(handlerType, false) as IEventHandler<TEvent>;
                    handlerTasks.Add(handler.Handle(@event));
                }
                Task.WhenAll(handlerTasks).ConfigureAwait(false);
                return true;
            }
            return false;
        }

        public bool Subscribe<TEvent, TEventHandler>()
            where TEvent : IEventBase
            where TEventHandler : IEventHandler<TEvent>
        {
            return _eventStore.AddSubscription<TEvent, TEventHandler>();
        }

        public bool Unsubscribe<TEvent, TEventHandler>()
            where TEvent : IEventBase
            where TEventHandler : IEventHandler<TEvent>
        {
            return _eventStore.RemoveSubscription<TEvent, TEventHandler>();
        }
    }
}
