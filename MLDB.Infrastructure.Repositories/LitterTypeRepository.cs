using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using  MLDB.Domain;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace MLDB.Infrastructure.Repositories
{
    public class LitterTypeRepository : ILitterTypeRepository
    {

        private readonly SiteSurveyContext _dbCtx;

        public LitterTypeRepository(SiteSurveyContext dbContext) {
            _dbCtx = dbContext;
        }

        public async Task<List<LitterType>> getAll()
        {
            var internalTypes = await _dbCtx.InternalLitterTypes
                                            .Include( x => x.SourceCategory)
                                            .Include( x => x.OsparMapping)
                                            .ThenInclude( m => m.MappedType)
                                            .Include( x => x.JointListMapping)
                                            .ThenInclude( m => m.MappedType )
                                            .AsNoTracking()
                                            .ToListAsync();

            
            return internalTypes.Select(InternalToLitterType).ToList();
        }

        public async Task<LitterType> findAsync(UInt32 id)
        {
            var internalType = await _dbCtx.InternalLitterTypes
                                            .Include( x => x.SourceCategory)
                                            .Include( x => x.OsparMapping)
                                            .ThenInclude( m => m.MappedType)
                                            .Include( x => x.JointListMapping)
                                            .ThenInclude( m => m.MappedType )
                                            .AsNoTracking()
                                            .SingleOrDefaultAsync( x => x.Code == id);
            if( internalType == null )
                return null;

            return InternalToLitterType(internalType);
        }

        private LitterType InternalToLitterType(Model.InternalLitterType internalType)
        {
            return new LitterType { 
                Id = internalType.Code, 
                Description = internalType.Description,
                SourceCategory = internalType.SourceCategory,
                OsparId = internalType.OsparMapping?.MappedType?.Code,
                OsparCategory = internalType.OsparMapping?.MappedType?.Category,
                JointListTypeCode = internalType.JointListMapping?.MappedType?.TypeCode,
                JointListJCode = internalType.JointListMapping?.MappedType?.J_Code
            };
        }
    }
}