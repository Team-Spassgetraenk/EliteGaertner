using System;
using System.Collections.Generic;

namespace DataManagement.Entities;

public partial class Profilepreference
{
    public int Tagid { get; set; }

    public int Profileid { get; set; }

    public DateTime Dateupdated { get; set; }

    public virtual Profile Profile { get; set; } = null!;

    public virtual Tag Tag { get; set; } = null!;
}
