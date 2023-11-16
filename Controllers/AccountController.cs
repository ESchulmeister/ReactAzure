using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.DirectoryServices.AccountManagement;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Configuration;

using reactAzure.Data;
using reactAzure.Models;
using reactAzure.Services;

namespace reactAzure.Controllers
{

    /// <summary>
    /// ref.  ~/ClientApp/src/setupProxy.js    
    /// </summary>
    [ApiController]
    [Route("[controller]/{action}")]

    public class AccountController : ReactSampleController<AccountController>
    {

        #region variables
        private readonly IUserRepository _usrRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly AppSettings _appSettings;
        private readonly IMapper _mapper;

        private const int FAILED_DEPENDENCY = 424;
        private const int APPLICATION_ERROR = 500;
        #endregion

        #region suppress warnings
        #pragma warning disable CA1416 // Validate platform compatibility
        #pragma warning disable CS8604 //possible null ref
        #pragma warning disable CS8600 //null
        #endregion

        public AccountController(IUserRepository usrRepository, IOptions<AppSettings> appSettings, IMapper mapper,
                IRoleRepository roleRepository)
        {
            _usrRepository = usrRepository;
            _mapper = mapper;
            _appSettings = appSettings.Value;  //appSettings:secret
            _roleRepository = roleRepository;

        }

        /// <param name="model">JSON- Login credentials</param>
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Login([FromBody] AuthenticateRequest model)
        {


            string errMsg;
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            string userName =  model.UserName;

            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(model.Password))
            {
                errMsg = "Credentials not detected";
                return BadRequest(errMsg);
            }

            try
            {
                //create response - AuthenticateResponse/User model
                var oAuthenticateResponse = await this.AuthenticateUserAsync(model);

                //set Auth cookie
                await this.SetCookie(oAuthenticateResponse);

                return Ok(oAuthenticateResponse);
            }
            catch (NotFoundException oNotFoundException)   //AD search/ex
            {
                errMsg = oNotFoundException.Message;
                this.Logger.Error($"{userName} - {errMsg}");
                return NotFound(errMsg);
            }
            catch (UserNotInDatabaseException oUserNotInDatabaseException)
            {
                errMsg = oUserNotInDatabaseException.Message;
                this.Logger.Error($"{userName} - {errMsg}");
                return StatusCode(FAILED_DEPENDENCY, Constants.Bad_Request_Error);
            }
            catch (Exception oException)
            {
                errMsg = $"{oException.Message} \n @ {oException.StackTrace}";
                this.Logger.Error(errMsg);
                return StatusCode(APPLICATION_ERROR, Constants.General_Error);
            }

        }





        /// <summary>
        /// Log Out user
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            var userName = User.Identity?.Name;

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            this.Logger.Info($"User {userName} logged out @ {DateTime.Now}.");

            return this.Ok();
        }


        #region private methods

        /// <summary>
        /// Authenticate User
        /// </summary>
        /// <param name="model">AuthenticateRequest model JSON - UserName/Password</param>
        private async Task<AuthenticateResponse> AuthenticateUserAsync(AuthenticateRequest model)
        {
            string username = model.UserName;
            
            UserModel? oUserModel;
            using var oPrincipalContext = new PrincipalContext(ContextType.Domain);

            string sLdapUrl = $"LDAP://{oPrincipalContext.ConnectedServer}";

            if (!oPrincipalContext.ValidateCredentials(username, model.Password))
            {
                var msg = "Unknown username and/or password";
                this.Logger.Error($"UserName:{username} - {msg}");

                throw new NotFoundException(msg);
            }


            using UserPrincipal userPrincipal = new(oPrincipalContext);
            userPrincipal.SamAccountName = username;

            using var search = new PrincipalSearcher(userPrincipal);
            UserPrincipal user = (UserPrincipal)search.FindOne();

            //load user entity by login - model.username
            var dbUser = await _usrRepository.LoadByLoginAsync(sLogin: username) ?? throw new UserNotInDatabaseException("User not found in the database");

            oUserModel = _mapper.Map<UserModel>(dbUser);

            var token = GenerateJwtToken(oUserModel);

            Role oRole = await this.GetUserRole(dbUser);

            return new AuthenticateResponse(dbUser, token, oRole);

        }
        /// <summary>
        /// Map user role - FK -- RoleID
        /// </summary>
        /// <param name="dbUser"></param>
        /// <returns></returns>
        private async Task<Role> GetUserRole(User dbUser)
        {
            var lstRoles = await _roleRepository.ListAsync();
            return lstRoles.FirstOrDefault(oRole => dbUser.UsrDefaultRole.HasValue
                    && oRole.RId == dbUser.UsrDefaultRole.Value)!;
        }

        /// <summary>
        /// Set Authentication cookie
        /// </summary>
        /// <param name="oUserModel">user model</param>
        private async Task SetCookie(AuthenticateResponse model)
        {
            try
            {
                string? username = (model == null) ? string.Empty : model.Login;

                var oRole = (model == null) ? null : model.Role;

                string sRole = (oRole == null) ? "Not Assigned" : oRole.Name;


                Claim claim = new("FullName", model!.FullName);
                var lstClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username),
                    claim,
                    //custom claims - role == permissions
                    new Claim("IsSupervisor", value: (oRole != null
                                                      && oRole.IsSupervisor
                                                      && !oRole.IsAdministrator).ToString()),
                    new Claim("IsAdministrator", (oRole != null && oRole.IsAdministrator).ToString()),
                };

                var claimsIdentity = new ClaimsIdentity(
                          lstClaims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                };

                //sign user in 
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                this.Logger.Info($"User {username} / Role - {oRole?.Name} ; logged in @ {DateTime.Now}.");
            }

            catch (Exception ex)
            {
                var errMsg = $"{ex.Message} \n @ {ex.StackTrace}";
                this.Logger.Error(errMsg);
            }


        }




        /// <summary>
        /// TODO - exp options?
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private string GenerateJwtToken(UserModel user)
        {
            // generate valid token
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_appSettings.Secret) ?? throw new DataValidationException("Settings Secret not Provided");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.ID.ToString()), new Claim("login", user.Login) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        #endregion
    }

}

