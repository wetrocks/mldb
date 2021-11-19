using System;
using MLDB.Domain;
using System.Threading.Tasks;

namespace MLDB.Infrastructure.Repositories
{
    public class SurveyTemplateRepository : ISurveyTemplateRepository
    {
        private readonly SiteSurveyContext _dbCtx;

        public SurveyTemplateRepository(SiteSurveyContext dbContext) {
            _dbCtx = dbContext;
        }

        public async Task<SurveyTemplate> getTemplateAsync(string id) {
            var template = await _dbCtx.SurveyTemplates.FindAsync(id);

            return template;
        }
    }
}
