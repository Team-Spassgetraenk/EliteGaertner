using System;
using System.Collections.Generic;

namespace DataManagement.Entities;

public partial class Harvestupload
{
    public int Uploadid { get; set; }
    public string Imageurl { get; set; } = null!;
    public string Description { get; set; } = null!;
    public float Weightgramm { get; set; }
    public float Widthcm { get; set; }
    public float Lengthcm { get; set; }
    public DateTime Uploaddate { get; set; }
    public int Profileid { get; set; }
    public virtual Profile Profile { get; set; } = null!;
    public virtual ICollection<Report> Reports { get; set; } = new List<Report>();
    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
}
