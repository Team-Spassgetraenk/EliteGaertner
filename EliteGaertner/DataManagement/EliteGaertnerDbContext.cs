using DataManagement.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataManagement;

//Erstellt mit Scaffolding (EntityFramework.Design)
public partial class EliteGaertnerDbContext : DbContext
{
    public EliteGaertnerDbContext()
    {
    }

    public EliteGaertnerDbContext(DbContextOptions<EliteGaertnerDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Harvestupload> Harvestuploads { get; set; }
    public virtual DbSet<Profile> Profiles { get; set; }
    public virtual DbSet<Profilepreference> Profilepreferences { get; set; }
    public virtual DbSet<Rating> Ratings { get; set; }
    public virtual DbSet<Report> Reports { get; set; }
    public virtual DbSet<Tag> Tags { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Harvestupload>(entity =>
        {
            entity.HasKey(e => e.Uploadid).HasName("harvestuploads_pkey");

            entity.ToTable("harvestuploads");

            entity.Property(e => e.Uploadid).HasColumnName("uploadid");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Imageurl).HasColumnName("imageurl");
            entity.Property(e => e.Lengthcm).HasColumnName("lengthcm");
            entity.Property(e => e.Profileid).HasColumnName("profileid");
            entity.Property(e => e.Uploaddate).HasColumnName("uploaddate");
            entity.Property(e => e.Weightgramm).HasColumnName("weightgramm");
            entity.Property(e => e.Widthcm).HasColumnName("widthcm");

            entity.HasOne(d => d.Profile).WithMany(p => p.Harvestuploads)
                .HasForeignKey(d => d.Profileid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("harvestuploads_profileid_fkey");
        });

        modelBuilder.Entity<Profile>(entity =>
        {
            entity.HasKey(e => e.Profileid).HasName("profile_pkey");

            entity.ToTable("profile");

            entity.Property(e => e.Profileid).HasColumnName("profileid");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.Firstname).HasColumnName("firstname");
            entity.Property(e => e.Lastname).HasColumnName("lastname");
            entity.Property(e => e.Passwordhash).HasColumnName("passwordhash");
            entity.Property(e => e.Phonenumber).HasColumnName("phonenumber");
            entity.Property(e => e.Profilepictureurl).HasColumnName("profilepictureurl");
            entity.Property(e => e.Profiletext).HasColumnName("profiletext");
            entity.Property(e => e.Sharemail).HasColumnName("sharemail");
            entity.Property(e => e.Sharephonenumber).HasColumnName("sharephonenumber");
            entity.Property(e => e.Usercreated).HasColumnName("usercreated");
            entity.Property(e => e.Username).HasColumnName("username");
        });

        modelBuilder.Entity<Profilepreference>(entity =>
        {
            entity.HasKey(e => new { e.Tagid, e.Profileid }).HasName("profilepreferences_pkey");

            entity.ToTable("profilepreferences");

            entity.Property(e => e.Tagid).HasColumnName("tagid");
            entity.Property(e => e.Profileid).HasColumnName("profileid");
            entity.Property(e => e.Dateupdated).HasColumnName("dateupdated");

            entity.HasOne(d => d.Profile).WithMany(p => p.Profilepreferences)
                .HasForeignKey(d => d.Profileid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("profilepreferences_profileid_fkey");

            entity.HasOne(d => d.Tag).WithMany(p => p.Profilepreferences)
                .HasForeignKey(d => d.Tagid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("profilepreferences_tagid_fkey");
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasKey(e => new { e.Contentcreatorid, e.Contentreceiverid }).HasName("rating_pkey");

            entity.ToTable("rating");

            entity.Property(e => e.Contentcreatorid).HasColumnName("contentcreatorid");
            entity.Property(e => e.Contentreceiverid).HasColumnName("contentreceiverid");
            entity.Property(e => e.Profilerating).HasColumnName("profilerating");
            entity.Property(e => e.Ratingdate).HasColumnName("ratingdate");

            entity.HasOne(d => d.Contentcreator).WithMany(p => p.RatingContentcreators)
                .HasForeignKey(d => d.Contentcreatorid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("rating_contentcreatorid_fkey");

            entity.HasOne(d => d.Contentreceiver).WithMany(p => p.RatingContentreceivers)
                .HasForeignKey(d => d.Contentreceiverid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("rating_contentreceiverid_fkey");
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.HasKey(e => e.Reportid).HasName("report_pkey");

            entity.ToTable("report");

            entity.Property(e => e.Reportid).HasColumnName("reportid");
            entity.Property(e => e.Reason).HasColumnName("reason");
            entity.Property(e => e.Reportdate).HasColumnName("reportdate");
            entity.Property(e => e.Uploadid).HasColumnName("uploadid");

            entity.HasOne(d => d.Upload).WithMany(p => p.Reports)
                .HasForeignKey(d => d.Uploadid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("report_uploadid_fkey");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.Tagid).HasName("tags_pkey");

            entity.ToTable("tags");

            entity.Property(e => e.Tagid).HasColumnName("tagid");
            entity.Property(e => e.Label).HasColumnName("label");

            entity.HasMany(d => d.Uploads).WithMany(p => p.Tags)
                .UsingEntity<Dictionary<string, object>>(
                    "Harvesttag",
                    r => r.HasOne<Harvestupload>().WithMany()
                        .HasForeignKey("Uploadid")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("harvesttags_uploadid_fkey"),
                    l => l.HasOne<Tag>().WithMany()
                        .HasForeignKey("Tagid")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("harvesttags_tagid_fkey"),
                    j =>
                    {
                        j.HasKey("Tagid", "Uploadid").HasName("harvesttags_pkey");
                        j.ToTable("harvesttags");
                        j.IndexerProperty<int>("Tagid").HasColumnName("tagid");
                        j.IndexerProperty<int>("Uploadid").HasColumnName("uploadid");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
