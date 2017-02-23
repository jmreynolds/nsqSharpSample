using System;
using Core.Messages;
using NsqSharp.Bus;

namespace TestPublisher.Handlers
{
    public class MessageReceivedUiHandler : IHandleMessages<MessageReceived>
    {
        public void Handle(MessageReceived message)
        {
            Console.WriteLine($"Handled by WPF Line: {message.Text}");
        }
    }
}