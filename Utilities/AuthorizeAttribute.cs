using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace reactAzure;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {

        var oClaimsPrincial = context.HttpContext.User; // will read the ASP.NET FORMS Auth cookie
        if (oClaimsPrincial == null)
        {
            this.SetUnauthorized(context);
            return;

        }

        var aAuthTokens = context.HttpContext.Request.Headers["Authorization"];

        string sAuthToken = aAuthTokens.Any() ? aAuthTokens[0] : String.Empty;
        if (String.IsNullOrWhiteSpace(sAuthToken))
        {
            this.SetUnauthorized(context);
            return;
        }

        JwtSecurityToken? oJwtSecurityToken = null;
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            oJwtSecurityToken = tokenHandler.ReadJwtToken(sAuthToken);
        }
        catch (Exception)
        {
            this.SetUnauthorized(context);
            return;
        }

        var oLoginClaim = (oJwtSecurityToken == null) ? null : oJwtSecurityToken.Claims.ToList().FirstOrDefault(c => c.Type == "login");
        if (oLoginClaim == null)
        {
            this.SetUnauthorized(context);
            return;
        }

        var oClaimsIdentity = context.HttpContext.User.Identity as ClaimsIdentity;
        if (oClaimsIdentity != null)
        {
            oClaimsIdentity.AddClaim(oLoginClaim);
        }
    }

    protected void SetUnauthorized(AuthorizationFilterContext context)
    {
        context.Result = new JsonResult(new { message = "Unauthorized", code = StatusCodes.Status401Unauthorized })
                            { StatusCode = StatusCodes.Status401Unauthorized };
    }
}