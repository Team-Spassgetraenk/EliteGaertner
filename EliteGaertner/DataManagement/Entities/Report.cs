using System;
using System.Collections.Generic;

namespace DataManagement.Entities;

public partial class Report
{
    public int Reportid { get; set; }

    public string Reason { get; set; } = null!;

    public DateTime Reportdate { get; set; }

    public int Uploadid { get; set; }

    public virtual Harvestupload Upload { get; set; } = null!;
}
