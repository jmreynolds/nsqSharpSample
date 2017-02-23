using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Handlers.MessageReceived;
using Newtonsoft.Json;
using NsqSharp;

namespace TestPublisher
{
    class Program
    {
        static void Main(string[] args)
        {
            var producer = new Producer("127.0.0.1:4150");
            Console.WriteLine("Enter your message (blank line to quit):");
            string line = Console.ReadLine();
            while (!string.IsNullOrEmpty(line))
            {
                MessageReceivedEvent foo = new MessageReceivedEvent(){Text = line};
                var jsonFoo = JsonConvert.SerializeObject(foo);
                producer.Publish(nameof(MessageReceivedEvent), jsonFoo);
                line = Console.ReadLine();
            }

            producer.Stop();
        }
    }
}
