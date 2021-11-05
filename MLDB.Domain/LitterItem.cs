using System;
using System.ComponentModel.DataAnnotations;

public class LitterItem {
    public int LitterTypeId { get; init; }

    public int Count{ get; set; } = 0;

    public LitterItem(int litterTypeId)  {
        LitterTypeId = litterTypeId;
    }
}