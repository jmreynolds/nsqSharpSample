using Core.Messages;
using Core.Nsq;
using NsqdPublisher.Handlers;

namespace NsqdPublisher.ChannelProviders
{
    internal class StartServiceChannelProvider : ChannelProviderBase
    {
        internal StartServiceChannelProvider()
        {
            Add<StartServiceHandler, StartService>(nameof(StartServiceHandler));
        }
    }
}
