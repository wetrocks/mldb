using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using  MLDB.Domain;
using System.Linq;

namespace MLDB.Infrastructure.Repositories
{
    
    public class SurveyRepository :  ISurveyRepository {

        private readonly SiteSurveyContext _dbCtx;

        public SurveyRepository(SiteSurveyContext dbContext) {
            _dbCtx = dbContext;
        }

        public async Task<bool> existsAsync(Guid id) {
            return false;
        }

        public async Task<Survey> findAsync(Guid id) {
            var survey = await _dbCtx.Surveys.FindAsync(id);

            return survey;
        }

        public async Task<Survey> insertAsync(Survey survey) {
            var created = await _dbCtx.Surveys.AddAsync(survey);

            return created.Entity;
        }

        public async Task<Survey> updateAsync(Survey survey) {
            var orig = await _dbCtx.Surveys.FindAsync(survey.Id);

            orig.CoordinatorName = survey.CoordinatorName;
            orig.StartTimeStamp = survey.StartTimeStamp;
            orig.EndTimeStamp = survey.EndTimeStamp;
            orig.VolunteerCount = survey.VolunteerCount;
            orig.TotalKg = survey.TotalKg;

            var updated = _dbCtx.Surveys.Update(orig);
            return updated.Entity;
        }
    }
}