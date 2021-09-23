using MLDB.Api.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace MLDB.Api.Services
{
    public interface ISiteSurveyService {

        public Task<List<Survey>> getAll();

        public Task<Survey> create(Survey survey, Guid siteId, ClaimsPrincipal userPrinciple);

    }
}