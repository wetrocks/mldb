using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using  MLDB.Domain;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace MLDB.Infrastructure.Repositories
{
    
    public class SurveyRepository :  ISurveyRepository {

        private readonly SiteSurveyContext _dbCtx;

        public SurveyRepository(SiteSurveyContext dbContext) {
            _dbCtx = dbContext;
        }

        public bool exists(Guid id) {
            return _dbCtx.Surveys.Any( x => x.Id == id );
        }

        public async Task<Survey> findAsync(Guid id) {
            var survey = await _dbCtx.Surveys.FindAsync(id);

            return survey;
        }

        public Task<List<Survey>> getSurveysForSite(Guid siteId) {
            var surveys = _dbCtx.Surveys.Where( x => x.SiteId == siteId ).ToListAsync();

            return surveys;
        }


        public async Task<Survey> insertAsync(Survey survey) {
            var created = await _dbCtx.Surveys.AddAsync(survey);

            return created.Entity;
        }

        public async Task<Survey> updateAsync(Survey survey) {
            var orig = await _dbCtx.Surveys.FindAsync(survey.Id);
            if( orig == null ) {
                // TODO: maybe throw something better?
                throw new System.Data.RowNotInTableException();
            }

            orig.CoordinatorName = survey.CoordinatorName;
            orig.SurveyDate = survey.SurveyDate;
            orig.StartTime = survey.StartTime;
            orig.EndTime = survey.EndTime;
            orig.VolunteerCount = survey.VolunteerCount;
            orig.TotalKg = survey.TotalKg;

            orig.updateLitterItems(survey.LitterItems);

            var updated = _dbCtx.Surveys.Update(orig);
            return updated.Entity;
        }
    }
}