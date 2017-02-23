using System;
using System.Threading.Tasks;
using Core.Handlers.MessageReceived;
using Core.Handlers.StartService;
using Core.IoC;
using Core.Nsq;
using NsqSharp.Bus;
using NsqSharp.Bus.Configuration;

namespace TestPublisher
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var busConfig = new MothershipBusConfig();
                var config = busConfig.GetBusConfiguration();
                Task.Factory.StartNew(()=> BusService.Start(config));
                var bus = ObjectFactory.Container.TryGetInstance<IBus>();
                var busCounter = 0;
                while (bus == null || busCounter<10)
                {
                    Task.Delay(500).Wait();
                    bus = ObjectFactory.Container.TryGetInstance<IBus>();
                    busCounter++;
                }
                bus.Send(new StartServiceEvent());
                Console.WriteLine("Enter your message (blank line to quit):");
                var line = Console.ReadLine();
                while (!string.IsNullOrEmpty(line))
                {
                    var foo = new MessageReceivedEvent {Text = line};
                    bus.Send(foo);
                    line = Console.ReadLine();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }

    public class BusStateChangedHandler : IBusStateChangedHandler
    {
        public void OnBusStarting(IBusConfiguration config) { }
        public void OnBusStopping(IBusConfiguration config, IBus bus) { }
        public void OnBusStopped(IBusConfiguration config) { }

        public void OnBusStarted(IBusConfiguration config, IBus bus)
        {
            Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Enter your message (^C to quit):");
                while (true)
                {
                    var line = Console.ReadLine();
                    if (!string.IsNullOrEmpty(line))
                        bus.Send(new MessageReceivedEvent { Text = line });
                }
            });
        }
    }
}
