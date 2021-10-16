using MLDB.Api.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;


namespace MLDB.Api.Services
{
    public class SiteService : ISiteService {

        private readonly SiteSurveyContext _dbCtx;
        private readonly IUserService _userSvc;

        public SiteService(SiteSurveyContext context, IUserService userService) { 
            _dbCtx = context;
            _userSvc = userService;
        }

        public async Task<IList<Site>> getAll() {
            return await _dbCtx.Sites.ToListAsync();
        }

        public async Task<Site> find(Guid id) {
            return await _dbCtx.Sites.Include( x => x.Surveys ).SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Site> create(Site site, ClaimsPrincipal userPrinciple) {
            var createUser = _userSvc.createFromClaimsPrinicpal(userPrinciple);

            var dbUser = _dbCtx.Users.Find(createUser.IdpId);
            
            site.CreateUser = dbUser ?? createUser;
            site.CreateTimestamp =  System.DateTime.UtcNow;

            _dbCtx.Sites.Add(site);

            await _dbCtx.SaveChangesAsync();

            return site;
        }
    }
}