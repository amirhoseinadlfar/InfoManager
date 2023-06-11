using InfoManager.Server.Models;

namespace InfoManager.Server.Ulitis
{
    public static class HttpContextExtensions
    {
        public static Session GetSession(this HttpContext httpContext)
        {
            return (Session)httpContext.Items["Session"]!;
        }
    }
}
