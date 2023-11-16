using reactAzure.Data;
using reactAzure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace reactAzure.Services
{
    /// <summary>
    /// User re. route endpoint definitions
    /// </summary>
    public interface IUserRepository
    {

       Task<User?> LoadByLoginAsync(string sLogin);

    }
}
