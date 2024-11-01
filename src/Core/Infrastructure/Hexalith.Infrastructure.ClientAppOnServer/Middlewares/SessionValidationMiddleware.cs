namespace Hexalith.Infrastructure.ClientAppOnServer.Middlewares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Hexalith.Infrastructure.ClientApp.Services;
using Microsoft.AspNetCore.Http;

public class SessionValidationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IUserSessionService _sessionService;

    public SessionValidationMiddleware(RequestDelegate next, IUserSessionService sessionService)
    {
        _next = next;
        _sessionService = sessionService;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.User.Identity.IsAuthenticated)
        {
            var sessionId = context.Session.GetString("SessionId");
            if (string.IsNullOrEmpty(sessionId))
            {
                // Create new session
                sessionId = await _sessionService.CreateSessionAsync(context.User);
                context.Session.SetString("SessionId", sessionId);
            }
            else
            {
                // Validate existing session
                var isValid = await _sessionService.ValidateSessionAsync(sessionId);
                if (!isValid)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return;
                }
            }
        }

        await _next(context);
    }
}

