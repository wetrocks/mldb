using MLDB.Api.Models;
using System.Security.Claims;
using System.Threading.Tasks;


namespace MLDB.Api.Services
{
    public interface ISiteService {

        public Task<Site> create(Site site, ClaimsPrincipal userPrinciple);

    }
}