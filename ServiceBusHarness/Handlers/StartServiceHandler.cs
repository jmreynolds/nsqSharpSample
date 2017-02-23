using System;
using Core.Messages;
using NsqSharp.Bus;

namespace NsqdPublisher.Handlers
{
    public class StartServiceHandler : IHandleMessages<StartService>
    {
        public void Handle(StartService message)
        {
            Console.WriteLine("Service Started");
        }
    }
}