﻿using Ascon.Pilot.DataClasses;
using Ascon.Pilot.Server.Api.Contracts;
using PilotRocketChatGateway.UserContext;

namespace PilotRocketChatGateway.PilotServer
{
    public class MessagesCallback : IMessageCallback
    {
        private readonly ILogger _logger;
        private readonly IContext _context;

        public MessagesCallback(IContext context, ILogger logger)
        {
            _logger = logger;
            _context = context;
        }
        public void CreateNotification(DNotification notification)
        {
        }

        public void NotifyMessageCreated(NotifiableDMessage message)
        {
            try
            {
                _context.WebSocketsSession.SendMessageToClientAsync(message.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }
        }

        public void NotifyTypingMessage(Guid chatId, int personId)
        {
        }
    }
}
