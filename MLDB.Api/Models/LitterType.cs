using System;
using System.ComponentModel.DataAnnotations;

public class LitterType {
    [Key]
    public int Id{ get; set; }

    public int OsparID{ get; set; }

    public string DadID{ get; set; }

    public string Description{ get; set; }
}