using DataManagement.Interfaces;
using Contracts.Data_Transfer_Objects;
using DataManagement.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataManagement;

public class MatchesDbs : IMatchesDbs
{
    private readonly EliteGaertnerDbContext _dbContext;
    
    public MatchesDbs(EliteGaertnerDbContext dbContext)
    {
        _dbContext = dbContext;
    }   
    
    public IEnumerable<PublicProfileDto> GetActiveMatches(int profileIdReceiver)
    {
        //Überprüfe, ob ProfileId <= 0 ist. Falls ja -> Gib leere Liste zurück 
        if (profileIdReceiver <= 0)
            return Enumerable.Empty<PublicProfileDto>();
        
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

        return result;
    }

    public HashSet<int> GetAlreadyRatedProfileIds(int profileIdReceiver)
    {
        //Ungültige profileId -> leeres Ergebnis (keine Bewertungshistorie)
        if (profileIdReceiver <= 0)
            return new HashSet<int>();

        //Alle Creator-ProfileIds, die der Receiver bereits bewertet hat (egal ob Like/Dislike)
        return _dbContext.Ratings
            .AsNoTracking()
            .Where(r => r.Contentreceiverid == profileIdReceiver)
            .Select(r => r.Contentcreatorid)
            .Distinct()
            .ToHashSet();
    }
    
    public void SaveRateInfo(RateDto matchDto)
    {
        //Prüfung, ob matchDto null ist -> ArgumentNullException
        if (matchDto is null)
            throw new ArgumentNullException(nameof(matchDto), "matchDto darf nicht null sein und kann nicht gespeichert werden.");
        //Prüfung der Inhalte der MatchDto -> ArgumentException
        if (matchDto.ContentCreator <= 0 || matchDto.ContentReceiver <= 0 ||
            matchDto.ContentCreator == matchDto.ContentReceiver)
            throw new ArgumentException("matchDto hat inhaltliche Fehler und kann nicht gespeichert werden.",
                nameof(matchDto));

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
        }
        //Falls schon -> Update die Zeile in der Datenbank
        //Das kann eigentlich nicht passieren, weil nur User zur Bewertung vorgeschlagen werden, die noch nie 
        //bewertet worden sind. Aber falls man in Zukunft das doch erlaubt -> Update in der Datenbank
        else
        {
            alreadyRated.Profilerating = matchDto.ContentReceiverValue;
            alreadyRated.Ratingdate = matchDto.ContentReceiverRatingDate;
        }
        
        _dbContext.SaveChanges();
    }
}