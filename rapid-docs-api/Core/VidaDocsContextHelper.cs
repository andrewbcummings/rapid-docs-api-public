using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using rapid_docs_core.Authentication;
using System.Security.Claims;

namespace rapid_docs_api.Core
{
    public static class VidaDocsContextHelper
    {
        public static IServiceCollection AddVidaDocsContextFactory(this IServiceCollection services)
        {
            services.AddScoped<VidaDocsContext, VidaDocsContext>((prov) =>
            {
                var http = prov.GetService<IHttpContextAccessor>();

                var hasHttpContext = http.HttpContext != null;
                if (hasHttpContext && http.HttpContext.User.Identity.IsAuthenticated)
                {
                    var socialId = http.HttpContext.User.Identity.Name;
                    var token = http.HttpContext.Request.Headers["Authorization"];
                    var email = http.HttpContext.User.FindFirst(ClaimTypes.Email).Value;

                    return new VidaDocsContext(socialId, email, token);
                }

                return null;
            });

            return services;
        }
    }
}
