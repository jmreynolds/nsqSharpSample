using System.Diagnostics;
using System.Text;
using NsqSharp.Bus;
using NsqSharp.Bus.Logging;

namespace Core.Nsq
{
    public class MessageAuditor : IMessageAuditor
    {
        public void OnReceived(IBus bus, IMessageInformation info)
        {
            if (info.MessageType != typeof(AuditMessageInformation))
            {
                bus.Send(Convert(info));
            }
        }

        public void OnSucceeded(IBus bus, IMessageInformation info)
        {
            if (info.MessageType != typeof(AuditMessageInformation))
            {
                bus.Send(Convert(info));
            }
        }

        public void OnFailed(IBus bus, IFailedMessageInformation failedInfo)
        {
            if (failedInfo.MessageType != typeof(AuditMessageInformation))
            {
                bus.Send(Convert(failedInfo));
            }
            else
            {
                // failed audit

                string logEntry =
                    $"id: {failedInfo.Message.Id} action:{failedInfo.FailedAction} reason:{failedInfo.FailedReason} topic:{failedInfo.Topic} channel:{failedInfo.Channel} msg:{Encoding.UTF8.GetString(failedInfo.Message.Body)} ex:{failedInfo.Exception}";

                if (failedInfo.FailedAction == FailedMessageQueueAction.Requeue)
                {
                    Trace.TraceWarning(logEntry);
                }
                else
                {
                    Trace.TraceError(logEntry);
                }
            }
        }

        private static AuditMessageInformation Convert(IMessageInformation info)
        {
            return new AuditMessageInformation
            {
                UniqueIdentifier = info.UniqueIdentifier,
                Topic = info.Topic,
                Channel = info.Channel,
                HandlerType = info.HandlerType.FullName,
                MessageType = info.MessageType.FullName,
                MessageId = info.Message.Id,
                MessageAttempt = info.Message.Attempts,
                MessageNsqdAddress = info.Message.NsqdAddress,
                MessageBody = TryGetString(info.Message.Body),
                MessageOriginalTimestamp = info.Message.Timestamp,
                Started = info.Started,
                Finished = info.Finished,
                Success = (info.Finished == null ? null : (bool?)true)
            };
        }

        private static AuditMessageInformation Convert(IFailedMessageInformation info)
        {
            return new AuditMessageInformation
            {
                UniqueIdentifier = info.UniqueIdentifier,
                Topic = info.Topic,
                Channel = info.Channel,
                HandlerType = info.HandlerType.FullName,
                MessageType = info.MessageType.FullName,
                MessageId = info.Message.Id,
                MessageAttempt = info.Message.Attempts,
                MessageNsqdAddress = info.Message.NsqdAddress,
                MessageBody = TryGetString(info.Message.Body),
                MessageOriginalTimestamp = info.Message.Timestamp,
                Started = info.Started,
                Finished = info.Finished,
                Success = false,
                FailedAction = info.FailedAction.ToString(),
                FailedReason = info.FailedReason.ToString(),
                FailedException = info.Exception?.ToString()
            };
        }

        private static string TryGetString(byte[] data)
        {
            try
            {
                return Encoding.UTF8.GetString(data);
            }
            catch
            {
            }

            return null;
        }
    }
}