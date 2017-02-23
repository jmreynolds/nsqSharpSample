using Core.Messages;
using Core.Nsq;
using NsqdPublisher.Handlers;

namespace NsqdPublisher.ChannelProviders
{
    internal class MessageReceivedChannelProvider : ChannelProviderBase
    {
        internal MessageReceivedChannelProvider()
        {
            Add<MessageReceivedServiceHandler, MessageReceived>(nameof(MessageReceivedServiceHandler));
        }
    }
}