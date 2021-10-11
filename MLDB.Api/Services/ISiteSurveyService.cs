using MLDB.Api.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace MLDB.Api.Services
{
    public interface ISiteSurveyService {

        public Task<Survey> getSurvey(Guid surveyId);

        public Task<Survey> create(Survey survey, Guid siteId, User user);

        public Task<Survey> update(Survey survey, Guid siteId, User user);
    }
}