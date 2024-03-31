using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Steer.Api.Data;
using Steer.Api.Entities;

namespace Steer.Api.Attributes
{
    public class SteerAuthorize : AuthorizeAttribute, IAuthorizationFilter
    {


        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var tokenRepository = context.HttpContext.RequestServices.GetRequiredService<IRepository<Token>>();
            var user = context.HttpContext.User;
            if (user == null || user.Identity == null || !user.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var accessToken = context.HttpContext.Request.Headers.Authorization[0].Replace("Bearer", "").Trim();
            var token = tokenRepository.GetAsync(x => x.AccessToken == accessToken).GetAwaiter().GetResult();
            if (token == null || token.IsValid == false)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
        }
    }
}
