using System;
using System.Collections.Generic;

namespace DataManagement.Entities;

public partial class Tag
{
    public int Tagid { get; set; }
    public string Label { get; set; } = null!;
    public virtual ICollection<Profilepreference> Profilepreferences { get; set; } = new List<Profilepreference>();
    public virtual ICollection<Harvestupload> Uploads { get; set; } = new List<Harvestupload>();
}
