using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace MLDB.Domain
{
    public class SurveyTemplate
    {
        public string Id { get; private set; }

        public string Description { get; set; }

        private readonly List<LitterType> _litterTypes;
        public IReadOnlyList<LitterType> LitterTypes => _litterTypes;

        public SurveyTemplate(string id, List<LitterType> litterTypes) {
            if( id == null || litterTypes == null ) {
                throw new ArgumentNullException();
            }

            Id = id;
            _litterTypes = litterTypes;
        }

    }
}