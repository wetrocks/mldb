using System.Security.Claims;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;


namespace MLDB.Domain
{
    public interface ISurveyTemplateRepository
    {
        public Task<SurveyTemplate> getTemplateAsync(string Id);
    }
}