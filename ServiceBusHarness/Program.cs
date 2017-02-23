using Core.Nsq;
using NsqdPublisher.ChannelProviders;

namespace NsqdPublisher
{
    class Program
    {
        static void Main(string[] args)
        {
            var channelProviders = new ChannelProviderBase[] {
                new MessageReceivedChannelProvider(),
                new StartServiceChannelProvider() };
            MothershipBus.Start(new CompositeChannelProvider(channelProviders));
        }
    }

}
