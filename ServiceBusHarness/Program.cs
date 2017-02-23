using Core.Handlers.MessageReceived;
using Core.Nsq;

namespace NsqdPublisher
{
    class Program
    {
        static void Main(string[] args)
        {
            var channelProviders = new ChannelProviderBase[] {
                new ChannelProvider(),
                new Core.Handlers.StartService.ChannelProvider() };
            MothershipBus.Start(new CompositeChannelProvider(channelProviders));
        }
    }

}
