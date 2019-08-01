using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Shawn.Fly.EventBus
{
    public static class EventBusExtensions
    {
        public static IServiceCollection AddEventBus(this IServiceCollection services)
        {
            services.AddSingleton<IEventBus, EventBus>();
            services.AddSingleton<IEventStore, EventStoreInMemory>();
            Subscribe.AutoSubscribe(services);
            return services;
        }

    }


    public class Subscribe
    {

        public static void AutoSubscribe(IServiceCollection services)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            assemblies = assemblies.Where(p =>
                p.FullName.StartsWith("Shawn.Fly", StringComparison.OrdinalIgnoreCase)

            ).ToArray();
            var types = assemblies.SelectMany(x =>
            {
                try
                {
                    return x.GetTypes();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    return ex.Types.Where(t => t != null);
                }
            }).ToList();

            var handlerTypes = types.Where(p => typeof(IEventHandler<>).IsAssignableFrom(p)).ToList();

            foreach (var handlerType in handlerTypes)
            {

                //   services.AddTransient(handlerType);

                var integationEventTypes = handlerType
               .GetTypeInfo()
               .GetInterfaces()
               .Where(x => typeof(IEventHandler<>).IsAssignableFrom(x))
               .SelectMany(x => x.GenericTypeArguments)
               .Where(x => typeof(IEventBase).IsAssignableFrom(x))
               .Distinct()
               .ToList();

                integationEventTypes.ForEach(x =>
                {
                    var subscribeItemType = typeof(SubscribeItem<,>).MakeGenericType(x, handlerType);
                    var subscribeItem = Activator.CreateInstance(subscribeItemType);
                    subscribeItemType.GetMethod("Subscribe").Invoke(subscribeItem, new object[] { });
                });
            }

        }
    }
    class SubscribeItem<T, TH>
     where T : IEventBase
     where TH : IEventHandler<T>
    {
        IEventBus _eventBus;

        public SubscribeItem(IEventBus eventBus)
        {
            this._eventBus = eventBus;
        }

        public void Subscribe()
        {
            _eventBus.Subscribe<T, TH>();
        }
    }
}
