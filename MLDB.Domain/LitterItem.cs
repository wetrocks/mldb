using System;
using System.ComponentModel.DataAnnotations;

public class LitterItem {
    public int LitterTypeId { get; init; }

    public int Count{ get; set; } = 0;

    public LitterItem(int litterTypeId) : this(litterTypeId,0) {}

    public LitterItem(int litterTypeId, int count)  {
        LitterTypeId = litterTypeId;
        Count = count;
    }

    public override int GetHashCode()
    {
        return LitterTypeId.GetHashCode();
    }

    public override bool Equals(object obj)
    {
        if ((obj == null) || !(this.GetType().Equals(obj.GetType())))
            return false;
        else {
            var li = (LitterItem)obj;
            return LitterTypeId.Equals(li.LitterTypeId) && Count.Equals(li.Count);
        }
    }
}