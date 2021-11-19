using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;
using System.Linq;
using MLDB.Domain;
using MLDB.Infrastructure.Repositories.DataLoad;

namespace MLDB.Infrastructure.Repositories.Configuration
{
    internal class LitterTypeEntityConfigurations : IEntityTypeConfiguration<LitterType>, IEntityTypeConfiguration<LitterTypesList>
    {
        private readonly IReadOnlyList<DataLoad.LitterTypesList> _litterTypesLists;

        public LitterTypeEntityConfigurations(IReadOnlyList<DataLoad.LitterTypesList> litterTypesLists) {
            _litterTypesLists = litterTypesLists;
        }  

        public void Configure(EntityTypeBuilder<LitterType> builder)
        {
            // distinct litter types from all lists
            var allTypes = _litterTypesLists.Aggregate(new List<LitterType>(),
                (typeList,template) => typeList.Union(template.LitterTypes).ToList());

             builder.HasData(allTypes);
        }

        public void Configure(EntityTypeBuilder<LitterTypesList> builder)
        {
            // just the info fields from litter type lists
            var lists = _litterTypesLists.Select( x => new LitterTypesList { Id = x.Id, Description = x.Description});
            builder.HasData(lists);

            // this seem weird but its apparently how its done.
            // build a list of id relationships linking littertypeslists to littertypes,
            // then configure an "entity" for the mapping
            var listsToTypes = _litterTypesLists.Aggregate(new List<(string,int)>(), 
                    (relList, typeList) => {
                        var relationships = typeList.LitterTypes.Select( x => (typeList.Id, x.Id ));

                        return relList.Concat(relationships).ToList();
                    });
            builder.HasMany( x => x.LitterTypes)
                    .WithMany( "LitterTypesList" )
                    .UsingEntity( j => j.HasData(listsToTypes.Select( x => 
                        new { LitterTypesListId = x.Item1, LitterTypesId = x.Item2} )));
        }
    }
}