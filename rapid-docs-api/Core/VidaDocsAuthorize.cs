using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace rapid_docs_api.Core
{
    public class VidaDocsAuthorize : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
            var expClaim = securityToken.Claims.First(x => x.Type == "exp").Value;
            DateTime tokenExpiryDateTime = new DateTime(1970, 1, 1).AddMilliseconds(Convert.ToInt64(expClaim) * 1000);
            if (tokenExpiryDateTime < DateTime.UtcNow)
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
