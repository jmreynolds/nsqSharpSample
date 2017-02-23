using System;
using NsqSharp.Bus;

namespace Core.Handlers.StartService
{
    public class StartServiceEventHandler : IHandleMessages<StartServiceEvent>
    {
        public void Handle(StartServiceEvent message)
        {
            Console.WriteLine("Service Started");
        }
    }
}