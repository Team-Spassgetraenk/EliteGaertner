using PresentationLayer.Components;
using DataManagement;
using Serilog;
using Microsoft.EntityFrameworkCore;
using AppLogic.Interfaces;
using AppLogic.Services;
using DataManagement.Interfaces;
using PresentationLayer.Services;
using PresentationLayer.State;

var builder = WebApplication.CreateBuilder(args);

//Serilog wird initialisiert
builder.Host.UseSerilog((context, services, loggerConfig) =>
{
    //Liest die Config aus 
    loggerConfig
        //Appsettings.json    
        .ReadFrom.Configuration(context.Configuration)
        //Damit weiß er welche Injections in der Program.cs durchgeführt werden
        .ReadFrom.Services(services);
});

//ConnectionString für EliteGaertnerDbContext
var connectionString = builder.Configuration.GetConnectionString("Default")
                       ?? "Host=localhost;Port=5432;Database=elitegaertner;Username=postgres;Password=postgres,Ssl Mode=Disable";
//EliteGaertnerDbContext wird mit den Optionen + ConnectionString aufgerufen
builder.Services.AddDbContext<EliteGaertnerDbContext>(options =>
    options.UseNpgsql(connectionString));

//DI der DataManagement-Schicht
builder.Services.AddScoped<IHarvestDbs, HarvestDbs>();
builder.Services.AddScoped<IMatchesDbs, MatchesDbs>();
builder.Services.AddScoped<ILeaderBoardDbs, LeaderboardDbs>();
builder.Services.AddScoped<IProfileDbs, ProfileDbs>();

//DI der AppLogic Schicht
builder.Services.AddScoped<IUploadService, UploadServiceImpl>();
builder.Services.AddScoped<IProfileMgm, ProfileMgm>();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents(); // Blazor Server interaktiv

//Registrierung der State-Klassen
builder.Services.AddScoped<UserRegistrationState>();
builder.Services.AddScoped<CurrentProfileState>();
builder.Services.AddScoped<PreferenceState>();
//Kümmert sich um das Logout Handling
builder.Services.AddScoped<SessionService>();
//Kümmert sich um den Upload und Löschservice
builder.Services.AddScoped<IImageUploadService, ImageUploadService>();

var app = builder.Build();

//HTTP Requests loggen (Statuscode, Dauer, etc.)
app.UseSerilogRequestLogging();

//Überprüft, ob Datenbankschema vorhanden ist
//Falls nicht -> Datenbankschema wird implementiert
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<EliteGaertnerDbContext>();
    db.Database.Migrate();
}

//Prüft, ob sich der Code im Development Modus befindet
//Falls nicht -> Logs werden nicht ausgegeben 
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

// Map Razor Components
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode(); // Für Server-Komponenten

app.Run();