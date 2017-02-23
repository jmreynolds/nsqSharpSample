using System;
using Core.Messages;
using NsqSharp.Bus;

namespace NsqdPublisher.Handlers
{
    public class MessageReceivedServiceHandler : IHandleMessages<MessageReceived>
    {
        public void Handle(MessageReceived message)
        {
            Console.WriteLine($"Handled by Command Line: {message.Text}");
        }
    }
}