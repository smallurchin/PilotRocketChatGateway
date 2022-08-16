﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PilotRocketChatGateway.Authentication;
using PilotRocketChatGateway.UserContext;
using PilotRocketChatGateway.WebSockets;
using System.Net.WebSockets;
using System.Text;

namespace PilotRocketChatGateway.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WebSocketsController : ControllerBase
    {
        private readonly ILogger<WebSocketsController> _logger;
        private readonly AuthSettings _authSettings;
        private IContextService _contextService;
        private readonly IWebSocketsServiceFactory _webSocketsServiceFactory;
        private readonly IWebSocketSessionFactory _webSocketSessionFactory;

        public WebSocketsController(ILogger<WebSocketsController> logger, IOptions<AuthSettings> authSettings, IContextService contextService, IWebSocketsServiceFactory webSocketsServiceFactory, IWebSocketSessionFactory webSocketSessionFactory)
        {
            _logger = logger;
            _authSettings = authSettings.Value;
            _contextService = contextService;
            _webSocketsServiceFactory = webSocketsServiceFactory;
            _webSocketSessionFactory = webSocketSessionFactory;
        }

        [HttpGet("/websocket")]
        public async Task Get()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                _logger.Log(LogLevel.Information, "WebSocket connection established");

                var webSocketService = _webSocketsServiceFactory.CreateWebSocketsService(webSocket, _logger, _authSettings, _contextService, _webSocketSessionFactory);
                await webSocketService.ProcessAsync();
            }
            else
            {
                HttpContext.Response.StatusCode = 400;
            }
        }
    }
}
