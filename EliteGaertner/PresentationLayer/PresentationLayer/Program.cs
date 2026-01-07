using PresentationLayer.Components;
using DataManagement;
using Microsoft.EntityFrameworkCore;
using AppLogic.Interfaces;
using AppLogic.Services;
using DataManagement.Interfaces;
using PresentationLayer.Components.Pages.Register;
using PresentationLayer.Services;
using PresentationLayer.State;

var builder = WebApplication.CreateBuilder(args);

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

//Überprüft, ob Datenbankschema vorhanden ist
//Falls nicht -> Datenbankschema wird implementiert
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<EliteGaertnerDbContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
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