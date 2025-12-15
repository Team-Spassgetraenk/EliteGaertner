using System;
using System.Collections.Generic;

namespace DataManagement.Entities;

public partial class Profile
{
    public int Profileid { get; set; }

    public string? Profilepictureurl { get; set; }

    public string Username { get; set; } = null!;

    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Passwordhash { get; set; } = null!;

    public string Phonenumber { get; set; } = null!;

    public string Profiletext { get; set; } = null!;

    public bool Sharemail { get; set; }

    public bool Sharephonenumber { get; set; }

    public DateTime Usercreated { get; set; }

    public virtual ICollection<Harvestupload> Harvestuploads { get; set; } = new List<Harvestupload>();

    public virtual ICollection<Profilepreference> Profilepreferences { get; set; } = new List<Profilepreference>();

    public virtual ICollection<Rating> RatingContentcreators { get; set; } = new List<Rating>();

    public virtual ICollection<Rating> RatingContentreceivers { get; set; } = new List<Rating>();
}
