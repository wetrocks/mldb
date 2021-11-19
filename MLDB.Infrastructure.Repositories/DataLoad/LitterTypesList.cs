using MLDB.Domain;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MLDB.Infrastructure.Repositories.DataLoad {
    // "private" entity type that collects litter types into lists
    internal class LitterTypesList
    {
        [Key]
        public string Id { get; set; }

        public string Description { get; set; }

        public List<LitterType> LitterTypes { get; set; }
    } 
}