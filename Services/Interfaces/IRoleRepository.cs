using reactAzure.Data;

namespace reactAzure.Services
{
    public interface IRoleRepository
    {

        Task<IEnumerable<Role>> ListAsync();
    }
}
