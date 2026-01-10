using DataManagement.Interfaces;
using Contracts.Data_Transfer_Objects;
using DataManagement.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataManagement;

public class MatchesDbs : IMatchesDbs
{
    private readonly EliteGaertnerDbContext _dbContext;
    private readonly ILogger<MatchesDbs> _logger;
    
    public MatchesDbs(EliteGaertnerDbContext dbContext, ILogger<MatchesDbs> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
        _logger.LogInformation("MatchesDbs created.");
    }   
    
    public IEnumerable<PublicProfileDto> GetActiveMatches(int profileIdReceiver)
    {
        _logger.LogDebug("GetActiveMatches called. profileIdReceiver={ProfileIdReceiver}", profileIdReceiver);

        //Überprüfe, ob ProfileId <= 0 ist. Falls ja -> Gib leere Liste zurück 
        if (profileIdReceiver <= 0)
        {
            _logger.LogWarning("GetActiveMatches: invalid profileIdReceiver={ProfileIdReceiver}. Returning empty result.", profileIdReceiver);
            return Enumerable.Empty<PublicProfileDto>();
        }

        try
        {
            //Finde alle Ratings, in denen sich der Receiver und der Creator gegenseitig
            //positiv bewertet haben -> Gib mir am Ende die CreatorIds zurück 
            var matches =
                //Nimm die Tabelle Ratings und mach ein Selfjoin und überprüfe, 
                //ob Receiver und Creator sich gegenseitig bewertet haben
                (from r1 in _dbContext.Ratings.AsNoTracking()
                    join r2 in _dbContext.Ratings.AsNoTracking()
                        on new { A = r1.Contentreceiverid, B = r1.Contentcreatorid }
                        equals new { A = r2.Contentcreatorid, B = r2.Contentreceiverid }
                    //Zeig alle Ratings vom Receiver 
                    where r1.Contentreceiverid == profileIdReceiver
                          //Überprüfe, ob sich beide gegenseitig positiv bewertet haben
                          && r1.Profilerating
                          && r2.Profilerating
                    //Speicher die Id vom Creator ab      
                    select r1.Contentcreatorid)
                //Eigentlich nicht möglich, da ProfileIds PK sind, aber vorsichtshalber 
                //können ProfileIds nicht mehrmals vorkommen
                .Distinct();

            //Gleich alle profileIds mit der Profiles-Tabelle ab und erstelle aus ihnen die PublicProfileDto
            var result =
                //Nimm jede ProfileId aus Matches
                (from pid in matches
                    //Macht einen Join mit den Inhalten der Profiles-Tabelle die mit profileIds aus matches übereinstimmen 
                    join p in _dbContext.Profiles.AsNoTracking()
                        on pid equals p.Profileid
                    //Aus den Ergebnissen werden die passenden PublicProfileDtos erstellt     
                    select new PublicProfileDto
                    {
                        //Wir benötigen für die Matchübersicht nur ProfileId, Username
                        //und möglicherweise Email und Phonenumber 
                        ProfileId = p.Profileid,
                        ProfilepictureUrl = p.Profilepictureurl,
                        UserName = p.Username,
                        //Prüft ob der Creator seine Mail und/oder Telefonnummer freigegeben hat
                        EMail = p.Sharemail ? p.Email : null,
                        Phonenumber = p.Sharephonenumber ? p.Phonenumber : null,
                    })
                .ToList();

            _logger.LogInformation("GetActiveMatches result built. profileIdReceiver={ProfileIdReceiver}, count={Count}", profileIdReceiver, result.Count);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetActiveMatches failed. profileIdReceiver={ProfileIdReceiver}", profileIdReceiver);
            throw;
        }
    }

    public HashSet<int> GetAlreadyRatedProfileIds(int profileIdReceiver)
    {
        _logger.LogDebug("GetAlreadyRatedProfileIds called. profileIdReceiver={ProfileIdReceiver}", profileIdReceiver);

        //Ungültige profileId -> leeres Ergebnis (keine Bewertungshistorie)
        if (profileIdReceiver <= 0)
        {
            _logger.LogWarning("GetAlreadyRatedProfileIds: invalid profileIdReceiver={ProfileIdReceiver}. Returning empty set.", profileIdReceiver);
            return new HashSet<int>();
        }

        try
        {
            //Alle Creator-ProfileIds, die der Receiver bereits bewertet hat (egal ob Like/Dislike)
            var result = _dbContext.Ratings
                .AsNoTracking()
                .Where(r => r.Contentreceiverid == profileIdReceiver)
                .Select(r => r.Contentcreatorid)
                .Distinct()
                .ToHashSet();

            _logger.LogInformation("GetAlreadyRatedProfileIds result built. profileIdReceiver={ProfileIdReceiver}, count={Count}", profileIdReceiver, result.Count);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetAlreadyRatedProfileIds failed. profileIdReceiver={ProfileIdReceiver}", profileIdReceiver);
            throw;
        }
    }
    
    public void SaveRateInfo(RateDto matchDto)
    {
        _logger.LogDebug("SaveRateInfo called. ContentReceiver={ContentReceiver}, ContentCreator={ContentCreator}, Value={Value}",
            matchDto?.ContentReceiver, matchDto?.ContentCreator, matchDto?.ContentReceiverValue);

        //Prüfung, ob matchDto null ist -> ArgumentNullException
        if (matchDto is null)
            throw new ArgumentNullException(nameof(matchDto), "matchDto darf nicht null sein und kann nicht gespeichert werden.");
        //Prüfung der Inhalte der MatchDto -> ArgumentException
        if (matchDto.ContentCreator <= 0 || matchDto.ContentReceiver <= 0 ||
            matchDto.ContentCreator == matchDto.ContentReceiver)
            throw new ArgumentException("matchDto hat inhaltliche Fehler und kann nicht gespeichert werden.",
                nameof(matchDto));

        try
        {
            //Prüft, ob Eintrag bereits vorhanden ist
            var alreadyRated = _dbContext.Ratings
                .SingleOrDefault(r => 
                    r.Contentreceiverid == matchDto.ContentReceiver &&
                    r.Contentcreatorid == matchDto.ContentCreator);

            //Falls nicht -> Füge Rating zu Datenbank
            if (alreadyRated is null)
            {
                var rating = new Rating
                {
                    Contentreceiverid = matchDto.ContentReceiver,
                    Contentcreatorid = matchDto.ContentCreator,
                    Profilerating = matchDto.ContentReceiverValue,
                    Ratingdate = matchDto.ContentReceiverRatingDate,
                };
                _dbContext.Ratings.Add(rating);

                _logger.LogInformation("SaveRateInfo: created new rating. ContentReceiver={ContentReceiver}, ContentCreator={ContentCreator}, Value={Value}",
                    matchDto.ContentReceiver, matchDto.ContentCreator, matchDto.ContentReceiverValue);
            }
            //Falls schon -> Update die Zeile in der Datenbank
            //Das kann eigentlich nicht passieren, weil nur User zur Bewertung vorgeschlagen werden, die noch nie 
            //bewertet worden sind. Aber falls man in Zukunft das doch erlaubt -> Update in der Datenbank
            else
            {
                alreadyRated.Profilerating = matchDto.ContentReceiverValue;
                alreadyRated.Ratingdate = matchDto.ContentReceiverRatingDate;

                _logger.LogInformation("SaveRateInfo: updated existing rating. ContentReceiver={ContentReceiver}, ContentCreator={ContentCreator}, Value={Value}",
                    matchDto.ContentReceiver, matchDto.ContentCreator, matchDto.ContentReceiverValue);
            }

            var rows = _dbContext.SaveChanges();
            _logger.LogDebug("SaveRateInfo saved. rows={Rows}", rows);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "SaveRateInfo failed. ContentReceiver={ContentReceiver}, ContentCreator={ContentCreator}, Value={Value}",
                matchDto.ContentReceiver, matchDto.ContentCreator, matchDto.ContentReceiverValue);
            throw;
        }
    }
}