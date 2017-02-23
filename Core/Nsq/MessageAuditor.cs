using System.Diagnostics;
using System.Text;
using NsqSharp.Bus;
using NsqSharp.Bus.Logging;

namespace Core.Nsq
{
    public class MessageAuditor : IMessageAuditor
    {
        public void OnReceived(IBus bus, IMessageInformation info) { }
        public void OnSucceeded(IBus bus, IMessageInformation info) { }
        public void OnFailed(IBus bus, IFailedMessageInformation failedInfo)
        {
            Trace.TraceError(failedInfo.Exception.ToString());
        }
    }
}