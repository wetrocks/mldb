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

        // needed for EF, can't bind navigation properties to ctor args 
        private SurveyTemplate(string id) {
            if( id == null ) {
                throw new ArgumentNullException();
            }

            Id = id;
        }

        public SurveyTemplate(string id, List<LitterType> litterTypes) : this(id) {
            if( litterTypes == null ) {
                throw new ArgumentNullException();
            }

            _litterTypes = litterTypes;
        }

    }
}