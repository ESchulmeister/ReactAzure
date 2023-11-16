using Microsoft.EntityFrameworkCore;
using reactAzure.Data;

namespace reactAzure.Services
{
    public class UserRepository :IUserRepository
    {

        #region Variables

        private readonly TechCMSContext _context;
        private string? _sLoginName = string.Empty;

        #endregion

        #region constructors

        public UserRepository(TechCMSContext context, IHttpContextAccessor oHttpContextAccessor, string? sLoginName = null)
        {
            _context = context;
            _sLoginName = oHttpContextAccessor.GetIdentity();
            _sLoginName = sLoginName;
        }
        #endregion

        #region methods

        public async Task<User?> LoadByLoginAsync(string sLogin)
        {
            IQueryable<User> query = _context.Users.Where(c => c.UsrLogin == sLogin)
                .Include(r => r.UsrDefaultRoleNavigation).AsNoTracking();


            return await query.FirstOrDefaultAsync();
        }





        #endregion

    }
}
