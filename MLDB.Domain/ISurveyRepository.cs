using System.Threading.Tasks;
using System;


namespace MLDB.Domain
{
    public interface ISurveyRepository {

        public Task<bool> existsAsync(Guid id);

        public Task<Survey> findAsync(Guid id);

        public Task<Survey> insertAsync(Survey survey);

        public Task<Survey> updateAsync(Survey survey);
    }
}