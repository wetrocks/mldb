using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace MLDB.Api.Services
{
    public class LitterTypeService : ILitterTypeService {

        private readonly ILitterTypeRepository _repo;

        public LitterTypeService(ILitterTypeRepository _repository) {
            _repo = _repository;
        }

        public async Task<IList<LitterType>> getLitterTypesForVersion(Decimal version) {
            var litterTypes = await _repo.getAllAsync();
            
            return litterTypes ?? new List<LitterType>();
        }
    }
}