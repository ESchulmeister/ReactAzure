using DevExpress.Web.Office;
using reactAzure.Data;
using System;

namespace reactAzure.Models
{
    /// <summary>
    /// AuthenticateResponse  
    /// copy of User model plus JWT Token generated @api Account controller 
    /// </summary>
    public class AuthenticateResponse
    {
        #region properties
        public int Id { get; set; }

        public string? Login { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string? FullName { get; set; }

        public int? Clock { get; set; } = 0;

        public double? FTE { get; set; } = 0;

        public virtual RoleModel Role { get; set; }

        public string? Token { get; set; }


        #endregion

        #region Constructors

        public AuthenticateResponse()
        {
            Role = new RoleModel();
        }


        public AuthenticateResponse(User user, string token, Role? oRole = null)
        {
            Id = user.UsrId;
            Login = user.UsrLogin;
            IsActive = (user.UsrActive == 1);
            FirstName = user.UsrFirst;
            LastName = user.UsrLast;
            FullName = String.Concat(FirstName, " ", LastName);
            Clock = user.UsrClock;
            FTE = user.UsrFte;
            Token = token;
                
            
            //map user's Role - property

            if (oRole == null)
            {
                Role = new RoleModel
                {
                    ID = user.UsrDefaultRole.HasValue ? user.UsrDefaultRole.Value : 0,
                    Name = (user.UsrDefaultRoleNavigation == null) ? string.Empty : user.UsrDefaultRoleNavigation.RName,
                };
            }
            else
            {
                Role = new RoleModel
                {
                    ID = oRole.RId,
                    Name = oRole.RName,
                    IsActive = oRole.RActive.HasValue ? oRole.RActive.Value : false,
                    IsAdministrator = oRole.RAdministrator,
                    IsSupervisor = oRole.RSupervisor
                };
            }

        }
        #endregion
    }
}
