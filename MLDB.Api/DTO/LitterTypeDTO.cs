using System;

namespace MLDB.Api.DTO {
    public class LitterTypeDTO
    {
        public UInt32 Id { get; init; }

        public UInt32? OsparId { get; init; }

        public String OsparCategory { get; init; }

        public string Description { get; init; }

        public String SourceCategory { get; init; }

        public String JointListTypeCode { get; init; }

        public String JointListJCode { get; init; }
    }
}