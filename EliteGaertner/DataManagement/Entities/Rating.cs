using System;
using System.Collections.Generic;

namespace DataManagement.Entities;

public partial class Rating
{
    public int Contentreceiverid { get; set; }
    public int Contentcreatorid { get; set; }
    public bool Profilerating { get; set; }
    public DateTime Ratingdate { get; set; }
    public virtual Profile Contentcreator { get; set; } = null!;
    public virtual Profile Contentreceiver { get; set; } = null!;
}
