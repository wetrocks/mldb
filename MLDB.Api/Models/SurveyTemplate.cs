using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

public class SurveyTemplate
{

    [Key]
    public string Id { get; set; }

    public string Description { get; set; }

    public List<LitterType> LitterTypes { get; set; }
}