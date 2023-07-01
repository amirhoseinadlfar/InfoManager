using InfoManager.Server.Models;
using InfoManager.Server.Services.Repositorys.Interfaces;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;

namespace InfoManager.Server.Middlewares
{
    public class SessionAuthMiddleware : IMiddleware
    {
        public Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var authorize = context.Features.GetRequiredFeature<IEndpointFeature>().Endpoint?.Metadata.FirstOrDefault(x => x is AuthorizeAttribute) as AuthorizeAttribute;
            if (authorize is null)
                return next(context);
            if (authorize is not null && authorize.Policy == "User")
            {
                var keyClaim = context.User.Claims.FirstOrDefault(x => x.Type == "loginKey");
                if (keyClaim is null)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.CompletedTask;
                }
                ISessionRepository sessionRepository = context.RequestServices.GetRequiredService<ISessionRepository>();
                Session? session = sessionRepository.FindAsync(keyClaim.Value).Result;
                if (session is null)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.CompletedTask;
                }
                context.Items["Session"] = session;
            }
            return next(context);
        }
    }
}
