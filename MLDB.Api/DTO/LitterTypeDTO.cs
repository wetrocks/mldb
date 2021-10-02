using System;

namespace MLDB.Api.DTO {
public record LitterTypeDTO
{
        public int Id{ get; init; }

        public int OsparID{ get; init; }

        public string DadID{ get; init; }

        public string Description{ get; init; }
    }
}
