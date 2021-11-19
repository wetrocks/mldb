using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace MLDB.Domain 
{
    public class LitterType
    {
        public int Id { get; private set; }

        public int OsparId { get; private set; }

        public string DadId { get; private set; }

        public string Description { get; private set; }

        public LitterType( int id, int osparId, string dadId, String description) {
            this.Id = id;
            this.OsparId = osparId;
            this.DadId = dadId;
            this.Description = description;
        }
    }
}