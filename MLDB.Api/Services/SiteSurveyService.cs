using MLDB.Api.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;


namespace MLDB.Api.Services
{
    public class SiteSurveyService : ISiteSurveyService {

        private readonly ISurveyRepository _repo;


        public SiteSurveyService(ISurveyRepository repository) {
            _repo = repository;
        }

        public async Task<Survey> getSurvey(Guid surveyId) {
            var survey = await _repo.findAsync(surveyId);

            return survey;
        }

        public async Task<Survey> create(Survey survey, Guid siteId, User user) {
            survey.CreateUser = user;
            survey.SiteId = siteId;
            survey.CreateTimestamp =  System.DateTime.UtcNow;

            await _repo.insertAsync(survey);

            return survey;
        }

        public async Task<Survey> update(Survey survey, Guid siteId, User user) {
            survey.CreateUser = user;
            survey.SiteId = siteId;

            await _repo.updateAsync(survey);

            return survey;
        }
    }
}