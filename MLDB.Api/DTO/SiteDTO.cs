using System;

namespace MLDB.Api.DTO {
    public record SiteDTO
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string CreatedBy{ get; init; }
    };
}
