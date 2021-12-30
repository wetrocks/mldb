using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace MLDB.Domain 
{
    public class LitterSourceCategory
    {
        public UInt16 Id { get; init; }

        public string Name{ get; init; }

        public string Description { get; init; }

        public LitterSourceCategory(UInt16 id, string name, string description) {
            this.Id = id;
            this.Name = name;
            this.Description = description;
        }
    }
}
