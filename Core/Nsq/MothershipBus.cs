using System;
using Core.IoC;
using Newtonsoft.Json;
using NsqSharp;
using NsqSharp.Bus;
using NsqSharp.Bus.Configuration;
using NsqSharp.Bus.Configuration.BuiltIn;
using NsqSharp.Bus.Configuration.Providers;

namespace Core.Nsq
{
    public static class MothershipBus
    {
        public static void Start(IHandlerTypeToChannelProvider channelProvider,
            IBusStateChangedHandler busStateChangedHandler = null,
            IMessageTypeToTopicProvider topicProvider = null)
        {
            if (channelProvider == null)
                throw new ArgumentNullException(nameof(channelProvider));
            var busConfig = new MothershipBusConfig();
            var config = busConfig.GetBusConfiguration(channelProvider, busStateChangedHandler, topicProvider);
            BusService.Start(config);
        }
    }

    public class MothershipBusConfig
    {
        public BusConfiguration GetBusConfiguration(
            IHandlerTypeToChannelProvider channelProvider = null, 
            IBusStateChangedHandler busStateChangedHandler = null, 
            IMessageTypeToTopicProvider topicProvider = null)
        {
            if (channelProvider == null)
                channelProvider = new CompositeChannelProvider(new ChannelProviderBase[]{});
            var config = new BusConfiguration(
                new StructureMapObjectBuilder(ObjectFactory.Container),
                new NewtonsoftJsonSerializer(typeof(JsonConvert).Assembly),
                new MessageAuditor(),
                topicProvider ?? new TopicProvider(),
                channelProvider,
                defaultThreadsPerHandler: 8,
                defaultNsqLookupdHttpEndpoints: new[] {"127.0.0.1:4161"},
                busStateChangedHandler: busStateChangedHandler,
                preCreateTopicsAndChannels: true,
                nsqConfig:
                new Config
                {
                    BackoffStrategy = new FullJitterStrategy()
                    //MaxRequeueDelay = TimeSpan.FromSeconds(0.1),
                    //MaxBackoffDuration = TimeSpan.FromSeconds(0.2)
                });
            return config;
        }
    }
}