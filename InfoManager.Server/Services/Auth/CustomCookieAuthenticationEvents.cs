using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace InfoManager.Server.Services.Auth
{
    public class CustomCookieAuthenticationEvents : CookieAuthenticationEvents
    {
        public CustomCookieAuthenticationEvents() 
        { 
        }
        public override Task RedirectToAccessDenied(RedirectContext<CookieAuthenticationOptions> context)
        {
            if(context.Request.Path.StartsWithSegments("/api",StringComparison.OrdinalIgnoreCase))
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                return Task.CompletedTask;
            } 
            return base.RedirectToAccessDenied(context);
        }
        public override Task RedirectToLogin(RedirectContext<CookieAuthenticationOptions> context)
        {
            if (context.Request.Path.StartsWithSegments("/api", StringComparison.OrdinalIgnoreCase))
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                return Task.CompletedTask;
            }
            return base.RedirectToLogin(context);
        }
    }
}
