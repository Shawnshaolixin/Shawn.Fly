using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Shawn.Fly.EventBus
{
    public class EventStoreInMemory : IEventStore
    {
        private readonly ConcurrentDictionary<string, HashSet<Type>> _eventhandlers = new ConcurrentDictionary<string, HashSet<Type>>();
        public bool AddSubscription<TEvent, TEventHandler>()
            where TEvent : IEventBase
            where TEventHandler : IEventHandler<TEvent>
        {
            var eventKey = GetEventKey<TEvent>();
            if (_eventhandlers.ContainsKey(eventKey))
            {
                return _eventhandlers[eventKey].Add(typeof(TEventHandler));
            }
            else
            {
                return _eventhandlers.TryAdd(eventKey, new HashSet<Type>()
                {
                    typeof(TEventHandler)
                });
            }
        }

        public bool Clear()
        {
            _eventhandlers.Clear();
            return true;
        }

        public ICollection<Type> GetEventHandlerTypes<TEvent>() where TEvent : IEventBase
        {
            if (_eventhandlers.Count == 0) return new Type[0];
            var eventKey = GetEventKey<TEvent>();
            if (_eventhandlers.TryGetValue(eventKey, out var handler))
            {
                return handler;
            }
            return new Type[0];
        }

        public string GetEventKey<TEvent>()
        {
            return typeof(TEvent).FullName;
        }

        public bool HasSubscriptionForEvent<TEvent>() where TEvent : IEventBase
        {
            if (_eventhandlers.Count == 0) return false;
            var eventKey = GetEventKey<TEvent>();
            return _eventhandlers.ContainsKey(eventKey);
        }

        public bool RemoveSubscription<TEvent, TEventHandler>()
            where TEvent : IEventBase
            where TEventHandler : IEventHandler<TEvent>
        {
            if (_eventhandlers.Count == 0) return false;
            var eventKey = GetEventKey<TEvent>();
            if (_eventhandlers.ContainsKey(eventKey))
            {
                return _eventhandlers[eventKey].Remove(typeof(TEventHandler));
            }
            return false;


        }
    }
}
