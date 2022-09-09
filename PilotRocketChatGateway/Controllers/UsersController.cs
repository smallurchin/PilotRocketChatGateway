﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PilotRocketChatGateway.PilotServer;
using PilotRocketChatGateway.UserContext;

namespace PilotRocketChatGateway.Controllers
{
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IContextService _contextService;

        public UsersController(IContextService contextService)
        {
            _contextService = contextService;
        }
        [Authorize]
        [HttpGet("api/v1/users.presence")]
        public string Presence(int ids)
        {
            var context = _contextService.GetContext(HttpContext.GetTokenActor());
            var user = context.ChatService.LoadUser(ids);

            var result = new { success = true, full = false, users = new User[] { user } };
            return JsonConvert.SerializeObject(result);
        }
    }
}
