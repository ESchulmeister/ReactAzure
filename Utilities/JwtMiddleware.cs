using log4net;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using reactAzure.Models;
using System;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace reactAzure
{
    public class JwtMiddleware
    {
        #region variables

        private readonly RequestDelegate _next;
        private readonly ILog _logger;
        private readonly AppSettings _appSettings;

        private const string General_Error = "An Unexpected  error has occurred. Please contact the system administrator.";

        #endregion

       #pragma warning disable CS8604 //null ref


        #region constructor

        public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
        {
            _next = next;
            _logger = LogManager.GetLogger("JwtMiddleware");
            _appSettings = appSettings.Value;

        }
        #endregion

        #region Methods

        /// <summary>
        /// Invoke - gets invoked on every Http Request
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                 var _request = context.Request;

                //check header/bearer token
                var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (token != null)
                   AttachUserToContext(context, token);

                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionMessageAsync(context, ex).ConfigureAwait(false); 
            }
        }


        /// <summary>
        /// Attach user data to Http context
        /// </summary>
        /// <param name="context"></param>
        /// <param name="token"></param>
        private void AttachUserToContext(HttpContext context, string token)
        {
            try
            {
              

                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

                var tokenHandler = new JwtSecurityTokenHandler();

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
            }
            catch
            {
                // do nothing if jwt validation fails
                // user is not attached to context so request won't have access to secure routes
            }
        }


        /// <summary>
        /// Handle errors @ each request
        /// </summary>
        /// <param name="oHttpContext"></param>
        /// <param name="oException">application error</param>
        /// <returns></returns>
        private async Task HandleExceptionMessageAsync(HttpContext oHttpContext, Exception oException)
        {
            try
            {
                string sErrorMsg;

                if (oException.InnerException == null)    //Message specified @ request,unhandled exeption being thrown 
                {
                    sErrorMsg = oException.ToString();
                }
                else
                {
                    sErrorMsg = oException.InnerException.ToString();
                }

                //log error 
                _logger.Error(sErrorMsg);

                int statusCode = (int)HttpStatusCode.InternalServerError;
                var sMessage = oException.Message.Replace("\r\n", " ");

                sMessage = General_Error;

                //type of  exption thrown:
                switch (oException)
                {
                    case SqlException     //any database specific sql ex, e.g. network error connecting
                        AmbiguousMatchException:
                        break;
                    case BadHttpRequestException:
                        statusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case DbUpdateException:
                        break;
                }

                var errorResponse = new { Status = statusCode, Message = sMessage };
                var oResponse = oHttpContext.Response;

                oResponse.ContentType = "application/json";
                oResponse.StatusCode = statusCode;

                //write  out error message
                sMessage = JsonConvert.SerializeObject(errorResponse);
                await oResponse.WriteAsync(sMessage);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                throw;
            }

        }




        #endregion
    }
}
