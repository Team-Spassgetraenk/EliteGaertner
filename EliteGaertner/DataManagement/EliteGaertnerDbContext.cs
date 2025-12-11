using Microsoft.EntityFrameworkCore;
using DataManagement.Entities;

namespace DataManagement;

public class EliteGaertnerDbContext : DbContext
{

    public EliteGaertnerDbContext(DbContextOptions<EliteGaertnerDbContext> options) : base(options)
    {
    }
    
    public DbSet<HarvestTags> HarvestTags { get; set; }
    public DbSet<HarvestUploads> HarvestUploads { get; set; }
    //public DbSet<LeaderBoard> LeaderBoard { get; set; } -> Müssen das noch machen
    public DbSet<ProfilePreferences> Preferences { get; set; }
    public DbSet<Profile> Profile { get; set; }
    public DbSet<Rating> Rating { get; set; }
    public DbSet<Report> Report { get; set; }
    public DbSet<Tags> Tags { get; set; }

    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    
        // ---------------------------
        // PROFILE
        // ---------------------------
        modelBuilder.Entity<Profile>(entity =>
        {
            entity.ToTable("profile");
    
            entity.HasKey(e => e.ProfileId);
    
            entity.Property(e => e.ProfileId)
                  .ValueGeneratedOnAdd(); // SERIAL
    
            entity.Property(e => e.UserName).IsRequired();
            entity.Property(e => e.FirstName).IsRequired();
            entity.Property(e => e.LastName).IsRequired();
            entity.Property(e => e.EMail).IsRequired();
            entity.Property(e => e.PasswordHash).IsRequired();
            entity.Property(e => e.PhoneNumber).IsRequired();
            entity.Property(e => e.ProfileText).IsRequired();
    
            entity.Property(e => e.ShareMail).IsRequired();
            entity.Property(e => e.SharePhoneNumber).IsRequired();
    
            entity.Property(e => e.UserCreated).IsRequired(); // TIMESTAMPTZ
        });
    
        // ---------------------------
        // HARVESTUPLOADS
        // ---------------------------
        modelBuilder.Entity<HarvestUploads>(entity =>
        {
            entity.ToTable("harvestuploads");
    
            entity.HasKey(e => e.UploadId);
    
            entity.Property(e => e.UploadId)
                  .ValueGeneratedOnAdd(); // SERIAL
    
            entity.Property(e => e.ImageUrl).IsRequired();
            entity.Property(e => e.Description).IsRequired();
    
            entity.Property(e => e.WeightGramm).IsRequired();
            entity.Property(e => e.WidthCm).IsRequired();
            entity.Property(e => e.LengthCm).IsRequired();
    
            entity.Property(e => e.UploadDate).IsRequired(); // TIMESTAMPTZ
    
            entity.Property(e => e.ProfileId).IsRequired();
    
            //Passt so
            entity.HasOne(e => e.Profile)                 // Navigation Property (optional, aber empfohlen)
                  .WithMany(p => p.HarvestUploads)        // Navigation Property
                  .HasForeignKey(e => e.ProfileId)
                  .OnDelete(DeleteBehavior.Restrict);     // DB sagt nix zu ON DELETE, Restrict ist sicher
        });
    
        // ---------------------------
        // TAGS
        // ---------------------------
        modelBuilder.Entity<Tags>(entity =>
        {
            entity.ToTable("tags");
    
            entity.HasKey(e => e.TagId);
    
            entity.Property(e => e.TagId)
                  .ValueGeneratedOnAdd(); // SERIAL
    
            entity.Property(e => e.Label).IsRequired();
        });
    
        // ---------------------------
        // PROFILEPREFERENCES (Join: Profile <-> Tag)
        // PK(TagId, ProfileId)
        // ---------------------------
        modelBuilder.Entity<ProfilePreferences>(entity =>
        {
            entity.ToTable("profilepreferences");
    
            entity.HasKey(e => new { e.TagId, e.ProfileId }); // Composite Key (zusammengesetzter Schlüssel)
    
            entity.Property(e => e.DateUpdated).IsRequired(); // TIMESTAMPTZ
    
            entity.HasOne(e => e.Tag)
                  .WithMany(t => t.ProfilePreferences)
                  .HasForeignKey(e => e.TagId)
                  .OnDelete(DeleteBehavior.Restrict);
    
            entity.HasOne(e => e.Profile)
                  .WithMany(p => p.ProfilePreferences)
                  .HasForeignKey(e => e.ProfileId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    
        // ---------------------------
        // HARVESTTAGS (Join: HarvestUpload <-> Tag)
        // PK(TagId, UploadId)
        // ---------------------------
        modelBuilder.Entity<HarvestTags>(entity =>
        {
            entity.ToTable("harvestags");
    
            entity.HasKey(e => new { e.TagId, e.UploadId }); // Composite Key
    
            entity.HasOne(e => e.Tag)
                  .WithMany(t => t.HarvestTags)
                  .HasForeignKey(e => e.TagId)
                  .OnDelete(DeleteBehavior.Restrict);
    
            entity.HasOne(e => e.HarvestUpload)
                  .WithMany(u => u.HarvestTags)
                  .HasForeignKey(e => e.UploadId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    
        // ---------------------------
        // REPORT
        // ---------------------------
        modelBuilder.Entity<Report>(entity =>
        {
            entity.ToTable("report");
    
            entity.HasKey(e => e.ReportId);
    
            entity.Property(e => e.ReportId)
                  .ValueGeneratedOnAdd(); // SERIAL
    
            entity.Property(e => e.Reason).IsRequired();
            entity.Property(e => e.UploadId).IsRequired();
    
            //Passt
            entity.HasOne(e => e.HarvestUpload)
                  .WithMany(u => u.Reports)
                  .HasForeignKey(e => e.UploadId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    
        // ---------------------------
        // RATING
        // PK(ContentCreatorId, ContentReceiverId)
        // beide FKs -> PROFILE(ProfileId)
        // ---------------------------
        modelBuilder.Entity<Rating>(entity =>
        {
            entity.ToTable("rating");
    
            entity.HasKey(e => new { e.ContentCreatorId, e.ContentReceiverId });
    
            entity.Property(e => e.ProfileRating).IsRequired();
            entity.Property(e => e.RatingDate).IsRequired(); // TIMESTAMPTZ
    
            entity.HasOne(e => e.ContentCreator)
                  .WithMany(p => p.RatingsReceived)           // z.B. "Ich wurde bewertet"
                  .HasForeignKey(e => e.ContentCreatorId)
                  .OnDelete(DeleteBehavior.Restrict);
    
            entity.HasOne(e => e.ContentReceiver)
                  .WithMany(p => p.RatingsGiven)              // z.B. "Ich habe bewertet"
                  .HasForeignKey(e => e.ContentReceiverId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }
}