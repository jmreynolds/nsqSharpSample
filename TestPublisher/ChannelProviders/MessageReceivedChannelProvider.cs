using Core.Messages;
using Core.Nsq;
using TestPublisher.Handlers;

namespace TestPublisher.ChannelProviders
{
    internal class MessageReceivedChannelProvider : ChannelProviderBase
    {
        internal MessageReceivedChannelProvider()
        {
            Add<MessageReceivedUiHandler, MessageReceived>(nameof(MessageReceivedUiHandler));
        }
    }
}