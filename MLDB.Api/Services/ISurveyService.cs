using System.Security.Claims;
using System.Threading.Tasks;
using System;
using  MLDB.Api.DTO;


namespace MLDB.Api.Services
{
    public interface ISurveyService {

        public Task<SurveyDTO> createSiteSurvey(ClaimsPrincipal principal, Guid siteId, SurveyDTO surveyDTO);

        public Task<SurveyDTO> updateSiteSurvey(ClaimsPrincipal principal, SurveyDTO surveyDTO);
    }
}