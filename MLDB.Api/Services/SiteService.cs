using MLDB.Api.Models;
using System.Security.Claims;
using System.Threading.Tasks;


namespace MLDB.Api.Services
{
    public class SiteService : ISiteService {

        private readonly SiteSurveyContext _dbCtx;
        private readonly IUserService _userSvc;

        public SiteService(SiteSurveyContext context, IUserService userService) { 
            _dbCtx = context;
            _userSvc = userService;
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