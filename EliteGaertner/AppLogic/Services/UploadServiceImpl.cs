using AppLogic.Interfaces;
using Contracts.Data_Transfer_Objects;
using DataManagement;
using DataManagement.Entities;
using Microsoft.EntityFrameworkCore;


namespace AppLogic.Services;

public class UploadServiceImpl : IUploadService
{
    private readonly EliteGaertnerDbContext _context;

    public UploadServiceImpl(EliteGaertnerDbContext context)
    {
        _context = context;
    }
    
    public bool CreateUpload(int userId, string imageUrl, string description, float weight, int width,  int length)
    {
        Console.WriteLine("Upload angekommen.");
        try
        {
            // Erst Profil mit userId laden (userId == Profileid)
            var profile = _context.Profiles.Find(userId);
            if (profile == null)
                return false;

            var entity = new Harvestupload
            {
                Imageurl = imageUrl,
                Description = description,
                Weightgramm = weight,
                Widthcm = width,
                Lengthcm = length,
                Uploaddate = DateTime.UtcNow,
                Profileid = userId
            };

            _context.Harvestuploads.Add(entity);
            _context.SaveChanges();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public bool CreateUpload(HarvestUploadDto uploadDto)
    {
        Console.WriteLine("Upload angekommen.");
        try
        {
            // Profil prüfen (verhindert ForeignKey-Fehler)
            var profileExists = _context.Profiles.Any(p => p.Profileid == uploadDto.ProfileId);
            if (!profileExists)
            {
                Console.WriteLine($"Profil mit ID {uploadDto.ProfileId} nicht gefunden.");
                return false;
            }

            var entity = new Harvestupload
            {
                Imageurl = uploadDto.ImageUrl,
                Description = uploadDto.Description,
                Weightgramm = uploadDto.WeightGram,
                Widthcm = uploadDto.WidthCm,
                Lengthcm = uploadDto.LengthCm,
                Uploaddate = uploadDto.UploadDate,
                Profileid = uploadDto.ProfileId
            };

            _context.Harvestuploads.Add(entity);
            _context.SaveChanges();
            return true;
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"DB-Fehler: {ex.InnerException?.Message ?? ex.Message}");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Allgemeiner Fehler: {ex.Message}\nStack: {ex.StackTrace}");
            return false;
        }
        
    //     try
    //     {
    //         var entity = new Harvestupload
    //         {
    //             Imageurl = uploadDto.ImageUrl,
    //             Description = uploadDto.Description,
    //             Weightgramm = uploadDto.WeightGram,
    //             Widthcm = uploadDto.WidthCm,
    //             Lengthcm = uploadDto.LengthCm,
    //             Uploaddate = uploadDto.UploadDate,
    //             Profileid = uploadDto.ProfileId
    //         };
    //
    //         _context.Harvestuploads.Add(entity);
    //         _context.SaveChanges();
    //         return true;
    //     }
    //     catch (Exception)
    //     {
    //         Console.WriteLine(Exception.ToString);
    //         return false;
    //     }
     }

    
    public bool DeleteUpload(int uploadId, int userId)
    {
        throw new NotImplementedException();
    }

    public List<HarvestUploadDto> GetUserUploads(int userId)
    {
        throw new NotImplementedException();
    }
}