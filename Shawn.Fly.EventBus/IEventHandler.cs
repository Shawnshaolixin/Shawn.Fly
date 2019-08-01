using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shawn.Fly.EventBus
{
    public interface IEventHandler<in TEvent> where TEvent : IEventBase
    {
        /// <summary>
        ///事件处理
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        Task Handle(TEvent @event);
    }
}
