using MLDB.Api.Models;
using System.Security.Claims;


namespace MLDB.Api.Services
{
    public interface IUserService {

        public const string EMAIL_CLAIMTYPE =  "https://mldb/claims/email";
        public const string EMAIL_VERIFIED_CLAIMTYPE =  "https://mldb/claims/email_verified";
        public const string NAME_CLAIMTYPE =  "https://mldb/claims/name";

        public User createFromClaimsPrinicpal(ClaimsPrincipal principal);

        public User findUser(ClaimsPrincipal principal);
    }
}