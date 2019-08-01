using Microsoft.Extensions.Logging;
using Shawn.Fly.EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shawn.Fly.WebApi.Controllers
{
    public class CounterEvent : EventBase
    {
        public int Counter { get; set; }
    }
    public class CounterEventHandler : IEventHandler<CounterEvent>
    {

        Task IEventHandler<CounterEvent>.Handle(CounterEvent @event)
        {
            Console.WriteLine("---------------------------" + @event.Counter);
            return Task.CompletedTask;
        }
    }


}
