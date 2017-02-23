using System;
using NsqSharp.Bus;

namespace Core.Handlers.MessageReceived
{
    public class MessageReceivedEventHandler : IHandleMessages<MessageReceivedEvent>
    {
        public void Handle(MessageReceivedEvent message)
        {
            Console.WriteLine(message.Text);
        }
    }

}