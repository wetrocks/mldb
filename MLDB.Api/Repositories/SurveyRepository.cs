using MLDB.Api.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using MLDB.Api.Services;
using Microsoft.EntityFrameworkCore;
using System.Linq;


namespace MLDB.Api.Repositories
{
    [Obsolete]
    public class SurveyRepository : ISurveyRepository {

        private readonly SiteSurveyContext _dbCtx;

        public SurveyRepository(SiteSurveyContext context) {
            this._dbCtx = context;
        }

        public bool exists(Guid surveyId) {
          return _dbCtx.Surveys.Any(e => e.Id == surveyId);
        }

        public async Task<Survey> findAsync(Guid surveyId) {
            var survey = await _dbCtx.Surveys
                                .FindAsync(surveyId);

            return survey;
        }

        public async Task<Survey> insertAsync(Survey survey) {
            _dbCtx.Surveys.Add(survey);

            await _dbCtx.SaveChangesAsync();

            return survey;
        }

        public async Task<Survey> updateAsync(Survey survey) {
            var orig = await _dbCtx.Surveys.FindAsync(survey.Id);
            orig.SiteId = survey.SiteId;
            orig.Coordinator = survey.Coordinator;
            orig.CreateTimestamp = survey.CreateTimestamp;
            orig.EndTimeStamp = survey.EndTimeStamp;
            orig.StartTimeStamp = survey.StartTimeStamp;
            orig.TotalKg = survey.TotalKg;
             orig.VolunteerCount = survey.VolunteerCount;
            
            // this should let EF figure out what to insert, update, or delete
            orig.LitterItems = survey.LitterItems;

            await _dbCtx.SaveChangesAsync();

            return survey;
        }
    }
}