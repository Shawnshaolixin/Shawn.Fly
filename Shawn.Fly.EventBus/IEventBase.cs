using Newtonsoft.Json;
using System;

namespace Shawn.Fly.EventBus
{
    public interface IEventBase
    {
        /// <summary>
        /// 事件发布事件
        /// </summary>
        DateTimeOffset EventAt { get; }
        /// <summary>
        /// 事件Id
        /// </summary>
        string EventId { get; }
    }
    public abstract class EventBase : IEventBase
    {
        [JsonProperty]
        public DateTimeOffset EventAt { get; private set; }

        [JsonProperty]
        public string EventId { get; private set; }

        protected EventBase()
        {
            EventId = Guid.NewGuid().ToString();
            EventAt = DateTimeOffset.UtcNow;
        }

        [JsonConstructor]
        public EventBase(string eventId, DateTimeOffset eventAt)
        {
            EventId = eventId;
            EventAt = eventAt;
        }
    }
}
