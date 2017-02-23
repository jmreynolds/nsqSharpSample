﻿using System;
using System.Collections.Generic;
using NsqSharp.Bus;
using NsqSharp.Bus.Configuration.Providers;
using NsqSharp.Core;

namespace Core.Nsq
{
    public abstract class ChannelProviderBase : IHandlerTypeToChannelProvider
    {
        private readonly Dictionary<Type, string> _channels;
        private readonly List<string> _messageTypeChannels;

        protected ChannelProviderBase()
        {
            _channels = new Dictionary<Type, string>();
            _messageTypeChannels = new List<string>();
        }

        public string GetChannel(Type handlerType)
        {
            return _channels[handlerType];
        }

        public IEnumerable<Type> GetHandlerTypes()
        {
            return _channels.Keys;
        }

        public void Add<THandler, TMessageType>(string channelName)
            where THandler : IHandleMessages<TMessageType>
        {
            if (string.IsNullOrEmpty(channelName))
                throw new ArgumentNullException(nameof(channelName));
            if (!Protocol.IsValidChannelName(channelName))
                throw new ArgumentException("invalid channel name", nameof(channelName));

            _channels.Add(typeof(THandler), channelName);

            // NsqSharp.Bus enforces that a message type can only be produced on a single topic.
            // If we try to add another handler which handles the same message type (and therefore topic) using
            // the same channel name we'll have two handlers competing for the same channel; throw an exception if
            // we accidentally try to do this.

            // TODO: This should be enforced in NsqSharp.Bus

            string messageTypeChannelKey = $"{typeof(TMessageType).FullName}.{channelName}";

            if (_messageTypeChannels.Contains(messageTypeChannelKey))
            {
                throw new Exception(
                    $"Another handler for message type '{typeof(TMessageType)}' already listening to channel '{channelName}'.");
            }

            _messageTypeChannels.Add(messageTypeChannelKey);
        }
    }
}