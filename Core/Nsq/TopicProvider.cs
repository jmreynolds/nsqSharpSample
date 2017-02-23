using System;
using System.Collections.Generic;
using System.Linq;
using Core.Handlers.MessageReceived;
using Core.Handlers.StartService;
using NsqSharp.Bus.Configuration.Providers;

namespace Core.Nsq
{
    public class TopicProvider : IMessageTypeToTopicProvider
    {
        private readonly Topics _topics = new Topics();

        public string GetTopic(Type messageType)
        {
            return _topics.GetTopic(messageType);
        }
    }

    internal class Topics
    {
        private readonly Dictionary<Type, string> _typeTopics;

        public Topics()
        {
            _typeTopics = new Dictionary<Type, string>();
            Add<StartServiceEvent>(nameof(StartServiceEvent));
            Add<AuditMessageInformation>(nameof(AuditMessageInformation));
            Add<MessageReceivedEvent>(nameof(MessageReceivedEvent));
        }
        public string GetTopic(Type messageType)
        {
            return _typeTopics[messageType];
        }

        protected void Add<T>(string topicName)
        {
            _typeTopics.Add(typeof(T), topicName);
        }

        protected void Validate()
        {
            // Check for duplicate topic names
            var dupes = _typeTopics
                            .GroupBy(p => p.Value.ToLower())
                            .Where(g => g.Count() > 1)
                            .Select(p => p.Key)
                            .ToList();

            if (dupes.Count != 0)
            {
                throw new Exception($"Duplicate topic name(s): {string.Join(", ", dupes)}");
            }

            // Check for missed types
            var missingTypes = new List<Type>();
            foreach (var messageType in typeof(Topics).Assembly.GetExportedTypes())
            {
                if (messageType == typeof(Topics))
                    continue;

                if (!_typeTopics.ContainsKey(messageType))
                    missingTypes.Add(messageType);
            }

            if (missingTypes.Count != 0)
            {
                throw new Exception($"Type(s) missing topic: {string.Join(", ", missingTypes.Select(p => p.Name))}");
            }
        }
    }
}