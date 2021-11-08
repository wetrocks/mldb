using MLDB.Api.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;


namespace MLDB.Api.Services
{
    [Obsolete]
    public interface ISurveyRepository {

        public bool exists(Guid id);

        public Task<Survey> findAsync(Guid id);

        public Task<Survey> insertAsync(Survey survey);

        public Task<Survey> updateAsync(Survey survey);
    }
}