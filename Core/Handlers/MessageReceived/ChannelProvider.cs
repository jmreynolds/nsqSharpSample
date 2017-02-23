using Core.Nsq;

namespace Core.Handlers.MessageReceived
{
    public class ChannelProvider : ChannelProviderBase
    {
        public ChannelProvider()
        {
            Add<MessageReceivedEventHandler, MessageReceivedEvent>(nameof(MessageReceivedEventHandler));
        }
    }
}