using System;
using System.Security.Claims;
using System.Linq;
using MLDB.Api.Jwt;
using MLDB.Domain;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;


namespace MLDB.Api.Services
{
    public class UserService : IUserService {
        
        private readonly IUserRepository _userRepo;
        private readonly ILogger<UserService> _logger;

        public UserService( IUserRepository userRepository , ILogger<UserService> logger) {
            _userRepo = userRepository;
            _logger = logger;
        }

        private String getIdPId(ClaimsPrincipal principal) {
            return principal.Claims.Single(x => x.Type.Equals(ClaimTypes.NameIdentifier)).Value;
        }

        public User createFromClaimsPrinicpal(ClaimsPrincipal principal) {
            var user = new User(getIdPId(principal));
            
            user.Name = principal.Claims.SingleOrDefault(x => x.Type.Equals(TokenClaims.NAME_CLAIMTYPE))?.Value;
            user.Email = principal.Claims.SingleOrDefault(x => x.Type.Equals(TokenClaims.EMAIL_CLAIMTYPE))?.Value;
            if( user.Email != null ) {
                var evStr = principal.Claims.SingleOrDefault( x => x.Type.Equals(TokenClaims.EMAIL_VERIFIED_CLAIMTYPE))?.Value;
                user.EmailVerified = Boolean.TrueString.Equals(evStr, StringComparison.OrdinalIgnoreCase);
            }

            return user;
        }

        public async Task<User> findOrAddUserAsync(ClaimsPrincipal principal) {
            var user = await _userRepo.findByIdpIdAsync(getIdPId(principal));

            if( user is null ) {
                var newUser = this.createFromClaimsPrinicpal(principal);

                _logger.LogInformation($"Creating new user: {newUser.IdpId}");

                user = await _userRepo.insertAsync(newUser);
            }

            return user;
        }
   }
}