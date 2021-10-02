using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

[Owned]
public class LitterItem {

    public Guid SurveyId{ get; set; }

    public LitterType LitterType{ get; set; }

    public int Count{ get; set; }
}