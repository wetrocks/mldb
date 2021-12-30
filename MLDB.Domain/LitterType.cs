using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace MLDB.Domain 
{
    public class LitterType
    {
        public UInt32 Id { get; init; }

        public UInt32? OsparId { get; init; }

        public String OsparCategory { get; init; }

        public string Description { get; init; }

        public LitterSourceCategory SourceCategory { get; init; }

        public String JointListTypeCode { get; init; }

        public String JointListJCode { get; init; }
    }
}