using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using reactAzure.Data;

namespace reactAzure.Services
{
    public class RoleRepository : IRoleRepository
    {


        #region Variables
        private readonly TechCMSContext _context;
        private readonly IMemoryCache _cache;
        #endregion


        #region constructors

        public RoleRepository(TechCMSContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }


        #endregion
        public async Task<IEnumerable<Role>> ListAsync()
        {
            //check cache
            if (_cache.TryGetValue(Constants.CacheKeys.Roles, out IEnumerable<Role> lstRoles))
            {
                return lstRoles;
            }

            IQueryable<Role> query = _context.Roles
                .Where(e => e.RActive == true)
                .AsNoTracking();

            lstRoles = await query.ToListAsync();

            if (lstRoles.Any())
            {
                var cacheExp = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddHours(Constants.CacheExpHrs),
                    Priority = CacheItemPriority.Normal,
                };

                _cache.Set(Constants.CacheKeys.Roles, lstRoles, cacheExp);
            }


            return lstRoles.OrderBy(oRole => oRole.RName);
        }
    }
}
