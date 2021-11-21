using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using  MLDB.Domain;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace MLDB.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SiteSurveyContext _dbCtx;

        public UserRepository(SiteSurveyContext dbContext) {
            _dbCtx = dbContext;
        }
       public async Task<User> findAsync(UInt16 id) {
           return await _dbCtx.Users.FindAsync(id);
        }

        public async Task<User> findByIdpIdAsync(String idpId) {
            return await _dbCtx.Users.Where( x => x.IdpId == idpId ).SingleOrDefaultAsync();
        }

        public async Task<User> insertAsync(User user) {
            var created = await _dbCtx.Users.AddAsync(user);

            return created.Entity;
        }
    }
}