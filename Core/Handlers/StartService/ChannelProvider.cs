using Core.Nsq;

namespace Core.Handlers.StartService
{
    public class ChannelProvider : ChannelProviderBase
    {
        public ChannelProvider()
        {
            Add<StartServiceEventHandler, StartServiceEvent>(nameof(StartServiceEventHandler));
        }
    }
}
