using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Core.Handlers.MessageReceived;
using Core.Handlers.StartService;
using Core.IoC;
using Core.Nsq;
using NsqSharp;
using NsqSharp.Bus;
using NsqSharp.Bus.Configuration;
using NsqSharp.Bus.Configuration.Providers;

namespace NsqSharpTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var channelProviders = new ChannelProviderBase[] {
                new Core.Handlers.MessageReceived.ChannelProvider(),
                new Core.Handlers.StartService.ChannelProvider() };
            MothershipBus.Start(
                channelProvider: new CompositeChannelProvider(channelProviders),
                busStateChangedHandler: new BusStateChangedHandler());

        }
    }
    public class BusStateChangedHandler : IBusStateChangedHandler
    {
        public void OnBusStarting(IBusConfiguration config) { }
        public void OnBusStopping(IBusConfiguration config, IBus bus) { }
        public void OnBusStopped(IBusConfiguration config) { }

        public void OnBusStarted(IBusConfiguration config, IBus bus)
        {
            Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Enter your message (^C to quit):");
                while (true)
                {
                    var line = Console.ReadLine();
                    if (!string.IsNullOrEmpty(line))
                        bus.Send(new MessageReceivedEvent(){ Text = line });
                }
            });
        }
    }
    public class CompositeChannelProvider : IHandlerTypeToChannelProvider
    {
        private readonly Dictionary<Type, string> _channels;

        public CompositeChannelProvider(IEnumerable<ChannelProviderBase> channelProviders)
        {
            if (channelProviders == null)
                throw new ArgumentNullException(nameof(channelProviders));

            _channels = new Dictionary<Type, string>();
            foreach (var channelProvider in channelProviders)
            {
                foreach (var handler in channelProvider.GetHandlerTypes())
                {
                    var channel = channelProvider.GetChannel(handler);
                    _channels.Add(handler, channel);
                }
            }
        }

        public string GetChannel(Type handlerType)
        {
            return _channels[handlerType];
        }

        public IEnumerable<Type> GetHandlerTypes()
        {
            return _channels.Keys;
        }
    }

}
