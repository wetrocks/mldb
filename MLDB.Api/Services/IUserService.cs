using MLDB.Domain;
using System.Security.Claims;
using System.Threading.Tasks;


namespace MLDB.Api.Services
{
    public interface IUserService {

        public User createFromClaimsPrinicpal(ClaimsPrincipal principal);

        public Task<User> findOrAddUserAsync(ClaimsPrincipal principal);
    }
}