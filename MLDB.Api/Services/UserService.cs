using System;
using MLDB.Api.Models;
using System.Security.Claims;
using System.Linq;


namespace MLDB.Api.Services
{
    public class UserService : IUserService {
        
        private readonly SiteSurveyContext _dbCtx;

        public UserService(SiteSurveyContext context) { 
            _dbCtx = context;
        }

        private String getIdPId(ClaimsPrincipal principal) {
            return principal.Claims.Single(x => x.Type.Equals(ClaimTypes.NameIdentifier)).Value;
        }

        public User createFromClaimsPrinicpal(ClaimsPrincipal principal) {
            var user = new User(getIdPId(principal));
            
            user.Name = principal.Claims.SingleOrDefault(x => x.Type.Equals(IUserService.NAME_CLAIMTYPE))?.Value;
            user.Email = principal.Claims.SingleOrDefault(x => x.Type.Equals(IUserService.EMAIL_CLAIMTYPE))?.Value;
            if( user.Email != null ) {
                var evStr = principal.Claims.SingleOrDefault( x => x.Type.Equals(IUserService.EMAIL_VERIFIED_CLAIMTYPE))?.Value;
                user.EmailVerified = Boolean.TrueString.Equals(evStr, StringComparison.OrdinalIgnoreCase);
            }
            user.CreateTime = DateTime.UtcNow;
            user.UpdateTime = DateTime.MinValue;

            return user;
        }

         public User findUser(ClaimsPrincipal principal) {       
            return _dbCtx.Users.Find( getIdPId(principal)  );
         }

        public User findOrAddUser(ClaimsPrincipal principal) {
            var userId = this.getIdPId(principal);

            var user = _dbCtx.Users.Find(userId);
            if( user == null ) {
               user = this.createFromClaimsPrinicpal(principal);
               _dbCtx.Add(user);
            }

            return user;            
        }
   }
}