using System.ComponentModel.DataAnnotations;

namespace reactAzure.Models
{

    /// <summary>
    /// AuthenticateRequest - Login Form data
    /// </summary>
    public class AuthenticateRequest
    {

        [Required]
        public string? UserName { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}
